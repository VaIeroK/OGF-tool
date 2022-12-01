using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using GitHubUpdate;

namespace OGF_tool
{
    public partial class Settings : Form
    {
        private EditorSettings pSettings = null;

        public Settings(EditorSettings settings)
        {
            InitializeComponent();
            pSettings = settings;

            BoxTextChanged(GameMtlPath, null);
            BoxTextChanged(FSLtxPath, null);
            BoxTextChanged(TexturesPath, null);
            BoxTextChanged(ImagePath, null);
            BoxTextChanged(OmfEditorPath, null);
            BoxTextChanged(ObjectEditorPath, null);

            ActiveControl = label4;
        }

        public void SaveParams()
        {
            pSettings.SaveVersion();
            pSettings.Save(GameMtlPath);
            pSettings.Save(FSLtxPath);
            pSettings.Save(TexturesPath);
            pSettings.Save(ImagePath);
            pSettings.Save(OmfEditorPath);
            pSettings.Save(ObjectEditorPath);
            pSettings.Save(ViewportAlpha);
        }

        public void Settings_Load(object sender, EventArgs e)
        {
            pSettings.Load(GameMtlPath);
            pSettings.Load(FSLtxPath);
            pSettings.Load(TexturesPath);
            pSettings.Load(ImagePath);
            pSettings.Load(OmfEditorPath);
            pSettings.Load(ObjectEditorPath);
            pSettings.Load(ViewportAlpha);
        }

        private string GetFSPath(string filename, string key)
        {
            using (StreamReader file = new StreamReader(filename))
            {
                string ln;
                string path = "";
                while ((ln = file.ReadLine()) != null)
                {
                    if (ln.Contains(key))
                    {
                        int separator_count = 0;
                        foreach (char c in ln)
                        {
                            if (separator_count == 3)
                            {
                                path += c;
                            }

                            if (c == '|')
                                separator_count++;
                        }

                        return GetCorrectString(path);
                    }
                }
                file.Close();
            }

            return null;
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

        private void FindImagePath(object sender, EventArgs e)
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            if (folderSelectDialog.ShowDialog())
            {
                string fname = folderSelectDialog.FileName;
                int slash_idx = fname.LastIndexOf('\\');
                if (slash_idx == fname.Count() - 1)
                    fname = fname.Substring(0, fname.LastIndexOf('\\'));
                ImagePath.Text = fname;
            }
        }

        private void FindFsPath(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "Ltx file|*.ltx";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FSLtxPath.Text = ofd.FileName;

                if (GetFSPath(ofd.FileName, "$game_data$") != null)
                {
                    string gamedata_path = ofd.FileName.Substring(0, ofd.FileName.LastIndexOf('\\')) + "\\" + GetFSPath(ofd.FileName, "$game_data$");

                    if (File.Exists(gamedata_path + "gamemtl.xr"))
                    {
                        GameMtlPath.Text = gamedata_path + "gamemtl.xr";
                    }

                    if (GetFSPath(ofd.FileName, "$game_textures$") != null)
                    {
                        TexturesPath.Text = gamedata_path + GetFSPath(ofd.FileName, "$game_textures$");

                        int slash_idx = TexturesPath.Text.LastIndexOf('\\');
                        if (slash_idx == TexturesPath.Text.Count() - 1)
                            TexturesPath.Text = TexturesPath.Text.Substring(0, TexturesPath.Text.LastIndexOf('\\'));
                    }
                }
            }
        }

        private void FindTexturesPath(object sender, EventArgs e)
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            if (folderSelectDialog.ShowDialog())
            {
                string fname = folderSelectDialog.FileName;
                int slash_idx = fname.LastIndexOf('\\');
                if (slash_idx == fname.Count() - 1)
                    fname = fname.Substring(0, fname.LastIndexOf('\\'));
                TexturesPath.Text = fname;
            }
        }

        private void FindGameMtlPath(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "Xr file|*.xr";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                GameMtlPath.Text = ofd.FileName;
            }
        }

        private void FindOmfEditor(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "Exe file|*.exe";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OmfEditorPath.Text = ofd.FileName;
            }
        }

        private void FindObjectEditor(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "Exe file|*.exe";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ObjectEditorPath.Text = ofd.FileName;
            }
        }

        private void FsPathTextChanged(object sender, EventArgs e)
        {
            BoxTextChanged(sender, e);

            if (Path.GetExtension(FSLtxPath.Text) == ".ltx" && File.Exists(FSLtxPath.Text))
            {
                string FileName = FSLtxPath.Text;

                if (GetFSPath(FileName, "$game_data$") != null)
                {
                    string gamedata_path = FileName.Substring(0, FileName.LastIndexOf('\\')) + "\\" + GetFSPath(FileName, "$game_data$");

                    if (File.Exists(gamedata_path + "gamemtl.xr"))
                    {
                        GameMtlPath.Text = gamedata_path + "gamemtl.xr";
                    }

                    if (GetFSPath(FileName, "$game_textures$") != null)
                    {
                        TexturesPath.Text = gamedata_path + GetFSPath(FileName, "$game_textures$");

                        int slash_idx = TexturesPath.Text.LastIndexOf('\\');
                        if (slash_idx == TexturesPath.Text.Count() - 1)
                            TexturesPath.Text = TexturesPath.Text.Substring(0, TexturesPath.Text.LastIndexOf('\\'));
                    }
                }
            }
        }

        private void BoxTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            bool file_or_folder = false;

            switch (textBox.Name)
            {
                case "ImagePath":
                    file_or_folder = false;
                    break;
                case "FSLtxPath":
                    file_or_folder = true;
                    break;
                case "TexturesPath":
                    file_or_folder = false;
                    break;
                case "GameMtlPath":
                    file_or_folder = true;
                    break;
                case "OmfEditorPath":
                    file_or_folder = true;
                    break;
                case "ObjectEditorPath":
                    file_or_folder = true;
                    break;
            }

            if (file_or_folder)
            {
                if (File.Exists(textBox.Text))
                    textBox.BackColor = SystemColors.Window;
                else
                    textBox.BackColor = Color.FromArgb(255, 255, 128, 128);
            }
            else
            {
                if (Directory.Exists(textBox.Text))
                    textBox.BackColor = SystemColors.Window;
                else
                    textBox.BackColor = Color.FromArgb(255, 255, 128, 128);

                string symbols = "./\\";

                for (int i = 0; i < symbols.Length; i++)
                {
                    int symbol_idx = textBox.Text.LastIndexOf(symbols[i]);
                    if (textBox.Text.Contains(symbols[i]) && symbol_idx == textBox.Text.Count() - 1)
                    {
                        textBox.BackColor = Color.FromArgb(255, 255, 128, 128);
                    }
                }
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            SaveParams();
            Close();
        }

        private void CheckUpdBnt_Click(object sender, EventArgs e)
        {
            string user = "VaIeroK";
            string repo = "OGF-tool";
            string vers = OGF_Editor.PROGRAM_VERSION;
            string asset = "OGF.Editor.";

            try
            {
                UpdateChecker checker;
                checker = new UpdateChecker(user, repo, vers);

                Button button = null;
                if (sender != null)
                {
                    button = sender as Button;
                    button.Enabled = false;
                }
                checker.CheckUpdate().ContinueWith((continuation) =>
                {
                    Invoke(new Action(() => // Go back to the UI thread
                    {
                        if (button != null)
                            button.Enabled = true;
                        if (continuation.Result != UpdateType.None)
                        {
                            var result = new UpdateNotifyDialog(checker).ShowDialog();
                            if (result == DialogResult.Yes)
                            {
                                checker.DownloadAsset(asset);
                            }
                        }
                        else if (e != null)
                        {
                            AutoClosingMessageBox.Show("Up to date!", "OGF Editor", 4000, MessageBoxIcon.Information);
                        }
                    }));
                });
            }
            catch (Exception) { }
        }
    }
}
