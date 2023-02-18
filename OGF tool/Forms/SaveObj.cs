using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGF_tool
{
    public partial class SaveObj : Form
    {
        public OGF_Editor.ExportFormat Fmt;

        public SaveObj()
        {
            InitializeComponent();
            Fmt = OGF_Editor.ExportFormat.Unknown;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Fmt = OGF_Editor.ExportFormat.OGF;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Fmt = OGF_Editor.ExportFormat.DM;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Fmt = OGF_Editor.ExportFormat.Object;
            Close();
        }
    }
}
