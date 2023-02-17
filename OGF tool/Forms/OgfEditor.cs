using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using GitHubUpdate;
using System.Reflection;

namespace OGF_tool
{
	public partial class OGF_Editor : Form
	{
		// File sytem
		public EditorSettings pSettings = null;
		public XRay_Model Model = null;
		FolderSelectDialog SaveSklDialog = null;
		public string[] game_materials = { };
		public bool UseTexturesCache = false;

		static public string PROGRAM_VERSION = "3.9";

		// Input
		public bool bKeyIsDown = false;
        string number_mask = @"^-[0-9.]*$";
		float CurrentLod = 0.0f; // 0 - HQ, 1 - LQ

		Process ViewerProcess = new Process();
		public bool ViewerWorking = false;
		public Thread ViewerThread = null;
		public bool ViewPortAlpha = true;
        public bool ViewPortTextures = true;
		public bool ViewPortBBox = false;
        public bool ViewPortBones = false;
		public bool ViewPortNeedReload = false;
        List<bool> OldChildVisible = new List<bool>();
		List<string> OldChildTextures = new List<string>();

        public Process[] ConverterProcess = new Process[2] { new Process(), new Process() };
		public bool[] ConverterWorking = new bool[2] { false, false };

        [DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32")]
		private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

		[DllImport("user32")]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

		[DllImport("Converter.dll")]
		private static extern int CSharpStartAgent(string path, string out_path, int mode, int convert_to_mode, string motion_list);

        private int RunConverter(string path, string out_path, int mode, int convert_to_mode)
		{
			string dll_path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\')) + "\\converter.dll";
			if (File.Exists(dll_path))
			{
                return CSharpStartAgent(path, out_path, mode, convert_to_mode, "");
			}
			else
			{
				MessageBox.Show("Can't find Converter.dll", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return -1;
			}
		}

		public enum ExportFormat
		{
			OGF,
			Object,
            Obj,
            Bones,
			OMF,
			Skl,
			Skls
		}

		public OGF_Editor()
		{
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            InitializeComponent();

            if (!Directory.Exists(TempFolder()))
                Directory.CreateDirectory(TempFolder());

            Text += " " + PROGRAM_VERSION;

            // Start init settings

            string file_path = AppPath() + "\\Settings.ini";
			bool SettingsExist = File.Exists(file_path);
			pSettings = new EditorSettings(file_path);

			string gamemtl = "";

			if (!pSettings.CheckVers())
			{
				if (SettingsExist)
					MessageBox.Show("Settings version conflict! Load defaults.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				File.Delete(file_path);
				Settings settings = new Settings(pSettings);
				settings.Settings_Load(null, null); // Load defaults
				settings.SaveParams(); // Save defaults
			}

			pSettings.LoadText("GameMtlPath", ref gamemtl);

			if (File.Exists(gamemtl))
				game_materials = GameMtlParser(gamemtl);

            bool BBoxEnabled = false;
            pSettings.Load("BBoxEnabled", ref BBoxEnabled);
			if (BBoxEnabled)
				showBBoxToolStripMenuItem_Click(showBBoxToolStripMenuItem, null);

            bool BonesEnabled = false;
            pSettings.Load("BonesEnabled", ref BonesEnabled);
            if (BonesEnabled)
                showBonesToolStripMenuItem_Click(showBonesToolStripMenuItem, null);

            bool DisableTextures = false;
            pSettings.Load("DisableTextures", ref DisableTextures, true);
			if (!DisableTextures)
				DisableTexturesMenuItem_Click(null, null);

            bool DisableAlpha = false;
            pSettings.Load("DisableAlpha", ref DisableAlpha, true);

            if (!DisableAlpha)
                disableAlphaToolStripMenuItem_Click(null, null);

            pSettings.Load(BkpCheckBox);

            // End init settings

            OgfInfo.Enabled = false;
			SaveMenuParam.Enabled = false;
			saveAsToolStripMenuItem.Enabled = false;

			// Tools
			OpenInObjectEditor.Enabled = false;
			importDataFromModelToolStripMenuItem.Enabled = false;
            recalcNormalsToolStripMenuItem.Enabled = false;
            recalcBoundingBoxToolStripMenuItem.Enabled = false;
			removeProgressiveMeshesToolStripMenuItem.Enabled = false;
			moveRotateModelToolStripMenuItem.Enabled = false;
			converterToolStripMenuItem.Enabled = false;

			exportToolStripMenuItem.Enabled = false;
			LabelBroken.Visible = false;
			viewPortToolStripMenuItem.Visible = false;
            LodMenuItem.Enabled = false;
            reloadToolStripMenuItem.Enabled = false;
            CurrentFormat.Enabled = false;
            AddMeshesMenuItem.Enabled = false;

            SaveSklDialog = new FolderSelectDialog();
			Model = new XRay_Model();

			if (Environment.GetCommandLineArgs().Length > 1)
			{
				if (Model.OpenFile(Environment.GetCommandLineArgs()[1]))
				{
                    Clear();
                    AfterLoad(true);
				}
			}
			else
            {
				TabControl.Controls.Clear();
			}

			AutoCheckUpdates();
		}

		public void AutoCheckUpdates()
		{
			DateTime current_time = DateTime.Now;
			DateTime last_update_time = pSettings.Load("LastUpdateTime", new System.DateTime(1970, 1, 1));

			int days_passed = Convert.ToInt32((current_time - last_update_time).TotalDays);

			if (days_passed >= 1)
			{
                string user = "VaIeroK";
                string repo = "OGF-tool";
                string vers = OGF_Editor.PROGRAM_VERSION;
                string asset = "OGF.Editor.";

                try
                {
                    UpdateChecker checker;
                    checker = new UpdateChecker(user, repo, vers);
                    checker.CheckUpdate().ContinueWith((continuation) =>
                    {
                        Invoke(new Action(() => // Go back to the UI thread
                        {
                            if (continuation.Result != UpdateType.None)
                            {
                                var result = new UpdateNotifyDialog(checker).ShowDialog();
                                if (result == DialogResult.Yes)
                                {
                                    checker.DownloadAsset(asset);
                                }
                            }
                        }));
                    });
                }
                catch (Exception) { }
                pSettings.Save("LastUpdateTime", current_time);
			}
        }

		public static string AppPath()
		{
			return Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
		}

		public static string TempFolder(bool check = true)
		{
			if (check)
				CheckTempFolder();
			return Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\')) + "\\temp";
		}

		public static void CheckTempFolder()
		{
			if (!Directory.Exists(TempFolder(false)))
				Directory.CreateDirectory(TempFolder(false));
		}

		private void Clear()
		{
			TexturesPage.Controls.Clear();
			BoneParamsPage.Controls.Clear();
			TabControl.Controls.Clear();
			MotionRefsBox.Clear();
			BoneNamesBox.Clear();
			UserDataBox.Clear();
			MotionBox.Clear();
			LodPathBox.Clear();
		}

        private void AfterLoad(bool main_file)
		{
            if (main_file)
			{
				StatusFile.Text = Model.FileName.Substring(Model.FileName.LastIndexOf('\\') + 1);

                reloadToolStripMenuItem.Enabled = true;
                SaveMenuParam.Enabled = true;
				saveAsToolStripMenuItem.Enabled = true;

                OpenInObjectEditor.Enabled = !Model.IsDM;
                importDataFromModelToolStripMenuItem.Enabled = !Model.IsDM;
                recalcNormalsToolStripMenuItem.Enabled = !Model.IsDM;
                recalcBoundingBoxToolStripMenuItem.Enabled = !Model.IsDM;
                moveRotateModelToolStripMenuItem.Enabled = !Model.IsDM;
                converterToolStripMenuItem.Enabled = !Model.IsDM;
                removeProgressiveMeshesToolStripMenuItem.Enabled = LodMenuItem.Enabled = Model.IsProgressive();

                exportToolStripMenuItem.Enabled = true;
				bonesToolStripMenuItem.Enabled = Model.Header.IsSkeleton();
                AddMeshesMenuItem.Enabled = Model.Header.IsSkeleton();
                OgfInfo.Enabled = !Model.IsDM;
				showBonesToolStripMenuItem.Enabled = Model.bonedata != null && Model.ikdata != null;
				objectToolStripMenuItem.Enabled = !Model.IsDetails;

                OpenOGFDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				OpenOGF_DmDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveAsDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveAsDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.'));
				OpenOMFDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				OpenProgramDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveSklDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveSklsDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveSklsDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".skls";
				SaveOmfDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveOmfDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".omf";
				SaveBonesDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveBonesDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".bones";
				SaveObjectDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
				SaveObjectDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".object";
                SaveObjDialog.InitialDirectory = Model.FileName.Substring(0, Model.FileName.LastIndexOf('\\'));
                SaveObjDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".obj";

                CurrentLod = 0;
            }

            omfToolStripMenuItem.Enabled = Model.motions.data() != null;
            sklToolStripMenuItem.Enabled = Model.motions.data() != null;
            sklsToolStripMenuItem.Enabled = Model.motions.data() != null;

            // Textures
            TabControl.Controls.Add(TexturesPage);

			if (Model.Header.IsSkeleton())
			{
				//Userdata
				TabControl.Controls.Add(UserDataPage);
				UserDataPage.Controls.Clear();
				UserDataPage.Controls.Add(UserDataBox);
				UserDataPage.Controls.Add(CreateUserdataButton);
				CreateUserdataButton.Visible = false;
				UserDataBox.Visible = false;

				if (Model.userdata != null)
					UserDataBox.Visible = true;
				else
					CreateUserdataButton.Visible = true;

				// Motion Refs
				TabControl.Controls.Add(MotionRefsPage);
				MotionRefsPage.Controls.Clear();
				MotionRefsPage.Controls.Add(MotionRefsBox);
				MotionRefsPage.Controls.Add(CreateMotionRefsButton);
				CreateMotionRefsButton.Visible = false;
				MotionRefsBox.Visible = false;

				if (Model.motion_refs != null)
					MotionRefsBox.Visible = true;
				else
					CreateMotionRefsButton.Visible = true;

				// Motions
				TabControl.Controls.Add(MotionPage);
				MotionBox.Text = "";

				if (Model.motions.data() != null)
				{
					AppendOMFButton.Visible = false;
					MotionBox.Visible = true;
                    MotionBox.Text = Model.motions.ToString();
                }
				else
				{
					MotionBox.Visible = false;
					AppendOMFButton.Visible = true;
				}

				// Bones
				if (Model.bonedata != null)
				{
					BoneNamesBox.Clear();
					TabControl.Controls.Add(BoneNamesPage);

					BoneNamesBox.Text += $"Bones count : {Model.bonedata.bones.Count}\n\n";
					for (int i = 0; i < Model.bonedata.bones.Count; i++)
					{
						BoneNamesBox.Text += $"{i + 1}. {Model.bonedata.bones[i].name}";

						if (i != Model.bonedata.bones.Count - 1)
							BoneNamesBox.Text += "\n";
					}

                    // Ik Data
                    if (Model.ikdata != null)
					{
						TabControl.Controls.Add(BoneParamsPage);

						for (int i = Model.bonedata.bones.Count - 1; i >= 0; i--)
						{
							CreateBoneGroupBox(i, Model.bonedata.bones[i].name, Model.bonedata.bones[i].parent_name, Model.ikdata.bones[i].material, Model.ikdata.bones[i].mass, Model.ikdata.bones[i].center_mass, Model.ikdata.bones[i].position, Model.ikdata.bones[i].rotation);
						}
                    }
				}

				// Lod
				if (Model.Header.format_version == 4)
					TabControl.Controls.Add(LodPage);

				if (Model.lod != null)
				{
					CreateLodButton.Visible = false;
					LodPathBox.Text = Model.lod.lod_path;
				}
				else
					CreateLodButton.Visible = true;
			}

            for (int i = Model.childs.Count - 1; i >= 0; i--)
			{
				CreateTextureGroupBox(i);

				var TextureGroupBox = TexturesPage.Controls["TextureGrpBox_" + i.ToString()];
                TextureGroupBox.Controls["textureBox_" + i.ToString()].Text = Model.childs[i].m_texture; ;
                TextureGroupBox.Controls["shaderBox_" + i.ToString()].Text = Model.childs[i].m_shader;
			}

            MotionRefsBox.Clear();
			UserDataBox.Clear();

			if (Model.motion_refs != null)
				MotionRefsBox.Lines = Model.motion_refs.refs.ToArray();

			if (Model.userdata != null)
				UserDataBox.Text = Model.userdata.userdata;

			if (main_file && !Model.IsDM)
			{
				LabelBroken.Text = "Broken type: " + Model.BrokenType.ToString();
				LabelBroken.Visible = Model.BrokenType > 0;
			}

			UpdateModelType();
			UpdateModelFormat();
            UpdateNPC();

            // View
            TabControl.Controls.Add(ViewPage);
        }

		private void ApplyParams()
		{
			if (Model.motion_refs != null)
			{
                Model.motion_refs.refs.Clear();

				if (IsTextCorrect(MotionRefsBox.Text))
				{
					for (int i = 0; i < MotionRefsBox.Lines.Count(); i++)
					{
						if (IsTextCorrect(MotionRefsBox.Lines[i]))
                            Model.motion_refs.refs.Add(GetCorrectString(MotionRefsBox.Lines[i]));
					}
				}
			}

			if (Model.userdata != null)
			{
                Model.userdata.userdata = "";

				if (IsTextCorrect(UserDataBox.Text))
				{
					for (int i = 0; i < UserDataBox.Lines.Count(); i++)
					{
						string ext = i == UserDataBox.Lines.Count() - 1 ? "" : "\r\n";
                        Model.userdata.userdata += UserDataBox.Lines[i] + ext;
					}
				}
			}

			if (Model.lod != null)
			{
                Model.lod.lod_path = "";

				if (IsTextCorrect(LodPathBox.Text))
                    Model.lod.lod_path = GetCorrectString(LodPathBox.Text);
			}

			UpdateModelType();
		}

		private bool CheckMeshes()
		{
			foreach (var ch in Model.childs)
			{
				if (!ch.to_delete)
					return true;
			}

            return false;
		}

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
		{
			bKeyIsDown = true;
		}

		private void ButtonFilter(object sender, EventArgs e)
		{
			Button curBox = sender as Button;

			string currentField = curBox.Name.ToString().Split('_')[0];
			int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);

			switch (currentField)
			{
				case "DeleteButton":
                    Model.childs[idx].to_delete = !Model.childs[idx].to_delete;

					if (Model.childs[idx].to_delete)
                    {
						curBox.Text = "Return Mesh";
						curBox.BackColor = Color.FromArgb(255, 255, 128, 128);
					}
					else
                    {
						curBox.Text = "Delete Mesh";
						curBox.BackColor = SystemColors.Control;
					}
					UpdateModelType();
                    Model.RecalcBBox(false);
                    break;
                case "MoveButton":
					float[] old_offs = Model.childs[idx].GetLocalOffset();
                    float[] old_rot = Model.childs[idx].GetLocalRotation();
                    bool old_rot_flag = Model.childs[idx].GetLocalRotationFlag();

                    MoveMesh moveMesh = new MoveMesh(old_offs, old_rot, old_rot_flag, true);
					moveMesh.ShowDialog();

					if (moveMesh.res)
					{
                        Model.childs[idx].SetLocalOffset(moveMesh.offset);
                        Model.childs[idx].SetLocalRotation(moveMesh.rotation, Model.childs[idx].Header.bs.c, moveMesh.LocalRotation);
                    }

					if (!FVec.Similar(old_offs, Model.childs[idx].GetLocalOffset()) || !FVec.Similar(old_rot, Model.childs[idx].GetLocalRotation()) || old_rot_flag != Model.childs[idx].GetLocalRotationFlag())
					{
                        Model.RecalcBBox(true);
                        ReloadViewPort(true, false, true);
					}
                    break;
            }
		}

		private void TextBoxFilter(object sender, EventArgs e)
		{
			TextBox curBox = sender as TextBox;

			string currentField = curBox.Name.ToString().Split('_')[0];
			int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);

			switch (currentField)
			{
				case "textureBox": Model.childs[idx].m_texture = curBox.Text; break;
				case "shaderBox": Model.childs[idx].m_shader = curBox.Text; break;
			}
		}

		void ReloadControlText(Control control, int cursor_pos)
		{
            string currentField = control.Name.ToString().Split('_')[0];
            int idx = Convert.ToInt32(control.Name.ToString().Split('_')[1]);

            switch (currentField)
            {
                case "MassBox": control.Text = ((decimal)Model.ikdata.bones[idx].mass).ToString(); break;
                case "CenterBoxX": control.Text = ((decimal)Model.ikdata.bones[idx].center_mass[0]).ToString(); break;
                case "CenterBoxY": control.Text = ((decimal)Model.ikdata.bones[idx].center_mass[1]).ToString(); break;
                case "CenterBoxZ": control.Text = ((decimal)Model.ikdata.bones[idx].center_mass[2]).ToString(); break;
                case "PositionX": control.Text = ((decimal)Model.ikdata.bones[idx].position[0]).ToString(); break;
                case "PositionY": control.Text = ((decimal)Model.ikdata.bones[idx].position[1]).ToString(); break;
                case "PositionZ": control.Text = ((decimal)Model.ikdata.bones[idx].position[2]).ToString(); break;
                case "RotationX": control.Text = ((decimal)Model.ikdata.bones[idx].rotation[0]).ToString(); break;
                case "RotationY": control.Text = ((decimal)Model.ikdata.bones[idx].rotation[1]).ToString(); break;
                case "RotationZ": control.Text = ((decimal)Model.ikdata.bones[idx].rotation[2]).ToString(); break;
			}

			if (control is TextBox)
			{
                TextBox curBox = control as TextBox;

                if (curBox.SelectionStart < 1)
					curBox.SelectionStart = control.Text.Length;

				curBox.SelectionStart = cursor_pos - 1;
			}
        }

		private void TextBoxBonesFilter(object sender, EventArgs e)
		{
			Control curControl = sender as Control;

			string currentField = curControl.Name.ToString().Split('_')[0];
			int idx = Convert.ToInt32(curControl.Name.ToString().Split('_')[1]);

			if (curControl.Text != "-" || currentField == "MassBox")
			{
				switch (curControl.Tag.ToString())
				{
					case "float":
						{
							if (bKeyIsDown)
							{
								TextBox curBox = sender as TextBox;

								if (curControl.Text.Length == 0)
									return;

								int temp = curBox.SelectionStart;

                                Regex.Match(curControl.Text, number_mask);

                                if (currentField == "MassBox" && curControl.Text.Contains("-"))
                                    ReloadControlText(curControl, temp);

                                try
								{
									Convert.ToSingle(curControl.Text);
								}
								catch (Exception)
								{
									ReloadControlText(curControl, temp);
								}
                            }
						}
						break;
				}

				bool need_recalc_bones = false;

				switch (currentField)
				{
					case "boneBox":
						{
                            Model.bonedata.bones[idx].name = curControl.Text;

							for (int j = 0; j < Model.bonedata.bones[idx].childs_id.Count; j++)
							{
								int child_id = Model.bonedata.bones[idx].childs_id[j];
								var MainGroup = BoneParamsPage.Controls["BoneGrpBox_" + child_id.ToString()];
                                Model.bonedata.bones[child_id].parent_name = curControl.Text;
								MainGroup.Controls["ParentboneBox_" + child_id.ToString()].Text = Model.bonedata.bones[child_id].parent_name;
							}

							BoneNamesBox.Clear();
							BoneNamesBox.Text += $"Bones count : {Model.bonedata.bones.Count}\n\n";

							for (int i = 0; i < Model.bonedata.bones.Count; i++)
							{
								BoneNamesBox.Text += $"{i + 1}. {Model.bonedata.bones[i].name}";
								if (i != Model.bonedata.bones.Count - 1)
									BoneNamesBox.Text += "\n";
							}
                            if (ViewPortBones)
								ViewPortNeedReload = true;
                        }
						break;
					case "MaterialBox": Model.ikdata.bones[idx].material = curControl.Text; break;
					case "MassBox": Model.ikdata.bones[idx].mass = Convert.ToSingle(curControl.Text); break;
					case "CenterBoxX": Model.ikdata.bones[idx].center_mass[0] = Convert.ToSingle(curControl.Text); break;
					case "CenterBoxY": Model.ikdata.bones[idx].center_mass[1] = Convert.ToSingle(curControl.Text); break;
					case "CenterBoxZ": Model.ikdata.bones[idx].center_mass[2] = Convert.ToSingle(curControl.Text); break;
					case "PositionX": Model.ikdata.bones[idx].position[0] = Convert.ToSingle(curControl.Text); need_recalc_bones = true; break;
					case "PositionY": Model.ikdata.bones[idx].position[1] = Convert.ToSingle(curControl.Text); need_recalc_bones = true; break;
					case "PositionZ": Model.ikdata.bones[idx].position[2] = Convert.ToSingle(curControl.Text); need_recalc_bones = true; break;
					case "RotationX": Model.ikdata.bones[idx].rotation[0] = Convert.ToSingle(curControl.Text); need_recalc_bones = true; break;
					case "RotationY": Model.ikdata.bones[idx].rotation[1] = Convert.ToSingle(curControl.Text); need_recalc_bones = true; break;
					case "RotationZ": Model.ikdata.bones[idx].rotation[2] = Convert.ToSingle(curControl.Text); need_recalc_bones = true; break;
				}

				if (need_recalc_bones && (ViewPortBones || Model.ikdata != null && Model.ikdata.chunk_version == 2)) // Если показываем кости или загружен старый меш зависящий от костей
					ViewPortNeedReload = true;
            }

			bKeyIsDown = false;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (Model.FileName == "") return;

			if (!CheckMeshes())
			{
                AutoClosingMessageBox.Show("Can't save model without meshes!", "", 1500, MessageBoxIcon.Error);
				return;
            }

			ApplyParams();
            Model.SaveFile(Model.FileName, BkpCheckBox.Checked);
			AutoClosingMessageBox.Show(Model.NeedRepair() ? "Repaired and Saved!" : "Saved!", "", Model.NeedRepair() ? 700 : 500, MessageBoxIcon.Information);
		}

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
			OpenOGF_DmDialog.FileName = "";
			DialogResult res = OpenOGF_DmDialog.ShowDialog();

			if (res == DialogResult.OK)
			{
				if (Model.OpenFile(OpenOGF_DmDialog.FileName))
				{
                    Clear();
                    OpenOGF_DmDialog.InitialDirectory = "";
					Model.FileName = OpenOGF_DmDialog.FileName;
					AfterLoad(true);
				}
			}
		}

        private void oGFInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

			OgfInfo Info = new OgfInfo(Model, IsTextCorrect(MotionRefsBox.Text), CurrentLod);
            Info.ShowDialog();

			if (Info.res && Model.description != null)
			{
				Model.description.m_source = Info.descr.m_source;
				Model.description.m_export_tool = Info.descr.m_export_tool;
				Model.description.m_owner_name = Info.descr.m_owner_name;
				Model.description.m_export_modif_name_tool = Info.descr.m_export_modif_name_tool;
				Model.description.m_creation_time = Info.descr.m_creation_time;
				Model.description.m_export_time = Info.descr.m_export_time;
				Model.description.m_modified_time = Info.descr.m_modified_time;
                Model.description.four_byte = Info.descr.four_byte;
			}

			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckMeshes())
            {
                AutoClosingMessageBox.Show("Can't save file without meshes!", "", 1500, MessageBoxIcon.Error);
                return;
            }

            if (Model.IsDetails)
                SaveAsDialog.Filter = "Detail file|*.details";
            else if (Model.IsDM)
				SaveAsDialog.Filter = "DM file|*.dm";
			else
				SaveAsDialog.Filter = "OGF file|*.ogf";

			if (SaveAsDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveAsDialog.FileName, 0);
				SaveAsDialog.InitialDirectory = "";
			}
		}

		private void SaveTools(string filename, ExportFormat format)
		{
            if (!CheckMeshes())
            {
                AutoClosingMessageBox.Show("Can't save file without meshes!", "", 1500, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(filename) && filename != Model.FileName)
                File.Delete(filename);

			int exit_code = 0;

            switch (format)
			{
				case ExportFormat.OGF:
                    if (filename != Model.FileName)
                        File.Copy(Model.FileName, filename);

                    ApplyParams();
                    Model.SaveFile(filename, BkpCheckBox.Checked);
                    break;
				case ExportFormat.Obj:
                    Model.SaveFileObj(filename, CurrentLod);
                    break;
                case ExportFormat.Object:
                    string ext = Model.IsDM ? ".dm" : ".ogf";

                    if (File.Exists(filename + ext))
                        File.Delete(filename + ext);

                    File.Copy(Model.FileName, filename + ext);

                    ApplyParams();
                    Model.SaveFile(filename + ext, BkpCheckBox.Checked);

                    exit_code = RunConverter(filename + ext, filename, Model.IsDM ? 2 : 0, 0);

                    if (File.Exists(filename + ext))
						File.Delete(filename + ext);
                    break;
				case ExportFormat.OMF:
					using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate))
					{
						fileStream.Write(Model.motions.data(), 0, Model.motions.data().Length);
						fileStream.Close();
                    }
                    break;
                case ExportFormat.Bones:
                    exit_code = RunConverter(Model.FileName, filename, 0, 1);
                    break;
                case ExportFormat.Skl:
                    exit_code = RunConverter(Model.FileName, filename, 0, 2);
                    break;
                case ExportFormat.Skls:
                    exit_code = RunConverter(Model.FileName, filename, 0, 3);
                    break;
            }

			if (exit_code == 0)
			{
				string Text = (Model.NeedRepair() ? "Repaired and " : "") + (format == ExportFormat.OGF ? "Saved!" : "Exported!");
				AutoClosingMessageBox.Show(Text, "", Model.NeedRepair() ? 700 : 500, MessageBoxIcon.Information);
			}
			else
				AutoClosingMessageBox.Show("Export aborted!", "", 1500, MessageBoxIcon.Error);
        }

		private void objectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveObjectDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveObjectDialog.FileName, ExportFormat.Object);
				SaveObjectDialog.InitialDirectory = "";
			}
		}

		private void bonesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveBonesDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveBonesDialog.FileName, ExportFormat.Bones);
				SaveBonesDialog.InitialDirectory = "";
			}
		}

		private void omfToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveOmfDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveOmfDialog.FileName, ExportFormat.OMF);
				SaveOmfDialog.InitialDirectory = "";
			}
		}

		private void objToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveObjDialog.ShowDialog() == DialogResult.OK)
			{
				float old_lod = CurrentLod;
				CurrentLod = 0.0f;
				SaveTools(SaveObjDialog.FileName, ExportFormat.Obj);
				CurrentLod = old_lod;
				SaveObjDialog.InitialDirectory = "";
			}
		}

		private void sklToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveSklDialog.ShowDialog(this.Handle))
			{
				SaveTools(SaveSklDialog.FileName, ExportFormat.Skl);
				SaveSklDialog.InitialDirectory = "";
			}
		}

		private void sklsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveSklsDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveSklsDialog.FileName, ExportFormat.Skls);
				SaveSklsDialog.InitialDirectory = "";
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (MessageBox.Show("Are you sure you want to exit?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				Close();
		}

        private void CreateUserdataButton_Click(object sender, EventArgs e)
        {
			CreateUserdataButton.Visible = false;
			UserDataBox.Visible = true;
			UserDataBox.Clear();
			if (Model.userdata == null)
                Model.userdata = new UserData();
		}

        private void CreateMotionRefsButton_Click(object sender, EventArgs e)
        {
			if (Model.motions.data() == null || Model.motions.data() != null && MessageBox.Show("New motion refs chunk will remove built-in motions, continue?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				// Чистим все связанное со встроенными анимами
				MotionBox.Clear();
				MotionBox.Visible = false;
				AppendOMFButton.Visible = true;
                Model.motions.SetData(null);

				// Обновляем тип модели
				UpdateModelType();
				UpdateModelFormat();

				// Обновляем визуал интерфейса моушн рефов
				CreateMotionRefsButton.Visible = false; 
				MotionRefsBox.Visible = true;
				MotionRefsBox.Clear();

				if (Model.motion_refs == null)
                    Model.motion_refs = new MotionRefs();
			}
		}

		private void CreateLodButton_Click(object sender, EventArgs e)
		{
			CreateLodButton.Visible = false;
			LodPathBox.Clear();
			if (Model.lod == null)
                Model.lod = new Lod();
		}

		private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (Model.FileName == "") return;

			string cur_fname = Model.FileName;
			if (Model.OpenFile(cur_fname))
			{
                Clear();
                Model.FileName = cur_fname;
				AfterLoad(true);
			}
		}

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (TabControl.SelectedIndex < 0) return;
			bool ViewPortItemVisible = false;

			switch (TabControl.Controls[TabControl.SelectedIndex].Name)
			{
				case "UserDataPage":
					{
						if (!IsTextCorrect(UserDataBox.Text))
						{
							CreateUserdataButton.Visible = true;
							UserDataBox.Visible = false;
						}
						break;
					}
				case "MotionRefsPage":
					{
						if (!IsTextCorrect(MotionRefsBox.Text))
						{
							CreateMotionRefsButton.Visible = true;
							MotionRefsBox.Visible = false;
						}
						break;
					}
				case "LodPage":
					{
						if (!IsTextCorrect(LodPathBox.Text))
						{
							CreateLodButton.Visible = true;
						}
						break;
					}
				case "ViewPage":
					{
						InitViewPort(true, false, ViewPortNeedReload);
						ViewPortItemVisible = true;
						break;
					}
				case "BoneParamsPage":
					{
                        for (int i = 0; i < BoneParamsPage.Controls.Count; i++)
						{
							if (Model.ikdata == null || Model.ikdata.bones.Count <= i)
								break;

							GroupBox box = BoneParamsPage.Controls["BoneGrpBox_" + i.ToString()] as GroupBox;
							TableLayoutPanel layoutPanel = box.Controls["LayoutPanel_" + i.ToString()] as TableLayoutPanel;
							(layoutPanel.Controls["PositionX_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].position[0]).ToString();
							(layoutPanel.Controls["PositionY_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].position[1]).ToString();
							(layoutPanel.Controls["PositionZ_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].position[2]).ToString();
							(layoutPanel.Controls["RotationX_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].rotation[0]).ToString();
							(layoutPanel.Controls["RotationY_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].rotation[1]).ToString();
							(layoutPanel.Controls["RotationZ_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].rotation[2]).ToString();
                            (layoutPanel.Controls["CenterBoxX_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].center_mass[0]).ToString();
                            (layoutPanel.Controls["CenterBoxY_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].center_mass[1]).ToString();
                            (layoutPanel.Controls["CenterBoxZ_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].center_mass[2]).ToString();
                            (layoutPanel.Controls["MassBox_" + i.ToString()] as TextBox).Text = ((decimal)Model.ikdata.bones[i].mass).ToString();
						}
						break;
					}

            }
			viewPortToolStripMenuItem.Visible = ViewPortItemVisible;
		}

		private void RichTextBoxTextChanged(object sender, EventArgs e)
		{
			RichTextBox curBox = sender as RichTextBox;
			switch (curBox.Name)
			{
                case "MotionRefsBox":
                    {
						UpdateModelType();
						UpdateModelFormat();
						break;
                    }
            }
		}

        private void EditInOmfEditor(object sender, EventArgs e)
        {
            string Filename = TempFolder() + $"\\{StatusFile.Text}_temp.omf";
            string OmfEditor = pSettings.Load("OmfEditorPath");

            if (!File.Exists(OmfEditor))
            {
                MessageBox.Show("Please, set OMF Editor path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var fileStream = new FileStream(Filename, FileMode.OpenOrCreate))
            {
                fileStream.Write(Model.motions.data(), 0, Model.motions.data().Length);
				fileStream.Close();
            }

            Process proc = new Process();
            proc.StartInfo.FileName = OmfEditor;
            proc.StartInfo.Arguments += $"\"{Filename}\"";
            proc.Start();
            proc.WaitForExit();

            OpenOMFDialog.FileName = Filename;
            AppendMotion(null, null);
            OpenOMFDialog.FileName = "";

            File.Delete(Filename);
        }

        private void DeleteOmf(object sender, EventArgs e)
        {
            MotionBox.Visible = false;
            AppendOMFButton.Visible = true;
            Model.motions.SetData(null);
            MotionBox.Clear();
            UpdateModelType();
            UpdateModelFormat();
        }

        private void AppendOMFButton_Click(object sender, EventArgs e)
        {
			if (!IsTextCorrect(MotionRefsBox.Text) && (Model.motion_refs == null || Model.motion_refs.refs.Count() == 0) || (IsTextCorrect(MotionRefsBox.Text) || Model.motion_refs != null && Model.motion_refs.refs.Count() > 0) && MessageBox.Show("Build-in motions will remove motion refs, continue?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				OpenOMFDialog.ShowDialog();
        }

		private void AppendMotion(object sender, CancelEventArgs e)
		{
			if (sender != null)
				OpenOMFDialog.InitialDirectory = "";

            byte[] OpenedOmf = File.ReadAllBytes(OpenOMFDialog.FileName);

			if (Model.motions.SetData(OpenedOmf))
			{
                // Апдейтим визуал встроенных анимаций
                AppendOMFButton.Visible = false;
                MotionBox.Visible = true;

                // Чистим встроенные рефы, интерфейс почистится сам при активации вкладки
                MotionRefsBox.Clear();
                if (Model.motion_refs != null)
                    Model.motion_refs.refs.Clear();

                MotionBox.Text = Model.motions.ToString();
            }

			UpdateModelType();
			UpdateModelFormat();
		}

		public void importDataFromModelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenOGFDialog.FileName = "";
			if (OpenOGFDialog.ShowDialog() == DialogResult.OK)
			{
				bool Update = false;

				XRay_Model SecondOgf = new XRay_Model();
				if (SecondOgf.OpenFile(OpenOGFDialog.FileName))
				{
					AutoClosingMessageBox.Show("Can't import OGF Model!", "Error", 1000, MessageBoxIcon.Error);
					return;
				}

                if (SecondOgf.Header.IsSkeleton())
				{
					ImportParams Params = new ImportParams(Model, SecondOgf);

					Params.ShowDialog();

					if (Params.res)
					{
						if (Params.Textures)
						{
							for (int i = 0; i < Model.childs.Count; i++)
							{
                                Model.childs[i].m_texture = SecondOgf.childs[i].m_texture;
                                Model.childs[i].m_shader = SecondOgf.childs[i].m_shader;
							}

							Update = true;
						}

						if (Params.Userdata)
						{
							if (Model.userdata == null)
                                Model.userdata = new UserData();

                            Model.userdata.userdata = SecondOgf.userdata.userdata;
                            Model.userdata.old_format = SecondOgf.userdata.old_format;

                            Update = true;
						}
						else if (Params.Remove && Model.userdata != null)
						{
                            Model.userdata.userdata = "";
							Update = true;
						}

						if (Params.Lod)
						{
							if (Model.lod == null)
                                Model.lod = new Lod();

                            Model.lod.lod_path = SecondOgf.lod.lod_path;

							Update = true;
						}
						else if (Params.Remove && Model.lod != null)
						{
                            Model.lod.lod_path = "";
							Update = true;
						}

						if (Params.MotionRefs)
						{
							if (Model.motion_refs == null)
                                Model.motion_refs = new MotionRefs();

                            Model.motion_refs.refs = SecondOgf.motion_refs.refs;

							Update = true;
						}
						else if (Params.Remove && Model.motion_refs != null)
						{
                            Model.motion_refs.refs.Clear();
							Update = true;
						}

						if (Params.Motions)
						{
                            Model.motions.SetData(SecondOgf.motions.data());

							if (Model.motion_refs != null)
                                Model.motion_refs.refs.Clear();

							Update = true;
						}
						else if (Params.Remove)
						{
                            Model.motions.SetData(null);
							Update = true;
						}

						if (Params.Materials)
						{
							for (int i = 0; i < Model.bonedata.bones.Count; i++)
							{
								Model.ikdata.bones[i].material = SecondOgf.ikdata.bones[i].material;
                                Model.ikdata.bones[i].mass = SecondOgf.ikdata.bones[i].mass;
							}

							Update = true;
						}

						if (Update)
						{
							Clear();
							AfterLoad(false);
							AutoClosingMessageBox.Show("OGF Params changed!", "", 1000, MessageBoxIcon.Information);
						}
						else
						{
							AutoClosingMessageBox.Show("OGF Params don't changed!", "", 1000, MessageBoxIcon.Warning);
						}
					}
				}
				else
                    AutoClosingMessageBox.Show("Can't load params from non skeleton model!", "", 1000, MessageBoxIcon.Warning);
            }
		}

        private void batchToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Batch batchForm = new Batch(this);
            batchForm.ShowDialog();
        }

        private void ChangeModelFormat(object sender, EventArgs e)
		{
			if (Model.Opened)
			{
				Model.ChangeModelFormat();
                UpdateModelFormat();
			}
		}

		private void UpdateModelType()
        {
			if (!Model.Opened) return;

			if (Model.bonedata == null)
                Model.Header.Static(Model.childs);
			else if (Model.motions.data() != null || IsTextCorrect(MotionRefsBox.Text))
                Model.Header.Animated();
			else
                Model.Header.Skeleton();

			// Апдейтим экспорт аним тут, т.к. при любом изменении омф вызывается эта функция
			omfToolStripMenuItem.Enabled = Model.motions.data() != null;
			sklToolStripMenuItem.Enabled = Model.motions.data() != null;
			sklsToolStripMenuItem.Enabled = Model.motions.data() != null;
		}

		private void UpdateModelFormat()
		{
			CurrentFormat.Enabled = (Model.Opened && !Model.IsDM && Model.Header.IsSkeleton());

			if (!CurrentFormat.Enabled)
			{
				CurrentFormat.Text = strings.AllFormat;
				return;
			}

			uint links = 0;

			foreach (var ch in Model.childs)
				links = Math.Max(links, ch.links);

            Model.IsCopModel = (IsTextCorrect(MotionRefsBox.Text) && Model.motion_refs != null && !Model.motion_refs.soc || !IsTextCorrect(MotionRefsBox.Text)) && links < 0x12071980;

			CurrentFormat.Text = (Model.IsCopModel ? strings.CoPFormat : strings.SoCFormat);
		}

		private void openSkeletonInObjectEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (!CheckMeshes())
            {
                AutoClosingMessageBox.Show("Can't open model without meshes!", "", 1500, MessageBoxIcon.Error);
                return;
            }

            string Filename = TempFolder() + $"\\{StatusFile.Text}_temp.ogf";
			string ObjectName = Filename.Substring(0, Filename.LastIndexOf('.'));
			ObjectName = ObjectName.Substring(0, ObjectName.LastIndexOf('.')) + ".object";

			string ObjectEditor = pSettings.Load("ObjectEditorPath");

			if (!File.Exists(ObjectEditor))
			{
				MessageBox.Show("Please, set Object Editor path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (File.Exists(Filename))
				File.Delete(Filename);

			File.Copy(Model.FileName, Filename);
            ApplyParams();
            Model.SaveFile(Filename, BkpCheckBox.Checked);
			int exit_code = RunConverter(Filename, ObjectName, 0, 0);

			if (exit_code == 0)
			{
				Process proc = new Process();
				proc.StartInfo.FileName = ObjectEditor;
				proc.StartInfo.Arguments += $"\"{ObjectName}\" skeleton_only \"{Model.FileName}\"";
				proc.Start();
				proc.WaitForExit();
			}
			else
                AutoClosingMessageBox.Show("Can't convert model to object!", "", 1500, MessageBoxIcon.Error);
        }

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string old_game_mtl = "";
			pSettings.Load("GameMtlPath", ref old_game_mtl);

            bool OldViewPortAlpha = false;
            pSettings.Load("ViewportAlpha", ref OldViewPortAlpha);

            Settings ProgramSettings = new Settings(pSettings);
			ProgramSettings.ShowDialog();

			string game_mtl = "";
			pSettings.Load("GameMtlPath", ref game_mtl);

			if (old_game_mtl != game_mtl)
				ReloadGameMtl(game_mtl);

            bool ViewPortAlpha = false;
            pSettings.Load("ViewportAlpha", ref ViewPortAlpha);

			if (OldViewPortAlpha != ViewPortAlpha)
                disableAlphaToolStripMenuItem_Click(null, null);
        }

		private void changeLodToolStripMenuItem_Click(object sender, EventArgs e)
		{
			float old_lod = CurrentLod;
			SwiLod swiLod = new SwiLod(CurrentLod);
			swiLod.ShowDialog();

			if (swiLod.res)
			{
				CurrentLod = swiLod.Lod;
				if (old_lod != CurrentLod)
				{
					RecalcLod();
					ReloadViewPort(true, false, true);
				}
			}
        }

		private void RecalcLod()
        {
			for (int idx = 0; idx < Model.childs.Count; idx++)
            {
				Control Mesh = TexturesPage.Controls["TextureGrpBox_" + idx.ToString()];
				Label FaceLbl = (Label)Mesh.Controls["FacesLbl_" + idx.ToString()];
				FaceLbl.Text = FaceLabel.Text + Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString();
			}
        }

        private void removeProgressiveMeshesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float old_lod = CurrentLod;

            SwiLod swiLod = new SwiLod(CurrentLod);
            swiLod.ShowDialog();

			if (swiLod.res)
			{
                Model.RemoveProgressive(CurrentLod);

				for (int idx = 0; idx < Model.childs.Count; idx++)
				{
					Control Mesh = TexturesPage.Controls["TextureGrpBox_" + idx.ToString()];
					Label FaceLbl = (Label)Mesh.Controls["LodsLbl_" + idx.ToString()];
					FaceLbl.Visible = false;
				}

				removeProgressiveMeshesToolStripMenuItem.Enabled = LodMenuItem.Enabled = Model.IsProgressive();

				if (old_lod != CurrentLod)
					ReloadViewPort(true, false, true);
			}
        }

        private void moveRotateModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (Model == null) return;

            float[] old_offs = Model.local_offset;
            float[] old_rot = Model.local_rotation;

            MoveMesh moveMesh = new MoveMesh(old_offs, old_rot, false, false);
            moveMesh.ShowDialog();

			if (moveMesh.res)
			{
                Model.local_offset = moveMesh.offset;
                Model.local_rotation = moveMesh.rotation;

                for (int i = 0; i < Model.childs.Count; i++)
				{
                    Model.childs[i].SetLocalOffsetMain(Model.local_offset);
                    Model.childs[i].SetLocalRotationMain(Model.local_rotation);
				}
            }

            if (!FVec.Similar(old_offs, Model.local_offset) || !FVec.Similar(old_rot, Model.local_rotation))
            {
                Model.RecalcBBox(true);
                ReloadViewPort(true, false, true);
            }
        }

        private string[] GameMtlParser(string filename)
		{
			List<string> materials = new List<string>();

			if (File.Exists(filename))
			{
				var xr_loader = new XRayLoader();
				using (var r = new BinaryReader(new FileStream(filename, FileMode.Open)))
				{
					xr_loader.SetStream(r.BaseStream);
					xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)MTL.GAMEMTLS_CHUNK_MTLS, false, true));

					int id = 0;
					uint size;

					while (true)
					{
						if (!xr_loader.find_chunk(id)) break;

						Stream temp = xr_loader.reader.BaseStream;

						if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

						size = xr_loader.find_chunkSize((int)MTL.GAMEMTL_CHUNK_MAIN);
						if (size == 0) break;
						xr_loader.ReadBytes(4);
						materials.Add(xr_loader.read_stringZ());

						id++;
						xr_loader.SetStream(temp);
					}
				}
			}
			string[] ret = materials.ToArray();
			Array.Sort(ret);
			return ret;
		}

		public void ReloadGameMtl(string filename)
		{
			game_materials = GameMtlParser(filename);

			if (Model.Opened && Model.bonedata != null)
			{
				BoneParamsPage.Controls.Clear();
				for (int i = 0; i < Model.bonedata.bones.Count; i++)
				{
					CreateBoneGroupBox(i, Model.bonedata.bones[i].name, Model.bonedata.bones[i].parent_name, Model.ikdata.bones[i].material, Model.ikdata.bones[i].mass, Model.ikdata.bones[i].center_mass, Model.ikdata.bones[i].position, Model.ikdata.bones[i].rotation);
				}
			}
		}

		string CheckNaN(float val)
        {
			if (val.ToString() == "NaN")
				return "0";
			return ((decimal)val).ToString();
		}

        private void RichTextBoxImgDefender(object sender, KeyEventArgs e)
		{
			RichTextBox TextBox = sender as RichTextBox;
			if (e.Control && e.KeyCode == Keys.V)
			{
				if (Clipboard.ContainsText())
					TextBox.Paste(DataFormats.GetFormat(DataFormats.Text));
				e.Handled = true;
			}
		}

		static public bool IsTextCorrect(string text)
        {
			foreach (char ch in text)
            {
				if (ch > 0x1F && ch != 0x20)
					return true;
			}
			return false;
        }

		private string GetCorrectString(string text)
		{
			string ret_text = "", symbols = "";
			bool started = false;
			foreach (char ch in text)
			{
				if (started)
                {
					if (ch <= 0x1F || ch == 0x20)
						symbols += ch;
                    else
					{
						ret_text += symbols + ch;
						symbols = "";
					}
				}
				else if (ch > 0x1F && ch != 0x20)
				{
					started = true;
					ret_text += ch;
				}
			}
			return ret_text;
		}

		private void ClosingForm(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (ViewerThread != null && ViewerThread.ThreadState != System.Threading.ThreadState.Stopped)
					ViewerThread.Abort();
			}
			catch (Exception) { }

			try
			{
				Model.Destroy();
			}
			catch (Exception) { }

			if (ViewerWorking)
				pSettings.Save("FirstLoad", false);

			try
			{
				if (ViewerWorking)
				{
					ViewerProcess.Kill();
					ViewerProcess.Close();
					ViewerWorking = false;
				}
			}
			catch (Exception) { }

			for (int i = 0; i < 2; i++)
			{
				try
				{
					if (ConverterWorking[i])
					{
						ConverterProcess[i].Kill();
						ConverterProcess[i].Close();
						ConverterWorking[i] = false;
					}
				}
				catch (Exception) { }
			}

			try
			{
				if (!UseTexturesCache && Directory.Exists(TempFolder(false)))
					Directory.Delete(TempFolder(false), true);
			}
			catch (Exception) { }
		}

		private void ClosedForm(object sender, FormClosedEventArgs e)
		{
			ClosingForm(sender, null);
		}

		private void DragEnterCallback(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

			string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			foreach (string file in fileList)
			{
				if (Path.GetExtension(file) == ".ogf" || Path.GetExtension(file) == ".dm")
				{
					e.Effect = DragDropEffects.Copy;
					break;
				}
			}
		}

		private void DragDropCallback(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

			string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			foreach (string file in fileList)
			{
				if (Path.GetExtension(file) == ".ogf" || Path.GetExtension(file) == ".dm")
				{
					if (Model.OpenFile(file))
					{
                        Clear();
                        Model.FileName = file;
						AfterLoad(true);
					}
					break;
				}
			}
		}

		private void addMeshesToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (OpenOGFDialog.ShowDialog() == DialogResult.OK)
			{
                XRay_Model SecondModel = new XRay_Model();
                SecondModel.OpenFile(OpenOGFDialog.FileName);

				if (SecondModel.Header.IsSkeleton())
				{
					int old_childs_count = Model.childs.Count;

					AddMesh addMeshDialog = new AddMesh(ref Model, SecondModel);
					addMeshDialog.ShowDialog();

					if (addMeshDialog.res && old_childs_count != Model.childs.Count)
					{
						TexturesPage.Controls.Clear();
						for (int i = Model.childs.Count - 1; i >= 0; i--)
						{
							CreateTextureGroupBox(i);

							var TextureGroupBox = TexturesPage.Controls["TextureGrpBox_" + i.ToString()];
							TextureGroupBox.Controls["textureBox_" + i.ToString()].Text = Model.childs[i].m_texture; ;
							TextureGroupBox.Controls["shaderBox_" + i.ToString()].Text = Model.childs[i].m_shader;
						}

                        Model.RecalcBBox(false);
                    }
				}
				else
                    AutoClosingMessageBox.Show("Can't merge non skeleton model!", "", 1000, MessageBoxIcon.Warning);
            }
        }

        private void recalcNormalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Model.Opened)
            {
				SelectMeshes selectMeshes = new SelectMeshes(Model);
				selectMeshes.ShowDialog();

				if (selectMeshes.res && selectMeshes.MeshChecked.Count == Model.childs.Count)
				{
                    bool Reloaded = false;

                    for (int i = 0; i < Model.childs.Count; i++)
                    {
						if (selectMeshes.MeshChecked[i])
						{
							Reloaded = true;
                            Model.childs[i].MeshNormalize();
						}
                    }

					if (Reloaded)
					{
                        ReloadViewPort(true, false, true);
                        AutoClosingMessageBox.Show("Mesh normals recalculated!", "", 1000, MessageBoxIcon.Information);
					}
					else
                        AutoClosingMessageBox.Show("Mesh normals don't changed!", "", 1000, MessageBoxIcon.Warning);
                }
            }
        }

        private void openImageFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string image_path = "";
            pSettings.Load("ImagePath", ref image_path);

            if (image_path != "" && Directory.Exists(image_path))
            {
                Process PrFolder = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.FileName = "explorer";
                psi.Arguments = image_path;
                PrFolder.StartInfo = psi;
                PrFolder.Start();
            }
        }

        private void BkpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pSettings.Save(BkpCheckBox);
        }

        private void recalcBoundingBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model.RecalcBBox(true);
            ReloadViewPort(true, false, true);
            AutoClosingMessageBox.Show("Bounding Box and Sphere recalculated!", "", 1000, MessageBoxIcon.Information);
        }

        private void UpdateNPC()
		{
			nPCCoPToSoCToolStripMenuItem.Enabled = CheckNPC(true);
			nPCSoCToCoPToolStripMenuItem.Enabled = CheckNPC(false);
        }

        private void NPC_ToSoC(object sender, EventArgs e)
        {
			if (CheckNPC(true))
			{
                Model.RemoveBone("root_stalker");
                Model.RemoveBone("bip01");

                Model.ChangeParent("root_stalker", "bip01_pelvis");
                Model.ChangeParent("bip01", "bip01_pelvis");

                Model.bonedata.bones[0].parent_name = "";
                Model.bonedata.RecalcChilds();

                for (int i = 0; i < Model.bonedata.bones.Count; i++)
				{
                    Model.ikdata.bones[i].position = Resources.SoCSkeleton.Pos(i);
                    Model.ikdata.bones[i].rotation = Resources.SoCSkeleton.Rot(i);
                    Model.ikdata.bones[i].center_mass = FVec.RotateXYZ(Model.ikdata.bones[i].center_mass, 0.0f, 180.0f, 0.0f);
				}

                foreach (var ch in Model.childs)
				{
					uint links = ch.LinksCount();

					for (int i = 0; i < ch.Vertices.Count; i++)
					{
						for (int j = 0; j < links; j++)
							ch.Vertices[i].bones_id[j] = (ch.Vertices[i].bones_id[j] >= 2 ? ch.Vertices[i].bones_id[j] - 2 : 0);

						ch.Vertices[i].offs = FVec.RotateXYZ(ch.Vertices[i].offs, 0.0f, 180.0f, 0.0f);
                        ch.Vertices[i].local_offset = FVec.RotateXYZ(ch.Vertices[i].local_offset, 0.0f, 180.0f, 0.0f);
                        ch.Vertices[i].norm = FVec.RotateXYZ(ch.Vertices[i].norm, 0.0f, 180.0f, 0.0f);
						ch.Vertices[i].tang = FVec.RotateXYZ(ch.Vertices[i].tang, 0.0f, 180.0f, 0.0f);
						ch.Vertices[i].binorm = FVec.RotateXYZ(ch.Vertices[i].binorm, 0.0f, 180.0f, 0.0f);
					}
				}
                ReloadViewPort(true, false, true);
                AutoClosingMessageBox.Show("Successful!", "", 700, MessageBoxIcon.Information);
			}
			else
				AutoClosingMessageBox.Show("This model is not CoP NPC!", "", 2500, MessageBoxIcon.Error);

			UpdateNPC();
        }

        private void NPC_ToCoP(object sender, EventArgs e)
        {
			if (CheckNPC(false))
			{
                Model.AddBone("root_stalker", "", 0);
                Model.AddBone("bip01", "root_stalker", 1);

                Model.bonedata.bones[2].parent_name = "bip01";
                Model.bonedata.RecalcChilds();

                for (int i = 0; i < Model.bonedata.bones.Count; i++)
                {
                    Model.ikdata.bones[i].position = Resources.CoPSkeleton.Pos(i);
                    Model.ikdata.bones[i].rotation = Resources.CoPSkeleton.Rot(i);
                    Model.ikdata.bones[i].center_mass = FVec.RotateXYZ(Model.ikdata.bones[i].center_mass, 0.0f, 180.0f, 0.0f);
                }

                foreach (var ch in Model.childs)
                {
                    for (int i = 0; i < ch.Vertices.Count; i++)
                    {
                        for (int j = 0; j < ch.LinksCount(); j++)
                            ch.Vertices[i].bones_id[j] = ch.Vertices[i].bones_id[j] + 2;

                        ch.Vertices[i].offs = FVec.RotateXYZ(ch.Vertices[i].offs, 0.0f, 180.0f, 0.0f);
                        ch.Vertices[i].local_offset = FVec.RotateXYZ(ch.Vertices[i].local_offset, 0.0f, 180.0f, 0.0f);
                        ch.Vertices[i].norm = FVec.RotateXYZ(ch.Vertices[i].norm, 0.0f, 180.0f, 0.0f);
                        ch.Vertices[i].tang = FVec.RotateXYZ(ch.Vertices[i].tang, 0.0f, 180.0f, 0.0f);
                        ch.Vertices[i].binorm = FVec.RotateXYZ(ch.Vertices[i].binorm, 0.0f, 180.0f, 0.0f);
                    }
                }
                ReloadViewPort(true, false, true);
                AutoClosingMessageBox.Show("Successful!", "", 700, MessageBoxIcon.Information);
            }
            else
                AutoClosingMessageBox.Show("This model is not SoC NPC!", "", 2500, MessageBoxIcon.Error);

            UpdateNPC();
        }

        private bool CheckNPC(bool cop_npc)
        {
			if (Model.Opened)
			{
				if (cop_npc)
				{
					if (Model.Header.IsSkeleton())
					{
						if (Model.bonedata.bones.Count == 47 && Model.bonedata.GetBoneID("root_stalker") != -1)
							return true;
					}
				}
				else
				{
					if (Model.Header.IsSkeleton())
					{
						if (Model.bonedata.bones.Count == 45 && Model.bonedata.GetBoneID("bip01_pelvis") != -1 && Model.bonedata.GetBoneID("root_stalker") == -1)
							return true;
					}
				}
			}

            return false;
        }

        public static void Msg(string text)
        {
			MessageBox.Show(text);
        }

        // Interface
		private void ReloadViewPort(bool create_model = true, bool force_texture_reload = false, bool force_reload = false)
		{
			if (ViewerWorking && ViewerProcess != null)
			{
                InitViewPort(create_model, force_texture_reload, force_reload);
			}
        }

        private void InitViewPort(bool create_model = true, bool force_texture_reload = false, bool force_reload = false)
        {
			if (!Model.Opened) return;

			if (ViewerWorking && ViewerProcess != null && CheckViewportModelVers() && !force_reload) return;

			bool old_viewer = ViewerWorking;
			ViewerWorking = false;
            ViewPortNeedReload = false;

            Model.FixOldBonesBind();
            Model.CalcBonesTransform();

            if (ViewerThread != null && ViewerThread.ThreadState != System.Threading.ThreadState.Stopped)
				ViewerThread.Abort();

			ViewerThread = new Thread(() => {
				string ObjName = TempFolder() + "\\" + Path.GetFileName(Path.ChangeExtension(Model.FileName, ".obj"));
				string exe_path = AppPath() + "\\f3d.exe";

				if (!File.Exists(exe_path))
				{
					MessageBox.Show("Can't find Viewport module.\nPlease, reinstall the app.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

                this.Invoke((MethodInvoker)delegate ()
				{
                    viewPortToolStripMenuItem.Enabled = false;
                });

                if (old_viewer)
				{
					ViewerProcess.Kill();
					ViewerProcess.Close();
				}

				for (int i = 0; i < 2; i++)
				{
					if (ConverterWorking[i])
					{
						ConverterProcess[i].Kill();
						ConverterProcess[i].Close();
						ConverterWorking[i] = false;
					}
				}

				if (ViewPortTextures)
				{
					string Textures = "";
					pSettings.LoadText("TexturesPath", ref Textures);

					List<string> pTextures = new List<string>();
					List<string> pConvertTextures = new List<string>();

					for (int i = 0; i < Model.childs.Count; i++)
					{
						string texture_main = Textures + "\\" + Model.childs[i].m_texture + ".dds";
						string texture_temp = TempFolder() + "\\" + Path.GetFileName(Model.childs[i].m_texture + ".png");

						if (File.Exists(texture_temp) && !force_texture_reload)
							continue;

						pTextures.Add(texture_main);
						pTextures.Add(texture_temp);
					}

					int chld = 0;
					for (int i = 0; i < pTextures.Count; i++)
					{
						if (File.Exists(pTextures[i]) && (!File.Exists(pTextures[i + 1]) || force_texture_reload))
						{
							if (Model.childs[chld].to_delete)
								continue;

							pConvertTextures.Add(pTextures[i]);
							pConvertTextures.Add(pTextures[i + 1]);
						}
						i++;
						chld++;
					}

					OldChildVisible.Clear();
					foreach (var ch in Model.childs)
						OldChildVisible.Add(ch.to_delete);

					OldChildTextures.Clear();
					foreach (var ch in Model.childs)
						OldChildTextures.Add(ch.m_texture);

					if (pConvertTextures.Count > 0)
					{
						string ConverterArgs = "";
						ConverterArgs += $"{(ViewPortAlpha ? 1 : 0)}";
						ConverterArgs += $" {pConvertTextures.Count}";

						for (int i = 0; i < pConvertTextures.Count; i++)
						{
							ConverterArgs += $" \"{pConvertTextures[i]}\"";
						}

						ConverterWorking[0] = true;
						ConverterProcess[0] = new Process();
						ProcessStartInfo psi = new ProcessStartInfo();
						psi.CreateNoWindow = true;
						psi.UseShellExecute = false;
						psi.FileName = AppPath() + "\\TextureConverter.exe";
						psi.Arguments = ConverterArgs;
						ConverterProcess[0].StartInfo = psi;
						ConverterProcess[0].Start();
					}
				}

				if (ViewPortBones && showBonesToolStripMenuItem.Enabled)
				{
					if (!Directory.Exists(TempFolder() + "\\bones"))
						Directory.CreateDirectory(TempFolder() + "\\bones");

                    string ConverterArgs = "";
					int TexturesCount = 0;

					for (int i = 0; i < Model.bonedata.bones.Count; i++)
					{
                        if (File.Exists($"{TempFolder()}\\bones\\{Model.bonedata.bones[i].GetNotNullName()}.png"))
							continue;
						ConverterArgs += $" \"{Model.bonedata.bones[i].name}\" \"{TempFolder()}\\bones\\{Model.bonedata.bones[i].GetNotNullName()}.png\"";
						TexturesCount++;
                    }
					
					ConverterWorking[1] = true;
					ConverterProcess[1] = new Process();
					ProcessStartInfo psi = new ProcessStartInfo();
					psi.CreateNoWindow = true;
					psi.UseShellExecute = false;
					psi.FileName = AppPath() + "\\TextToPng.exe";
					psi.Arguments = $"{TexturesCount}{ConverterArgs}";
					psi.WorkingDirectory = AppPath();
                    ConverterProcess[1].StartInfo = psi;
					ConverterProcess[1].Start();
				}


                for (int i = 0; i < 2; i++)
				{
					if (ConverterWorking[i])
					{
						ConverterProcess[i].WaitForExit();
						ConverterWorking[i] = false;
                    }
                }

				string bbox_texture_main = TempFolder() + "\\bbox_main_texture.png";
                string bbox_texture = TempFolder() + "\\bbox_texture.png";
                string null_texture = TempFolder() + "\\null_texture.png";
                if (ViewPortBBox && !File.Exists(bbox_texture_main))
				{
                    using (Bitmap b = new Bitmap(8, 8))
                    {
                        using (Graphics g = Graphics.FromImage(b))
                        {
                            g.Clear(Color.FromArgb(127, Color.Green));
                        }
                        b.Save(bbox_texture_main, ImageFormat.Png);
                    }
                }

                if (ViewPortBBox && !File.Exists(bbox_texture))
                {
                    using (Bitmap b = new Bitmap(8, 8))
                    {
                        using (Graphics g = Graphics.FromImage(b))
                        {
                            g.Clear(Color.FromArgb(127, Color.Red));
                        }
                        b.Save(bbox_texture, ImageFormat.Png);
                    }
                }

				if (ViewPortBones && showBonesToolStripMenuItem.Enabled && !File.Exists(null_texture))
				{
                    using (Bitmap b = new Bitmap(8, 8))
                    {
                        using (Graphics g = Graphics.FromImage(b))
                        {
                            g.Clear(Color.FromArgb(70, 15, 25, 15));
                        }
                        b.Save(null_texture, ImageFormat.Png);
                    }
                }

                string image_path = "";
				pSettings.Load("ImagePath", ref image_path);

				bool first_load = true;
				pSettings.Load("FirstLoad", ref first_load, true);

				if (create_model)
                    Model.SaveFileObj(ObjName, CurrentLod, ViewPortBones && showBonesToolStripMenuItem.Enabled, ViewPortBBox, ViewPortTextures);

				ViewerProcess.StartInfo.FileName = exe_path;
				ViewerProcess.StartInfo.Arguments = $"--input=\"{ObjName}\" --output=\"{image_path}\"" + (first_load ? " --filename" : "");
				ViewerProcess.StartInfo.UseShellExecute = false;
				ViewerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

				ViewerProcess.Start();
				ViewerProcess.WaitForInputIdle();
				ViewerWorking = true;

				try
				{
					this.Invoke((MethodInvoker)delegate ()
					{
						const int GWL_STYLE = -16;
						const int WS_CAPTION = 0x00C00000;
						const int WS_THICKFRAME = 0x00040000;

						SetParent(ViewerProcess.MainWindowHandle, ViewPage.Handle);
						int style = GetWindowLong(ViewerProcess.MainWindowHandle, GWL_STYLE);
						style = style & ~WS_CAPTION & ~WS_THICKFRAME;
						SetWindowLong(ViewerProcess.MainWindowHandle, GWL_STYLE, style);
						ResizeEmbeddedApp(null, null);
                        viewPortToolStripMenuItem.Enabled = true;
                    });
				}
				catch(Exception) { }
            });
			ViewerThread.Start();
        }

		private bool CheckViewportModelVers()
        {
            if (OldChildTextures.Count != Model.childs.Count || OldChildVisible.Count != Model.childs.Count) return false;

			if (OldChildTextures.Count != 0)
            {
				int i = 0;
				foreach (var ch in Model.childs)
				{
					if (ch.m_texture != OldChildTextures[i])
						return false;
					i++;
				}
			}

			if (OldChildVisible.Count != 0)
			{
				int i = 0;
				foreach (var ch in Model.childs)
				{
					if (ch.to_delete != OldChildVisible[i])
						return false;
					i++;
				}
			}

			return true;
		}

		private void reloadToolStripMenuItem1_Click(object sender, EventArgs e)
		{
            ReloadViewPort(true, true, true);
		}

		private void disableAlphaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewPortAlpha = !ViewPortAlpha;
            pSettings.Save("DisableAlpha", ViewPortAlpha);

            if (ViewPortAlpha)
                disableAlphaToolStripMenuItem.Text = "Disable Alpha";
            else
                disableAlphaToolStripMenuItem.Text = "Enable Alpha";

            ReloadViewPort(false, true, true);
		}

        private void DisableTexturesMenuItem_Click(object sender, EventArgs e)
        {
            ViewPortTextures = !ViewPortTextures;
            pSettings.Save("DisableTextures", ViewPortTextures);

			if (Model.Opened)
			{
				string mtl_name = TempFolder() + "\\" + Path.GetFileName(Path.ChangeExtension(Model.FileName, ".mtl"));
                Model.SaveMtl(mtl_name, ViewPortBones && showBonesToolStripMenuItem.Enabled, ViewPortBBox, ViewPortTextures);
			}

            if (!ViewPortTextures)
				DisableTexturesMenuItem.Text = "Enable Textures";
			else
                DisableTexturesMenuItem.Text = "Disable Textures";

            ReloadViewPort(false, false, true);
        }

        private void showBBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ViewPortBBox = !ViewPortBBox;

            pSettings.Save("BBoxEnabled", ViewPortBBox);

            if (!ViewPortBBox)
                item.Text = "Show Bounding Box";
            else
                item.Text = "Hide Bounding Box";

            ReloadViewPort(true, false, true);
        }

        private void showBonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ViewPortBones = !ViewPortBones;

			if (e != null)
				pSettings.Save("BonesEnabled", ViewPortBones);

            if (!ViewPortBones)
                item.Text = "Show Bones";
            else
                item.Text = "Hide Bones";

            ReloadViewPort(true, false, true);
        }

		private void ResizeEmbeddedApp(object sender, EventArgs e)
		{
			if (ViewerProcess == null || !ViewerWorking)
				return;

			const int SWP_NOACTIVATE = 0x0010;
			const int SWP_NOZORDER = 0x0004;

			int width = ViewPage.Width;
			int height = ViewPage.Height;
			SetWindowPos(ViewerProcess.MainWindowHandle, IntPtr.Zero, 0, 0, width, height, SWP_NOACTIVATE | SWP_NOZORDER);
		}

        private void CreateTextureGroupBox(int idx)
		{
			var GroupBox = new GroupBox();
			GroupBox.Location = new System.Drawing.Point(TexturesGropuBox.Location.X, TexturesGropuBox.Location.Y + (TexturesGropuBox.Size.Height + 2) * idx);
			GroupBox.Size = TexturesGropuBox.Size;
			GroupBox.Text = TexturesGropuBox.Text + " [" + idx + "]";
			GroupBox.Name = "TextureGrpBox_" + idx;
			GroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			GroupBox.Dock = TexturesGropuBox.Dock;
			CreateTextureBoxes(idx, GroupBox);
			CreateTextureLabels(idx, GroupBox);
			TexturesPage.Controls.Add(GroupBox);
		}

		private void CreateTextureBoxes(int idx, GroupBox box)
		{
			var newTextBox = Copy.TextBox(TexturesTextBoxEx);
			newTextBox.Name = "textureBox_" + idx;
			newTextBox.TextChanged += new System.EventHandler(this.TextBoxFilter);

			var newTextBox2 = Copy.TextBox(ShaderTextBoxEx);
			newTextBox2.Name = "shaderBox_" + idx;
			newTextBox2.TextChanged += new System.EventHandler(this.TextBoxFilter);

			var newButton = Copy.Button(DeleteMesh);
			newButton.Name = "DeleteButton_" + idx;
			newButton.Click += new System.EventHandler(this.ButtonFilter);
			newButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			newButton.Enabled = !Model.IsDM;

            var newButton2 = Copy.Button(MoveMeshButton);
            newButton2.Name = "MoveButton_" + idx;
            newButton2.Click += new System.EventHandler(this.ButtonFilter);
            newButton2.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            newButton2.Enabled = !Model.IsDM;

            box.Controls.Add(newTextBox);
			box.Controls.Add(newTextBox2);
			box.Controls.Add(newButton);
            box.Controls.Add(newButton2);
        }

		private void CreateTextureLabels(int idx, GroupBox box)
		{
			var newLbl = Copy.Label(TexturesPathLabelEx);
			newLbl.Name = "textureLbl_" + idx;

			var newLbl2 = Copy.Label(ShaderNameLabelEx);
			newLbl2.Name = "shaderLbl_" + idx;

			var newLbl3 = Copy.Label(FaceLabel);
			newLbl3.Name = "FacesLbl_" + idx;
			newLbl3.Text = FaceLabel.Text + Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString();
			newLbl3.Size = new Size(FaceLabel.Size.Width + (Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), FaceLabel.Size.Height);
			newLbl3.Location = new Point(FaceLabel.Location.X - (Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), FaceLabel.Location.Y);

			var newLbl4 = Copy.Label(VertsLabel);
			newLbl4.Name = "VertsLbl_" + idx;
			newLbl4.Text = VertsLabel.Text + Model.childs[idx].Vertices.Count.ToString();
			newLbl4.Size = new Size(VertsLabel.Size.Width + (Model.childs[idx].Vertices.Count.ToString().Length * 6), VertsLabel.Size.Height);
			newLbl4.Location = new Point(VertsLabel.Location.X - (Model.childs[idx].Vertices.Count.ToString().Length * 6) - (Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), VertsLabel.Location.Y);

			var newLbl5 = Copy.Label(LinksLabel);
			newLbl5.Name = "LinksLbl_" + idx;
			newLbl5.Text = LinksLabel.Text + Model.childs[idx].LinksCount().ToString();
			newLbl5.Size = new Size(LinksLabel.Size.Width + (Model.childs[idx].LinksCount().ToString().Length * 6), LinksLabel.Size.Height);
			newLbl5.Location = new Point(LinksLabel.Location.X - (Model.childs[idx].Vertices.Count.ToString().Length * 6) - (Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6) - (Model.childs[idx].LinksCount().ToString().Length * 6), LinksLabel.Location.Y);

			var newLbl6 = Copy.Label(LodLabel);
			newLbl6.Name = "LodsLbl_" + idx;
			newLbl6.Text = LodLabel.Text + Model.childs[idx].SWI.Count.ToString();
			newLbl6.Size = new Size(LodLabel.Size.Width + (Model.childs[idx].SWI.Count.ToString().Length * 6), LodLabel.Size.Height);
			newLbl6.Location = new Point(LodLabel.Location.X - (Model.childs[idx].Vertices.Count.ToString().Length * 6) - (Model.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6) - (Model.childs[idx].LinksCount().ToString().Length * 6) - (Model.childs[idx].SWI.Count.ToString().Length * 6), LodLabel.Location.Y);

			box.Controls.Add(newLbl);
			box.Controls.Add(newLbl2);
			box.Controls.Add(newLbl3);
			box.Controls.Add(newLbl4);

			if (Model.Header.IsSkeleton())
				box.Controls.Add(newLbl5);

			if (Model.childs[idx].SWI.Count > 0)
				box.Controls.Add(newLbl6);
		}

		private void CreateBoneGroupBox(int idx, string bone_name, string parent_bone_name, string material, float mass, float[] center, float[] pos, float[] rot)
		{
			var GroupBox = new GroupBox();
			GroupBox.Location = new System.Drawing.Point(BoneParamsGroupBox.Location.X, BoneParamsGroupBox.Location.Y + (BoneParamsGroupBox.Size.Height + 2) * idx);
			GroupBox.Size = BoneParamsGroupBox.Size;
			GroupBox.Text = BoneParamsGroupBox.Text + " [" + idx + "]";
			GroupBox.Name = "BoneGrpBox_" + idx;
			GroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			GroupBox.Dock = BoneParamsGroupBox.Dock;

			CreateBoneTextBox(idx, GroupBox, bone_name, parent_bone_name, material, mass, center, pos, rot);
			BoneParamsPage.Controls.Add(GroupBox);
		}

		private void CreateBoneTextBox(int idx, GroupBox box, string bone_name, string parent_bone_name, string material, float mass, float[] center, float[] pos, float[] rot)
		{
			var BoneNameTextBox = Copy.TextBox(BoneNameTextBoxEx);
			BoneNameTextBox.Name = "boneBox_" + idx;
			BoneNameTextBox.Text = bone_name;
			BoneNameTextBox.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			BoneNameTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var BoneNameLabel = Copy.Label(BoneNameLabelEx);
			BoneNameLabel.Name = "boneLabel_" + idx;

			var ParentBoneNameTextBox = Copy.TextBox(ParentBoneTextBoxEx);
			ParentBoneNameTextBox.Name = "ParentboneBox_" + idx;
			ParentBoneNameTextBox.Text = parent_bone_name;

			var ParentBoneNameLabel = Copy.Label(ParentBoneLabelEx);
			ParentBoneNameLabel.Name = "ParentboneLabel_" + idx;
			ParentBoneNameLabel.Size = ParentBoneLabelEx.Size;
			ParentBoneNameLabel.Location = ParentBoneLabelEx.Location;
			ParentBoneNameLabel.Text = ParentBoneLabelEx.Text;

			var MateriaBox = new Control();
			if (game_materials.Count() == 0)
			{
				var MaterialTextBox = Copy.TextBox(MaterialTextBoxEx);
				MaterialTextBox.Name = "MaterialBox_" + idx;
				MaterialTextBox.Text = material;
				MaterialTextBox.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
				MaterialTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

				MateriaBox = MaterialTextBox;
			}
			else
			{
				var MaterialTextBox = new ComboBox();
				MaterialTextBox.Name = "MaterialBox_" + idx;
				MaterialTextBox.Size = MaterialTextBoxEx.Size;
				MaterialTextBox.Location = MaterialTextBoxEx.Location;
				MaterialTextBox.Text = material;
				MaterialTextBox.Tag = "string";
				MaterialTextBox.SelectedIndexChanged += new System.EventHandler(this.TextBoxBonesFilter);
				MaterialTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				MaterialTextBox.Items.AddRange(game_materials);
				MaterialTextBox.DropDownStyle = ComboBoxStyle.DropDownList;

				if (MaterialTextBox.Items.Contains(material))
					MaterialTextBox.SelectedIndex = MaterialTextBox.Items.IndexOf(material);
				else
				{
					MaterialTextBox.Items.Insert(0, material);
					MaterialTextBox.SelectedIndex = 0;
				}

				MateriaBox = MaterialTextBox;
			}

			var MaterialLabel = Copy.Label(MaterialLabelEx);
			MaterialLabel.Name = "MaterialLabel_" + idx;

			var MassTextBox = Copy.TextBox(MassTextBoxEx);
			MassTextBox.Name = "MassBox_" + idx;
			MassTextBox.Text = CheckNaN(mass);
			MassTextBox.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			MassTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var MassLabel = Copy.Label(MassLabelEx);
			MassLabel.Name = "MassLabel_" + idx;

			var LayoutPanel = Copy.TableLayoutPanel(BonesParamsPanel);
			LayoutPanel.Name = "LayoutPanel_" + idx;

			var CenterMassTextBoxX = Copy.TextBox(CenterOfMassXTextBox);
			CenterMassTextBoxX.Name = "CenterBoxX_" + idx;
			CenterMassTextBoxX.Text = CheckNaN(center[0]);
			CenterMassTextBoxX.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var CenterMassTextBoxY = Copy.TextBox(CenterOfMassYTextBox);
			CenterMassTextBoxY.Name = "CenterBoxY_" + idx;
			CenterMassTextBoxY.Text = CheckNaN(center[1]);
			CenterMassTextBoxY.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var CenterMassTextBoxZ = Copy.TextBox(CenterOfMassZTextBox);
			CenterMassTextBoxZ.Name = "CenterBoxZ_" + idx;
			CenterMassTextBoxZ.Text = CheckNaN(center[2]);
			CenterMassTextBoxZ.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var CenterMassLabel = Copy.Label(CenterOfMassLabelEx);
			CenterMassLabel.Name = "CenterMassLabel_" + idx;

			var PositionX = Copy.TextBox(PositionXTextBox);
			PositionX.Name = "PositionX_" + idx;
			PositionX.Text = CheckNaN(pos[0]);
			PositionX.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			PositionX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var PositionY = Copy.TextBox(PositionYTextBox);
			PositionY.Name = "PositionY_" + idx;
			PositionY.Text = CheckNaN(pos[1]);
			PositionY.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			PositionY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var PositionZ = Copy.TextBox(PositionZTextBox);
			PositionZ.Name = "PositionZ_" + idx;
			PositionZ.Text = CheckNaN(pos[2]);
			PositionZ.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			PositionZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var PositionLabel = Copy.Label(PositionLabelEx);
			PositionLabel.Name = "PositionLabel_" + idx;

			var RotationX = Copy.TextBox(RotationXTextBox);
			RotationX.Name = "RotationX_" + idx;
			RotationX.Text = CheckNaN(rot[0]);
			RotationX.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			RotationX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var RotationY = Copy.TextBox(RotationYTextBox);
			RotationY.Name = "RotationY_" + idx;
			RotationY.Text = CheckNaN(rot[1]);
			RotationY.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			RotationY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var RotationZ = Copy.TextBox(RotationZTextBox);
			RotationZ.Name = "RotationZ_" + idx;
			RotationZ.Text = CheckNaN(rot[2]);
			RotationZ.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			RotationZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);

			var RotationLabel = Copy.Label(RotationLabelEx);
			RotationLabel.Name = "RotationLabel_" + idx;

			LayoutPanel.Controls.Add(MassTextBox, 0, 0);
			LayoutPanel.Controls.Add(CenterMassTextBoxX, 0, 1);
			LayoutPanel.Controls.Add(CenterMassTextBoxY, 1, 1);
			LayoutPanel.Controls.Add(CenterMassTextBoxZ, 2, 1);
			LayoutPanel.Controls.Add(PositionX, 0, 2);
			LayoutPanel.Controls.Add(PositionY, 1, 2);
			LayoutPanel.Controls.Add(PositionZ, 2, 2);
			LayoutPanel.Controls.Add(RotationX, 0, 3);
			LayoutPanel.Controls.Add(RotationY, 1, 3);
			LayoutPanel.Controls.Add(RotationZ, 2, 3);

			box.Controls.Add(LayoutPanel);

			box.Controls.Add(BoneNameTextBox);
			box.Controls.Add(ParentBoneNameTextBox);
			box.Controls.Add(MateriaBox);

			box.Controls.Add(BoneNameLabel);
			box.Controls.Add(ParentBoneNameLabel);
			box.Controls.Add(MaterialLabel);
			box.Controls.Add(MassLabel);
			box.Controls.Add(CenterMassLabel);
			box.Controls.Add(PositionLabel);
			box.Controls.Add(RotationLabel);
		}
    }
}
