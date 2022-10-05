using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OGF_tool
{
    public partial class Settings : Form
    {
        private EditorSettings pSettings = null;

        public Settings(EditorSettings settings)
        {
            InitializeComponent();
            pSettings = settings;
        }

        public void SaveParams(object sender, FormClosingEventArgs e)
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
    }
}
