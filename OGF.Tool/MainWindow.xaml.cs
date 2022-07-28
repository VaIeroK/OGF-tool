using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OGF_Tool
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public static class FrameworkElementHelper
	{
		public static void RemoveFromParent(this FrameworkElement item)
		{
			if (item != null)
			{
				//var parentItemsControl = (ItemsControl)item.Parent;
				var parentItemsControl = (Grid)item.Parent;
				if (parentItemsControl != null)
				{
					//parentItemsControl.Items.Remove(item as UIElement);
					parentItemsControl.Children.Remove(item as UIElement);
				}
			}
		}
	}
	public partial class MainWindow : Window
    {
		OpenFileDialog OpenOGFDialog =  new OpenFileDialog();
		OpenFileDialog OpenOGF_DmDialog = new OpenFileDialog();
		OpenFileDialog OpenOMFDialog = new OpenFileDialog();
		OpenFileDialog OpenProgramDialog = new OpenFileDialog();

		SaveFileDialog SaveObjectDialog = new SaveFileDialog();
		SaveFileDialog SaveAsDialog = new SaveFileDialog();
		SaveFileDialog SaveSklsDialog = new SaveFileDialog();
		SaveFileDialog SaveOmfDialog = new SaveFileDialog();
		SaveFileDialog SaveBonesDialog = new SaveFileDialog();

		// File sytem
		public OGF_Children OGF_V			= null;
        public byte[]		Current_OGF		= null;
        public byte[]		Current_OMF		= null;
        public List<byte>	file_bytes		= new List<byte>();
        public string		FILE_NAME		= "";
        IniFile				Settings		= null;
		//FolderSelectDialog	SaveSklDialog	= null;
		FolderSelectDialog SaveSklDialog = null;


		// Input
		public	bool		bKeyIsDown		= false;
        string				number_mask		= "";

        [DllImport("converter.dll")]
        private static extern int CSharpStartAgent(string path, string out_path, int mode, int convert_to_mode, string motion_list);

        private int RunConverter(string path, string out_path, int mode, int convert_to_mode)
        {
            string dll_path = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\converter.dll";
            if (File.Exists(dll_path))
                return CSharpStartAgent(path, out_path, mode, convert_to_mode, "");
            else
            {
                MessageBox.Show("Can't find converter.dll", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

		private void SetUpDialogs()
        {
			OpenOGFDialog.Filter = "OGF file|*.ogf";
			OpenOGF_DmDialog.Filter = "OGF file|*.ogf|DM file|*.dm";
			SaveObjectDialog.Filter = "Object file|*object";
			SaveAsDialog.Filter = "OGF file|*.ogf|Object file|*.object|Bones file|*.bones|Skl file|*.skl|Skls file|*.skls|OMF file|*.omf";
			OpenOMFDialog.Filter = "OMF file|*.omf";
			OpenProgramDialog.Filter = "Program|*.exe";
			SaveSklsDialog.Filter = "Skls file|*.skls";
			SaveOmfDialog.Filter = "OMF file|*omf";
			SaveBonesDialog.Filter = "Bones file|*bones";

		}

        public MainWindow()
        {
            InitializeComponent();

			SetUpDialogs();

			number_mask = @"^-[0-9.]*$";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            oGFInfoToolStripMenuItem.IsEnabled = false;
            viewToolStripMenuItem.IsEnabled = false;
            SaveMenuParam.IsEnabled = false;
            saveAsToolStripMenuItem.IsEnabled = false;
			//motionToolsToolStripMenuItem.Enabled = false; //!G
			//openSkeletonInObjectEditorToolStripMenuItem.Enabled = false; //!G
			toolStripMenuItem1.IsEnabled = false;
            exportToolStripMenuItem.IsEnabled = false;
            LabelBroken.Visibility = Visibility.Collapsed;

            SaveSklDialog = new FolderSelectDialog();

            string file_path = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\Settings.ini";
            Settings = new IniFile(file_path);

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
				TabControl_w.Items.Clear();
            }

            if (Directory.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp"))
                Directory.Delete(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp", true);

			OpenOMFDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.AppendMotion);
		}

        private void Clear(bool ui_only)
        {
            if (!ui_only)
            {
                FILE_NAME = "";
                OGF_V = null;
                file_bytes.Clear();
            }
            TexturesPage.Content = null;
            BoneParamsPage.Content = null;
            TabControl_w.Items.Clear();
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
				StatusFile.Content = FILE_NAME.Substring(FILE_NAME.LastIndexOf('\\') + 1);

				oGFInfoToolStripMenuItem.IsEnabled = true;
				SaveMenuParam.IsEnabled = true;
				saveAsToolStripMenuItem.IsEnabled = true;
				toolStripMenuItem1.IsEnabled = !OGF_V.IsDM;
				oGFInfoToolStripMenuItem.IsEnabled = !OGF_V.IsDM;
				openSkeletonInObjectEditorToolStripMenuItem.IsEnabled = OGF_V.IsSkeleton();
				viewToolStripMenuItem.IsEnabled = OGF_V.IsSkeleton();
				exportToolStripMenuItem.IsEnabled = true;
				bonesToolStripMenuItem.IsEnabled = OGF_V.IsSkeleton();
				omfToolStripMenuItem.IsEnabled = Current_OMF != null;
				sklToolStripMenuItem.IsEnabled = Current_OMF != null;
				sklsToolStripMenuItem.IsEnabled = Current_OMF != null;

				OpenOGFDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				OpenOGF_DmDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveAsDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveAsDialog.FileName = StatusFile.Content.ToString().Substring(0, StatusFile.Content.ToString().LastIndexOf('.'));
				OpenOMFDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				OpenProgramDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveSklDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveSklsDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveSklsDialog.FileName = StatusFile.Content.ToString().Substring(0, StatusFile.Content.ToString().LastIndexOf('.')) + ".skls";
				SaveOmfDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveOmfDialog.FileName = StatusFile.Content.ToString().Substring(0, StatusFile.Content.ToString().LastIndexOf('.')) + ".omf";
				SaveBonesDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveBonesDialog.FileName = StatusFile.Content.ToString().Substring(0, StatusFile.Content.ToString().LastIndexOf('.')) + ".bones";
				SaveObjectDialog.InitialDirectory = FILE_NAME.Substring(0, FILE_NAME.LastIndexOf('\\'));
				SaveObjectDialog.FileName = StatusFile.Content.ToString().Substring(0, StatusFile.Content.ToString().LastIndexOf('.')) + ".object";
			}

			// Textures
			TabControl_w.Items.Add(TexturesPage);

			if (OGF_V.IsSkeleton())
			{
				FrameworkElementHelper.RemoveFromParent(UserDataBox);
				FrameworkElementHelper.RemoveFromParent(CreateUserdataButton);
				FrameworkElementHelper.RemoveFromParent(MotionRefsBox);
				FrameworkElementHelper.RemoveFromParent(CreateMotionRefsButton);

				//Userdata
				TabControl_w.Items.Add(UserDataPage);
				Grid grid = new Grid();
				grid.Children.Add(UserDataBox);
				grid.Children.Add(CreateUserdataButton);
				UserDataPage.Content = grid;
				CreateUserdataButton.Visibility = Visibility.Collapsed;
				UserDataBox.Visibility = Visibility.Collapsed;

				if (OGF_V.userdata != null)
					UserDataBox.Visibility = Visibility.Visible;
				else
					CreateUserdataButton.Visibility = Visibility.Visible;

				// Motion Refs
				TabControl_w.Items.Add(MotionRefsPage);
				
				Grid grid1 = new Grid();
				grid1.Children.Add(MotionRefsBox);
				grid1.Children.Add(CreateMotionRefsButton);
				MotionRefsPage.Content = grid1;
				CreateMotionRefsButton.Visibility = Visibility.Collapsed;
				MotionRefsBox.Visibility = Visibility.Collapsed;

				if (OGF_V.motion_refs != null)
					MotionRefsBox.Visibility = Visibility.Visible;
				else
					CreateMotionRefsButton.Visibility = Visibility.Visible;

				// Motions
				TabControl_w.Items.Add(MotionPage);
				MotionBox.Text = OGF_V.motions;

				if (OGF_V.motions != "")
				{
					AppendOMFButton.Visibility = Visibility.Collapsed;
					MotionBox.Visibility = Visibility.Visible;
				}
				else
				{
					MotionBox.Visibility = Visibility.Collapsed;
					AppendOMFButton.Visibility = Visibility.Visible;
				}

				// Bones
				if (OGF_V.bones != null)
				{
					BoneNamesBox.Text = "";
					TabControl_w.Items.Add(BoneNamesPage);

					BoneNamesBox.Text += $"Bones count : {OGF_V.bones.bone_names.Count}\n\n";
					for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
					{
						BoneNamesBox.Text += $"{i + 1}. {OGF_V.bones.bone_names[i]}";

						if (i != OGF_V.bones.bone_names.Count - 1)
							BoneNamesBox.Text += "\n";
					}
				}

				// Ik Data
				TabControl_w.Items.Add(BoneParamsPage);

				// Lod
				TabControl_w.Items.Add(LodPage);

				if (OGF_V.lod != null)
				{
					CreateLodButton.Visibility = Visibility.Collapsed;
					LodPathBox.Text = OGF_V.lod.lod_path;
				}
				else
					CreateLodButton.Visibility = Visibility.Visible;
			}
			ScrollViewer TextureGroupScrollViewer = new ScrollViewer();
			StackPanel TextureGroupPanel = new StackPanel();
			for (int i = 0; i < OGF_V.childs.Count; i++)
			{
				CreateTextureGroupBox(i, TextureGroupPanel);

				var box_ch = (GroupBox)TextureGroupPanel.Children[TextureGroupPanel.Children.Count - 1];

				if (box_ch != null)
				{
					var Cntrl = (TextBox)((StackPanel)box_ch.Content).Children[1];
					Cntrl.Text = OGF_V.childs[i].m_texture;
					var Cntrl2 = (TextBox)((StackPanel)box_ch.Content).Children[3];
					Cntrl2.Text = OGF_V.childs[i].m_shader;
				}
			}
			TextureGroupScrollViewer.Content = TextureGroupPanel;
			TexturesPage.Content = TextureGroupScrollViewer;


			if (OGF_V.bones != null)
			{
				ScrollViewer a = new ScrollViewer();
				StackPanel b = new StackPanel();

				for (int i = 0; i < OGF_V.bones.bone_names.Count; i++)
				{
					
					CreateBoneGroupBox(b, i, OGF_V.bones.bone_names[i], OGF_V.bones.parent_bone_names[i], OGF_V.ikdata.materials[i], OGF_V.ikdata.mass[i], OGF_V.ikdata.center_mass[i], OGF_V.ikdata.position[i], OGF_V.ikdata.rotation[i]);
					
				}
				a.Content = b;
				BoneParamsPage.Content = (a);
			}

			MotionRefsBox.Clear();
			UserDataBox.Clear();

			if (OGF_V.motion_refs != null) 
				MotionRefsBox.Text = String.Join("\n", OGF_V.motion_refs.refs.ToArray());

			if (OGF_V.userdata != null)
				UserDataBox.Text = OGF_V.userdata.userdata;

			if (main_file && !OGF_V.IsDM)
			{
				LabelBroken.Content = "Broken type: " + OGF_V.BrokenType.ToString();
				LabelBroken.Visibility = (OGF_V.BrokenType > 0) ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		private void CopyParams()
		{
			if (OGF_V.motion_refs != null)
			{
				OGF_V.motion_refs.refs.Clear();

				if (IsTextCorrect(MotionRefsBox.Text))
				{
					for (int i = 0; i < MotionRefsBox.LineCount; i++)
					{
						if (IsTextCorrect(MotionRefsBox.GetLineText(i)))
							OGF_V.motion_refs.refs.Add(GetCorrectString(MotionRefsBox.GetLineText(i)));
					}

					if (OGF_V.motion_refs.refs.Count() > 1)
						OGF_V.motion_refs.v3 = false;
				}
			}

			if (OGF_V.userdata != null)
			{
				OGF_V.userdata.userdata = "";

				if (IsTextCorrect(UserDataBox.Text))
				{
					List<string> kostil = new List<string>();
					kostil = UserDataBox.Text.Split('\n').ToList();
					if (kostil.Count > 1)
                    for (int i = 0; i < kostil.Count-1; i++) { kostil[i]= kostil[i].Remove(kostil[i].Length - 1);}

					for (int i = 0; i < kostil.Count; i++)
					{
						string ext = i == kostil.Count - 1 ? "" : "\r\n";
						OGF_V.userdata.userdata += kostil[i] + ext;
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
			if (BkpCheckBox.IsChecked == true)
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

				fileStream.ReadBytes(8);
				file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_HEADER));
				file_bytes.AddRange(BitConverter.GetBytes((uint)44));

				fileStream.ReadBytes(2);
				file_bytes.Add(OGF_V.m_version);
				file_bytes.Add(OGF_V.m_model_type);

				byte[] temp = fileStream.ReadBytes(2);
				file_bytes.AddRange(temp);

				temp = fileStream.ReadBytes(40);
				if (OGF_V.BrokenType == 2)
				{
					for (int i = 0; i < 40; i++)
						file_bytes.Add(0);
				}
				else
				{
					file_bytes.AddRange(temp);
				}

				bool old_byte = OGF_V.description.four_byte;
				if (OGF_V.BrokenType > 0) // Åñëè ìîäåëü ñëîìàíà, òî âîññòàíàâëèâàåì ÷àíê ñ 8 áàéòíûìè òàéìåðàìè
					OGF_V.description.four_byte = false;

				file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_DESC));
				file_bytes.AddRange(BitConverter.GetBytes(OGF_V.description.chunk_size()));
				file_bytes.AddRange(OGF_V.description.data());

				OGF_V.description.four_byte = old_byte; // Âîññòàíàâëèâàåì îòîáðàæåíèå êîëëè÷åñòâà áàéòîâ ó òàéìåðà

				fileStream.ReadBytes((int)(OGF_V.pos - fileStream.BaseStream.Position));

				fileStream.ReadBytes(4);
				uint new_size = fileStream.ReadUInt32();

				foreach (var ch in OGF_V.childs)
					new_size += ch.NewSize();

				file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_CHILDREN));
				file_bytes.AddRange(BitConverter.GetBytes(new_size));

				uint last_child_size = 0;
				foreach (var ch in OGF_V.childs)
				{
					temp = fileStream.ReadBytes((int)(ch.parent_pos - fileStream.BaseStream.Position));
					file_bytes.AddRange(temp);
					fileStream.ReadUInt32();
					new_size = fileStream.ReadUInt32();
					new_size += ch.NewSize();
					last_child_size = new_size;
					file_bytes.AddRange(BitConverter.GetBytes(ch.parent_id));
					file_bytes.AddRange(BitConverter.GetBytes(new_size));

					last_child_size -= (uint)(ch.pos - fileStream.BaseStream.Position);
					temp = fileStream.ReadBytes((int)(ch.pos - fileStream.BaseStream.Position));
					file_bytes.AddRange(temp);
					file_bytes.AddRange(ch.data());
					last_child_size -= (uint)(ch.old_size + 8);
					fileStream.BaseStream.Position += ch.old_size + 8;
				}

				if (OGF_V.IsSkeleton())
				{
					temp = fileStream.ReadBytes((int)(last_child_size));
					file_bytes.AddRange(temp);

					file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_BONE_NAMES));
					file_bytes.AddRange(BitConverter.GetBytes(OGF_V.bones.chunk_size()));
					file_bytes.AddRange(OGF_V.bones.data(false));

					fileStream.ReadBytes(OGF_V.bones.old_size + 8);

					file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_IKDATA));
					file_bytes.AddRange(BitConverter.GetBytes(OGF_V.ikdata.chunk_size()));
					file_bytes.AddRange(OGF_V.ikdata.data());

					fileStream.ReadBytes(OGF_V.ikdata.old_size + 8);

					if (OGF_V.userdata != null)
					{
						if (OGF_V.userdata.pos > 0 && (OGF_V.userdata.pos - fileStream.BaseStream.Position) > 0) // Äâèãàåìñÿ äî òåêóùåãî ÷àíêà
						{
							temp = fileStream.ReadBytes((int)(OGF_V.userdata.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.userdata.userdata != "") // Ïèøåì åñëè åñòü ÷òî ïèñàòü
						{
							file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_USERDATA));
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.userdata.chunk_size()));
							file_bytes.AddRange(OGF_V.userdata.data());
						}

						if (OGF_V.userdata.old_size > 0) // Ñäâèãàåì ïîçèöèþ ðèàäåðà åñëè â ìîäåëè áûë ÷àíê
							fileStream.ReadBytes(OGF_V.userdata.old_size + 8);
					}

					if (OGF_V.lod != null)
					{
						if (OGF_V.lod.pos > 0 && (OGF_V.lod.pos - fileStream.BaseStream.Position) > 0) // Äâèãàåìñÿ äî òåêóùåãî ÷àíêà
						{
							temp = fileStream.ReadBytes((int)(OGF_V.lod.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.lod.lod_path != "") // Ïèøåì åñëè åñòü ÷òî ïèñàòü
						{
							file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_LODS));
							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.lod.chunk_size()));
							file_bytes.AddRange(OGF_V.lod.data());
						}

						if (OGF_V.lod.old_size > 0) // Ñäâèãàåì ïîçèöèþ ðèàäåðà åñëè â ìîäåëè áûë ÷àíê
							fileStream.ReadBytes(OGF_V.lod.old_size + 8);
					}

					bool refs_created = false;
					if (OGF_V.motion_refs != null)
					{
						if (OGF_V.motion_refs.pos > 0 && (OGF_V.motion_refs.pos - fileStream.BaseStream.Position) > 0) // Äâèãàåìñÿ äî òåêóùåãî ÷àíêà
						{
							temp = fileStream.ReadBytes((int)(OGF_V.motion_refs.pos - fileStream.BaseStream.Position));
							file_bytes.AddRange(temp);
						}

						if (OGF_V.motion_refs.refs.Count > 0) // Ïèøåì åñëè åñòü ÷òî ïèñàòü
						{
							refs_created = true;

							if (!OGF_V.motion_refs.v3)
								file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_MOTION_REFS2));
							else
								file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_MOTION_REFS));

							file_bytes.AddRange(BitConverter.GetBytes(OGF_V.motion_refs.chunk_size(OGF_V.motion_refs.v3)));

							if (!OGF_V.motion_refs.v3)
								file_bytes.AddRange(OGF_V.motion_refs.count());

							file_bytes.AddRange(OGF_V.motion_refs.data(OGF_V.motion_refs.v3));
						}

						if (OGF_V.motion_refs.old_size > 0) // Ñäâèãàåì ïîçèöèþ ðèàäåðà åñëè â ìîäåëè áûë ÷àíê
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

			string format = System.IO.Path.GetExtension(filename);

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
					OGF_Child chld = new OGF_Child(0, 0, 0, shader.Length + texture.Length + 2, texture, shader);
					OGF_C.childs.Add(chld);
					return true;
				}

				if (!xr_loader.find_chunk((int)OGF.OGF4_HEADER, false, true))
				{
					MessageBox.Show("Unsupported OGF format! Can't find header chunk!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}
				else
				{
					OGF_C.m_version = xr_loader.ReadByte();
					OGF_C.m_model_type = xr_loader.ReadByte();
					xr_loader.ReadBytes(42);
				}

				uint DescriptionSize = xr_loader.find_chunkSize((int)OGF.OGF4_S_DESC, false, true);
				if (DescriptionSize == 0)
				{
					MessageBox.Show("Unsupported OGF format! Can't find description chunk!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}
				else
				{
					OGF_C.description = new Description();
					OGF_C.description.pos = xr_loader.chunk_pos;

					// ×èòàåì òàéìåðû â 8 áàéò
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

					if ((description_end_pos - reader_start_pos) != DescriptionSize) // Ðàçìåð íå ñîñòûêîâûâàåòñÿ, ïðîáóåì ÷èòàòü 4 áàéòà
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
						OGF_C.description.four_byte = true; // Ñòàâèì ôëàã íà òî ÷òî ìû ïðî÷èòàëè ÷àíê ñ 4õ áàéòíûìè òàéìåðàìè, åñëè ìîäåëü áóäåò ñëîìàíà òî ÷èíèòü ÷àíê áóäåì â 8 áàéò

						if ((description_end_pos - reader_start_pos) != DescriptionSize) // Âñå ðàâíî ðàçíûé ðàçìåð? Ïîõîäó ìîäåëü ñëîìàíà
						{
							OGF_C.BrokenType = 1;

							// ×èñòèì òàéìåðû, òàê êàê ïðî÷èòàíû áèòûå áàéòû
							OGF_C.description.m_export_time = 0;
							OGF_C.description.m_creation_time = 0;
							OGF_C.description.m_modified_time = 0;
						}
					}
				}

				if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF4_CHILDREN, false, true)))
				{
					MessageBox.Show("Unsupported OGF format! Can't find children chunk!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				OGF_C.pos = xr_loader.chunk_pos;

				int id = 0;
				uint size;

				// Texture && shader
				while (true)
				{
					if (!xr_loader.find_chunk(id)) break;

					Stream temp = xr_loader.reader.BaseStream;

					if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

					long pos = xr_loader.chunk_pos + OGF_C.pos + 16;
					size = xr_loader.find_chunkSize((int)OGF.OGF4_TEXTURE);

					if (size == 0) break;

					OGF_Child chld = new OGF_Child(xr_loader.chunk_pos + pos, id, pos - 8, (int)size, xr_loader.read_stringZ(), xr_loader.read_stringZ());
					OGF_C.childs.Add(chld);

					id++;
					xr_loader.SetStream(temp);
				}

				xr_loader.SetStream(r.BaseStream);

				if (OGF_C.IsSkeleton())
				{
					// Bones
					if (!xr_loader.find_chunk((int)OGF.OGF4_S_BONE_NAMES, false, true))
					{
						MessageBox.Show("Unsupported OGF format! Can't find bones chunk!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return false;
					}
					else
					{
						OGF_C.bones = new BoneData();

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
					if (!xr_loader.find_chunk((int)OGF.OGF4_S_IKDATA, false, true))
					{
						MessageBox.Show("Unsupported OGF format! Can't find ik data chunk!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return false;
					}
					else
					{
						OGF_C.ikdata = new IK_Data();

						for (int i = 0; i < OGF_C.bones.bone_names.Count; i++)
						{
							List<byte[]> bytes_1 = new List<byte[]>();
							OGF_C.ikdata.old_size += 4;

							byte[] temp_byte;
							uint version = xr_loader.ReadUInt32();
							string gmtl_name = xr_loader.read_stringZ();

							temp_byte = xr_loader.ReadBytes(112);   // struct SBoneShape
							bytes_1.Add(temp_byte);
							OGF_C.ikdata.old_size += gmtl_name.Length + 1 + 112;

							// Import
							{
								temp_byte = xr_loader.ReadBytes(4);
								bytes_1.Add(temp_byte);
								temp_byte = xr_loader.ReadBytes(16 * 3);
								bytes_1.Add(temp_byte);
								temp_byte = xr_loader.ReadBytes(4);
								bytes_1.Add(temp_byte);
								temp_byte = xr_loader.ReadBytes(4);
								bytes_1.Add(temp_byte);
								temp_byte = xr_loader.ReadBytes(4);
								bytes_1.Add(temp_byte);
								temp_byte = xr_loader.ReadBytes(4);
								bytes_1.Add(temp_byte);
								temp_byte = xr_loader.ReadBytes(4);
								bytes_1.Add(temp_byte);

								OGF_C.ikdata.old_size += 4 + 16 * 3 + 4 + 4 + 4 + 4 + 4;

								if (version > 0)
								{
									temp_byte = xr_loader.ReadBytes(4);
									bytes_1.Add(temp_byte);
									OGF_C.ikdata.old_size += 4;
								}
							}

							Fvector rotation = new Fvector();
							rotation.x = xr_loader.ReadFloat();
							rotation.y = xr_loader.ReadFloat();
							rotation.z = xr_loader.ReadFloat();

							Fvector position = new Fvector();
							position.x = xr_loader.ReadFloat();
							position.y = xr_loader.ReadFloat();
							position.z = xr_loader.ReadFloat();

							float mass = xr_loader.ReadFloat();

							Fvector center = new Fvector();
							center.x = xr_loader.ReadFloat();
							center.y = xr_loader.ReadFloat();
							center.z = xr_loader.ReadFloat();

							OGF_C.ikdata.old_size += 12 + 12 + 4 + 12;

							OGF_C.ikdata.materials.Add(gmtl_name);
							OGF_C.ikdata.mass.Add(mass);
							OGF_C.ikdata.version.Add(version);
							OGF_C.ikdata.center_mass.Add(center);
							OGF_C.ikdata.bytes_1.Add(bytes_1);
							OGF_C.ikdata.position.Add(position);
							OGF_C.ikdata.rotation.Add(rotation);
						}
					}

					// Userdata
					if (xr_loader.find_chunk((int)OGF.OGF4_S_USERDATA, false, true))
					{
						OGF_C.userdata = new UserData();
						OGF_C.userdata.pos = xr_loader.chunk_pos;
						OGF_C.userdata.userdata = xr_loader.read_stringZ();
						OGF_C.userdata.old_size = OGF_C.userdata.userdata.Length + 1;
					}

					// Lod ref
					if (xr_loader.find_chunk((int)OGF.OGF4_S_LODS, false, true))
					{
						OGF_C.lod = new Lod();
						OGF_C.lod.pos = xr_loader.chunk_pos;
						OGF_C.lod.lod_path = xr_loader.read_stringData(ref OGF_C.lod.data_str);
						OGF_C.lod.old_size = OGF_C.lod.lod_path.Length + (OGF_C.lod.data_str ? 2 : 1);
					}

					// Motion Refs
					bool v3 = xr_loader.find_chunk((int)OGF.OGF4_S_MOTION_REFS, false, true);

					if (v3 || xr_loader.find_chunk((int)OGF.OGF4_S_MOTION_REFS2, false, true))
					{
						OGF_C.motion_refs = new MotionRefs();
						OGF_C.motion_refs.pos = xr_loader.chunk_pos;
						OGF_C.motion_refs.refs = new List<string>();

						if (v3)
						{
							OGF_C.motion_refs.v3 = true;

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
					if (xr_loader.find_chunk((int)OGF.OGF4_S_MOTIONS, false, true))
					{
						xr_loader.reader.BaseStream.Position -= 8;
						Cur_OMF = xr_loader.ReadBytes((int)xr_loader.reader.BaseStream.Length - (int)xr_loader.reader.BaseStream.Position);
					}

					if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF4_S_MOTIONS, false, true)))
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

		private void TextBoxKeyDown(object sender, KeyEventArgs e)
		{
			bKeyIsDown = true;
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
			TextBox curBox = sender as TextBox;

			string currentField = curBox.Name.ToString().Split('_')[0];
			int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);

			switch (curBox.Tag.ToString())
			{
				case "float":
					{
						if (bKeyIsDown)
						{
							if (curBox.Text.Length == 0)
								return;

							int temp = curBox.SelectionStart;
							string mask = number_mask;
							Regex.Match(curBox.Text, mask);

							try
							{
								Convert.ToSingle(curBox.Text);
							}
							catch (Exception)
							{
								switch (currentField)
								{
									case "MassBox": curBox.Text = OGF_V.ikdata.mass[idx].ToString(); break;
									case "CenterBoxX": curBox.Text = OGF_V.ikdata.center_mass[idx].x.ToString(); break;
									case "CenterBoxY": curBox.Text = OGF_V.ikdata.center_mass[idx].y.ToString(); break;
									case "CenterBoxZ": curBox.Text = OGF_V.ikdata.center_mass[idx].z.ToString(); break;
									case "PositionX": curBox.Text = OGF_V.ikdata.position[idx].x.ToString(); break;
									case "PositionY": curBox.Text = OGF_V.ikdata.position[idx].y.ToString(); break;
									case "PositionZ": curBox.Text = OGF_V.ikdata.position[idx].z.ToString(); break;
									case "RotationX": curBox.Text = OGF_V.ikdata.rotation[idx].x.ToString(); break;
									case "RotationY": curBox.Text = OGF_V.ikdata.rotation[idx].y.ToString(); break;
									case "RotationZ": curBox.Text = OGF_V.ikdata.rotation[idx].z.ToString(); break;
								}

								if (curBox.SelectionStart < 1)
									curBox.SelectionStart = curBox.Text.Length;

								curBox.SelectionStart = temp - 1;
							}
						}
					}
					break;
			}

			Fvector vec = new Fvector();
			switch (currentField)
			{
				case "boneBox":
					{
						string old_name = OGF_V.bones.bone_names[idx];
						OGF_V.bones.bone_names[idx] = curBox.Text;

						for (int i = 0; i < OGF_V.bones.parent_bone_names.Count; i++)
						{
							var BoneParamsPage_count = ((BoneParamsPage.Content as ScrollViewer).Content as StackPanel).Children.Count;
							if (BoneParamsPage_count < i) continue;
							for (int j = 0; j < OGF_V.bones.bone_childs[idx].Count; j++)
							{
								if (OGF_V.bones.bone_childs[idx][j] == i)
								{
									var MainGroup = (((BoneParamsPage.Content) as ScrollViewer).Content as StackPanel).Children[i] as GroupBox;
									OGF_V.bones.parent_bone_names[i] = curBox.Text;
									((MainGroup.Content as Grid).Children[2] as TextBox).Text = OGF_V.bones.parent_bone_names[i];
								}
							}
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
				case "MaterialBox": OGF_V.ikdata.materials[idx] = curBox.Text; break;
				case "MassBox": OGF_V.ikdata.mass[idx] = Convert.ToSingle(curBox.Text); break;
				case "CenterBoxX": vec.x = Convert.ToSingle(curBox.Text); vec.y = OGF_V.ikdata.center_mass[idx].y; vec.z = OGF_V.ikdata.center_mass[idx].z; OGF_V.ikdata.center_mass[idx] = vec; break;
				case "CenterBoxY": vec.x = OGF_V.ikdata.center_mass[idx].x; vec.y = Convert.ToSingle(curBox.Text); vec.z = OGF_V.ikdata.center_mass[idx].z; OGF_V.ikdata.center_mass[idx] = vec; break;
				case "CenterBoxZ": vec.x = OGF_V.ikdata.center_mass[idx].x; vec.y = OGF_V.ikdata.center_mass[idx].y; vec.z = Convert.ToSingle(curBox.Text); OGF_V.ikdata.center_mass[idx] = vec; break;
				case "PositionX": vec.x = Convert.ToSingle(curBox.Text); vec.y = OGF_V.ikdata.position[idx].y; vec.z = OGF_V.ikdata.position[idx].z; OGF_V.ikdata.position[idx] = vec; break;
				case "PositionY": vec.x = OGF_V.ikdata.position[idx].x; vec.y = Convert.ToSingle(curBox.Text); vec.z = OGF_V.ikdata.position[idx].z; OGF_V.ikdata.position[idx] = vec; break;
				case "PositionZ": vec.x = OGF_V.ikdata.position[idx].x; vec.y = OGF_V.ikdata.position[idx].y; vec.z = Convert.ToSingle(curBox.Text); OGF_V.ikdata.position[idx] = vec; break;
				case "RotationX": vec.x = Convert.ToSingle(curBox.Text); vec.y = OGF_V.ikdata.rotation[idx].y; vec.z = OGF_V.ikdata.rotation[idx].z; OGF_V.ikdata.rotation[idx] = vec; break;
				case "RotationY": vec.x = OGF_V.ikdata.rotation[idx].x; vec.y = Convert.ToSingle(curBox.Text); vec.z = OGF_V.ikdata.rotation[idx].z; OGF_V.ikdata.rotation[idx] = vec; break;
				case "RotationZ": vec.x = OGF_V.ikdata.rotation[idx].x; vec.y = OGF_V.ikdata.rotation[idx].y; vec.z = Convert.ToSingle(curBox.Text); OGF_V.ikdata.rotation[idx] = vec; break;
			}

			bKeyIsDown = false;
		}

		private void saveToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (!SaveMenuParam.IsEnabled) return;
			if (FILE_NAME == "") return;

			CopyParams();
			SaveFile(FILE_NAME);
			AutoClosingMessageBox.Show(OGF_V.BrokenType > 0 ? "Repaired and Saved!" : "Saved!", "", OGF_V.BrokenType > 0 ? 700 : 500, MessageBoxImage.Information);
		}

		private void loadToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			OpenOGF_DmDialog.FileName = "";

			if (OpenOGF_DmDialog.ShowDialog() == true)
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

		private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			loadToolStripMenuItem_Click(null, null);
		}
		private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			saveAsToolStripMenuItem_Click(null, null);
		}
		private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			saveToolStripMenuItem_Click(null, null);
		}
		private void ReloadCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			reloadToolStripMenuItem_Click(null, null);
		}
		/* !DONE!
		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.S)
				saveToolStripMenuItem_Click(null, null);

			switch (e.KeyData)
			{
				case Keys.F3: reloadToolStripMenuItem_Click(null, null); break;
				case Keys.F4: loadToolStripMenuItem_Click(null, null); break;
				case Keys.F5: saveToolStripMenuItem_Click(null, null); break;
				case Keys.F6: saveAsToolStripMenuItem_Click(null, null); break;
			}
		}
		*/

		private void oGFInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

			//InfoWindow Info = new InfoWindow(OGF_V.description, OGF_V.m_version, OGF_V.m_model_type);
			//Info.ShowDialog();
			Description descr = new Description();
			bool res = InfoMessage.ShowHandlerDialog(OGF_V.description, OGF_V.m_version, OGF_V.m_model_type, ref descr);

			if (res)
			{
				OGF_V.description.m_source = descr.m_source;
				OGF_V.description.m_export_tool = descr.m_export_tool;
				OGF_V.description.m_owner_name = descr.m_owner_name;
				OGF_V.description.m_export_modif_name_tool = descr.m_export_modif_name_tool;
				OGF_V.description.m_creation_time = descr.m_creation_time;
				OGF_V.description.m_export_time = descr.m_export_time;
				OGF_V.description.m_modified_time = descr.m_modified_time;
				OGF_V.description.four_byte = descr.four_byte;
			}

			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			
		}

		private void saveAsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (!saveAsToolStripMenuItem.IsEnabled) return;

			if (OGF_V.IsDM)
				SaveAsDialog.Filter = "DM file|*.dm";
			else
				SaveAsDialog.Filter = "OGF file|*.ogf";

			if (SaveAsDialog.ShowDialog() == true)
			{
				SaveTools(SaveAsDialog.FileName, 0);
				SaveAsDialog.InitialDirectory = "";
			}
		}

		private void SaveTools(string filename, int format)
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
				AutoClosingMessageBox.Show(OGF_V.BrokenType > 0 ? "Repaired and Saved!" : "Saved!", "", OGF_V.BrokenType > 0 ? 700 : 500, MessageBoxImage.Information);
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
			if (!has_msg)
				AutoClosingMessageBox.Show(OGF_V.BrokenType > 0 ? "Repaired and Exported!" : "Exported!", "", OGF_V.BrokenType > 0 ? 700 : 500, MessageBoxImage.Information);
		}

		private void objectToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (SaveObjectDialog.ShowDialog() == true)
			{
				SaveTools(SaveObjectDialog.FileName, 1);
				SaveObjectDialog.InitialDirectory = "";
			}
		}

		private void bonesToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (SaveBonesDialog.ShowDialog() == true)
			{
				SaveTools(SaveBonesDialog.FileName, 2);
				SaveBonesDialog.InitialDirectory = "";
			}
		}

		private void omfToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (SaveOmfDialog.ShowDialog() == true)
			{
				SaveTools(SaveOmfDialog.FileName, 5);
				SaveOmfDialog.InitialDirectory = "";
			}
		}

		private void sklToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (SaveSklDialog.ShowDialog())
			{
				SaveTools(SaveSklDialog.FileName, 3);
				SaveSklDialog.InitialDirectory = "";
			}
		}

		private void sklsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (SaveSklsDialog.ShowDialog() == true)
			{
				SaveTools(SaveSklsDialog.FileName, 4);
				SaveSklsDialog.InitialDirectory = "";
			}
		}

		private void exitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to exit?", "OGF Editor", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				Close();
		}

		private void viewToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			string exe_path = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\OGFViewer.exe";
			if (File.Exists(exe_path))
			{
				System.Diagnostics.Process p = new System.Diagnostics.Process();
				p.StartInfo.FileName = exe_path;
				p.StartInfo.Arguments += FILE_NAME;
				p.Start();
			}
			else
			{
				MessageBox.Show("Can't find OGFViewer.exe in program folder.\nDownload OGF Viewer 1.0.2 or later!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CreateUserdataButton_Click(object sender, RoutedEventArgs e)
		{
			CreateUserdataButton.Visibility = Visibility.Collapsed;
			UserDataBox.Visibility = Visibility.Visible;
			UserDataBox.Clear();
			if (OGF_V.userdata == null)
				OGF_V.userdata = new UserData();
		}

		private void CreateMotionRefsButton_Click(object sender, RoutedEventArgs e)
		{
			if (Current_OMF == null || Current_OMF != null && MessageBox.Show("New motion refs chunk will remove built-in motions, continue?", "OGF Editor", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				// ×èñòèì âñå ñâÿçàííîå ñî âñòðîåííûìè àíèìàìè
				MotionBox.Clear();
				MotionBox.Visibility = Visibility.Collapsed;
				Current_OMF = null;
				AppendOMFButton.Visibility = Visibility.Visible;
				OGF_V.motions = "";

				// Îáíîâëÿåì òèï ìîäåëè
				UpdateModelType();

				// Îáíîâëÿåì âèçóàë èíòåðôåéñà ìîóøí ðåôîâ
				CreateMotionRefsButton.Visibility = Visibility.Collapsed;
				MotionRefsBox.Visibility = Visibility.Visible;
				MotionRefsBox.Clear();

				if (OGF_V.motion_refs == null)
					OGF_V.motion_refs = new MotionRefs();
			}
		}

		private void CreateLodButton_Click(object sender, RoutedEventArgs e)
		{
			CreateLodButton.Visibility = Visibility.Collapsed;
			LodPathBox.Clear();
			if (OGF_V.lod == null)
				OGF_V.lod = new Lod();
		}

		private void reloadToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (!reloadToolStripMenuItem.IsEnabled) return;

			string cur_fname = FILE_NAME;
			Clear(false);
			if (OpenFile(cur_fname, ref OGF_V, ref Current_OGF, ref Current_OMF))
			{
				FILE_NAME = cur_fname;
				AfterLoad(true);
			}
		}

		private void TabControl_SelectedIndexChanged(object sender, RoutedEventArgs e)
		{
			if (TabControl_w.SelectedIndex < 0) return;
			motionToolsToolStripMenuItem.IsEnabled = false;

			var a = (TabItem)TabControl_w.Items[TabControl_w.SelectedIndex];
			
			switch (a.Name)
			{
				case "UserDataPage":
					{
						if (!IsTextCorrect(UserDataBox.Text))
						{
							CreateUserdataButton.Visibility = Visibility.Visible;
							UserDataBox.Visibility = Visibility.Collapsed;
						}
						break;
					}
				case "MotionRefsPage":
					{
						if (!IsTextCorrect(MotionRefsBox.Text))
						{
							CreateMotionRefsButton.Visibility = Visibility.Visible;
							MotionRefsBox.Visibility = Visibility.Collapsed;
						}
						break;
					}
				case "MotionPage":
					{
						if (Current_OMF != null)
							motionToolsToolStripMenuItem.IsEnabled = true;
						break;
					}
				case "LodPage":
					{
						if (!IsTextCorrect(LodPathBox.Text))
						{
							CreateLodButton.Visibility = Visibility.Visible;
						}
						break;
					}
			}
		}

		private void RichTextBoxTextChanged(object sender, RoutedEventArgs e)
		{
			RichTextBox curBox = sender as RichTextBox;
			switch (curBox.Name)
			{
				case "MotionRefsBox":
					{
						UpdateModelType();
						break;
					}
			}
		}

		private void AppendOMFButton_Click(object sender, RoutedEventArgs e)
		{
			if (!IsTextCorrect(MotionRefsBox.Text) && (OGF_V.motion_refs == null || OGF_V.motion_refs.refs.Count() == 0) || (IsTextCorrect(MotionRefsBox.Text) || OGF_V.motion_refs != null && OGF_V.motion_refs.refs.Count() > 0) && MessageBox.Show("Build-in motions will remove motion refs, continue?", "OGF Editor", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				OpenOMFDialog.ShowDialog();
		}

		private void AppendMotion(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (sender != null)
				OpenOMFDialog.InitialDirectory = "";

			byte[] OpenedOmf = File.ReadAllBytes(OpenOMFDialog.FileName);

			var xr_loader = new XRayLoader();

			using (var r = new BinaryReader(new MemoryStream(OpenedOmf)))
			{
				xr_loader.SetStream(r.BaseStream);

				if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF4_S_MOTIONS, false, true)))
				{
					// Àïäåéòèì âèçóàë âñòðîåííûõ àíèìàöèé
					AppendOMFButton.Visibility = Visibility.Collapsed;
					MotionBox.Visibility = Visibility.Visible;
					motionToolsToolStripMenuItem.IsEnabled = true;

					// ×èñòèì âñòðîåííûå ðåôû, èíòåðôåéñ ïî÷èñòèòñÿ ñàì ïðè àêòèâàöèè âêëàäêè
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
		}

		private void deleteChunkToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			MotionBox.Visibility = Visibility.Collapsed;
			AppendOMFButton.Visibility = Visibility.Visible;
			motionToolsToolStripMenuItem.IsEnabled = false;
			Current_OMF = null;
			MotionBox.Clear();
			OGF_V.motions = "";
			UpdateModelType();
		}

		private void importDataFromModelToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			OpenOGFDialog.FileName = "";
			if (OpenOGFDialog.ShowDialog() == true)
			{
				bool UpdateUi = false;

				OGF_Children SecondOgf = null;
				byte[] SecondOgfByte = null;
				byte[] SecondOmfByte = null;
				OpenFile(OpenOGFDialog.FileName, ref SecondOgf, ref SecondOgfByte, ref SecondOmfByte);

				if (OGF_V.childs.Count == SecondOgf.childs.Count && MessageBox.Show("Import textures and shaders path?\nThey may have different positions", "OGF Editor", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
						Current_OMF = SecondOmfByte;
						UpdateUi = true;
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
					AutoClosingMessageBox.Show("OGF Params changed!", "", 1000, MessageBoxImage.Information);
				}
				else
				{
					AutoClosingMessageBox.Show("OGF Params don't changed!", "", 1000, MessageBoxImage.Error);
				}
			}
		}

		private void UpdateModelType()
		{
			if (OGF_V == null) return;

			if (OGF_V.bones == null)
				OGF_V.m_model_type = 1;
			else if (Current_OMF == null && !IsTextCorrect(MotionRefsBox.Text)) 
				OGF_V.m_model_type = 10;
			else
				OGF_V.m_model_type = 3;

			// Àïäåéòèì ýêñïîðò àíèì òóò, ò.ê. ïðè ëþáîì èçìåíåíèè îìô âûçûâàåòñÿ ýòà ôóíêöèÿ
			omfToolStripMenuItem.IsEnabled = Current_OMF != null;
			sklToolStripMenuItem.IsEnabled = Current_OMF != null;
			sklsToolStripMenuItem.IsEnabled = Current_OMF != null;
		}

		private void editImOMFEditorToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (!Directory.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp"))
				Directory.CreateDirectory(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp");

			string Filename = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + $"\\temp\\{StatusFile.Content}_temp.omf";
			string OmfEditor = GetOmfEditorPath();

			if (OmfEditor == null)
			{
				MessageBox.Show("Please, set OMF Editor path!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			using (var fileStream = new FileStream(Filename, FileMode.OpenOrCreate))
			{
				fileStream.Write(Current_OMF, 0, Current_OMF.Length);
			}

			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = OmfEditor;
			proc.StartInfo.Arguments += $"\"{Filename}\"";
			proc.Start();
			proc.WaitForExit();

			OpenOMFDialog.FileName = Filename;
			AppendMotion(null, null);
			OpenOMFDialog.FileName = "";

			File.Delete(Filename);

			if (Directory.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp"))
				Directory.Delete(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp", true);
		}

		private void openSkeletonInObjectEditorToolStripMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (!Directory.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp"))
				Directory.CreateDirectory(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp");

			string Filename = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + $"\\temp\\{StatusFile.Content}_temp.ogf";
			string ObjectName = Filename.Substring(0, Filename.LastIndexOf('.'));
			ObjectName = ObjectName.Substring(0, ObjectName.LastIndexOf('.')) + ".object";

			string ObjectEditor = GetObjectEditorPath();

			if (ObjectEditor == null || ObjectEditor == "")
			{
				MessageBox.Show("Please, set Object Editor path!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			File.Copy(FILE_NAME, Filename);
			CopyParams();
			SaveFile(Filename);
			RunConverter(Filename, ObjectName, 0, 0);

			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = ObjectEditor;
			proc.StartInfo.Arguments += $"\"{ObjectName}\" skeleton_only \"{FILE_NAME}\"";
			proc.Start();
			proc.WaitForExit();

			if (Directory.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp"))
				Directory.Delete(System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "\\temp", true);
		}

		private string GetOmfEditorPath()
		{
			string omf_editor_path = Settings.Read("omf_editor", "settings");
			if (!File.Exists(omf_editor_path))
			{
				MessageBox.Show("Please, open OMF Editor path", "", MessageBoxButton.OK, MessageBoxImage.Information);
				if (OpenProgramDialog.ShowDialog() == true)
				{
					OpenProgramDialog.InitialDirectory = "";
					omf_editor_path = OpenProgramDialog.FileName;
					Settings.Write("omf_editor", omf_editor_path, "settings");
				}
			}

			return omf_editor_path;
		}

		private string GetObjectEditorPath()
		{
			string object_editor_path = Settings.Read("object_editor", "settings");
			if (!File.Exists(object_editor_path))
			{
				MessageBox.Show("Please, open Object Editor path", "", MessageBoxButton.OK, MessageBoxImage.Information);
				if (OpenProgramDialog.ShowDialog() == true)
				{
					OpenProgramDialog.InitialDirectory = "";
					object_editor_path = OpenProgramDialog.FileName;
					Settings.Write("object_editor", object_editor_path, "settings");
				}
			}

			return object_editor_path;
		}

		string CheckNaN(string str)
		{
			if (str == "NaN")
				str = "0";
			return str;
		}

		private void RichTextBoxImgDefender(object sender, KeyEventArgs e)
		{
			/*
			RichTextBox TextBox = sender as RichTextBox;
			if (e.Control && e.KeyCode == System.Windows.Forms.Keys.V)
			{
				if (Clipboard.ContainsText())
					TextBox.Paste(DataFormats.GetFormat(DataFormats.Text));
				e.Handled = true;
			}
			*/
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

		// Interface
		private void CreateTextureGroupBox(int idx, StackPanel TextureGroupPanel)
		{

			
			var GroupBox = new GroupBox();
			GroupBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF939393");
			GroupBox.Style = this.FindResource("GroupBox_normal") as Style;
			GroupBox.Header = "Set: [" + idx + "]";
			GroupBox.Name = "TextureGrpBox_" + idx;

			StackPanel a = new StackPanel();
			GroupBox.Content = a;


			var newLbl = new Label();
			newLbl.Name = "textureLbl_" + idx;
			newLbl.Content = "Texture Path:";
			newLbl.Foreground = Brushes.White;
			a.Children.Add(newLbl);

			var newTextBox = new TextBox();
			newTextBox.Style = this.FindResource("TextBox_normal") as Style;
			newTextBox.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			newTextBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");


			newTextBox.Name = "textureBox_" + idx;
			newTextBox.TextChanged += new TextChangedEventHandler(this.TextBoxFilter);
			a.Children.Add(newTextBox);

			var newLbl2 = new Label();
			newLbl2.Name = "shaderLbl_" + idx;
			newLbl2.Content = "Shader Name:";
			newLbl2.Foreground = Brushes.White;
			a.Children.Add(newLbl2);

			var newTextBox2 = new TextBox();
			newTextBox2.Name = "shaderBox_" + idx;
			newTextBox2.TextChanged += new TextChangedEventHandler(this.TextBoxFilter);
			newTextBox2.Style = this.FindResource("TextBox_normal") as Style;
			newTextBox2.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			newTextBox2.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			a.Children.Add(newTextBox2);
			

			TextureGroupPanel.Children.Add(GroupBox);
		}

		private void CreateBoneGroupBox(StackPanel b, int idx, string bone_name, string parent_bone_name, string material, float mass, Fvector center, Fvector pos, Fvector rot)
		{
			var GroupBox = new GroupBox();
			GroupBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF939393");
			GroupBox.Style = this.FindResource("GroupBox_normal") as Style;
			//GroupBox.Location = new System.Drawing.Point(3, 3 + 205 * idx);
			//GroupBox.Size = new System.Drawing.Size(421, 203);
			GroupBox.Header = "Bone id: [" + idx + "]";
			GroupBox.Name = "BoneGrpBox_" + idx;
			GroupBox.Foreground = Brushes.White;
			//GroupBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

			CreateBoneTextBox(idx, GroupBox, bone_name, parent_bone_name, material, mass, center, pos, rot);
			b.Children.Add(GroupBox);
		}

		private void CreateBoneTextBox(int idx, GroupBox box, string bone_name, string parent_bone_name, string material, float mass, Fvector center, Fvector pos, Fvector rot)
		{

			Grid grid = new Grid();
			ColumnDefinition Col1 = new ColumnDefinition();
			ColumnDefinition Col2 = new ColumnDefinition();

			Col1.Width = new GridLength(100);
			grid.ColumnDefinitions.Add(Col1);

			Col2.Width = GridLength.Auto;
			grid.ColumnDefinitions.Add(Col2);

			RowDefinition row1 = new RowDefinition();
			RowDefinition row2 = new RowDefinition();
			RowDefinition row3 = new RowDefinition();
			RowDefinition row4 = new RowDefinition();
			RowDefinition row5 = new RowDefinition();
			RowDefinition row6 = new RowDefinition();
			RowDefinition row7 = new RowDefinition();
			RowDefinition row8 = new RowDefinition();
			row1.Height = GridLength.Auto;
			row2.Height = GridLength.Auto;
			row3.Height = GridLength.Auto;
			row4.Height = GridLength.Auto;
			row5.Height = GridLength.Auto;
			row6.Height = GridLength.Auto;
			row7.Height = GridLength.Auto;
			row8.Height = GridLength.Auto;

			grid.RowDefinitions.Add(row1);
			grid.RowDefinitions.Add(row2);
			grid.RowDefinitions.Add(row3);
			grid.RowDefinitions.Add(row4);
			grid.RowDefinitions.Add(row5);
			grid.RowDefinitions.Add(row6);
			grid.RowDefinitions.Add(row7);
			grid.RowDefinitions.Add(row8);

			StackPanel stackPanel1 = new StackPanel();
			StackPanel stackPanel2 = new StackPanel();
			StackPanel stackPanel3 = new StackPanel();
			stackPanel1.Orientation = Orientation.Horizontal;
			stackPanel1.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel2.Orientation = Orientation.Horizontal;
			stackPanel2.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel3.Orientation = Orientation.Horizontal;
			stackPanel3.HorizontalAlignment = HorizontalAlignment.Stretch;
			////


			var BoneNameTextBox = new TextBox();
			BoneNameTextBox.Name = "boneBox_" + idx;
			BoneNameTextBox.Style = this.FindResource("TextBox_normal") as Style;
			BoneNameTextBox.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			BoneNameTextBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			//BoneNameTextBox.Size = new System.Drawing.Size(326, 58);
			//BoneNameTextBox.Width = 326;
			//BoneNameTextBox.Height = 58;
			//BoneNameTextBox.Location = new System.Drawing.Point(86, 18);
			BoneNameTextBox.Text = bone_name;
			BoneNameTextBox.Tag = "string";
			BoneNameTextBox.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			BoneNameTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			//BoneNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			BoneNameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
			grid.Children.Add(BoneNameTextBox);
			Grid.SetColumn(BoneNameTextBox, 1);


			var BoneNameLabel = new Label();
			BoneNameLabel.Name = "boneLabel_" + idx;
			//BoneNameLabel.Size = new System.Drawing.Size(100, 20);
			//BoneNameLabel.Location = new System.Drawing.Point(6, 20);
			BoneNameLabel.Content = "Bone Name:";
			grid.Children.Add(BoneNameLabel);
			Grid.SetColumn(BoneNameLabel, 0);


			var ParentBoneNameTextBox = new TextBox();
			ParentBoneNameTextBox.Style = this.FindResource("TextBox_normal") as Style;
			ParentBoneNameTextBox.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			ParentBoneNameTextBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#828282");
			ParentBoneNameTextBox.Name = "ParentboneBox_" + idx;
			//ParentBoneNameTextBox.Size = new System.Drawing.Size(326, 58);
			//ParentBoneNameTextBox.Location = new System.Drawing.Point(86, 45);
			ParentBoneNameTextBox.Text = parent_bone_name;
			//ParentBoneNameTextBox.Width = 326;
			ParentBoneNameTextBox.Tag = "string";
			ParentBoneNameTextBox.IsReadOnly = true;
			ParentBoneNameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
			//ParentBoneNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			grid.Children.Add(ParentBoneNameTextBox);
			Grid.SetColumn(ParentBoneNameTextBox, 1);
			Grid.SetRow(ParentBoneNameTextBox, 1);


			var ParentBoneNameLabel = new Label();
			ParentBoneNameLabel.Name = "ParentboneLabel_" + idx;
			ParentBoneNameLabel.Content = "Parent Bone:";
			grid.Children.Add(ParentBoneNameLabel);
			Grid.SetRow(ParentBoneNameLabel, 1);

			var MaterialTextBox = new TextBox();
			MaterialTextBox.Style = this.FindResource("TextBox_normal") as Style;
			MaterialTextBox.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			MaterialTextBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			MaterialTextBox.Name = "MaterialBox_" + idx;
			//MaterialTextBox.Width = 326;
			MaterialTextBox.Text = material;
			MaterialTextBox.Tag = "string";
			MaterialTextBox.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			MaterialTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			MaterialTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
			grid.Children.Add(MaterialTextBox);
			Grid.SetColumn(MaterialTextBox, 1);
			Grid.SetRow(MaterialTextBox, 2);

			var MaterialLabel = new Label();
			MaterialLabel.Name = "MaterialLabel_" + idx;
			MaterialLabel.Content = "Material:";
			grid.Children.Add(MaterialLabel);
			Grid.SetRow(MaterialLabel, 2);

			var MassTextBox = new TextBox();
			MassTextBox.Style = this.FindResource("TextBox_normal") as Style;
			MassTextBox.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			MassTextBox.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			MassTextBox.Name = "MassBox_" + idx;
			MassTextBox.Width = 84;
			MassTextBox.Text = CheckNaN(((decimal)mass).ToString());
			MassTextBox.Tag = "float";
			MassTextBox.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			MassTextBox.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			MassTextBox.HorizontalAlignment = HorizontalAlignment.Left;
			grid.Children.Add(MassTextBox);
			Grid.SetColumn(MassTextBox, 1);
			Grid.SetRow(MassTextBox, 3);


			var MassLabel = new Label();
			MassLabel.Name = "MassLabel_" + idx;
			MassLabel.Content = "Mass:";
			grid.Children.Add(MassLabel);
			Grid.SetRow(MassLabel, 3);

			var CenterMassTextBoxX = new TextBox();
			CenterMassTextBoxX.Style = this.FindResource("TextBox_normal") as Style;
			CenterMassTextBoxX.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			CenterMassTextBoxX.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			CenterMassTextBoxX.Name = "CenterBoxX_" + idx;
			CenterMassTextBoxX.Width = 84;
			CenterMassTextBoxX.Text = CheckNaN(((decimal)center.x).ToString());
			CenterMassTextBoxX.Tag = "float";
			CenterMassTextBoxX.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel1.Children.Add(CenterMassTextBoxX);


			var CenterMassTextBoxY = new TextBox();
			CenterMassTextBoxY.Style = this.FindResource("TextBox_normal") as Style;
			CenterMassTextBoxY.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			CenterMassTextBoxY.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			CenterMassTextBoxY.Name = "CenterBoxY_" + idx;
			CenterMassTextBoxY.Width = 84;
			CenterMassTextBoxY.Text = CheckNaN(((decimal)center.y).ToString());
			CenterMassTextBoxY.Tag = "float";
			CenterMassTextBoxY.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel1.Children.Add(CenterMassTextBoxY);

			var CenterMassTextBoxZ = new TextBox();
			CenterMassTextBoxZ.Style = this.FindResource("TextBox_normal") as Style;
			CenterMassTextBoxZ.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			CenterMassTextBoxZ.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			CenterMassTextBoxZ.Name = "CenterBoxZ_" + idx;
			CenterMassTextBoxZ.Width = 84;
			CenterMassTextBoxZ.Text = CheckNaN(((decimal)center.z).ToString());
			CenterMassTextBoxZ.Tag = "float";
			CenterMassTextBoxZ.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			CenterMassTextBoxZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			
			stackPanel1.Children.Add(CenterMassTextBoxZ);
			grid.Children.Add(stackPanel1);
			Grid.SetColumn(stackPanel1, 1);
			Grid.SetRow(stackPanel1, 4);

			var CenterMassLabel = new Label();
			CenterMassLabel.Name = "CenterMassLabel_" + idx;
			//CenterMassLabel.Size = new System.Drawing.Size(100, 20);
			//CenterMassLabel.Location = new System.Drawing.Point(6, 127);
			CenterMassLabel.Content = "Center of Mass:";
			grid.Children.Add(CenterMassLabel);
			Grid.SetRow(CenterMassLabel, 4);

			var PositionX = new TextBox();
			PositionX.Style = this.FindResource("TextBox_normal") as Style;
			PositionX.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			PositionX.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			PositionX.Name = "PositionX_" + idx;
			PositionX.Width = 84;
			//PositionX.Size = new System.Drawing.Size(84, 58);

			//PositionX.Location = new System.Drawing.Point(86, 151);
			PositionX.Text = CheckNaN(((decimal)pos.x).ToString());
			PositionX.Tag = "float";
			PositionX.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			PositionX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel2.Children.Add(PositionX);

			var PositionY = new TextBox();
			PositionY.Style = this.FindResource("TextBox_normal") as Style;
			PositionY.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			PositionY.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			PositionY.Name = "PositionY_" + idx;
			PositionY.Width = 84;
			//PositionY.Size = new System.Drawing.Size(84, 58);
			//PositionY.Location = new System.Drawing.Point(182, 151);
			PositionY.Text = CheckNaN(((decimal)pos.y).ToString());
			PositionY.Tag = "float";
			PositionY.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			PositionY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel2.Children.Add(PositionY);


			var PositionZ = new TextBox();
			PositionZ.Style = this.FindResource("TextBox_normal") as Style;
			PositionZ.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			PositionZ.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			PositionZ.Name = "PositionZ_" + idx;
			PositionZ.Width = 84;
			//PositionZ.Size = new System.Drawing.Size(84, 58);
			//PositionZ.Location = new System.Drawing.Point(277, 151);
			PositionZ.Text = CheckNaN(((decimal)pos.z).ToString());
			PositionZ.Tag = "float";
			PositionZ.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			PositionZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel2.Children.Add(PositionZ);
			grid.Children.Add(stackPanel2);
			Grid.SetColumn(stackPanel2, 1);
			Grid.SetRow(stackPanel2, 5);

			var PositionLabel = new Label();
			PositionLabel.Name = "PositionLabel_" + idx;
			//PositionLabel.Size = new System.Drawing.Size(100, 20);
			//PositionLabel.Location = new System.Drawing.Point(6, 153);
			PositionLabel.Content = "Position:";
			grid.Children.Add(PositionLabel);
			Grid.SetRow(PositionLabel, 5);

			var RotationX = new TextBox();
			RotationX.Style = this.FindResource("TextBox_normal") as Style;
			RotationX.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			RotationX.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			RotationX.Name = "RotationX_" + idx;
			RotationX.Width = 84;
			//RotationX.Size = new System.Drawing.Size(84, 58);
			//RotationX.Location = new System.Drawing.Point(86, 177);
			RotationX.Text = CheckNaN(((decimal)rot.x).ToString());
			RotationX.Tag = "float";
			RotationX.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			RotationX.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel3.Children.Add(RotationX);


			var RotationY = new TextBox();
			RotationY.Style = this.FindResource("TextBox_normal") as Style;
			RotationY.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			RotationY.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			RotationY.Name = "RotationY_" + idx;
			RotationY.Width = 84;
			//RotationY.Size = new System.Drawing.Size(84, 58);
			//RotationY.Location = new System.Drawing.Point(182, 177);
			RotationY.Text = CheckNaN(((decimal)rot.y).ToString());
			RotationY.Tag = "float";
			RotationY.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			RotationY.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel3.Children.Add(RotationY);

			var RotationZ = new TextBox();
			RotationZ.Style = this.FindResource("TextBox_normal") as Style;
			RotationZ.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF292929");
			RotationZ.Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#e1e3e6");
			RotationZ.Name = "RotationZ_" + idx;
			RotationZ.Width = 84;
			//RotationZ.Size = new System.Drawing.Size(84, 58);
			//RotationZ.Location = new System.Drawing.Point(277, 177);
			RotationZ.Text = CheckNaN(((decimal)rot.z).ToString());
			RotationZ.Tag = "float";
			RotationZ.TextChanged += new TextChangedEventHandler(this.TextBoxBonesFilter);
			RotationZ.KeyDown += new KeyEventHandler(this.TextBoxKeyDown);
			stackPanel3.Children.Add(RotationZ);
			grid.Children.Add(stackPanel3);
			Grid.SetColumn(stackPanel3, 1);
			Grid.SetRow(stackPanel3, 6);

			var RotationLabel = new Label();
			RotationLabel.Name = "RotationLabel_" + idx;
			RotationLabel.Content = "Rotation:";
			grid.Children.Add(RotationLabel);
			Grid.SetRow(RotationLabel, 6);

			RotationLabel.Foreground = Brushes.White;
			PositionLabel.Foreground = Brushes.White;
			CenterMassLabel.Foreground = Brushes.White;
			MassLabel.Foreground = Brushes.White;
			MaterialLabel.Foreground = Brushes.White;
			BoneNameLabel.Foreground = Brushes.White;
			ParentBoneNameLabel.Foreground = Brushes.White;

			box.Content = grid;
		}

        private void TabControl_w_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (e.Source is TabControl)
			{
				TabControl_SelectedIndexChanged(sender, e);
			}
		}
    }
}
