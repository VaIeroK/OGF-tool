using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OGF_tool
{
    public partial class SelectMeshes : Form
    {
        public List<bool> MeshChecked = new List<bool>();

        public SelectMeshes(OGF_Children OGF)
        {
            InitializeComponent();

            MeshPanel.Controls.Clear();

            for (int i = 0; i < OGF.childs.Count; i++)
            {
                var MeshCbx = Copy.CheckBox(MeshCheckBox);
                MeshCbx.Name = "MeshCheckBox_" + i;
                MeshCbx.Text += $" [{OGF.childs[i].m_texture}] | [{OGF.childs[i].m_shader}]";
                MeshCbx.Location = new Point(MeshCbx.Location.X, MeshCbx.Location.Y + 27 * i);
                MeshCbx.Size = new Size(1000, MeshCbx.Size.Height);

                if (i != OGF.childs.Count - 1)
                    Size = new Size(Size.Width, Size.Height + 27);

                MeshPanel.Controls.Add(MeshCbx);
                MeshChecked.Add(MeshCbx.Checked);
            }
        }

        private void ClosingCallback(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < MeshPanel.Controls.Count; i++)
            {
                if (MeshPanel.Controls[i] is CheckBox)
                {
                    CheckBox curBox = MeshPanel.Controls[i] as CheckBox;
                    int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);

                    MeshChecked[idx] = curBox.Checked;
                }
            }
        }
    }
}
