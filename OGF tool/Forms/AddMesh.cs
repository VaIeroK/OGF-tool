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
    public partial class AddMesh : Form
    {
        private OGF_Children OGF, LoadedOGF;

        public AddMesh(ref OGF_Children Main_OGF, OGF_Children Loaded_OGF)
        {
            InitializeComponent();

            MeshPanel.Controls.Clear();

            OGF = Main_OGF;
            LoadedOGF = Loaded_OGF;

            for (int i = LoadedOGF.childs.Count - 1; i >= 0; i--)
            {
                CreateMeshGroupBox(i);
            }
        }

        public void CreateMeshGroupBox(int idx)
        {
            var GroupBox = new GroupBox();
            GroupBox.Location = new System.Drawing.Point(MeshGroupBox.Location.X, MeshGroupBox.Location.Y + (MeshGroupBox.Size.Height + 2) * idx);
            GroupBox.Size = MeshGroupBox.Size;
            GroupBox.Text = MeshGroupBox.Text + " [" + idx + "]";
            GroupBox.Name = "MeshGrpBox_" + idx;
            GroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GroupBox.Dock = MeshGroupBox.Dock;
            CreateMeshElements(idx, GroupBox);
            MeshPanel.Controls.Add(GroupBox);
        }

        public void CreateMeshElements(int idx, GroupBox box)
        {
            var newLbl1 = Copy.Label(TextureNameLabel);
            var newLbl2 = Copy.Label(Bone1_Label);
            var newLbl3 = Copy.Label(Bone2_Label);
            var newLbl4 = Copy.Label(Bone3_Label);
            var newLbl5 = Copy.Label(Bone4_Label);

            var newCombo1 = Copy.ComboBox(Bone1_ComboBox);
            var newCombo2 = Copy.ComboBox(Bone2_ComboBox);
            var newCombo3 = Copy.ComboBox(Bone3_ComboBox);
            var newCombo4 = Copy.ComboBox(Bone4_ComboBox);

            box.Controls.Add(newLbl1);
            box.Controls.Add(newLbl2);
            box.Controls.Add(newLbl3);
            box.Controls.Add(newLbl4);
            box.Controls.Add(newLbl5);

            box.Controls.Add(newCombo1);
            box.Controls.Add(newCombo2);
            box.Controls.Add(newCombo3);
            box.Controls.Add(newCombo4);
        }
    }
}
