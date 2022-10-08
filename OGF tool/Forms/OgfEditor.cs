using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Security.Policy;
using System.Runtime.InteropServices.ComTypes;


namespace OGF_tool
{
	public partial class OGF_Editor : Form
	{
		// File sytem
		public EditorSettings pSettings = null;
		public OGF_Children OGF_V = null;
		public byte[] Current_OGF = null;
		public List<byte> file_bytes = new List<byte>();
		public string FILE_NAME = "";
		FolderSelectDialog SaveSklDialog = null;
		public string[] game_materials = { };

		// Input
		public bool bKeyIsDown = false;
		string number_mask = "";
		StreamWriter ObjWriter = null; // for closing
		float CurrentLod = 0.0f;

		Process ViewerProcess = new Process();
		public bool ViewerWorking = false;
		public Thread ViewerThread = null;
		bool ViewPortAlpha = true;
		List<bool> OldChildVisible = new List<bool>();
		List<string> OldChildTextures = new List<string>();

		Process ConverterProcess = new Process();
		public bool ConverterWorking = false;

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32")]
		private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

		[DllImport("user32")]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

		[DllImport("converter.dll")]
		private static extern int CSharpStartAgent(string path, string out_path, int mode, int convert_to_mode, string motion_list);

		delegate void TriangleParser(XRayLoader loader, OGF_Child child, bool v3);
        private int RunConverter(string path, string out_path, int mode, int convert_to_mode)
		{
			string dll_path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\')) + "\\converter.dll";
			if (File.Exists(dll_path))
				return CSharpStartAgent(path, out_path, mode, convert_to_mode, "");
			else
			{
				MessageBox.Show("Can't find converter.dll", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				settings.SaveParams(null, null); // Save defaults
			}

			pSettings.LoadText("GameMtlPath", ref gamemtl);

			if (File.Exists(gamemtl))
				game_materials = GameMtlParser(gamemtl);

			bool ViewPortAlpha = false;
			pSettings.Load("ViewportAlpha", ref ViewPortAlpha);
			SetAlphaToolStrip(ViewPortAlpha);

            // End init settings

            number_mask = @"^-[0-9.]*$";

			OgfInfo.Enabled = false;
			SaveMenuParam.Enabled = false;
			saveAsToolStripMenuItem.Enabled = false;
			OpenInObjectEditor.Enabled = false;
			ToolsMenuItem.Enabled = false;
			exportToolStripMenuItem.Enabled = false;
			LabelBroken.Visible = false;
			viewPortToolStripMenuItem.Visible = false;
            LodMenuItem.Enabled = false;
            reloadToolStripMenuItem.Enabled = false;
            CurrentFormat.Enabled = false;
            AddMeshesMenuItem.Enabled = false;

            SaveSklDialog = new FolderSelectDialog();

			if (Environment.GetCommandLineArgs().Length > 1)
			{
				Clear(false);
				if (OpenFile(Environment.GetCommandLineArgs()[1], ref OGF_V, ref Current_OGF))
				{
					FILE_NAME = Environment.GetCommandLineArgs()[1];
					AfterLoad(true);
				}
			}
			else
            {
				TabControl.Controls.Clear();
			}

			if (!Directory.Exists(TempFolder()))
				Directory.CreateDirectory(TempFolder());
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

		private void Clear(bool ui_only)
		{
			if (!ui_only)
			{
				FILE_NAME = "";
				OGF_V = null;
				file_bytes.Clear();
			}
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
				StatusFile.Text = FILE_NAME.Substring(FILE_NAME.LastIndexOf('\\') + 1);

                reloadToolStripMenuItem.Enabled = true;
                SaveMenuParam.Enabled = true;
				saveAsToolStripMenuItem.Enabled = true;
				ToolsMenuItem.Enabled = !OGF_V.IsDM;
				OpenInObjectEditor.Enabled = true;
				exportToolStripMenuItem.Enabled = true;
				bonesToolStripMenuItem.Enabled = OGF_V.IsSkeleton();
                LodMenuItem.Enabled = OGF_V.IsProgressive();
                AddMeshesMenuItem.Enabled = OGF_V.IsSkeleton();
                OgfInfo.Enabled = !OGF_V.IsDM;

				OpenOGFDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				OpenOGF_DmDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveAsDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveAsDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.'));
				OpenOMFDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				OpenProgramDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveSklDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveSklsDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveSklsDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".skls";
				SaveOmfDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveOmfDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".omf";
				SaveBonesDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveBonesDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".bones";
				SaveObjectDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveObjectDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".object";
                SaveObjDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
                SaveObjDialog.FileName = StatusFile.Text.Substring(0, StatusFile.Text.LastIndexOf('.')) + ".obj";

                CurrentLod = 0;
			}

            omfToolStripMenuItem.Enabled = OGF_V.motions.data() != null;
            sklToolStripMenuItem.Enabled = OGF_V.motions.data() != null;
            sklsToolStripMenuItem.Enabled = OGF_V.motions.data() != null;

            // Textures
            TabControl.Controls.Add(TexturesPage);

			if (OGF_V.IsSkeleton())
			{
				//Userdata
				TabControl.Controls.Add(UserDataPage);
				UserDataPage.Controls.Clear();
				UserDataPage.Controls.Add(UserDataBox);
				UserDataPage.Controls.Add(CreateUserdataButton);
				CreateUserdataButton.Visible = false;
				UserDataBox.Visible = false;

				if (OGF_V.userdata != null)
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

				if (OGF_V.motion_refs != null)
					MotionRefsBox.Visible = true;
				else
					CreateMotionRefsButton.Visible = true;

				// Motions
				TabControl.Controls.Add(MotionPage);
				MotionBox.Text = "";

				if (OGF_V.motions.data() != null)
				{
					AppendOMFButton.Visible = false;
					MotionBox.Visible = true;
                    MotionBox.Text = OGF_V.motions.ToString();
                }
				else
				{
					MotionBox.Visible = false;
					AppendOMFButton.Visible = true;
				}

				// Bones
				if (OGF_V.bones != null)
				{
					BoneNamesBox.Clear();
					TabControl.Controls.Add(BoneNamesPage);

					BoneNamesBox.Text += $"Bones count : {OGF_V.bones.bone_names.Count}\n\n";
					for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
					{
						BoneNamesBox.Text += $"{i + 1}. {OGF_V.bones.bone_names[i]}";

						if (i != OGF_V.bones.bone_names.Count - 1)
							BoneNamesBox.Text += "\n";
					}


					// Ik Data
					if (OGF_V.ikdata != null)
					{
						TabControl.Controls.Add(BoneParamsPage);

						for (int i = OGF_V.bones.bone_names.Count - 1; i >= 0; i--)
						{
							CreateBoneGroupBox(i, OGF_V.bones.bone_names[i], OGF_V.bones.parent_bone_names[i], OGF_V.ikdata.materials[i], OGF_V.ikdata.mass[i], OGF_V.ikdata.center_mass[i], OGF_V.ikdata.position[i], OGF_V.ikdata.rotation[i]);
						}
					}
				}

				// Lod
				if (OGF_V.Header.format_version == 4)
					TabControl.Controls.Add(LodPage);

				if (OGF_V.lod != null)
				{
					CreateLodButton.Visible = false;
					LodPathBox.Text = OGF_V.lod.lod_path;
				}
				else
					CreateLodButton.Visible = true;
			}

            for (int i = OGF_V.childs.Count - 1; i >= 0; i--)
			{
				CreateTextureGroupBox(i);

				var TextureGroupBox = TexturesPage.Controls["TextureGrpBox_" + i.ToString()];
                TextureGroupBox.Controls["textureBox_" + i.ToString()].Text = OGF_V.childs[i].m_texture; ;
                TextureGroupBox.Controls["shaderBox_" + i.ToString()].Text = OGF_V.childs[i].m_shader;
			}

            MotionRefsBox.Clear();
			UserDataBox.Clear();

			if (OGF_V.motion_refs != null)
				MotionRefsBox.Lines = OGF_V.motion_refs.refs.ToArray();

			if (OGF_V.userdata != null)
				UserDataBox.Text = OGF_V.userdata.userdata;

			if (main_file && !OGF_V.IsDM)
			{
				LabelBroken.Text = "Broken type: " + OGF_V.BrokenType.ToString();
				LabelBroken.Visible = OGF_V.BrokenType > 0;
			}

			UpdateModelFormat();

			// View
			TabControl.Controls.Add(ViewPage);
		}

		private void CopyParams()
		{
			if (OGF_V.motion_refs != null)
			{
				OGF_V.motion_refs.refs.Clear();

				if (IsTextCorrect(MotionRefsBox.Text))
				{
					for (int i = 0; i < MotionRefsBox.Lines.Count(); i++)
					{
						if (IsTextCorrect(MotionRefsBox.Lines[i]))
							OGF_V.motion_refs.refs.Add(GetCorrectString(MotionRefsBox.Lines[i]));
					}
				}
			}

			if (OGF_V.userdata != null)
			{
				OGF_V.userdata.userdata = "";

				if (IsTextCorrect(UserDataBox.Text))
				{
					for (int i = 0; i < UserDataBox.Lines.Count(); i++)
					{
						string ext = i == UserDataBox.Lines.Count() - 1 ? "" : "\r\n";
						OGF_V.userdata.userdata += UserDataBox.Lines[i] + ext;
					}
				}
			}

			if (OGF_V.lod != null)
			{
				OGF_V.lod.lod_path = "";

				if (IsTextCorrect(LodPathBox.Text))
					OGF_V.lod.lod_path = GetCorrectString(LodPathBox.Text);
			}

			UpdateModelType();
		}

		private void WriteFile(string filename, byte[] data)
        {
			if (BkpCheckBox.Checked)
			{
				string backup_path = filename + ".bak";

				if (File.Exists(backup_path))
					File.Delete(backup_path);

				File.Copy(filename, backup_path);
			}

			using (var fileStream = new FileStream(filename, File.Exists(filename) ? FileMode.Truncate : FileMode.Create))
			{
				fileStream.Write(data, 0, data.Length);
				fileStream.Close();
            }
		}

		private bool CheckMeshes()
		{
			if (OGF_V == null) return false;

			foreach (var ch in OGF_V.childs)
			{
				if (!ch.to_delete)
					return true;
			}

            return false;
		}

		private void SaveFile(string filename)
		{
			file_bytes.Clear();

			if (Current_OGF == null) return;

			using (var fileStream = new BinaryReader(new MemoryStream(Current_OGF)))
			{
				byte[] temp;

                if (OGF_V.IsDM)
				{
                    fileStream.ReadBytes(OGF_V.childs[0].old_size);
					file_bytes.AddRange(Encoding.Default.GetBytes(OGF_V.childs[0].m_shader));
					file_bytes.Add(0);
					file_bytes.AddRange(Encoding.Default.GetBytes(OGF_V.childs[0].m_texture));
					file_bytes.Add(0);
                    byte[] dm_data = fileStream.ReadBytes((int)(fileStream.BaseStream.Length - fileStream.BaseStream.Position));
					file_bytes.AddRange(dm_data);
					WriteFile(filename, file_bytes.ToArray());
					return;
				}

				if (!OGF_V.IsStaticSingle())
				{
					if (OGF_V.BrokenType == 2)
					{
						OGF_V.Header.bb = new BBox();
                        OGF_V.Header.bs = new BSphere();
                    }

                    file_bytes.AddRange(OGF_V.Header.data());
				}

				if (OGF_V.description != null)
				{
					bool old_byte = OGF_V.description.four_byte;
					if (OGF_V.BrokenType > 0) // Если модель сломана, то восстанавливаем чанк с 8 байтными таймерами
						OGF_V.description.four_byte = false;

					file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_DESC));
					file_bytes.AddRange(BitConverter.GetBytes(OGF_V.description.data().Length));
					file_bytes.AddRange(OGF_V.description.data());

					OGF_V.description.four_byte = old_byte; // Восстанавливаем отображение колличества байтов у таймера
				}

				if (OGF_V.IsStaticSingle()) // Single mesh
				{
					file_bytes.AddRange(OGF_V.childs[0].data());
					fileStream.BaseStream.Position += OGF_V.childs[0].old_size;
                }
				else // Hierrarhy mesh
				{
					fileStream.ReadBytes((int)(OGF_V.pos - fileStream.BaseStream.Position));

					fileStream.ReadBytes(4);
					uint OldChildrenChunkSize = fileStream.ReadUInt32();
					fileStream.BaseStream.Position += OldChildrenChunkSize;

					uint ChildrenChunkSize = 0;
                    foreach (var ch in OGF_V.childs)
					{
						if (!ch.to_delete)
							ChildrenChunkSize += (uint)ch.data().Length + 8;
					}

					int ChildrenChunk = (OGF_V.Header.format_version == 4 ? (int)OGF.OGF4_CHILDREN : (int)OGF.OGF3_CHILDREN);
					file_bytes.AddRange(BitConverter.GetBytes(ChildrenChunk));
					file_bytes.AddRange(BitConverter.GetBytes(ChildrenChunkSize));

					int ChildChunk = 0;
					foreach (var ch in OGF_V.childs)
					{
						if (ch.to_delete) continue;

                        file_bytes.AddRange(BitConverter.GetBytes(ChildChunk));
                        file_bytes.AddRange(BitConverter.GetBytes(ch.data().Length));

                        file_bytes.AddRange(ch.data());
                        ChildChunk++;
                    }
				}

				if (OGF_V.IsSkeleton())
                {
					if (OGF_V.bones != null)
					{
						if (OGF_V.BrokenType == 0 && OGF_V.bones.pos > 0 && (OGF_V.bones.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
						{
							temp = fileStream.ReadBytes((int)(OGF_V.bones.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF_S_BONE_NAMES));
						file_bytes.AddRange(BitConverter.GetBytes(OGF_V.bones.data(false).Length));
						file_bytes.AddRange(OGF_V.bones.data(false));

						fileStream.ReadBytes(OGF_V.bones.old_size + 8);
					}

					if (OGF_V.ikdata != null)
					{
						if (OGF_V.BrokenType == 0 && OGF_V.ikdata.pos > 0 && (OGF_V.ikdata.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
						{
							temp = fileStream.ReadBytes((int)(OGF_V.ikdata.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						uint IKDataChunk = 0;

						switch (OGF_V.ikdata.chunk_version)
						{
							case 4:
								IKDataChunk = (OGF_V.Header.format_version == 4 ? (uint)OGF.OGF4_S_IKDATA : (uint)OGF.OGF3_S_IKDATA_2);
								break;
							case 3:
								IKDataChunk = (uint)OGF.OGF3_S_IKDATA;
								break;
							case 2:
								IKDataChunk = (uint)OGF.OGF3_S_IKDATA_0;
								break;
						}

						file_bytes.AddRange(BitConverter.GetBytes(IKDataChunk));
						file_bytes.AddRange(BitConverter.GetBytes(OGF_V.ikdata.data().Length));
						file_bytes.AddRange(OGF_V.ikdata.data());

						fileStream.ReadBytes(OGF_V.ikdata.old_size + 8);
					}

					if (OGF_V.userdata != null)
					{
						if (OGF_V.userdata.pos > 0 && (OGF_V.userdata.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
						{
							temp = fileStream.ReadBytes((int)(OGF_V.userdata.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.userdata.userdata != "") // Пишем если есть что писать
						{
							uint UserDataChunk = (OGF_V.Header.format_version == 4 ? (uint)OGF.OGF4_S_USERDATA : (uint)OGF.OGF3_S_USERDATA);
							file_bytes.AddRange(BitConverter.GetBytes(UserDataChunk));
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.userdata.data(OGF_V.Header.format_version).Length));
							file_bytes.AddRange(OGF_V.userdata.data(OGF_V.Header.format_version));
						}

						if (OGF_V.userdata.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
							fileStream.ReadBytes(OGF_V.userdata.old_size + 8);
					}

					if (OGF_V.lod != null && OGF_V.Header.format_version == 4) // Стринг лод только у релизных OGF
					{
						if (OGF_V.lod.pos > 0 && (OGF_V.lod.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
						{
							temp = fileStream.ReadBytes((int)(OGF_V.lod.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.lod.lod_path != "") // Пишем если есть что писать
						{
							file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_LODS));
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.lod.data().Length));
							file_bytes.AddRange(OGF_V.lod.data());
						}

						if (OGF_V.lod.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
							fileStream.ReadBytes(OGF_V.lod.old_size + 8);
					}

					bool refs_created = false;
					if (OGF_V.motion_refs != null)
					{
						if (OGF_V.motion_refs.pos > 0 && (OGF_V.motion_refs.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
						{
							temp = fileStream.ReadBytes((int)(OGF_V.motion_refs.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.motion_refs.refs.Count > 0) // Пишем если есть что писать
						{
							refs_created = true;

							if (!OGF_V.motion_refs.soc)
								file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_MOTION_REFS2));
							else
							{
								uint RefsChunk = (OGF_V.Header.format_version == 4 ? (uint)OGF.OGF4_S_MOTION_REFS : (uint)OGF.OGF3_S_MOTION_REFS);
								file_bytes.AddRange(BitConverter.GetBytes(RefsChunk));
							}
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.motion_refs.data(OGF_V.motion_refs.soc).Length));

							file_bytes.AddRange(OGF_V.motion_refs.data(OGF_V.motion_refs.soc));
						}

						if (OGF_V.motion_refs.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
							fileStream.ReadBytes(OGF_V.motion_refs.old_size + 8);
					}

					if (OGF_V.motions.data() != null && !refs_created)
						file_bytes.AddRange(OGF_V.motions.data());
				}
				else
				{
					temp = fileStream.ReadBytes((int)(fileStream.BaseStream.Length - fileStream.BaseStream.Position));
					file_bytes.AddRange(temp);
				}
			}

			WriteFile(filename, file_bytes.ToArray());
		}

		private bool OpenFile(string filename, ref OGF_Children OGF_C, ref byte[] Cur_OGF)
		{
			var xr_loader = new XRayLoader();

			string format = Path.GetExtension(filename);

			OGF_C = new OGF_Children();

			if (format == ".dm")
				OGF_C.IsDM = true;

			Cur_OGF = File.ReadAllBytes(filename);

			using (var r = new BinaryReader(new MemoryStream(Cur_OGF)))
			{
				xr_loader.SetStream(r.BaseStream);

				if (OGF_C.IsDM)
                {
					string shader = xr_loader.read_stringZ();
					string texture = xr_loader.read_stringZ();
					xr_loader.ReadUInt32();
					xr_loader.ReadFloat();
					xr_loader.ReadFloat();
					OGF_Child chld = new OGF_Child();
					chld.old_size = shader.Length + texture.Length + 2;
					chld.m_texture = texture;
					chld.m_shader = shader;

                    uint verts = xr_loader.ReadUInt32();
					uint faces = xr_loader.ReadUInt32() / 3;

					for (int i = 0; i < verts; i++)
					{
						SSkelVert Vert = new SSkelVert();
						Vert.offs = xr_loader.ReadVector();
						Vert.uv = xr_loader.ReadVector2();
						chld.Vertices.Add(Vert);
					}

					for (int i = 0; i < faces; i++)
					{
						SSkelFace Face = new SSkelFace();
						Face.v[0] = (ushort)xr_loader.ReadUInt16();
						Face.v[1] = (ushort)xr_loader.ReadUInt16();
						Face.v[2] = (ushort)xr_loader.ReadUInt16();
						chld.Faces.Add(Face);
					}

					OGF_C.childs.Add(chld);
					return true;
				}

				if (!xr_loader.find_chunk((int)OGF.OGF_HEADER, false, true))
				{
					MessageBox.Show("Unsupported OGF format! Can't find header chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				else
				{
					OGF_C.Header.Load(xr_loader);

					if (OGF_C.Header.format_version < 3)
                    {
						MessageBox.Show($"Unsupported OGF version: {OGF_C.Header.format_version}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}

				int DescChunk = (OGF_C.Header.format_version == 4 ? (int)OGF.OGF4_S_DESC : (int)OGF.OGF3_S_DESC);
				uint DescriptionSize = xr_loader.find_chunkSize(DescChunk, false, true);
				if (DescriptionSize > 0)
				{
					OGF_C.description = new Description();
					OGF_C.description.Load(xr_loader, DescriptionSize);
				}

				int ChildChunk = (OGF_C.Header.format_version == 4 ? (int)OGF.OGF4_CHILDREN : (int)OGF.OGF3_CHILDREN);
				bool bFindChunk = xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(ChildChunk, false, true));

				OGF_C.pos = xr_loader.chunk_pos;

				int id = 0;

				// Childs
				if (bFindChunk)
				{
					while (true)
					{
						if (!xr_loader.find_chunk(id)) break;

						Stream temp = xr_loader.reader.BaseStream;

						if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

                        OGF_Child Child = new OGF_Child();
						if (!Child.Load(xr_loader, OGF_C.Header.format_version))
							break;

						OGF_C.childs.Add(Child);

						id++;
						xr_loader.SetStream(temp);
					}

					xr_loader.SetStream(r.BaseStream);
				}
				else
                {
                    OGF_Child Child = new OGF_Child();
                    if (Child.Load(xr_loader, OGF_C.Header.format_version))
						OGF_C.childs.Add(Child);
                }

				if (OGF_C.childs.Count == 0)
				{
					MessageBox.Show("Unsupported OGF format! Can't find children chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (OGF_C.IsSkeleton())
				{
					// Bones
					if (!xr_loader.find_chunk((int)OGF.OGF_S_BONE_NAMES, false, true))
					{
						MessageBox.Show("Unsupported OGF format! Can't find bones chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else
					{
						OGF_C.bones = new BoneData();
						OGF_C.bones.pos = xr_loader.chunk_pos;

						if (xr_loader.chunk_pos < OGF_C.pos)
							OGF_C.BrokenType = 2;

						uint count = xr_loader.ReadUInt32();
						OGF_C.bones.old_size += 4;

						for (; count != 0; count--)
						{
							string bone_name = xr_loader.read_stringZ();
							string parent_name = xr_loader.read_stringZ();
							byte[] obb = xr_loader.ReadBytes(60);    // Fobb
							OGF_C.bones.bone_names.Add(bone_name);
							OGF_C.bones.parent_bone_names.Add(parent_name);
							OGF_C.bones.fobb.Add(obb);

							OGF_C.bones.old_size += bone_name.Length + 1 + parent_name.Length + 1 + 60;
						}

						for (int i = 0; i < OGF_C.bones.bone_names.Count; i++)
						{
							List<int> childs = new List<int>();
							for (int j = 0; j < OGF_C.bones.parent_bone_names.Count; j++)
							{
								if (OGF_C.bones.parent_bone_names[j] == OGF_C.bones.bone_names[i])
								{
									childs.Add(j);
								}
							}
							OGF_C.bones.bone_childs.Add(childs);
						}
					}

					// Ik Data
					byte IKDataVers = 0;

					int IKDataChunkRelease = (OGF_C.Header.format_version == 4 ? (int)OGF.OGF4_S_IKDATA : (int)OGF.OGF3_S_IKDATA_2);
					bool IKDataChunkFind = xr_loader.find_chunk(IKDataChunkRelease, false, true);

					if (IKDataChunkFind) // Load Release chunk
					{
						IKDataVers = 4;
						goto load_ik_data;
					}

					IKDataChunkFind = OGF_C.Header.format_version == 3 && xr_loader.find_chunk((int)OGF.OGF3_S_IKDATA, false, true);

					if (IKDataChunkFind) // Load Pre Release chunk
					{
						IKDataVers = 3;
						goto load_ik_data;
					}

					IKDataChunkFind = OGF_C.Header.format_version == 3 && xr_loader.find_chunk((int)OGF.OGF3_S_IKDATA_0, false, true);

					if (IKDataChunkFind) // Load Builds chunk
					{
						IKDataVers = 2;
						goto load_ik_data;
					}

					goto skip_ik_data;

load_ik_data:
					OGF_C.ikdata = new IK_Data();
					OGF_C.ikdata.pos = xr_loader.chunk_pos;
					OGF_C.ikdata.chunk_version = IKDataVers;

					for (int i = 0; i < OGF_C.bones.bone_names.Count; i++)
					{
						List<byte[]> bytes_1 = new List<byte[]>();

						byte[] temp_byte;
						uint version = 0;

						if (IKDataVers == 4)
						{
							version = xr_loader.ReadUInt32();
							OGF_C.ikdata.old_size += 4;
						}

						string gmtl_name = xr_loader.read_stringZ();

						temp_byte = xr_loader.ReadBytes(112);   // struct SBoneShape
						bytes_1.Add(temp_byte);
						OGF_C.ikdata.old_size += gmtl_name.Length + 1 + 112;

						int ImportBytes = ((IKDataVers == 4) ? 76 : ((IKDataVers == 3) ? 72 : 60));

						temp_byte = xr_loader.ReadBytes(ImportBytes); // Import
						bytes_1.Add(temp_byte);
						OGF_C.ikdata.old_size += ImportBytes;

						float[] rotation = new float[3];
						rotation = xr_loader.ReadVector();

						float[] position = new float[3];
						position = xr_loader.ReadVector();

						float mass = xr_loader.ReadFloat();

						float[] center = new float[3];
						center = xr_loader.ReadVector();

						OGF_C.ikdata.old_size += 12 + 12 + 4 + 12;

						OGF_C.ikdata.materials.Add(gmtl_name);
						OGF_C.ikdata.mass.Add(mass);
						OGF_C.ikdata.version.Add(version);
						OGF_C.ikdata.center_mass.Add(center);
						OGF_C.ikdata.bytes_1.Add(bytes_1);
						OGF_C.ikdata.position.Add(position);
						OGF_C.ikdata.rotation.Add(rotation);
					}

skip_ik_data:

					if (IKDataVers == 0 && OGF_C.Header.format_version == 4) // Chunk not find, exit if Release OGF
					{
						MessageBox.Show("Unsupported OGF format! Can't find ik data chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					// Userdata
					int UserDataChunk = (OGF_C.Header.format_version == 4 ? (int)OGF.OGF4_S_USERDATA : (int)OGF.OGF3_S_USERDATA);
					uint UserDataSize = xr_loader.find_chunkSize(UserDataChunk, false, true);
					if (UserDataSize > 0)
					{
						OGF_C.userdata = new UserData();
						OGF_C.userdata.pos = xr_loader.chunk_pos;

						long UserdataStreamPos = r.BaseStream.Position;
						OGF_C.userdata.userdata = xr_loader.read_stringZ();
						OGF_C.userdata.old_size = OGF_C.userdata.userdata.Length + 1;

						if (OGF_C.userdata.userdata.Length + 1 != UserDataSize)
                        {
							r.BaseStream.Position = UserdataStreamPos;
							OGF_C.userdata.userdata = xr_loader.read_stringSize(UserDataSize);
							OGF_C.userdata.old_size = OGF_C.userdata.userdata.Length;
						}
					}

					// Lod ref
					if (OGF_C.Header.format_version == 4 && xr_loader.find_chunk((int)OGF.OGF4_S_LODS, false, true))
					{
						OGF_C.lod = new Lod();
						OGF_C.lod.pos = xr_loader.chunk_pos;
						OGF_C.lod.lod_path = xr_loader.read_stringData(ref OGF_C.lod.data_str);
						OGF_C.lod.old_size = OGF_C.lod.lod_path.Length + (OGF_C.lod.data_str ? 2 : 1);
					}

					// Motion Refs
					int RefsChunk = (OGF_C.Header.format_version == 4 ? (int)OGF.OGF4_S_MOTION_REFS : (int)OGF.OGF3_S_MOTION_REFS);
					bool StringRefs = xr_loader.find_chunk(RefsChunk, false, true);

					if (StringRefs || OGF_C.Header.format_version == 4 && xr_loader.find_chunk((int)OGF.OGF4_S_MOTION_REFS2, false, true))
					{
						OGF_C.motion_refs = new MotionRefs();
						OGF_C.motion_refs.pos = xr_loader.chunk_pos;
						OGF_C.motion_refs.refs = new List<string>();

						if (StringRefs)
						{
							OGF_C.motion_refs.soc = true;

							string motions = xr_loader.read_stringZ();
							string motion = "";
							List<string> refs = new List<string>();

							for (int i = 0; i < motions.Length; i++)
							{
								if (motions[i] != ',')
									motion += motions[i];
								else
								{
									refs.Add(motion);
									motion = "";
								}

							}

							if (motion != "")
								refs.Add(motion);

							OGF_C.motion_refs.refs.AddRange(refs);
							OGF_C.motion_refs.old_size = motions.Length + 1;
						}
						else
						{
							uint count = xr_loader.ReadUInt32();

							OGF_C.motion_refs.old_size = 4;

							for (int i = 0; i < count; i++)
							{
								string refs = xr_loader.read_stringZ();
								OGF_C.motion_refs.refs.Add(refs);
								OGF_C.motion_refs.old_size += refs.Length + 1;
							}
						}
					}

					//Motions
					if (xr_loader.find_chunk((int)OGF.OGF_S_MOTIONS, false, true))
					{
						xr_loader.reader.BaseStream.Position -= 8;
						byte[] OMF = xr_loader.ReadBytes((int)xr_loader.reader.BaseStream.Length - (int)xr_loader.reader.BaseStream.Position);
						OGF_C.motions.SetData(OMF);
                    }
				}
			}
			return true;
		}

		private void SaveAsObj(string filename, float lod)
		{
			using (ObjWriter = File.CreateText(filename))
			{
				uint v_offs = 0;
				uint childs = 0;

				string mtl_name = Path.ChangeExtension(filename, ".mtl");
				SaveMtl(mtl_name);

				try
				{
					ObjWriter.WriteLine("# This file uses meters as units for non-parametric coordinates.");
					ObjWriter.WriteLine("mtllib " + Path.GetFileName(mtl_name));
					foreach (var ch in OGF_V.childs)
					{
						if (ch.to_delete) continue;

						ObjWriter.WriteLine($"g {childs}");
						ObjWriter.WriteLine($"usemtl {Path.GetFileName(ch.m_texture)}");
						childs++;

						for (int i = 0; i < ch.Vertices.Count; i++)
						{
							ObjWriter.WriteLine($"v {vPUSH(MirrorZ_transform(ch.Vertices[i].offs))}");
						}

						for (int i = 0; i < ch.Vertices.Count; i++)
						{
							float x = ch.Vertices[i].uv[0];
							float y = Math.Abs(1.0f - ch.Vertices[i].uv[1]);
							ObjWriter.WriteLine($"vt {x.ToString("0.000000")} {y.ToString("0.000000")}");
						}

						for (int i = 0; i < ch.Vertices.Count; i++)
						{
							ObjWriter.WriteLine($"vn {vPUSH(MirrorZ_transform(ch.Vertices[i].norm))}");
						}

						for (int i = 0; i < ch.Vertices.Count; i++)
						{
							ObjWriter.WriteLine($"vg {vPUSH(MirrorZ_transform(ch.Vertices[i].tang))}");
						}

						for (int i = 0; i < ch.Vertices.Count; i++)
						{
							ObjWriter.WriteLine($"vb {vPUSH(MirrorZ_transform(ch.Vertices[i].binorm))}");
						}

						foreach (var f_it in ch.Faces_SWI(lod))
						{
							string tmp = $"f {v_offs+f_it.v[2]+1}/{v_offs+f_it.v[2]+1}/{v_offs+f_it.v[2]+1} {v_offs+f_it.v[1]+1}/{v_offs+f_it.v[1]+1}/{v_offs+f_it.v[1]+1} {v_offs+f_it.v[0]+1}/{v_offs+f_it.v[0]+1}/{v_offs+f_it.v[0]+1}";
							ObjWriter.WriteLine(tmp);
						}
						v_offs += (uint)ch.Vertices.Count;
					}
					ObjWriter.Close();
					ObjWriter = null;
				}
				catch(Exception) { }
			}
		}

		private void SaveMtl(string filename)
        {
			using (StreamWriter writer = File.CreateText(filename))
			{
				foreach (var ch in OGF_V.childs)
				{
					if (ch.to_delete) continue;

					writer.WriteLine("newmtl " + Path.GetFileName(ch.m_texture));
					writer.WriteLine("Ka  0 0 0");
					writer.WriteLine("Kd  1 1 1");
					writer.WriteLine("Ks  0 0 0");
					writer.WriteLine("map_Kd " + Path.GetFileName(ch.m_texture) + ".png\n");
				}
				writer.Close();
			}
		}

		private float[] MirrorZ_transform(float[] vec)
        {
			float[] dest = new float[3];
			dest[0] = vec[0];
			dest[1] = vec[1];
			dest[2] = -vec[2];
			return dest;
		}

        private string vPUSH(float[] vec)
        {
			return vec[0].ToString("0.000000") + " " + vec[1].ToString("0.000000") + " " + vec[2].ToString("0.000000");
        }

		private void AddBone(string bone, string parent_bone, int pos)
		{
			if (OGF_V != null && OGF_V.IsSkeleton())
			{
				OGF_V.bones.bone_names.Insert(pos, bone);
				OGF_V.bones.parent_bone_names.Insert(pos, parent_bone);

				// Create null OBB
				List<byte> obb = new List<byte>();
				for (int i = 0; i < 60; i++)
					obb.Add(0);

				OGF_V.bones.fobb.Insert(pos, obb.ToArray());

				// Create null Bone Shape
				List<byte[]> shape = new List<byte[]>();
				for (int i = 0; i < 112 + 76; i++)
				{
					byte[] one = { 0 };
					shape.Add(one);
				}

                OGF_V.ikdata.materials.Insert(pos, "default_object");
                OGF_V.ikdata.mass.Insert(pos, 10.0f);
                OGF_V.ikdata.version.Insert(pos, 1);
                OGF_V.ikdata.center_mass.Insert(pos, new float[3]);

                OGF_V.ikdata.bytes_1.Insert(pos, shape);
                OGF_V.ikdata.position.Insert(pos, new float[3]);
                OGF_V.ikdata.rotation.Insert(pos, new float[3]);
            }
		}

		private void WriteIkData(string filename)
		{
			if (OGF_V != null && OGF_V.IsSkeleton() && OGF_V.ikdata != null)
			{
                List<byte> data = new List<byte>();

                data.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_IKDATA));
                data.AddRange(BitConverter.GetBytes(4 * 6 * OGF_V.ikdata.position.Count));

                for (int i = 0; i < OGF_V.ikdata.position.Count; i++)
				{
					data.AddRange(BitConverter.GetBytes(OGF_V.ikdata.rotation[i][0]));
					data.AddRange(BitConverter.GetBytes(OGF_V.ikdata.rotation[i][1]));
					data.AddRange(BitConverter.GetBytes(OGF_V.ikdata.rotation[i][2]));

					data.AddRange(BitConverter.GetBytes(OGF_V.ikdata.position[i][0]));
					data.AddRange(BitConverter.GetBytes(OGF_V.ikdata.position[i][1]));
					data.AddRange(BitConverter.GetBytes(OGF_V.ikdata.position[i][2]));
				}

                WriteFile(filename, data.ToArray());
            }
        }

        private void RemoveBone(string bone)
        {
			if (OGF_V != null && OGF_V.IsSkeleton() && OGF_V.bones != null)
				RemoveBone(OGF_V.bones.GetBoneID(bone));
        }

        private void RemoveBone(int bone)
        {
            if (OGF_V != null && OGF_V.IsSkeleton())
            {
                OGF_V.bones.bone_names.RemoveAt(bone);
                OGF_V.bones.parent_bone_names.RemoveAt(bone);
                OGF_V.bones.fobb.RemoveAt(bone);

                OGF_V.ikdata.materials.RemoveAt(bone);
                OGF_V.ikdata.mass.RemoveAt(bone);
                OGF_V.ikdata.version.RemoveAt(bone);
                OGF_V.ikdata.center_mass.RemoveAt(bone);

                OGF_V.ikdata.bytes_1.RemoveAt(bone);
                OGF_V.ikdata.position.RemoveAt(bone);
                OGF_V.ikdata.rotation.RemoveAt(bone);
            }
        }

        private void ChangeParent(string old, string _new)
        {
            if (OGF_V != null && OGF_V.IsSkeleton())
            {
                for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
				{
                    if (OGF_V.bones.parent_bone_names[i] == old)
						OGF_V.bones.parent_bone_names[i] = _new;
                }
            }
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
					OGF_V.childs[idx].to_delete = !OGF_V.childs[idx].to_delete;

					if (OGF_V.childs[idx].to_delete)
                    {
						curBox.Text = "Return Mesh";
						curBox.BackColor = Color.FromArgb(255, 255, 128, 128);
					}
					else
                    {
						curBox.Text = "Delete Mesh";
						curBox.BackColor = SystemColors.Control;
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
				case "textureBox": OGF_V.childs[idx].m_texture = curBox.Text; break;
				case "shaderBox": OGF_V.childs[idx].m_shader = curBox.Text; break;
			}
		}

		private void TextBoxBonesFilter(object sender, EventArgs e)
		{
			Control curControl = sender as Control;

			string currentField = curControl.Name.ToString().Split('_')[0];
			int idx = Convert.ToInt32(curControl.Name.ToString().Split('_')[1]);

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

                            try
							{
								Convert.ToSingle(curControl.Text);
							}
							catch (Exception)
							{
								switch (currentField)
								{
									case "MassBox": curControl.Text = OGF_V.ikdata.mass[idx].ToString(); break;
									case "CenterBoxX": curControl.Text = OGF_V.ikdata.center_mass[idx][0].ToString(); break;
									case "CenterBoxY": curControl.Text = OGF_V.ikdata.center_mass[idx][1].ToString(); break;
									case "CenterBoxZ": curControl.Text = OGF_V.ikdata.center_mass[idx][2].ToString(); break;
									case "PositionX": curControl.Text = OGF_V.ikdata.position[idx][0].ToString(); break;
									case "PositionY": curControl.Text = OGF_V.ikdata.position[idx][1].ToString(); break;
									case "PositionZ": curControl.Text = OGF_V.ikdata.position[idx][2].ToString(); break;
									case "RotationX": curControl.Text = OGF_V.ikdata.rotation[idx][0].ToString(); break;
									case "RotationY": curControl.Text = OGF_V.ikdata.rotation[idx][1].ToString(); break;
									case "RotationZ": curControl.Text = OGF_V.ikdata.rotation[idx][2].ToString(); break;
								}

								if (curBox.SelectionStart < 1)
									curBox.SelectionStart = curControl.Text.Length;

								curBox.SelectionStart = temp - 1;
							}
						}
					}
					break;
			}

			switch (currentField)
			{
				case "boneBox":
					{
						OGF_V.bones.bone_names[idx] = curControl.Text;

						for (int j = 0; j < OGF_V.bones.bone_childs[idx].Count; j++)
                        {
							int child_id = OGF_V.bones.bone_childs[idx][j];
							var MainGroup = BoneParamsPage.Controls["BoneGrpBox_" + child_id.ToString()];
							OGF_V.bones.parent_bone_names[child_id] = curControl.Text;
                            MainGroup.Controls["ParentboneBox_" + child_id.ToString()].Text = OGF_V.bones.parent_bone_names[child_id];
						}

						BoneNamesBox.Clear();
						BoneNamesBox.Text += $"Bones count : {OGF_V.bones.bone_names.Count}\n\n";

						for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
						{
							BoneNamesBox.Text += $"{i + 1}. {OGF_V.bones.bone_names[i]}";
							if (i != OGF_V.bones.bone_names.Count - 1)
								BoneNamesBox.Text += "\n";
						}
					}
					break;
				case "MaterialBox": OGF_V.ikdata.materials[idx] = curControl.Text; break;
				case "MassBox": OGF_V.ikdata.mass[idx] = Convert.ToSingle(curControl.Text); break;
				case "CenterBoxX": OGF_V.ikdata.center_mass[idx][0] = Convert.ToSingle(curControl.Text); break;
				case "CenterBoxY": OGF_V.ikdata.center_mass[idx][1] = Convert.ToSingle(curControl.Text); break;
				case "CenterBoxZ": OGF_V.ikdata.center_mass[idx][2] = Convert.ToSingle(curControl.Text); break;
				case "PositionX": OGF_V.ikdata.position[idx][0] = Convert.ToSingle(curControl.Text); break;
				case "PositionY": OGF_V.ikdata.position[idx][1] = Convert.ToSingle(curControl.Text); break;
				case "PositionZ": OGF_V.ikdata.position[idx][2] = Convert.ToSingle(curControl.Text); break;
				case "RotationX": OGF_V.ikdata.rotation[idx][0] = Convert.ToSingle(curControl.Text); break;
				case "RotationY": OGF_V.ikdata.rotation[idx][1] = Convert.ToSingle(curControl.Text); break;
				case "RotationZ": OGF_V.ikdata.rotation[idx][2] = Convert.ToSingle(curControl.Text); break;
			}

			bKeyIsDown = false;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (FILE_NAME == "") return;

			if (!CheckMeshes())
			{
                AutoClosingMessageBox.Show("Can't save model without meshes!", "", 1500, MessageBoxIcon.Error);
				return;
            }

			CopyParams();
			SaveFile(FILE_NAME);
			AutoClosingMessageBox.Show(OGF_V.BrokenType > 0 ? "Repaired and Saved!" : "Saved!", "", OGF_V.BrokenType > 0 ? 700 : 500, MessageBoxIcon.Information);
		}

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
			OpenOGF_DmDialog.FileName = "";
			DialogResult res = OpenOGF_DmDialog.ShowDialog();

			if (res == DialogResult.OK)
			{
				Clear(false);
				if (OpenFile(OpenOGF_DmDialog.FileName, ref OGF_V, ref Current_OGF))
				{
					OpenOGF_DmDialog.InitialDirectory = "";
					FILE_NAME = OpenOGF_DmDialog.FileName;
					AfterLoad(true);
				}
			}
		}

        private void oGFInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

			OgfInfo Info = new OgfInfo(OGF_V, IsTextCorrect(MotionRefsBox.Text), CurrentLod);
            Info.ShowDialog();

			if (Info.res && OGF_V.description != null)
			{
				OGF_V.description.m_source = Info.descr.m_source;
				OGF_V.description.m_export_tool = Info.descr.m_export_tool;
				OGF_V.description.m_owner_name = Info.descr.m_owner_name;
				OGF_V.description.m_export_modif_name_tool = Info.descr.m_export_modif_name_tool;
				OGF_V.description.m_creation_time = Info.descr.m_creation_time;
				OGF_V.description.m_export_time = Info.descr.m_export_time;
				OGF_V.description.m_modified_time = Info.descr.m_modified_time;
				OGF_V.description.four_byte = Info.descr.four_byte;
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

            if (OGF_V.IsDM)
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

            if (File.Exists(filename) && filename != FILE_NAME)
                File.Delete(filename);

            switch (format)
			{
				case ExportFormat.OGF:
                    if (filename != FILE_NAME)
                        File.Copy(FILE_NAME, filename);

                    CopyParams();
                    SaveFile(filename);
                    break;
				case ExportFormat.Obj:
                    SaveAsObj(filename, CurrentLod);
                    break;
                case ExportFormat.Object:
                    string ext = OGF_V.IsDM ? ".dm" : ".ogf";

                    if (File.Exists(filename + ext))
                        File.Delete(filename + ext);

                    File.Copy(FILE_NAME, filename + ext);

                    CopyParams();
                    SaveFile(filename + ext);

                    RunConverter(filename + ext, filename, OGF_V.IsDM ? 2 : 0, 0);

                    if (File.Exists(filename + ext))
						File.Delete(filename + ext);
                    break;
				case ExportFormat.OMF:
					using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate))
					{
						fileStream.Write(OGF_V.motions.data(), 0, OGF_V.motions.data().Length);
						fileStream.Close();
                    }
                    break;
                case ExportFormat.Bones:
                    RunConverter(FILE_NAME, filename, 0, 1);
                    break;
                case ExportFormat.Skl:
                    RunConverter(FILE_NAME, filename, 0, 2);
                    break;
                case ExportFormat.Skls:
                    RunConverter(FILE_NAME, filename, 0, 3);
                    break;
            }

			string Text = (OGF_V.BrokenType > 0 ? "Repaired and " : "") + (format == ExportFormat.OGF ? "Saved!" : "Exported!");
            AutoClosingMessageBox.Show(Text, "", OGF_V.BrokenType > 0 ? 700 : 500, MessageBoxIcon.Information);
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
			if (OGF_V.userdata == null)
				OGF_V.userdata = new UserData();
		}

        private void CreateMotionRefsButton_Click(object sender, EventArgs e)
        {
			if (OGF_V.motions.data() == null || OGF_V.motions.data() != null && MessageBox.Show("New motion refs chunk will remove built-in motions, continue?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				// Чистим все связанное со встроенными анимами
				MotionBox.Clear();
				MotionBox.Visible = false;
				AppendOMFButton.Visible = true;
				OGF_V.motions.SetData(null);

				// Обновляем тип модели
				UpdateModelType();
				UpdateModelFormat();

				// Обновляем визуал интерфейса моушн рефов
				CreateMotionRefsButton.Visible = false; 
				MotionRefsBox.Visible = true;
				MotionRefsBox.Clear();

				if (OGF_V.motion_refs == null)
					OGF_V.motion_refs = new MotionRefs();
			}
		}

		private void CreateLodButton_Click(object sender, EventArgs e)
		{
			CreateLodButton.Visible = false;
			LodPathBox.Clear();
			if (OGF_V.lod == null)
				OGF_V.lod = new Lod();
		}

		private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (FILE_NAME == "") return;

			string cur_fname = FILE_NAME;
			Clear(false);
			if (OpenFile(cur_fname, ref OGF_V, ref Current_OGF))
			{
				FILE_NAME = cur_fname;
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
						InitViewPort();
						ViewPortItemVisible = true;
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
                fileStream.Write(OGF_V.motions.data(), 0, OGF_V.motions.data().Length);
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
            OGF_V.motions.SetData(null);
            MotionBox.Clear();
            UpdateModelType();
            UpdateModelFormat();
        }

        private void AppendOMFButton_Click(object sender, EventArgs e)
        {
			if (!IsTextCorrect(MotionRefsBox.Text) && (OGF_V.motion_refs == null || OGF_V.motion_refs.refs.Count() == 0) || (IsTextCorrect(MotionRefsBox.Text) || OGF_V.motion_refs != null && OGF_V.motion_refs.refs.Count() > 0) && MessageBox.Show("Build-in motions will remove motion refs, continue?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				OpenOMFDialog.ShowDialog();
        }

		private void AppendMotion(object sender, CancelEventArgs e)
		{
			if (sender != null)
				OpenOMFDialog.InitialDirectory = "";

            byte[] OpenedOmf = File.ReadAllBytes(OpenOMFDialog.FileName);

			if (OGF_V.motions.SetData(OpenedOmf))
			{
                // Апдейтим визуал встроенных анимаций
                AppendOMFButton.Visible = false;
                MotionBox.Visible = true;

                // Чистим встроенные рефы, интерфейс почистится сам при активации вкладки
                MotionRefsBox.Clear();
                if (OGF_V.motion_refs != null)
                    OGF_V.motion_refs.refs.Clear();

                MotionBox.Text = OGF_V.motions.ToString();
            }

			UpdateModelType();
			UpdateModelFormat();
		}

		private void importDataFromModelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenOGFDialog.FileName = "";
			if (OpenOGFDialog.ShowDialog() == DialogResult.OK)
			{
				bool Update = false;

				OGF_Children SecondOgf = null;
				byte[] SecondOgfByte = null;
				OpenFile(OpenOGFDialog.FileName, ref SecondOgf, ref SecondOgfByte);

				ImportParams Params = new ImportParams(OGF_V, SecondOgf);

                Params.ShowDialog();

                if (Params.Textures)
				{
					for (int i = 0; i < OGF_V.childs.Count; i++)
					{
						OGF_V.childs[i].m_texture = SecondOgf.childs[i].m_texture;
						OGF_V.childs[i].m_shader = SecondOgf.childs[i].m_shader;
					}

					Update = true;
                }

				if (Params.Userdata)
				{
					if (OGF_V.userdata == null)
						OGF_V.userdata = new UserData();

					OGF_V.userdata.userdata = SecondOgf.userdata.userdata;

					Update = true;
				}
				else if (Params.Remove && OGF_V.userdata != null)
				{
					OGF_V.userdata.userdata = "";
                    Update = true;
                }

				if (Params.Lod)
				{
					if (OGF_V.lod == null)
						OGF_V.lod = new Lod();

					OGF_V.lod.lod_path = SecondOgf.lod.lod_path;

					Update = true;
				}
				else if (Params.Remove && OGF_V.lod != null)
				{
					OGF_V.lod.lod_path = "";
                    Update = true;
                }

				if (Params.MotionRefs)
				{
					if (OGF_V.motion_refs == null)
						OGF_V.motion_refs = new MotionRefs();

					OGF_V.motion_refs.refs = SecondOgf.motion_refs.refs;

					Update = true;
				}
				else if (Params.Remove && OGF_V.motion_refs != null)
				{
					OGF_V.motion_refs.refs.Clear();
					Update = true;
				}

				if (Params.Motions)
				{
					OGF_V.motions.SetData(SecondOgf.motions.data());

					if (OGF_V.motion_refs != null)
						OGF_V.motion_refs.refs.Clear();

					Update = true;
				}
				else if (Params.Remove)
				{
					OGF_V.motions.SetData(null);
                    Update = true;
                }

                if (Params.Materials)
				{
					for (int i = 0; i < OGF_V.ikdata.materials.Count; i++)
					{
						OGF_V.ikdata.materials[i] = SecondOgf.ikdata.materials[i];
						OGF_V.ikdata.mass[i] = SecondOgf.ikdata.mass[i];
					}

                    Update = true;
                }

				if (Update)
				{
                    Clear(true);
					AfterLoad(false);
					AutoClosingMessageBox.Show("OGF Params changed!", "", 1000, MessageBoxIcon.Information);
				}
				else
                {
					AutoClosingMessageBox.Show("OGF Params don't changed!", "", 1000, MessageBoxIcon.Warning);
				}
			}
		}

		private void ChangeModelFormat(object sender, EventArgs e)
		{
			if (OGF_V != null)
			{
				if (OGF_V.Header.format_version != 4)
				{
					MessageBox.Show("Can't convert model. Unsupported OGF version: " + OGF_V.Header.format_version.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				OGF_V.IsCopModel = !OGF_V.IsCopModel;

				if (OGF_V.IsCopModel)
                {
					if (OGF_V.motion_refs != null)
						OGF_V.motion_refs.soc = false;

					foreach (var ch in OGF_V.childs)
					{
						if (ch.links >= 0x12071980)
							ch.links /= 0x12071980;
					}
				}
				else
                {
					uint links = 0;

                    foreach (var ch in OGF_V.childs)
                        links = Math.Max(links, ch.LinksCount());

                    if (links > 2 && MessageBox.Show("Model has more than 2 links. After converting to SoC model will lose influence data, continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        OGF_V.IsCopModel = !OGF_V.IsCopModel;
                        return;
                    }

					foreach (var ch in OGF_V.childs)
					{
						if (ch.LinksCount() > 2)
							ch.SetLinks(1);
					}

                    if (OGF_V.motions.Anims != null)
					{
						foreach (var Anim in OGF_V.motions.Anims)
						{
                            bool key16bit = (Anim.flags & (int)MotionKeyFlags.flTKey16IsBit) == (int)MotionKeyFlags.flTKey16IsBit;
                            bool keynocompressbit = (Anim.flags & (int)MotionKeyFlags.flTKeyFFT_Bit) == (int)MotionKeyFlags.flTKeyFFT_Bit;

                            if (key16bit || keynocompressbit)
                            {
								if (MessageBox.Show("Build-in motions are in " + (keynocompressbit ? "no compression" : "16 bit compression") + " format, not supported in SoC. Delete motions?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
									OGF_V.motions.SetData(null);
                                break;
                            }
                        }
					}

					if (OGF_V.motion_refs != null)
						OGF_V.motion_refs.soc = true;

					foreach (var ch in OGF_V.childs)
					{
						if (ch.links < 0x12071980)
							ch.links *= 0x12071980;
					}
				}

				UpdateModelFormat();
			}
		}

		private void UpdateModelType()
        {
			if (OGF_V == null) return;

			if (OGF_V.bones == null)
				OGF_V.Header.type = OGF_V.Static();
			else if (OGF_V.motions.data() == null && !IsTextCorrect(MotionRefsBox.Text))
                OGF_V.Header.type = OGF_V.Skeleton();
			else
                OGF_V.Header.type = OGF_V.Animated();

			// Апдейтим экспорт аним тут, т.к. при любом изменении омф вызывается эта функция
			omfToolStripMenuItem.Enabled = OGF_V.motions.data() != null;
			sklToolStripMenuItem.Enabled = OGF_V.motions.data() != null;
			sklsToolStripMenuItem.Enabled = OGF_V.motions.data() != null;
		}

		private void UpdateModelFormat()
		{
			CurrentFormat.Enabled = (OGF_V != null && !OGF_V.IsDM && OGF_V.IsSkeleton());

			if (!CurrentFormat.Enabled)
			{
				CurrentFormat.Text = strings.AllFormat;
				return;
			}

			uint links = 0;

			foreach (var ch in OGF_V.childs)
				links = Math.Max(links, ch.links);

			OGF_V.IsCopModel = (IsTextCorrect(MotionRefsBox.Text) && OGF_V.motion_refs != null && !OGF_V.motion_refs.soc || !IsTextCorrect(MotionRefsBox.Text)) && links < 0x12071980;

			CurrentFormat.Text = (OGF_V.IsCopModel ? strings.CoPFormat : strings.SoCFormat);
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

			File.Copy(FILE_NAME, Filename);
			CopyParams();
			SaveFile(Filename);
			RunConverter(Filename, ObjectName, 0, 0);

            Process proc = new Process();
            proc.StartInfo.FileName = ObjectEditor;
            proc.StartInfo.Arguments += $"\"{ObjectName}\" skeleton_only \"{FILE_NAME}\"";
            proc.Start();
			proc.WaitForExit();
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
			CurrentLod = swiLod.Lod;

			if (old_lod != CurrentLod)
				RecalcLod();
		}

		private void RecalcLod()
        {
			if (ViewerWorking)
				InitViewPort(true, false, true);

			for (int idx = 0; idx < OGF_V.childs.Count; idx++)
            {
				Control Mesh = TexturesPage.Controls["TextureGrpBox_" + idx.ToString()];
				Label FaceLbl = (Label)Mesh.Controls["FacesLbl_" + idx.ToString()];
				FaceLbl.Text = FaceLabel.Text + OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString();
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

			if (OGF_V != null && OGF_V.bones != null)
			{
				BoneParamsPage.Controls.Clear();
				for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
				{
					CreateBoneGroupBox(i, OGF_V.bones.bone_names[i], OGF_V.bones.parent_bone_names[i], OGF_V.ikdata.materials[i], OGF_V.ikdata.mass[i], OGF_V.ikdata.center_mass[i], OGF_V.ikdata.position[i], OGF_V.ikdata.rotation[i]);
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

		private bool IsTextCorrect(string text)
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
				if (ObjWriter != null)
                {
					ObjWriter.Close();
					ObjWriter.Dispose();
					ObjWriter = null;
				}
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

			try
			{
				if (ConverterWorking)
                {
					ConverterProcess.Kill();
					ConverterProcess.Close();
					ConverterWorking = false;
				}
			}
			catch (Exception) { }

			try
			{
				if (Directory.Exists(TempFolder(false)))
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
					Clear(false);
					if (OpenFile(file, ref OGF_V, ref Current_OGF))
					{
						FILE_NAME = file;
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
                OGF_Children SecondOgf = null;
                byte[] SecondOgfByte = null;

                OpenFile(OpenOGFDialog.FileName, ref SecondOgf, ref SecondOgfByte);

				int old_childs_count = OGF_V.childs.Count;

                AddMesh addMeshDialog = new AddMesh(ref OGF_V, SecondOgf);
				addMeshDialog.ShowDialog();

				if (old_childs_count != OGF_V.childs.Count)
				{
					TexturesPage.Controls.Clear();
					for (int i = OGF_V.childs.Count - 1; i >= 0; i--)
					{
						CreateTextureGroupBox(i);

						var TextureGroupBox = TexturesPage.Controls["TextureGrpBox_" + i.ToString()];
						TextureGroupBox.Controls["textureBox_" + i.ToString()].Text = OGF_V.childs[i].m_texture; ;
						TextureGroupBox.Controls["shaderBox_" + i.ToString()].Text = OGF_V.childs[i].m_shader;
					}
				}
            }
        }

        private float[] RotateZ(float[] vec)
        {
            float[] dest = new float[3];
            dest[0] = -vec[0];
            dest[1] = vec[1];
            dest[2] = -vec[2];
            return dest;
        }

        private void NPC_ToSoC(object sender, EventArgs e)
        {
			if (CheckNPC(true))
			{
				float[] skel_data = Resources.SoCSkeleton.InitIK();

				if (skel_data.Length == 0)
				{
                    AutoClosingMessageBox.Show("NPC data not loaded!", "", 2500, MessageBoxIcon.Error);
					return;
                }

				RemoveBone("root_stalker");
				RemoveBone("bip01");

				ChangeParent("root_stalker", "bip01_pelvis");
				ChangeParent("bip01", "bip01_pelvis");

				OGF_V.bones.parent_bone_names[0] = "";

				for (int i = 0; i < OGF_V.ikdata.position.Count; i++)
				{
					OGF_V.ikdata.position[i] = Resources.SoCSkeleton.Pos(i, skel_data);
					OGF_V.ikdata.rotation[i] = Resources.SoCSkeleton.Rot(i, skel_data);
					OGF_V.ikdata.center_mass[i] = RotateZ(OGF_V.ikdata.center_mass[i]);
				}

                foreach (var ch in OGF_V.childs)
				{
					uint links = ch.LinksCount();

					for (int i = 0; i < ch.Vertices.Count; i++)
					{
						// Чиним id костей
						for (int j = 0; j < links; j++)
							ch.Vertices[i].bones_id[j] = (ch.Vertices[i].bones_id[j] >= 2 ? ch.Vertices[i].bones_id[j] - 2 : 0);

						ch.Vertices[i].offs = RotateZ(ch.Vertices[i].offs);
						ch.Vertices[i].norm = RotateZ(ch.Vertices[i].norm);
						ch.Vertices[i].tang = RotateZ(ch.Vertices[i].tang);
						ch.Vertices[i].binorm = RotateZ(ch.Vertices[i].binorm);
					}
				}
				AutoClosingMessageBox.Show("Successful!", "", 700, MessageBoxIcon.Information);
			}
			else
				AutoClosingMessageBox.Show("This model is not CoP NPC!", "", 2500, MessageBoxIcon.Error);
        }

        private void NPC_ToCoP(object sender, EventArgs e)
        {
            if (CheckNPC(false))
            {
                float[] skel_data = Resources.CoPSkeleton.InitIK();

                if (skel_data.Length == 0)
                {
                    AutoClosingMessageBox.Show("NPC data not loaded!", "", 2500, MessageBoxIcon.Error);
                    return;
                }

                AddBone("root_stalker", "", 0);
                AddBone("bip01", "root_stalker", 1);
				OGF_V.bones.parent_bone_names[2] = "bip01";

                for (int i = 0; i < OGF_V.ikdata.position.Count; i++)
                {
                    OGF_V.ikdata.position[i] = Resources.CoPSkeleton.Pos(i, skel_data);
                    OGF_V.ikdata.rotation[i] = Resources.CoPSkeleton.Rot(i, skel_data);
                    OGF_V.ikdata.center_mass[i] = RotateZ(OGF_V.ikdata.center_mass[i]);
                }

                foreach (var ch in OGF_V.childs)
                {
                    for (int i = 0; i < ch.Vertices.Count; i++)
                    {
                        for (int j = 0; j < ch.LinksCount(); j++)
                            ch.Vertices[i].bones_id[j] = ch.Vertices[i].bones_id[j] + 2;

                        ch.Vertices[i].offs = RotateZ(ch.Vertices[i].offs);
                        ch.Vertices[i].norm = RotateZ(ch.Vertices[i].norm);
                        ch.Vertices[i].tang = RotateZ(ch.Vertices[i].tang);
                        ch.Vertices[i].binorm = RotateZ(ch.Vertices[i].binorm);
                    }
                }
                AutoClosingMessageBox.Show("Successful!", "", 700, MessageBoxIcon.Information);
            }
            else
                AutoClosingMessageBox.Show("This model is not SoC NPC!", "", 2500, MessageBoxIcon.Error);
        }

        private bool CheckNPC(bool cop_npc)
        {
            if (cop_npc)
            {
                if (OGF_V != null && OGF_V.IsSkeleton())
                {
                    if (OGF_V.ikdata.position.Count == 47 && OGF_V.bones.bone_names.Contains("root_stalker"))
                        return true;
                }
            }
            else
            {
                if (OGF_V != null && OGF_V.IsSkeleton())
                {
                    if (OGF_V.ikdata.position.Count == 45 && OGF_V.bones.bone_names.Contains("bip01_pelvis") && !OGF_V.bones.bone_names.Contains("root_stalker"))
                        return true;
                }
            }

            return false;
        }

        public static void Msg(string text)
        {
			MessageBox.Show(text);
        }

        static public byte[] GetVec3Bytes(float[] vec)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(vec[0]));
            bytes.AddRange(BitConverter.GetBytes(vec[1]));
            bytes.AddRange(BitConverter.GetBytes(vec[2]));
            return bytes.ToArray();
        }

        static public byte[] GetVec2Bytes(float[] vec)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(vec[0]));
            bytes.AddRange(BitConverter.GetBytes(vec[1]));
            return bytes.ToArray();
        }

        // Interface
        private void InitViewPort(bool create_model = true, bool force_texture_reload = false, bool force_reload = false)
        {
			if (OGF_V == null) return;

			if (ViewerWorking && ViewerProcess != null && CheckViewportModelVers() && !force_reload) return;

			bool old_viewer = ViewerWorking;
			ViewerWorking = false;

			if (ViewerThread != null && ViewerThread.ThreadState != System.Threading.ThreadState.Stopped)
				ViewerThread.Abort();

			ViewerThread = new Thread(() => {
				string ObjName = TempFolder() + "\\" + Path.GetFileName(Path.ChangeExtension(FILE_NAME, ".obj"));
				string exe_path = AppPath() + "\\f3d.exe";

				if (!File.Exists(exe_path))
				{
					MessageBox.Show("Can't find Viewport module.\nPlease, reinstall the app.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (old_viewer)
				{
					ViewerProcess.Kill();
					ViewerProcess.Close();
				}

				if (ConverterWorking)
				{
					ConverterProcess.Kill();
					ConverterProcess.Close();
					ConverterWorking = false;
				}

				string Textures = "";
				pSettings.LoadText("TexturesPath", ref Textures);

				List<string> pTextures = new List<string>();
				List<string> pConvertTextures = new List<string>();

				for (int i = 0; i < OGF_V.childs.Count; i++)
				{
					string texture_main = Textures + "\\" + OGF_V.childs[i].m_texture + ".dds";
					string texture_temp = TempFolder() + "\\" + Path.GetFileName(OGF_V.childs[i].m_texture + ".png");

					pTextures.Add(texture_main);
					pTextures.Add(texture_temp);
				}

				int chld = 0;
				for (int i = 0; i < pTextures.Count; i++)
				{
					if (File.Exists(pTextures[i]) && (!File.Exists(pTextures[i + 1]) || force_texture_reload))
					{
						if (OGF_V.childs[chld].to_delete)
							continue;

						pConvertTextures.Add(pTextures[i]);
						pConvertTextures.Add(pTextures[i + 1]);
					}
					i++;
					chld++;
				}

				OldChildVisible.Clear();
				foreach (var ch in OGF_V.childs)
					OldChildVisible.Add(ch.to_delete);

				OldChildTextures.Clear();
				foreach (var ch in OGF_V.childs)
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

					ConverterWorking = true;
					ConverterProcess = new Process();
					ProcessStartInfo psi = new ProcessStartInfo();
					psi.CreateNoWindow = true;
					psi.UseShellExecute = false;
					psi.FileName = AppPath() + "\\TextureConverter.exe";
					psi.Arguments = ConverterArgs;
					ConverterProcess.StartInfo = psi;
					ConverterProcess.Start();
					ConverterProcess.WaitForExit();
					ConverterWorking = false;
				}

				string image_path = "";
				pSettings.Load("ImagePath", ref image_path);

				bool first_load = true;
				pSettings.Load("FirstLoad", ref first_load, true);

				if (create_model)
                    SaveAsObj(ObjName, CurrentLod);

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
					});
				}
				catch(Exception) { }
			});
			ViewerThread.Start();
		}

		private bool CheckViewportModelVers()
        {
			if (OldChildTextures.Count != OGF_V.childs.Count || OldChildVisible.Count != OGF_V.childs.Count) return false;

			if (OldChildTextures.Count != 0)
            {
				int i = 0;
				foreach (var ch in OGF_V.childs)
				{
					if (ch.m_texture != OldChildTextures[i])
						return false;
					i++;
				}
			}

			if (OldChildVisible.Count != 0)
			{
				int i = 0;
				foreach (var ch in OGF_V.childs)
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
			if (ViewerProcess == null || !ViewerWorking)
				return;

			InitViewPort(true, true, true);
		}

		private void disableAlphaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ViewerProcess == null || !ViewerWorking)
				return;

			ViewPortAlpha = !ViewPortAlpha;
			SetAlphaToolStrip(ViewPortAlpha);

			InitViewPort(false, true, true);
		}

		private void SetAlphaToolStrip(bool enable)
		{
            ViewPortAlpha = enable;
            if (ViewPortAlpha)
                disableAlphaToolStripMenuItem.Text = "Disable Alpha";
            else
                disableAlphaToolStripMenuItem.Text = "Enable Alpha";
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

			box.Controls.Add(newTextBox);
			box.Controls.Add(newTextBox2);
			box.Controls.Add(newButton);
		}

		private void CreateTextureLabels(int idx, GroupBox box)
		{
			var newLbl = Copy.Label(TexturesPathLabelEx);
			newLbl.Name = "textureLbl_" + idx;

			var newLbl2 = Copy.Label(ShaderNameLabelEx);
			newLbl2.Name = "shaderLbl_" + idx;

			var newLbl3 = Copy.Label(FaceLabel);
			newLbl3.Name = "FacesLbl_" + idx;
			newLbl3.Text = FaceLabel.Text + OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString();
			newLbl3.Size = new Size(FaceLabel.Size.Width + (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), FaceLabel.Size.Height);
			newLbl3.Location = new Point(FaceLabel.Location.X - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), FaceLabel.Location.Y);

			var newLbl4 = Copy.Label(VertsLabel);
			newLbl4.Name = "VertsLbl_" + idx;
			newLbl4.Text = VertsLabel.Text + OGF_V.childs[idx].Vertices.Count.ToString();
			newLbl4.Size = new Size(VertsLabel.Size.Width + (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6), VertsLabel.Size.Height);
			newLbl4.Location = new Point(VertsLabel.Location.X - (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6) - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), VertsLabel.Location.Y);

			var newLbl5 = Copy.Label(LinksLabel);
			newLbl5.Name = "LinksLbl_" + idx;
			newLbl5.Text = LinksLabel.Text + OGF_V.childs[idx].LinksCount().ToString();
			newLbl5.Size = new Size(LinksLabel.Size.Width + (OGF_V.childs[idx].LinksCount().ToString().Length * 6), LinksLabel.Size.Height);
			newLbl5.Location = new Point(LinksLabel.Location.X - (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6) - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6) - (OGF_V.childs[idx].LinksCount().ToString().Length * 6), LinksLabel.Location.Y);

			var newLbl6 = Copy.Label(LodLabel);
			newLbl6.Name = "LodsLbl_" + idx;
			newLbl6.Text = LodLabel.Text + OGF_V.childs[idx].SWI.Count.ToString();
			newLbl6.Size = new Size(LodLabel.Size.Width + (OGF_V.childs[idx].SWI.Count.ToString().Length * 6), LodLabel.Size.Height);
			newLbl6.Location = new Point(LodLabel.Location.X - (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6) - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6) - (OGF_V.childs[idx].LinksCount().ToString().Length * 6) - (OGF_V.childs[idx].SWI.Count.ToString().Length * 6), LodLabel.Location.Y);

			box.Controls.Add(newLbl);
			box.Controls.Add(newLbl2);
			box.Controls.Add(newLbl3);
			box.Controls.Add(newLbl4);

			if (OGF_V.IsSkeleton())
				box.Controls.Add(newLbl5);

			if (OGF_V.childs[idx].SWI.Count > 0)
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
	}
}
