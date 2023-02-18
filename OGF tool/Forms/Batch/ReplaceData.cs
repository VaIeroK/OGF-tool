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

namespace OGF_tool
{
    public partial class ReplaceData : Form
    {
        private string folder;
        private Batch.BatchChunks Chunk;
        public ReplaceData(Batch.BatchChunks chunk, string ogf_folder_path)
        {
            InitializeComponent();
            folder = ogf_folder_path;
            Chunk = chunk;
            Text = "Replace " + Batch.Capitalize(chunk.ToString());

            BoxTextChanged(NewTextBox, null);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Editor.IsTextCorrect(NewTextBox.Text))
            {
                uint FilesCount = 0;
                uint LinesCount = 0;
                string[] files = Directory.GetFiles(folder, "*.ogf", SearchOption.AllDirectories);

                for (int i = 0; i < files.Length; i++)
                {
                    XRay_Model Model = new XRay_Model();
                    if (Model.OpenFile(files[i]))
                    {
                        if (Batch.ProcessReplace(Model, Chunk, ReplacerTextBox.Text, NewTextBox.Text, ref LinesCount))
                        {
                            Model.SaveFile(files[i]);
                            FilesCount++;
                        }
                    }
                }
                MessageBox.Show($"{LinesCount} lines changed in {FilesCount} files!", "Batch Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                AutoClosingMessageBox.Show("Please, fill in the \"Replace with\" field.", "Error", 1500, MessageBoxIcon.Error);
        }

        private void BoxTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (Editor.IsTextCorrect(textBox.Text))
                textBox.BackColor = SystemColors.Window;
            else
                textBox.BackColor = Color.FromArgb(255, 255, 128, 128);
        }
    }
}
