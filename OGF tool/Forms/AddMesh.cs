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
    public partial class AddMesh : Form
    {
        private OGF_Children OGF, LoadedOGF;
        int last_height = 0;
        List<bool> mesh_to_add_list = new List<bool>();

        public AddMesh(ref OGF_Children Main_OGF, OGF_Children Loaded_OGF)
        {
            InitializeComponent();

            MeshPanel.Controls.Clear();

            OGF = Main_OGF;
            LoadedOGF = Loaded_OGF;

            for (int i = 0; i < LoadedOGF.childs.Count; i++)
            {
                mesh_to_add_list.Add(false);
                CreateMeshGroupBox(i);
            }
        }

        public void CreateMeshGroupBox(int idx)
        {
            var GroupBox = new GroupBox();
            GroupBox.Location = new System.Drawing.Point(MeshGroupBox.Location.X, last_height);
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
            OGF_Child child = LoadedOGF.childs[idx];
            int links = (int)child.LinksCount();

            var newLbl1 = Copy.Label(Texture_Label);
            var newLbl2 = Copy.Label(Shader_Label);
            var newLbl3 = Copy.Label(Bone1_Label);
            var newLbl7 = Copy.Label(Reassign1_Label);

            var newCombo1 = FillComboBox(Copy.ComboBox(Bone1_ComboBox), child);
            int bone_id = OGF.bonedata.GetBoneID(LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[0]].name);
            if (bone_id != -1)
                newCombo1.SelectedIndex = bone_id;

            var newTextbox1 = Copy.TextBox(Texture_Textbox);
            newTextbox1.Text = child.m_texture;
            var newTextbox2 = Copy.TextBox(Shader_Textbox);
            newTextbox2.Text = child.m_shader;

            var newTextbox3 = Copy.TextBox(OldBone1_Textbox);
            newTextbox3.Text = LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[0]].name;

            var newButton = Copy.Button(AddMeshButton);
            newButton.Click += new System.EventHandler(this.AddMeshButton_Click);
            newButton.Name = "AddBnt_" + idx;

            box.Controls.Add(newLbl1);
            box.Controls.Add(newLbl2);
            box.Controls.Add(newLbl3);
            box.Controls.Add(newLbl7);

            box.Controls.Add(newCombo1);

            box.Controls.Add(newTextbox1);
            box.Controls.Add(newTextbox2);
            box.Controls.Add(newTextbox3);

            box.Controls.Add(newButton);

            if (links >= 2) // Сет для 2х и более костей
            {
                var newLbl4 = Copy.Label(Bone2_Label);
                var newLbl8 = Copy.Label(Reassign2_Label);
                var newCombo2 = FillComboBox(Copy.ComboBox(Bone2_ComboBox), child);
                var newTextbox4 = Copy.TextBox(OldBone2_Textbox);

                bone_id = OGF.bonedata.GetBoneID(LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[1]].name);
                if (bone_id != -1)
                    newCombo2.SelectedIndex = bone_id;

                newTextbox4.Text = LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[1]].name;

                box.Controls.Add(newLbl4);
                box.Controls.Add(newLbl8);
                box.Controls.Add(newCombo2);
                box.Controls.Add(newTextbox4);
            }

            if (links >= 3) // Сет для 3х и более костей
            {
                var newLbl5 = Copy.Label(Bone3_Label);
                var newLbl9 = Copy.Label(Reassign3_Label);
                var newCombo3 = FillComboBox(Copy.ComboBox(Bone3_ComboBox), child);
                var newTextbox5 = Copy.TextBox(OldBone3_Textbox);

                bone_id = OGF.bonedata.GetBoneID(LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[2]].name);
                if (bone_id != -1)
                    newCombo3.SelectedIndex = bone_id;

                newTextbox5.Text = LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[2]].name;

                box.Controls.Add(newLbl5);
                box.Controls.Add(newLbl9);
                box.Controls.Add(newCombo3);
                box.Controls.Add(newTextbox5);
            }

            if (links >= 4) // Сет для 4х и более костей
            {
                var newLbl6 = Copy.Label(Bone4_Label);
                var newLbl10 = Copy.Label(Reassign4_Label);
                var newCombo4 = FillComboBox(Copy.ComboBox(Bone4_ComboBox), child);
                var newTextbox6 = Copy.TextBox(OldBone4_Textbox);

                bone_id = OGF.bonedata.GetBoneID(LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[3]].name);
                if (bone_id != -1)
                    newCombo4.SelectedIndex = bone_id;

                newTextbox6.Text = LoadedOGF.bonedata.bones[(int)child.Vertices[0].bones_id[3]].name;

                box.Controls.Add(newLbl6);
                box.Controls.Add(newLbl10);
                box.Controls.Add(newCombo4);
                box.Controls.Add(newTextbox6);
            }

            switch (links)
            {
                case 1:
                    box.Size = new Size(box.Width, 130);
                    last_height += box.Size.Height + 2;
                    break;
                case 2:
                    box.Size = new Size(box.Width, 156);
                    last_height += box.Size.Height + 2;
                    break;
                case 3:
                    box.Size = new Size(box.Width, 182);
                    last_height += box.Size.Height + 2;
                    break;
                case 4:
                    last_height += box.Size.Height + 2;
                    break;
            }
        }

        public ComboBox FillComboBox(ComboBox box, OGF_Child child)
        {
            for (int i = 0; i < OGF.bonedata.bones.Count; i++)
                box.Items.Add(OGF.bonedata.bones[i].name);

            return box;
        }

        private void AddMeshButton_Click(object sender, EventArgs e)
        {
            Button curBox = sender as Button;

            string currentField = curBox.Name.ToString().Split('_')[0];
            int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);

            OGF_Child child = LoadedOGF.childs[idx];
            int links = (int)child.LinksCount();

            GroupBox groupBox = MeshPanel.Controls["MeshGrpBox_" + idx.ToString()] as GroupBox;

            ComboBox comboBox = groupBox.Controls["Bone1_ComboBox"] as ComboBox;
            bool can_add = comboBox.SelectedIndex != -1;

            if (links >= 2)
            {
                comboBox = groupBox.Controls["Bone2_ComboBox"] as ComboBox;
                if (comboBox.SelectedIndex == -1)
                    can_add = false;
            }

            if (links >= 3)
            {
                comboBox = groupBox.Controls["Bone3_ComboBox"] as ComboBox;
                if (comboBox.SelectedIndex == -1)
                    can_add = false;
            }
            if (links >= 4)
            {
                comboBox = groupBox.Controls["Bone4_ComboBox"] as ComboBox;
                if (comboBox.SelectedIndex == -1)
                    can_add = false;
            }

            if (can_add)
            {
                mesh_to_add_list[idx] = !mesh_to_add_list[idx];

                if (mesh_to_add_list[idx])
                {
                    curBox.Text = "Remove Mesh";
                    curBox.BackColor = Color.FromArgb(255, 128, 255, 128);
                }
                else
                {
                    curBox.Text = "Add Mesh";
                    curBox.BackColor = SystemColors.Control;
                }
            }
        }

        private void ClosingCallback(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < mesh_to_add_list.Count; i++)
            {
                if (mesh_to_add_list[i])
                {
                    OGF_Child old_child = LoadedOGF.childs[i];
                    OGF.childs.Add(old_child);
                }
            }
        }
    }
}
