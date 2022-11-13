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
        public SwiLod(float lod)
        {
            InitializeComponent();
            LodBar.Value = (int)(lod * LodBar.Maximum);

            ActiveControl = label1;
        }

        private void LodNum_ValueChanged(object sender, EventArgs e)
        {
            Lod = (float)LodBar.Value / (float)LodBar.Maximum;
        }
    }
}
