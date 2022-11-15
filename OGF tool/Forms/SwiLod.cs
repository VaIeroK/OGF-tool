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
    public partial class SwiLod : Form
    {
        public float Lod = 0.0f;
        public bool res = false;
        public SwiLod(float lod)
        {
            InitializeComponent();
            LodBar.Value = (int)(lod * LodBar.Maximum);
            Lod = lod;

            ActiveControl = label1;
        }

        private void LodNum_ValueChanged(object sender, EventArgs e)
        {
            Lod = (float)LodBar.Value / (float)LodBar.Maximum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            res = true;
            Close();
        }
    }
}
