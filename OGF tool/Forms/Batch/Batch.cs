using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace OGF_tool
{
    public partial class Batch : Form
    {
        public enum BatchChunks
        {
            Texture,
            Shader,
            UserData,
            MotionRefs,
            Lod
        }

        private OGF_Editor Editor;

        public Batch(OGF_Editor editor)
        {
            InitializeComponent();
            Editor = editor;

            PathTextBox.Text = Editor.pSettings.Load("BatchFolderPath");

            BoxTextChanged(PathTextBox, null);
        }

        static private bool SafeCheckLineExist(string[] arr, string chk_line)
        {
            foreach (string line in arr)
            {
                if (RemoveSpacesAndNewLines(line) == RemoveSpacesAndNewLines(chk_line))
                    return true;
            }

            return false;
        }

        static public bool ProcessReplace(OGF_Model OGF, BatchChunks chunk, string replacer, string new_val, ref uint lines_counter)
        {
            bool ret = false;
            switch (chunk)
            {
                case BatchChunks.Texture:
                    foreach (var ch in OGF.childs)
                    {
                        if (ch.m_texture == replacer)
                        {
                            ch.m_texture = new_val;
                            ret = true;
                            lines_counter++;
                        }
                    }
                    break;
                case BatchChunks.Shader:
                    foreach (var ch in OGF.childs)
                    {
                        if (ch.m_shader == replacer)
                        {
                            ch.m_shader = new_val;
                            ret = true;
                            lines_counter++;
                        }
                    }
                    break;
                case BatchChunks.UserData:
                    if (OGF.userdata != null)
                    {
                        string[] Lines = StringToStringArray(OGF.userdata.userdata);
                        string userdata = "";
                        foreach (string line in Lines)
                        {
                            if (RemoveSpacesAndNewLines(line) == RemoveSpacesAndNewLines(replacer))
                            {
                                userdata += new_val + "\r\n";
                                ret = true;
                                lines_counter++;
                            }
                            else
                                userdata += RemoveNewLines(line) + "\r\n";
                        }
                        if (ret)
                            OGF.userdata.userdata = userdata.TrimEnd(new char[] { '\r', '\n' });
                    }
                    break;
                case BatchChunks.MotionRefs:
                    if (OGF.motion_refs != null)
                    {
                        string[] Lines = OGF.motion_refs.refs.ToArray();
                        OGF.motion_refs.refs.Clear();
                        foreach (string line in Lines)
                        {
                            if (RemoveSpacesAndNewLines(line) == RemoveSpacesAndNewLines(replacer))
                            {
                                OGF.motion_refs.refs.Add(new_val);
                                ret = true;
                                lines_counter++;
                            }
                            else
                                OGF.motion_refs.refs.Add(line);
                        }
                    }
                    break;
                case BatchChunks.Lod:
                    if (OGF.lod != null && RemoveSpacesAndNewLines(OGF.lod.lod_path) == RemoveSpacesAndNewLines(replacer))
                    {
                        OGF.lod.lod_path = new_val;
                        ret = true;
                        lines_counter++;
                    }
                    break;
            }

            return ret;
        }

        static public bool ProcessAdd(OGF_Model OGF, BatchChunks chunk, string new_line, ref uint lines_counter, bool create_chunks)
        {
            bool ret = false;
            switch (chunk)
            {
                case BatchChunks.UserData:
                    if (OGF.userdata == null || !SafeCheckLineExist(StringToStringArray(OGF.userdata.userdata), new_line))
                    {
                        if (OGF.userdata != null)
                            OGF.userdata.userdata += "\r\n";

                        if (create_chunks && OGF.userdata == null)
                            OGF.userdata = new UserData();

                        if (OGF.userdata != null)
                        {
                            OGF.userdata.userdata += new_line;
                            ret = true;
                            lines_counter++;
                        }
                    }
                    break;
                case BatchChunks.MotionRefs:
                    if (OGF.motion_refs == null || !SafeCheckLineExist(OGF.motion_refs.refs.ToArray(), new_line))
                    {
                        if (create_chunks && OGF.motion_refs == null)
                            OGF.motion_refs = new MotionRefs();

                        if (OGF.motion_refs != null && !OGF.motion_refs.refs.Contains(new_line))
                        {
                            OGF.motion_refs.refs.Add(new_line);
                            ret = true;
                            lines_counter++;
                        }
                    }
                    break;
                case BatchChunks.Lod:
                    if (OGF.lod == null && create_chunks)
                    {
                        OGF.lod = new Lod();
                        OGF.lod.lod_path = new_line;
                        ret = true;
                        lines_counter++;
                    }
                    break;
            }

            return ret;
        }

        static public bool ProcessDelete(OGF_Model OGF, BatchChunks chunk, string delete_line, ref uint lines_counter)
        {
            bool ret = false;
            string clear_del_line = RemoveSpacesAndNewLines(delete_line);

            switch (chunk)
            {
                case BatchChunks.UserData:
                    if (OGF.userdata != null)
                    {
                        string[] Lines = StringToStringArray(OGF.userdata.userdata);
                        if (delete_line == "$all")
                        {
                            lines_counter += (uint)Lines.Length;
                            OGF.userdata.userdata = "";
                            ret = true;
                        }
                        else
                        {
                            string userdata = "";
                            foreach (string line in Lines)
                            {
                                if (RemoveSpacesAndNewLines(line) == RemoveSpacesAndNewLines(delete_line))
                                {
                                    ret = true;
                                    lines_counter++;
                                }
                                else
                                    userdata += RemoveNewLines(line) + "\r\n";
                            }
                            if (ret)
                                OGF.userdata.userdata = userdata.TrimEnd(new char[] { '\r', '\n' });
                        }
                    }
                    break;
                case BatchChunks.MotionRefs:
                    if (OGF.motion_refs != null)
                    {
                        if (delete_line == "$all")
                        {
                            lines_counter += (uint)OGF.motion_refs.refs.Count;
                            OGF.motion_refs.refs.Clear();
                            ret = true;
                        }
                        else
                        {
                            foreach (string line in OGF.motion_refs.refs)
                            {
                                if (RemoveSpacesAndNewLines(line) == clear_del_line)
                                {
                                    OGF.motion_refs.refs.Remove(line);
                                    ret = true;
                                    lines_counter++;
                                }
                            }
                        }
                    }
                    break;
                case BatchChunks.Lod:
                    if (OGF.lod != null)
                    {
                        if (RemoveSpacesAndNewLines(OGF.lod.lod_path) == clear_del_line || delete_line == "$all")
                            OGF.lod.lod_path = "";
                        ret = true;
                        lines_counter++;
                    }
                    break;
            }

            return ret;
        }

        static public string Capitalize(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        static public string UnCapitalize(string input)
        {
            return char.ToLower(input[0]) + input.Substring(1);
        }

        static public string RemoveSpacesAndNewLines(string input)
        {
            return input.Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }

        static public string RemoveNewLines(string input)
        {
            return input.Replace("\n", "").Replace("\r", "");
        }

        static public string[] StringToStringArray(string input)
        {
            return input.Split(new string[] { "\n" }, StringSplitOptions.None);
        }

        private void ProcessReplacer(BatchChunks chunk)
        {
            if (Directory.Exists(PathTextBox.Text))
            {
                ReplaceData textBoxReplacer = new ReplaceData(Editor, this, chunk, PathTextBox.Text);
                textBoxReplacer.Show();
            }
            else
                AutoClosingMessageBox.Show("Can't find ogf folder path!", "Error", 1000, MessageBoxIcon.Error);
        }

        private void ProcessAdd(BatchChunks chunk)
        {
            if (Directory.Exists(PathTextBox.Text))
            {
                AddDeleteData textBoxReplacer = new AddDeleteData(Editor, this, chunk, PathTextBox.Text, false);
                textBoxReplacer.Show();
            }
            else
                AutoClosingMessageBox.Show("Can't find ogf folder path!", "Error", 1000, MessageBoxIcon.Error);
        }

        private void ProcessDelete(BatchChunks chunk)
        {
            if (Directory.Exists(PathTextBox.Text))
            {
                AddDeleteData textBoxReplacer = new AddDeleteData(Editor, this, chunk, PathTextBox.Text, true);
                textBoxReplacer.Show();
            }
            else
                AutoClosingMessageBox.Show("Can't find ogf folder path!", "Error", 1000, MessageBoxIcon.Error);
        }

        private void TexturesReplaceButton_Click(object sender, EventArgs e)
        {
            ProcessReplacer(BatchChunks.Texture);
        }

        private void ShadersReplaceButton_Click(object sender, EventArgs e)
        {
            ProcessReplacer(BatchChunks.Shader);
        }

        private void UserDataReplaceButton_Click(object sender, EventArgs e)
        {
            ProcessReplacer(BatchChunks.UserData);
        }

        private void MotionRefsReplaceButton_Click(object sender, EventArgs e)
        {
            ProcessReplacer(BatchChunks.MotionRefs);
        }

        private void LodReplaceButton_Click(object sender, EventArgs e)
        {
            ProcessReplacer(BatchChunks.Lod);
        }

        private void UserDataAddButton_Click(object sender, EventArgs e)
        {
            ProcessAdd(BatchChunks.UserData);
        }

        private void MotionRefsAddButton_Click(object sender, EventArgs e)
        {
            ProcessAdd(BatchChunks.MotionRefs);
        }

        private void LodAddButton_Click(object sender, EventArgs e)
        {
            ProcessAdd(BatchChunks.Lod);
        }

        private void UserDataDeleteButton_Click(object sender, EventArgs e)
        {
            ProcessDelete(BatchChunks.UserData);
        }

        private void MotionRefsDeleteButton_Click(object sender, EventArgs e)
        {
            ProcessDelete(BatchChunks.MotionRefs);
        }

        private void LodDeleteButton_Click(object sender, EventArgs e)
        {
            ProcessDelete(BatchChunks.Lod);
        }

        private void FindPathButton_Click(object sender, EventArgs e)
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            if (folderSelectDialog.ShowDialog())
            {
                PathTextBox.Text = folderSelectDialog.FileName;
                Editor.pSettings.Save("BatchFolderPath", PathTextBox.Text);
            }
        }

        private void BoxTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

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
}
