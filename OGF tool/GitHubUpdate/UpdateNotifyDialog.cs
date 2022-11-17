using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitHubUpdate
{
    public partial class UpdateNotifyDialog : Form
    {
        readonly UpdateChecker checker;
        bool loadednotes;

        public UpdateNotifyDialog(UpdateChecker checker)
        {
            this.checker = checker;

            InitializeComponent();

            label1.Text = string.Format(label1.Text, checker.repostoryName);
        }

        void panel1_Resize(object sender, EventArgs e)
        {
            label1.Left = (panel1.ClientSize.Width - label1.Width) / 2;
            label2.Left = (panel1.ClientSize.Width - label2.Width) / 2;
        }

        async void boxReleaseNotes_CheckedChanged(object sender, EventArgs e)
        {
            ReleaseNotesPanel.Visible = boxReleaseNotes.Checked;

            if (!loadednotes)
            {
                ReleaseNotes.DocumentText = await checker.RenderReleaseNotes();
                loadednotes = true;
            }
        }
    }
}
