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


namespace OGF_tool
{
	public partial class OGF_Editor : Form
	{
		// File sytem
		public EditorSettings pSettings = null;
		public OGF_Children OGF_V = null;
		public byte[] Current_OGF = null;
		public byte[] Current_OMF = null;
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

		public OGF_Editor()
		{
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

			// End init settings

			number_mask = @"^-[0-9.]*$";
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

			OgfInfo.Enabled = false;
			SaveMenuParam.Enabled = false;
			saveAsToolStripMenuItem.Enabled = false;
			motionToolsToolStripMenuItem.Enabled = false;
			openSkeletonInObjectEditorToolStripMenuItem.Enabled = false;
			ToolsMenuItem.Enabled = false;
			exportToolStripMenuItem.Enabled = false;
			LabelBroken.Visible = false;
			viewPortToolStripMenuItem.Visible = false;
			ChangeLodButton.Enabled = false;

			SaveSklDialog = new FolderSelectDialog();

			if (Environment.GetCommandLineArgs().Length > 1)
			{
				Clear(false);
				if (OpenFile(Environment.GetCommandLineArgs()[1], ref OGF_V, ref Current_OGF, ref Current_OMF))
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

		public string AppPath()
		{
			return Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
		}

		public string TempFolder(bool check = true)
		{
			if (check)
				CheckTempFolder();
			return Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\')) + "\\temp";
		}

		public void CheckTempFolder()
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

				SaveMenuParam.Enabled = true;
				saveAsToolStripMenuItem.Enabled = true;
				ToolsMenuItem.Enabled = !OGF_V.IsDM;
				openSkeletonInObjectEditorToolStripMenuItem.Enabled = OGF_V.IsSkeleton();
				exportToolStripMenuItem.Enabled = true;
				bonesToolStripMenuItem.Enabled = OGF_V.IsSkeleton();
				omfToolStripMenuItem.Enabled = Current_OMF != null;
				sklToolStripMenuItem.Enabled = Current_OMF != null;
				sklsToolStripMenuItem.Enabled = Current_OMF != null;
				ChangeLodButton.Enabled = OGF_V.IsProgressive();
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

				CurrentLod = 0;
			}

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
				MotionBox.Text = OGF_V.motions;

				if (OGF_V.motions != "")
				{
					AppendOMFButton.Visible = false;
					MotionBox.Visible = true;
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
				if (OGF_V.m_version == 4)
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

				var box = TexturesPage.Controls["TextureGrpBox_" + i.ToString()];

				if (box != null)
				{
					var Cntrl = box.Controls["textureBox_" + i.ToString()];
					Cntrl.Text = OGF_V.childs[i].m_texture;
					var Cntrl2 = box.Controls["shaderBox_" + i.ToString()];
					Cntrl2.Text = OGF_V.childs[i].m_shader;
				}
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

		private void WriteFile(string filename)
        {
			if (BkpCheckBox.Checked)
			{
				string backup_path = filename + ".bak";

				if (File.Exists(backup_path))
				{
					FileInfo backup_file = new FileInfo(backup_path);
					backup_file.Delete();
				}

				FileInfo file = new FileInfo(filename);
				file.CopyTo(backup_path);
			}

			using (var fileStream = new FileStream(filename, FileMode.Truncate))
			{
				byte[] data = file_bytes.ToArray();
				fileStream.Write(data, 0, data.Length);
			}
		}

		private void SaveFile(string filename)
		{
			file_bytes.Clear();

			if (Current_OGF == null) return;

			using (var fileStream = new BinaryReader(new MemoryStream(Current_OGF)))
			{
				if (OGF_V.IsDM)
				{
                    fileStream.ReadBytes(OGF_V.childs[0].old_size);
					file_bytes.AddRange(Encoding.Default.GetBytes(OGF_V.childs[0].m_shader));
					file_bytes.Add(0);
					file_bytes.AddRange(Encoding.Default.GetBytes(OGF_V.childs[0].m_texture));
					file_bytes.Add(0);
                    byte[] dm_data = fileStream.ReadBytes((int)(fileStream.BaseStream.Length - fileStream.BaseStream.Position));
					file_bytes.AddRange(dm_data);
					WriteFile(filename);
					return;
				}

				uint HeaderSize = (uint)((OGF_V.m_version == 4) ? 44 : 4);
				fileStream.ReadBytes(8);
				file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF_HEADER));
				file_bytes.AddRange(BitConverter.GetBytes(HeaderSize));

				fileStream.ReadBytes(2);
				file_bytes.Add(OGF_V.m_version);
				file_bytes.Add(OGF_V.m_model_type);

				byte[] temp = fileStream.ReadBytes(2);
				file_bytes.AddRange(temp);

				if (OGF_V.m_version == 4)
				{
					temp = fileStream.ReadBytes(40);
					if (OGF_V.BrokenType == 2)
					{
						for (int i = 0; i < 40; i++)
							file_bytes.Add(0);
					}
					else
						file_bytes.AddRange(temp);
				}

				if (OGF_V.description != null)
				{
					bool old_byte = OGF_V.description.four_byte;
					if (OGF_V.BrokenType > 0) // Если модель сломана, то восстанавливаем чанк с 8 байтными таймерами
						OGF_V.description.four_byte = false;

					file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_DESC));
					file_bytes.AddRange(BitConverter.GetBytes(OGF_V.description.chunk_size()));
					file_bytes.AddRange(OGF_V.description.data());

					OGF_V.description.four_byte = old_byte; // Восстанавливаем отображение колличества байтов у таймера
				}

				if (OGF_V.IsStaticSingle()) // Single mesh
				{
					file_bytes.AddRange(OGF_V.childs[0].data());
					fileStream.ReadBytes(OGF_V.childs[0].old_size + 8);
				}
				else // Hierrarhy mesh
				{
					fileStream.ReadBytes((int)(OGF_V.pos - fileStream.BaseStream.Position));

					fileStream.ReadBytes(4);
					uint new_size = fileStream.ReadUInt32();

					foreach (var ch in OGF_V.childs)
					{
						if (ch.to_delete)
							new_size -= ch.chunk_size + 8;
						else
							new_size += ch.NewSize();
					}

					int ChildChunk = (OGF_V.m_version == 4 ? (int)OGF.OGF4_CHILDREN : (int)OGF.OGF3_CHILDREN);
					file_bytes.AddRange(BitConverter.GetBytes(ChildChunk));
					file_bytes.AddRange(BitConverter.GetBytes(new_size));

					int child_id = 0;
					foreach (var ch in OGF_V.childs)
					{
						fileStream.ReadUInt32();
						uint size = fileStream.ReadUInt32();
						long old_pos = fileStream.BaseStream.Position; // Записываем начальную позицию чанка

						if (!ch.to_delete)
						{
							new_size = size + ch.NewSize();
							file_bytes.AddRange(BitConverter.GetBytes(child_id));
							file_bytes.AddRange(BitConverter.GetBytes(new_size));

							child_id++;
						}

						temp = fileStream.ReadBytes((int)(ch.pos - fileStream.BaseStream.Position));

						if (!ch.to_delete)
						{
							file_bytes.AddRange(temp);
							file_bytes.AddRange(ch.data());
						}

						fileStream.BaseStream.Position += ch.old_size + 8;

						if (OGF_V.IsSkeleton()) // Verts
						{
                            uint VertsChunk = fileStream.ReadUInt32(); // VertsChunk
                            uint VertsSize = fileStream.ReadUInt32(); // VertsSize

                            if (!ch.to_delete)
							{
								file_bytes.AddRange(BitConverter.GetBytes(VertsChunk));
                                file_bytes.AddRange(BitConverter.GetBytes(VertsSize));

                                file_bytes.AddRange(BitConverter.GetBytes(ch.links)); // Линковка
                                file_bytes.AddRange(BitConverter.GetBytes(ch.Vertices.Count));

                                for (int i = 0; i < ch.Vertices.Count; i++)
								{
									SSkelVert vert = ch.Vertices[i];

                                    switch (ch.LinksCount())
									{
										case 1:
											file_bytes.AddRange(GetVec3Bytes(vert.offs));
                                            file_bytes.AddRange(GetVec3Bytes(vert.norm));

											if (OGF_V.m_version == 4)
											{
                                                file_bytes.AddRange(GetVec3Bytes(vert.tang));
                                                file_bytes.AddRange(GetVec3Bytes(vert.binorm));
                                            }

                                            file_bytes.AddRange(GetVec2Bytes(vert.uv));
                                            file_bytes.AddRange(BitConverter.GetBytes(vert.bones_id[0]));
                                            break;
										case 2:
                                            file_bytes.AddRange(BitConverter.GetBytes((short)vert.bones_id[0]));
                                            file_bytes.AddRange(BitConverter.GetBytes((short)vert.bones_id[1]));

                                            file_bytes.AddRange(GetVec3Bytes(vert.offs));
                                            file_bytes.AddRange(GetVec3Bytes(vert.norm));

                                            file_bytes.AddRange(GetVec3Bytes(vert.tang));
                                            file_bytes.AddRange(GetVec3Bytes(vert.binorm));

                                            file_bytes.AddRange(BitConverter.GetBytes(vert.bones_infl[0]));
                                            file_bytes.AddRange(GetVec2Bytes(vert.uv));
											break;
										case 3:
										case 4:
                                            for (int j = 0; j < ch.LinksCount(); j++)
                                            {
                                                file_bytes.AddRange(BitConverter.GetBytes((short)vert.bones_id[j]));
                                            }

                                            file_bytes.AddRange(GetVec3Bytes(vert.offs));
                                            file_bytes.AddRange(GetVec3Bytes(vert.norm));

                                            file_bytes.AddRange(GetVec3Bytes(vert.tang));
                                            file_bytes.AddRange(GetVec3Bytes(vert.binorm));

                                            for (int j = 0; j < ch.LinksCount() - 1; j++)
                                            {
                                                file_bytes.AddRange(BitConverter.GetBytes(vert.bones_infl[j]));
                                            }

                                            file_bytes.AddRange(GetVec2Bytes(vert.uv));
											break;
									}
								}
                                fileStream.BaseStream.Position += VertsSize; 
                            }
						}

						temp = fileStream.ReadBytes((int)ch.chunk_size - (int)(fileStream.BaseStream.Position - old_pos)); // Читаем все до конца чанка

						if (!ch.to_delete) // Дописываем чанк если он не претендент на удаление
							file_bytes.AddRange(temp);
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
						file_bytes.AddRange(BitConverter.GetBytes(OGF_V.bones.chunk_size()));
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
								IKDataChunk = (OGF_V.m_version == 4 ? (uint)OGF.OGF4_S_IKDATA : (uint)OGF.OGF3_S_IKDATA_2);
								break;
							case 3:
								IKDataChunk = (uint)OGF.OGF3_S_IKDATA;
								break;
							case 2:
								IKDataChunk = (uint)OGF.OGF3_S_IKDATA_0;
								break;
						}

						file_bytes.AddRange(BitConverter.GetBytes(IKDataChunk));
						file_bytes.AddRange(BitConverter.GetBytes(OGF_V.ikdata.chunk_size()));
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
							uint UserDataChunk = (OGF_V.m_version == 4 ? (uint)OGF.OGF4_S_USERDATA : (uint)OGF.OGF3_S_USERDATA);
							file_bytes.AddRange(BitConverter.GetBytes(UserDataChunk));
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.userdata.chunk_size(OGF_V.m_version)));
							file_bytes.AddRange(OGF_V.userdata.data(OGF_V.m_version));
						}

						if (OGF_V.userdata.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
							fileStream.ReadBytes(OGF_V.userdata.old_size + 8);
					}

					if (OGF_V.lod != null && OGF_V.m_version == 4) // Стринг лод только у релизных OGF
					{
						if (OGF_V.lod.pos > 0 && (OGF_V.lod.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
						{
							temp = fileStream.ReadBytes((int)(OGF_V.lod.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.lod.lod_path != "") // Пишем если есть что писать
						{
							file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_LODS));
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.lod.chunk_size()));
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
								uint RefsChunk = (OGF_V.m_version == 4 ? (uint)OGF.OGF4_S_MOTION_REFS : (uint)OGF.OGF3_S_MOTION_REFS);
								file_bytes.AddRange(BitConverter.GetBytes(RefsChunk));
							}

							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.motion_refs.chunk_size(OGF_V.motion_refs.soc)));

							if (!OGF_V.motion_refs.soc)
								file_bytes.AddRange(OGF_V.motion_refs.count());

							file_bytes.AddRange(OGF_V.motion_refs.data(OGF_V.motion_refs.soc));
						}

						if (OGF_V.motion_refs.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
							fileStream.ReadBytes(OGF_V.motion_refs.old_size + 8);
					}

					if (Current_OMF != null && !refs_created)
						file_bytes.AddRange(Current_OMF);
				}
				else
				{
					temp = fileStream.ReadBytes((int)(fileStream.BaseStream.Length - fileStream.BaseStream.Position));
					file_bytes.AddRange(temp);
				}
			}

			WriteFile(filename);
		}

		private bool OpenFile(string filename, ref OGF_Children OGF_C, ref byte[] Cur_OGF, ref byte[] Cur_OMF)
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
					OGF_Child chld = new OGF_Child( 0, 0, 0, shader.Length + texture.Length + 2, texture, shader);
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
					OGF_C.m_version = xr_loader.ReadByte();

					if (OGF_C.m_version < 3)
                    {
						MessageBox.Show($"Unsupported OGF version: {OGF_C.m_version}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					OGF_C.m_model_type = xr_loader.ReadByte();
					xr_loader.ReadBytes(42);
				}

				int DescChunk = (OGF_C.m_version == 4 ? (int)OGF.OGF4_S_DESC : (int)OGF.OGF3_S_DESC);
				uint DescriptionSize = xr_loader.find_chunkSize(DescChunk, false, true);
				if (DescriptionSize > 0)
				{
					OGF_C.description = new Description();
					OGF_C.description.pos = xr_loader.chunk_pos;

					// Читаем таймеры в 8 байт
					long reader_start_pos = xr_loader.reader.BaseStream.Position;
					OGF_C.description.m_source = xr_loader.read_stringZ();
					OGF_C.description.m_export_tool = xr_loader.read_stringZ();
					OGF_C.description.m_export_time = xr_loader.ReadInt64();
					OGF_C.description.m_owner_name = xr_loader.read_stringZ();
					OGF_C.description.m_creation_time = xr_loader.ReadInt64();
					OGF_C.description.m_export_modif_name_tool = xr_loader.read_stringZ();
					OGF_C.description.m_modified_time = xr_loader.ReadInt64();
					long description_end_pos = xr_loader.reader.BaseStream.Position;

					OGF_C.description.old_size = OGF_C.description.m_source.Length + 1 + OGF_C.description.m_export_tool.Length + 1 + 8 + OGF_C.description.m_owner_name.Length + 1 + 8 + OGF_C.description.m_export_modif_name_tool.Length + 1 + 8;

					if ((description_end_pos - reader_start_pos) != DescriptionSize) // Размер не состыковывается, пробуем читать 4 байта
                    {
						xr_loader.reader.BaseStream.Position = reader_start_pos;
						OGF_C.description.m_source = xr_loader.read_stringZ();
						OGF_C.description.m_export_tool = xr_loader.read_stringZ();
						OGF_C.description.m_export_time = xr_loader.ReadUInt32();
						OGF_C.description.m_owner_name = xr_loader.read_stringZ();
						OGF_C.description.m_creation_time = xr_loader.ReadUInt32();
						OGF_C.description.m_export_modif_name_tool = xr_loader.read_stringZ();
						OGF_C.description.m_modified_time = xr_loader.ReadUInt32();
						description_end_pos = xr_loader.reader.BaseStream.Position;

						OGF_C.description.old_size = OGF_C.description.m_source.Length + 1 + OGF_C.description.m_export_tool.Length + 1 + 4 + OGF_C.description.m_owner_name.Length + 1 + 4 + OGF_C.description.m_export_modif_name_tool.Length + 1 + 4;
						OGF_C.description.four_byte = true; // Ставим флаг на то что мы прочитали чанк с 4х байтными таймерами, если модель будет сломана то чинить чанк будем в 8 байт

						if ((description_end_pos - reader_start_pos) != DescriptionSize) // Все равно разный размер? Походу модель сломана
						{
							OGF_C.BrokenType = 1;

							// Чистим таймеры, так как прочитаны битые байты
							OGF_C.description.m_export_time = 0;
							OGF_C.description.m_creation_time = 0;
							OGF_C.description.m_modified_time = 0;
						}
					}
				}

				TriangleParser LoadTriangles = (loader, child, v3) =>
				{
					int VertsChunk = v3 ? (int)OGF.OGF3_VERTICES : (int)OGF.OGF4_VERTICES;
                    if (loader.find_chunk(VertsChunk, false, true))
					{
                        child.links = loader.ReadUInt32();
						uint verts = loader.ReadUInt32();

                        for (int i = 0; i < verts; i++)
						{
							SSkelVert Vert = new SSkelVert();
							switch (child.LinksCount())
							{
								case 1:
									Vert.offs = loader.ReadVector();
									Vert.norm = loader.ReadVector();
									if (!v3)
									{
										Vert.tang = loader.ReadVector();
										Vert.binorm = loader.ReadVector();
									}
									Vert.uv = loader.ReadVector2();

                                    Vert.bones_id[0] = loader.ReadUInt32();
									break;
								case 2:
                                    Vert.bones_id[0] = loader.ReadUInt16();
                                    Vert.bones_id[1] = loader.ReadUInt16();

									Vert.offs = loader.ReadVector();
									Vert.norm = loader.ReadVector();
									Vert.tang = loader.ReadVector();
									Vert.binorm = loader.ReadVector();
                                    Vert.bones_infl[0] = loader.ReadFloat();
									Vert.uv = loader.ReadVector2();
									break;
								case 3:
								case 4:
                                    for (int j = 0; j < child.LinksCount(); j++)
                                    {
                                        Vert.bones_id[j] = loader.ReadUInt16();
                                    }

                                    Vert.offs = loader.ReadVector();
									Vert.norm = loader.ReadVector();
									Vert.tang = loader.ReadVector();
									Vert.binorm = loader.ReadVector();

                                    for (int j = 0; j < child.LinksCount() - 1; j++)
                                    {
                                        Vert.bones_infl[j] = loader.ReadFloat();
                                    }

                                    Vert.uv = loader.ReadVector2();
									break;
								default:
									Vert.offs = loader.ReadVector();
									Vert.norm = loader.ReadVector();
									Vert.uv = loader.ReadVector2();
									break;
							}
							child.Vertices.Add(Vert);
						}
					}

					int FacesChunk = v3 ? (int)OGF.OGF3_INDICES : (int)OGF.OGF4_INDICES;
					if (loader.find_chunk(FacesChunk, false, true))
					{
						uint faces = loader.ReadUInt32() / 3;

						for (uint i = 0; i < faces; i++)
						{
							SSkelFace Face = new SSkelFace();
							Face.v[0] = (ushort)loader.ReadUInt16();
							Face.v[1] = (ushort)loader.ReadUInt16();
							Face.v[2] = (ushort)loader.ReadUInt16();
							child.Faces.Add(Face);
						}
					}

					if (!v3)
					{
						if (loader.find_chunk((int)OGF.OGF4_SWIDATA, false, true))
						{
							loader.ReadUInt32();
							loader.ReadUInt32();
							loader.ReadUInt32();
							loader.ReadUInt32();

							uint swi_size = loader.ReadUInt32();

							for (uint i = 0; i < swi_size; i++)
							{
								VIPM_SWR SWR = new VIPM_SWR();
								SWR.offset = loader.ReadUInt32();
								SWR.num_tris = (ushort)loader.ReadUInt16();
								SWR.num_verts = (ushort)loader.ReadUInt16();
								child.SWI.Add(SWR);
							}
						}
					}
				};

				int ChildChunk = (OGF_C.m_version == 4 ? (int)OGF.OGF4_CHILDREN : (int)OGF.OGF3_CHILDREN);
				bool bFindChunk = xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(ChildChunk, false, true));

				OGF_C.pos = xr_loader.chunk_pos;

				int id = 0;
				uint size;

				// Texture && shader
				if (bFindChunk)
				{
					while (true)
					{
						uint chunk_size = xr_loader.find_chunkSize(id);
						if (chunk_size == 0) break;

						Stream temp = xr_loader.reader.BaseStream;

						if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

						long pos = xr_loader.chunk_pos + OGF_C.pos + 16;
						size = xr_loader.find_chunkSize((int)OGF.OGF_TEXTURE);

						if (size == 0) break;

						OGF_Child chld = new OGF_Child(xr_loader.chunk_pos + pos, pos - 8, chunk_size, (int)size, xr_loader.read_stringZ(), xr_loader.read_stringZ());

						LoadTriangles(xr_loader, chld, OGF_C.m_version != 4);

						OGF_C.childs.Add(chld);

						id++;
						xr_loader.SetStream(temp);
					}

					xr_loader.SetStream(r.BaseStream);
				}
				else
                {
					size = xr_loader.find_chunkSize((int)OGF.OGF_TEXTURE, false, true);

					if (size != 0)
					{
						OGF_Child chld = new OGF_Child(0, 0, 0, (int)size, xr_loader.read_stringZ(), xr_loader.read_stringZ());

						LoadTriangles(xr_loader, chld, false);

						OGF_C.childs.Add(chld);
					}
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

					int IKDataChunkRelease = (OGF_C.m_version == 4 ? (int)OGF.OGF4_S_IKDATA : (int)OGF.OGF3_S_IKDATA_2);
					bool IKDataChunkFind = xr_loader.find_chunk(IKDataChunkRelease, false, true);

					if (IKDataChunkFind) // Load Release chunk
					{
						IKDataVers = 4;
						goto load_ik_data;
					}

					IKDataChunkFind = OGF_C.m_version == 3 && xr_loader.find_chunk((int)OGF.OGF3_S_IKDATA, false, true);

					if (IKDataChunkFind) // Load Pre Release chunk
					{
						IKDataVers = 3;
						goto load_ik_data;
					}

					IKDataChunkFind = OGF_C.m_version == 3 && xr_loader.find_chunk((int)OGF.OGF3_S_IKDATA_0, false, true);

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

					if (IKDataVers == 0 && OGF_C.m_version == 4) // Chunk not find, exit if Release OGF
					{
						MessageBox.Show("Unsupported OGF format! Can't find ik data chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					// Userdata
					int UserDataChunk = (OGF_C.m_version == 4 ? (int)OGF.OGF4_S_USERDATA : (int)OGF.OGF3_S_USERDATA);
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
					if (OGF_C.m_version == 4 && xr_loader.find_chunk((int)OGF.OGF4_S_LODS, false, true))
					{
						OGF_C.lod = new Lod();
						OGF_C.lod.pos = xr_loader.chunk_pos;
						OGF_C.lod.lod_path = xr_loader.read_stringData(ref OGF_C.lod.data_str);
						OGF_C.lod.old_size = OGF_C.lod.lod_path.Length + (OGF_C.lod.data_str ? 2 : 1);
					}

					// Motion Refs
					int RefsChunk = (OGF_C.m_version == 4 ? (int)OGF.OGF4_S_MOTION_REFS : (int)OGF.OGF3_S_MOTION_REFS);
					bool StringRefs = xr_loader.find_chunk(RefsChunk, false, true);

					if (StringRefs || OGF_C.m_version == 4 && xr_loader.find_chunk((int)OGF.OGF4_S_MOTION_REFS2, false, true))
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
						Cur_OMF = xr_loader.ReadBytes((int)xr_loader.reader.BaseStream.Length - (int)xr_loader.reader.BaseStream.Position);
					}

					if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF_S_MOTIONS, false, true)))
					{
						id = 0;

						while (true)
						{
							if (!xr_loader.find_chunk(id)) break;

							Stream temp = xr_loader.reader.BaseStream;

							if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

							if (id == 0)
								OGF_C.motions += $"Motions count : {xr_loader.ReadUInt32()}\n";
							else
								OGF_C.motions += $"\n{id}. {xr_loader.read_stringZ()}";

							id++;
							xr_loader.SetStream(temp);
						}
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
				OGF_V.bones.fobb.Insert(pos, OGF_V.bones.fobb[OGF_V.bones.fobb.Count - 1]);

				float[] temp_vec = new float[3];
				temp_vec[0] = 0.0f;
				temp_vec[1] = 0.0f;
				temp_vec[2] = 0.0f;

                OGF_V.ikdata.materials.Insert(pos, "default_object");
                OGF_V.ikdata.mass.Insert(pos, 10.0f);
                OGF_V.ikdata.version.Insert(pos, 4);
                OGF_V.ikdata.center_mass.Insert(pos, temp_vec);

                OGF_V.ikdata.bytes_1.Insert(pos, OGF_V.ikdata.bytes_1[OGF_V.ikdata.bytes_1.Count - 1]);
                OGF_V.ikdata.position.Insert(pos, temp_vec);
                OGF_V.ikdata.rotation.Insert(pos, temp_vec);
            }
		}

		private int GetBoneID(string bone)
		{
            if (OGF_V != null && OGF_V.IsSkeleton())
            {
                for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
                {
                    if (OGF_V.bones.bone_names[i] == bone)
						return i;
                }
            }
			return -1;
        }

        private string GetBoneName(int bone)
        {
            if (OGF_V != null && OGF_V.IsSkeleton())
            {
				return OGF_V.bones.bone_names[bone];
            }
            return "";
        }

        private void RemoveBone(string bone)
        {
			RemoveBone(GetBoneID(bone));
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
					int chld_cnt = OGF_V.childs.Count;

					foreach (var ch in OGF_V.childs)
                    {
						if (ch.to_delete) chld_cnt--;
					}

					if (chld_cnt > 1 || OGF_V.childs[idx].to_delete)
					{
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
					}
					else
                    {
						MessageBox.Show("Can't delete last mesh!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
							string mask = number_mask;
							Regex.Match(curControl.Text, mask);

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
				if (OpenFile(OpenOGF_DmDialog.FileName, ref OGF_V, ref Current_OGF, ref Current_OMF))
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

			List<byte> flags = new List<byte>();
			if (Current_OMF != null)
			{
				var xr_loader = new XRayLoader();
				using (var fileStream = new BinaryReader(new MemoryStream(Current_OMF)))
				{
					xr_loader.SetStream(fileStream.BaseStream);

					if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF_S_MOTIONS, false, true)))
					{
						int id = 0;

						while (true)
						{
							if (!xr_loader.find_chunk(id)) break;

							Stream temp = xr_loader.reader.BaseStream;

							if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

							if (id == 0)
								xr_loader.ReadUInt32();
							else
							{
								xr_loader.read_stringZ();
								xr_loader.ReadUInt32();
								flags.Add(xr_loader.ReadByte());
							}

							id++;
							xr_loader.SetStream(temp);
						}
					}
				}
			}

			OgfInfo Info = new OgfInfo(OGF_V, IsTextCorrect(MotionRefsBox.Text), CurrentLod, flags);
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

		private void SaveTools(string filename, int format, bool silent = false)
		{
			bool has_msg = false;

			if (File.Exists(filename) && filename != FILE_NAME)
			{
				FileInfo backup_file = new FileInfo(filename);
				backup_file.Delete();
			}

			if (format == 0)
			{
				if (filename != FILE_NAME)
				{
					FileInfo file = new FileInfo(FILE_NAME);
					file.CopyTo(filename);
				}

				CopyParams();
				SaveFile(filename);
				has_msg = true;
			}
			else if (format == 1)
			{
				string ext = OGF_V.IsDM ? ".dm" : ".ogf";

				if (filename != FILE_NAME)
				{
					if (File.Exists(filename + ext))
					{
						FileInfo backup_file = new FileInfo(filename + ext);
						backup_file.Delete();
					}

					FileInfo file = new FileInfo(FILE_NAME);
					file.CopyTo(filename + ext);
				}

				CopyParams();
				SaveFile(filename + ext);

				RunConverter(filename + ext, filename, OGF_V.IsDM ? 2 : 0, 0);

				if (File.Exists(filename + ext))
				{
					FileInfo backup_file = new FileInfo(filename + ext);
					backup_file.Delete();
				}
			}
			else if (format == 2)
				RunConverter(FILE_NAME, filename, 0, 1);
			else if (format == 3)
				RunConverter(FILE_NAME, filename, 0, 2);
			else if (format == 4)
				RunConverter(FILE_NAME, filename, 0, 3);
			else if (format == 5)
			{
				using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate))
				{
					fileStream.Write(Current_OMF, 0, Current_OMF.Length);
				}
			}
			else if (format == 6)
				SaveAsObj(filename, CurrentLod);

			if (!has_msg && !silent)
				AutoClosingMessageBox.Show(OGF_V.BrokenType > 0 ? "Repaired and Exported!" : "Exported!", "", OGF_V.BrokenType > 0 ? 700 : 500, MessageBoxIcon.Information);
		}

		private void objectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveObjectDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveObjectDialog.FileName, 1);
				SaveObjectDialog.InitialDirectory = "";
			}
		}

		private void bonesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveBonesDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveBonesDialog.FileName, 2);
				SaveBonesDialog.InitialDirectory = "";
			}
		}

		private void omfToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveOmfDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveOmfDialog.FileName, 5);
				SaveOmfDialog.InitialDirectory = "";
			}
		}

		private void objToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveObjDialog.ShowDialog() == DialogResult.OK)
			{
				float old_lod = CurrentLod;
				CurrentLod = 0.0f;
				SaveTools(SaveObjDialog.FileName, 6);
				CurrentLod = old_lod;
				SaveObjDialog.InitialDirectory = "";
			}
		}

		private void sklToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveSklDialog.ShowDialog(this.Handle))
			{
				SaveTools(SaveSklDialog.FileName, 3);
				SaveSklDialog.InitialDirectory = "";
			}
		}

		private void sklsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SaveSklsDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTools(SaveSklsDialog.FileName, 4);
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
			if (Current_OMF == null || Current_OMF != null && MessageBox.Show("New motion refs chunk will remove built-in motions, continue?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				// Чистим все связанное со встроенными анимами
				MotionBox.Clear();
				MotionBox.Visible = false;
				Current_OMF = null;
				AppendOMFButton.Visible = true;
				OGF_V.motions = "";

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
			string cur_fname = FILE_NAME;
			Clear(false);
			if (OpenFile(cur_fname, ref OGF_V, ref Current_OGF, ref Current_OMF))
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

			var xr_loader = new XRayLoader();

			using (var r = new BinaryReader(new MemoryStream(OpenedOmf)))
			{
				xr_loader.SetStream(r.BaseStream);

				if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF_S_MOTIONS, false, true)))
				{
					// Апдейтим визуал встроенных анимаций
					AppendOMFButton.Visible = false;
					MotionBox.Visible = true;

					// Чистим встроенные рефы, интерфейс почистится сам при активации вкладки
					MotionRefsBox.Clear();
					if (OGF_V.motion_refs != null)
						OGF_V.motion_refs.refs.Clear();

					Current_OMF = OpenedOmf;
					OGF_V.motions = "";

					int id = 0;

					while (true)
					{
						if (!xr_loader.find_chunk(id)) break;

						Stream temp = xr_loader.reader.BaseStream;

						if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

						if (id == 0)
							OGF_V.motions += $"Motions count : {xr_loader.ReadUInt32()}\n";
						else
							OGF_V.motions += $"\n{id}. {xr_loader.read_stringZ()}";

						id++;
						xr_loader.SetStream(temp);
					}

					MotionBox.Text = OGF_V.motions;
				}
			}

			UpdateModelType();
			UpdateModelFormat();
		}

        private void deleteChunkToolStripMenuItem_Click(object sender, EventArgs e)
        {
			MotionBox.Visible = false;
			AppendOMFButton.Visible = true;
			Current_OMF = null;
			MotionBox.Clear();
			OGF_V.motions = "";
			UpdateModelType();
			UpdateModelFormat();
		}

		private void importDataFromModelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenOGFDialog.FileName = "";
			if (OpenOGFDialog.ShowDialog() == DialogResult.OK)
			{
				bool UpdateUi = false;

				OGF_Children SecondOgf = null;
				byte[] SecondOgfByte = null;
				byte[] SecondOmfByte = null;
				OpenFile(OpenOGFDialog.FileName, ref SecondOgf, ref SecondOgfByte, ref SecondOmfByte);

				if (OGF_V.childs.Count == SecondOgf.childs.Count && MessageBox.Show("Import textures and shaders path?\nThey may have different positions", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					for (int i = 0; i < OGF_V.childs.Count; i++)
					{
						OGF_V.childs[i].m_texture = SecondOgf.childs[i].m_texture;
						OGF_V.childs[i].m_shader = SecondOgf.childs[i].m_shader;
					}
					UpdateUi = true;
				}

				if (OGF_V.IsSkeleton())
				{
					if (SecondOgf.userdata != null)
					{
						if (OGF_V.userdata == null)
							OGF_V.userdata = new UserData();
						OGF_V.userdata.userdata = SecondOgf.userdata.userdata;

						UpdateUi = true;
					}
					else
						OGF_V.userdata = null;

					if (SecondOgf.lod != null)
					{
						if (OGF_V.lod == null)
							OGF_V.lod = new Lod();
						OGF_V.lod.lod_path = SecondOgf.lod.lod_path;

						UpdateUi = true;
					}
					else
						OGF_V.lod = null;

					if (SecondOgf.motion_refs != null)
					{
						if (OGF_V.motion_refs == null)
							OGF_V.motion_refs = new MotionRefs();

						OGF_V.motion_refs.refs = SecondOgf.motion_refs.refs;

						UpdateUi = true;
					}
					else
						OGF_V.motion_refs = null;

					if (SecondOmfByte != null)
					{
						if (!IsTextCorrect(MotionRefsBox.Text) && (OGF_V.motion_refs == null || OGF_V.motion_refs.refs.Count() == 0) || (IsTextCorrect(MotionRefsBox.Text) || OGF_V.motion_refs != null && OGF_V.motion_refs.refs.Count() > 0) && MessageBox.Show("Build-in motions will remove motion refs, continue?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							Current_OMF = SecondOmfByte;
							UpdateUi = true;
						}
					}

					if (OGF_V.ikdata.materials.Count == SecondOgf.ikdata.materials.Count)
					{
						for (int i = 0; i < OGF_V.ikdata.materials.Count; i++)
						{
							OGF_V.ikdata.materials[i] = SecondOgf.ikdata.materials[i];
							OGF_V.ikdata.mass[i] = SecondOgf.ikdata.mass[i];
						}
						UpdateUi = true;
					}
				}

				if (UpdateUi)
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

		private void ToolsClick(object sender, EventArgs e)
		{
			motionToolsToolStripMenuItem.Enabled = Current_OMF != null;
		}

		private void ChangeRefsFormat(object sender, EventArgs e)
		{
			if (OGF_V != null)
			{
				if (OGF_V.m_version != 4)
				{
					MessageBox.Show("Can't convert model. Unsupported OGF version: " + OGF_V.m_version.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					{
						if (ch.links >= 0x12071980)
							links = Math.Max(links, ch.links / 0x12071980);
						else
							links = Math.Max(links, ch.links);
					}

					if (links > 2)
                    {
						MessageBox.Show("Can't convert to SoC. Model has more than 2 links!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						OGF_V.IsCopModel = !OGF_V.IsCopModel;
						return;
					}

					if (Current_OMF != null)
					{
						var xr_loader = new XRayLoader();
						using (var fileStream = new BinaryReader(new MemoryStream(Current_OMF)))
						{
							xr_loader.SetStream(fileStream.BaseStream);

							if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF_S_MOTIONS, false, true)))
							{
								int id = 0;

								while (true)
								{
									if (!xr_loader.find_chunk(id)) break;

									Stream temp = xr_loader.reader.BaseStream;

									if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

									if (id == 0)
										xr_loader.ReadUInt32();
									else
									{
										xr_loader.read_stringZ();
										xr_loader.ReadUInt32();
										byte flags = xr_loader.ReadByte();

										bool key16bit = (flags & (int)MotionKeyFlags.flTKey16IsBit) == (int)MotionKeyFlags.flTKey16IsBit;
										bool keynocompressbit = (flags & (int)MotionKeyFlags.flTKeyFFT_Bit) == (int)MotionKeyFlags.flTKeyFFT_Bit;

										if (key16bit || keynocompressbit)
                                        {
											MessageBox.Show("Build-in motions are in " + (keynocompressbit ? "no compression" : "16 bit compression") + " format, not supported in SoC.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
											break;
										}
									}

									id++;
									xr_loader.SetStream(temp);
								}
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
				OGF_V.m_model_type = OGF_V.Static();
			else if (Current_OMF == null && !IsTextCorrect(MotionRefsBox.Text))
				OGF_V.m_model_type = OGF_V.Skeleton();
			else
				OGF_V.m_model_type = OGF_V.Animated();

			// Апдейтим экспорт аним тут, т.к. при любом изменении омф вызывается эта функция
			omfToolStripMenuItem.Enabled = Current_OMF != null;
			sklToolStripMenuItem.Enabled = Current_OMF != null;
			sklsToolStripMenuItem.Enabled = Current_OMF != null;
		}

		private void UpdateModelFormat()
		{
			CurrentFormat.Enabled = (OGF_V != null && !OGF_V.IsDM && OGF_V.IsSkeleton());

			if (!CurrentFormat.Enabled)
			{
				CurrentFormat.Text = "Model Format: All";
				return;
			}

			uint links = 0;

			foreach (var ch in OGF_V.childs)
				links = Math.Max(links, ch.links);

			OGF_V.IsCopModel = (IsTextCorrect(MotionRefsBox.Text) && OGF_V.motion_refs != null && !OGF_V.motion_refs.soc || !IsTextCorrect(MotionRefsBox.Text)) && links < 0x12071980;

			CurrentFormat.Text = "Model Format: " + (OGF_V.IsCopModel ? "CoP" : "SoC");
		}

		private void editImOMFEditorToolStripMenuItem_Click(object sender, EventArgs e)
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
				fileStream.Write(Current_OMF, 0, Current_OMF.Length);
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

		private void openSkeletonInObjectEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string Filename = TempFolder() + $"\\{StatusFile.Text}_temp.ogf";
			string ObjectName = Filename.Substring(0, Filename.LastIndexOf('.'));
			ObjectName = ObjectName.Substring(0, ObjectName.LastIndexOf('.')) + ".object";

			string ObjectEditor = pSettings.Load("ObjectEditorPath");

			if (!File.Exists(ObjectEditor))
			{
				MessageBox.Show("Please, set Object Editor path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

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

			Settings ProgramSettings = new Settings(pSettings);
			ProgramSettings.ShowDialog();

			string game_mtl = "";
			pSettings.Load("GameMtlPath", ref game_mtl);

			if (old_game_mtl != game_mtl)
				ReloadGameMtl(game_mtl);
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
					if (OpenFile(file, ref OGF_V, ref Current_OGF, ref Current_OMF))
					{
						FILE_NAME = file;
						AfterLoad(true);
					}
					break;
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
                RemoveBone("root_stalker");
                RemoveBone("bip01");

                ChangeParent("root_stalker", "bip01_pelvis");
                ChangeParent("bip01", "bip01_pelvis");

                OGF_V.bones.parent_bone_names[0] = "";

                for (int i = 0; i < OGF_V.ikdata.position.Count; i++)
                {
                    OGF_V.ikdata.position[i] = Resources.SoCSkeleton.Pos(i);
                    OGF_V.ikdata.rotation[i] = Resources.SoCSkeleton.Rot(i);
                    OGF_V.ikdata.center_mass[i] = RotateZ(OGF_V.ikdata.center_mass[i]); // Нужно тестить
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
                AddBone("root_stalker", "", 0);
                AddBone("bip01", "root_stalker", 1);

                for (int i = 0; i < OGF_V.ikdata.position.Count; i++)
                {
                    OGF_V.ikdata.position[i] = Resources.CoPSkeleton.Pos(i);
                    OGF_V.ikdata.rotation[i] = Resources.CoPSkeleton.Rot(i);
                    OGF_V.ikdata.center_mass[i] = RotateZ(OGF_V.ikdata.center_mass[i]); // Нужно тестить
                }

                foreach (var ch in OGF_V.childs)
                {
                    for (int i = 0; i < ch.Vertices.Count; i++)
                    {
                        for (int j = 0; j < ch.LinksCount(); j++)
                        {
                            ch.Vertices[i].bones_id[j] = ch.Vertices[i].bones_id[j] + 2;
                        }

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

        void Msg(string text)
        {
			MessageBox.Show(text);
        }

        private byte[] GetVec3Bytes(float[] vec)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(vec[0]));
            bytes.AddRange(BitConverter.GetBytes(vec[1]));
            bytes.AddRange(BitConverter.GetBytes(vec[2]));
            return bytes.ToArray();
        }

        private byte[] GetVec2Bytes(float[] vec)
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
					SaveTools(ObjName, 6, true);

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
			if (ViewPortAlpha)
				disableAlphaToolStripMenuItem.Text = "Disable Alpha";
			else
				disableAlphaToolStripMenuItem.Text = "Enable Alpha";

			InitViewPort(false, true, true);
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
			GroupBox.Text = "Mesh: [" + idx + "]";
			GroupBox.Name = "TextureGrpBox_" + idx;
			GroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			GroupBox.Dock = TexturesGropuBox.Dock;
			CreateTextureBoxes(idx, GroupBox);
			CreateTextureLabels(idx, GroupBox);
			TexturesPage.Controls.Add(GroupBox);
		}

		private void CreateTextureBoxes(int idx, GroupBox box)
		{
			var newTextBox = new TextBox();
			newTextBox.Name = "textureBox_" + idx;
			newTextBox.Size = TexturesTextBoxEx.Size;
			newTextBox.Location = TexturesTextBoxEx.Location;
			newTextBox.TextChanged += new System.EventHandler(this.TextBoxFilter);
			newTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

			var newTextBox2 = new TextBox();
			newTextBox2.Name = "shaderBox_" + idx;
			newTextBox2.Size = ShaderTextBoxEx.Size;
			newTextBox2.Location = ShaderTextBoxEx.Location;
			newTextBox2.TextChanged += new System.EventHandler(this.TextBoxFilter);
			newTextBox2.Anchor = AnchorStyles.Left | AnchorStyles.Right;;

			var newButton = new Button();
			newButton.Name = "DeleteButton_" + idx;
			newButton.Size = DeleteMesh.Size;
			newButton.Location = DeleteMesh.Location;
			newButton.Click += new System.EventHandler(this.ButtonFilter);
			newButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			newButton.Text = DeleteMesh.Text;

			box.Controls.Add(newTextBox);
			box.Controls.Add(newTextBox2);
			box.Controls.Add(newButton);
		}

		private void CreateTextureLabels(int idx, GroupBox box)
		{
			var newLbl = new Label();
			newLbl.Name = "textureLbl_" + idx;
			newLbl.Text = "Texture Path:";
			newLbl.Location = TexturesPathLabelEx.Location;

			var newLbl2 = new Label();
			newLbl2.Name = "shaderLbl_" + idx;
			newLbl2.Text = "Shader Name:";
			newLbl2.Location = ShaderNameLabelEx.Location;

			var newLbl3 = new Label();
			newLbl3.Name = "FacesLbl_" + idx;
			newLbl3.Text = FaceLabel.Text + OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString();
			newLbl3.Size = new Size(FaceLabel.Size.Width + (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), FaceLabel.Size.Height);
			newLbl3.Location = new Point(FaceLabel.Location.X - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), FaceLabel.Location.Y);
			newLbl3.Anchor = FaceLabel.Anchor;
			newLbl3.TextAlign = FaceLabel.TextAlign;

			var newLbl4 = new Label();
			newLbl4.Name = "VertsLbl_" + idx;
			newLbl4.Text = VertsLabel.Text + OGF_V.childs[idx].Vertices.Count.ToString();
			newLbl4.Size = new Size(VertsLabel.Size.Width + (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6), VertsLabel.Size.Height);
			newLbl4.Location = new Point(VertsLabel.Location.X - (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6) - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6), VertsLabel.Location.Y);
			newLbl4.Anchor = VertsLabel.Anchor;
			newLbl4.TextAlign = VertsLabel.TextAlign;

			var newLbl5 = new Label();
			newLbl5.Name = "LinksLbl_" + idx;
			newLbl5.Text = LinksLabel.Text + OGF_V.childs[idx].LinksCount().ToString();
			newLbl5.Size = new Size(LinksLabel.Size.Width + (OGF_V.childs[idx].LinksCount().ToString().Length * 6), LinksLabel.Size.Height);
			newLbl5.Location = new Point(LinksLabel.Location.X - (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6) - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6) - (OGF_V.childs[idx].LinksCount().ToString().Length * 6), LinksLabel.Location.Y);
			newLbl5.Anchor = LinksLabel.Anchor;
			newLbl5.TextAlign = LinksLabel.TextAlign;

			var newLbl6 = new Label();
			newLbl6.Name = "LodsLbl_" + idx;
			newLbl6.Text = LodLabel.Text + OGF_V.childs[idx].SWI.Count.ToString();
			newLbl6.Size = new Size(LodLabel.Size.Width + (OGF_V.childs[idx].SWI.Count.ToString().Length * 6), LodLabel.Size.Height);
			newLbl6.Location = new Point(LodLabel.Location.X - (OGF_V.childs[idx].Vertices.Count.ToString().Length * 6) - (OGF_V.childs[idx].Faces_SWI(CurrentLod).Count.ToString().Length * 6) - (OGF_V.childs[idx].LinksCount().ToString().Length * 6) - (OGF_V.childs[idx].SWI.Count.ToString().Length * 6), LodLabel.Location.Y);
			newLbl6.Anchor = LodLabel.Anchor;
			newLbl6.TextAlign = LodLabel.TextAlign;

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
			GroupBox.Text = "Bone id: [" + idx + "]";
			GroupBox.Name = "BoneGrpBox_" + idx;
			GroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			GroupBox.Dock = BoneParamsGroupBox.Dock;

			CreateBoneTextBox(idx, GroupBox, bone_name, parent_bone_name, material, mass, center, pos, rot);
			BoneParamsPage.Controls.Add(GroupBox);
		}

		private void CreateBoneTextBox(int idx, GroupBox box, string bone_name, string parent_bone_name, string material, float mass, float[] center, float[] pos, float[] rot)
		{
			var BoneNameTextBox = new TextBox();
			BoneNameTextBox.Name = "boneBox_" + idx;
			BoneNameTextBox.Size = BoneNameTextBoxEx.Size;
			BoneNameTextBox.Location = BoneNameTextBoxEx.Location;
			BoneNameTextBox.Text = bone_name;
			BoneNameTextBox.Tag = "string";
			BoneNameTextBox.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			BoneNameTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			BoneNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

			var BoneNameLabel = new Label();
			BoneNameLabel.Name = "boneLabel_" + idx;
			BoneNameLabel.Size = BoneNameLabelEx.Size;
			BoneNameLabel.Location = BoneNameLabelEx.Location;
			BoneNameLabel.Text = "Bone Name:";

			var ParentBoneNameTextBox = new TextBox();
			ParentBoneNameTextBox.Name = "ParentboneBox_" + idx;
			ParentBoneNameTextBox.Size = ParentBoneTextBoxEx.Size;
			ParentBoneNameTextBox.Location = ParentBoneTextBoxEx.Location;
			ParentBoneNameTextBox.Text = parent_bone_name;
			ParentBoneNameTextBox.Tag = "string";
			ParentBoneNameTextBox.ReadOnly = true;
			ParentBoneNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

			var ParentBoneNameLabel = new Label();
			ParentBoneNameLabel.Name = "ParentboneLabel_" + idx;
			ParentBoneNameLabel.Size = ParentBoneLabelEx.Size;
			ParentBoneNameLabel.Location = ParentBoneLabelEx.Location;
			ParentBoneNameLabel.Text = "Parent Bone:";

			var MateriaBox = new Control();
			if (game_materials.Count() == 0)
			{
				var MaterialTextBox = new TextBox();
				MaterialTextBox.Name = "MaterialBox_" + idx;
				MaterialTextBox.Size = MaterialTextBoxEx.Size;
				MaterialTextBox.Location = MaterialTextBoxEx.Location;
				MaterialTextBox.Text = material;
				MaterialTextBox.Tag = "string";
				MaterialTextBox.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
				MaterialTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
				MaterialTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

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

			var MaterialLabel = new Label();
			MaterialLabel.Name = "MaterialLabel_" + idx;
			MaterialLabel.Size = MaterialLabelEx.Size;
			MaterialLabel.Location = MaterialLabelEx.Location;
			MaterialLabel.Text = "Material:";

			var MassTextBox = new TextBox();
			MassTextBox.Name = "MassBox_" + idx;
			MassTextBox.Size = MassTextBoxEx.Size;
			MassTextBox.Location = MassTextBoxEx.Location;
			MassTextBox.Text = CheckNaN(mass);
			MassTextBox.Tag = "float";
			MassTextBox.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			MassTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			MassTextBox.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var MassLabel = new Label();
			MassLabel.Name = "MassLabel_" + idx;
			MassLabel.Size = MassLabelEx.Size;
			MassLabel.Location = MassLabelEx.Location;
			MassLabel.Text = "Mass:";

			var LayoutPanel = new TableLayoutPanel();
			LayoutPanel.Name = "LayoutPanel_" + idx;
			LayoutPanel.Size = BonesParamsPanel.Size;
			LayoutPanel.Location = BonesParamsPanel.Location;
			LayoutPanel.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			LayoutPanel.RowCount = 4;
			for (int x = 0; x < LayoutPanel.RowCount; x++)
				LayoutPanel.RowStyles.Add(new RowStyle() { Height = 25.0f, SizeType = SizeType.Percent });

			LayoutPanel.ColumnCount = 3;
			for (int x = 0; x < LayoutPanel.ColumnCount; x++)
				LayoutPanel.ColumnStyles.Add(new ColumnStyle() { Width = 33.33f, SizeType = SizeType.Percent });

			var CenterMassTextBoxX = new TextBox();
			CenterMassTextBoxX.Name = "CenterBoxX_" + idx;
			CenterMassTextBoxX.Size = CenterOfMassXTextBox.Size;
			CenterMassTextBoxX.Location = CenterOfMassXTextBox.Location;
			CenterMassTextBoxX.Text = CheckNaN(center[0]);
			CenterMassTextBoxX.Tag = "float";
			CenterMassTextBoxX.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			CenterMassTextBoxX.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var CenterMassTextBoxY = new TextBox();
			CenterMassTextBoxY.Name = "CenterBoxY_" + idx;
			CenterMassTextBoxY.Size = CenterOfMassYTextBox.Size;
			CenterMassTextBoxY.Location = CenterOfMassYTextBox.Location;
			CenterMassTextBoxY.Text = CheckNaN(center[1]);
			CenterMassTextBoxY.Tag = "float";
			CenterMassTextBoxY.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			CenterMassTextBoxY.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var CenterMassTextBoxZ = new TextBox();
			CenterMassTextBoxZ.Name = "CenterBoxZ_" + idx;
			CenterMassTextBoxZ.Size = CenterOfMassZTextBox.Size;
			CenterMassTextBoxZ.Location = CenterOfMassZTextBox.Location;
			CenterMassTextBoxZ.Text = CheckNaN(center[2]);
			CenterMassTextBoxZ.Tag = "float";
			CenterMassTextBoxZ.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			CenterMassTextBoxZ.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var CenterMassLabel = new Label();
			CenterMassLabel.Name = "CenterMassLabel_" + idx;
			CenterMassLabel.Size = CenterOfMassLabelEx.Size;
			CenterMassLabel.Location = CenterOfMassLabelEx.Location;
			CenterMassLabel.Text = "Center of Mass:";

			var PositionX = new TextBox();
			PositionX.Name = "PositionX_" + idx;
			PositionX.Size = PositionXTextBox.Size;
			PositionX.Location = PositionXTextBox.Location;
			PositionX.Text = CheckNaN(pos[0]);
			PositionX.Tag = "float";
			PositionX.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			PositionX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			PositionX.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var PositionY = new TextBox();
			PositionY.Name = "PositionY_" + idx;
			PositionY.Size = PositionYTextBox.Size;
			PositionY.Location = PositionYTextBox.Location;
			PositionY.Text = CheckNaN(pos[1]);
			PositionY.Tag = "float";
			PositionY.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			PositionY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			PositionY.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var PositionZ = new TextBox();
			PositionZ.Name = "PositionZ_" + idx;
			PositionZ.Size = PositionZTextBox.Size;
			PositionZ.Location = PositionZTextBox.Location;
			PositionZ.Text = CheckNaN(pos[2]);
			PositionZ.Tag = "float";
			PositionZ.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			PositionZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			PositionZ.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var PositionLabel = new Label();
			PositionLabel.Name = "PositionLabel_" + idx;
			PositionLabel.Size = PositionLabelEx.Size;
			PositionLabel.Location = PositionLabelEx.Location;
			PositionLabel.Text = "Position:";

			var RotationX = new TextBox();
			RotationX.Name = "RotationX_" + idx;
			RotationX.Size = RotationXTextBox.Size;
			RotationX.Location = RotationXTextBox.Location;
			RotationX.Text = CheckNaN(rot[0]);
			RotationX.Tag = "float";
			RotationX.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			RotationX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			RotationX.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var RotationY = new TextBox();
			RotationY.Name = "RotationY_" + idx;
			RotationY.Size = RotationYTextBox.Size;
			RotationY.Location = RotationYTextBox.Location;
			RotationY.Text = CheckNaN(rot[1]);
			RotationY.Tag = "float";
			RotationY.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			RotationY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			RotationY.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var RotationZ = new TextBox();
			RotationZ.Name = "RotationZ_" + idx;
			RotationZ.Size = RotationZTextBox.Size;
			RotationZ.Location = RotationZTextBox.Location;
			RotationZ.Text = CheckNaN(rot[2]);
			RotationZ.Tag = "float";
			RotationZ.TextChanged += new System.EventHandler(this.TextBoxBonesFilter);
			RotationZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			RotationZ.Anchor = AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right;

			var RotationLabel = new Label();
			RotationLabel.Name = "RotationLabel_" + idx;
			RotationLabel.Size = RotationLabelEx.Size;
			RotationLabel.Location = RotationLabelEx.Location;
			RotationLabel.Text = "Rotation:";

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
