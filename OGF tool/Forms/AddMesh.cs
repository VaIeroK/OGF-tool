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
        private class Reassign
        {
            public int old_bone;
            public int new_bone;
        }

        private OGF_Model OGF, LoadedOGF;
        private int last_height = 0;
        private List<bool> mesh_to_add_list = new List<bool>();
        public bool res = false;

        public AddMesh(ref OGF_Model Main_OGF, OGF_Model Loaded_OGF)
        {
            InitializeComponent();

            MeshPanel.Controls.Clear();

            OGF = Main_OGF;
            LoadedOGF = Loaded_OGF;

            if (!OGF.IsProgressive())
                LoadedOGF.RemoveProgressive(0.0f);

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

            var newLbl1 = Copy.Label(Texture_Label);
            var newLbl2 = Copy.Label(Shader_Label);

            var newTextbox1 = Copy.TextBox(Texture_Textbox);
            newTextbox1.Text = child.m_texture;
            var newTextbox2 = Copy.TextBox(Shader_Textbox);
            newTextbox2.Text = child.m_shader;

            var newButton = Copy.Button(AddMeshButton);
            newButton.Click += new System.EventHandler(this.AddMeshButton_Click);
            newButton.Name = "AddBnt_" + idx;

            box.Controls.Add(newLbl1);
            box.Controls.Add(newLbl2);

            box.Controls.Add(newTextbox1);
            box.Controls.Add(newTextbox2);

            box.Controls.Add(newButton);

            List<uint> BonesList = new List<uint>(); // Лист всех костей к которым привязан меш

            for (int i = 0; i < child.Vertices.Count; i++)
            {
                SSkelVert vert = child.Vertices[i];

                for (int j = 0; j < child.LinksCount(); j++)
                {
                    uint bone = vert.bones_id[j];
                    if (!BonesList.Contains(bone))
                        BonesList.Add(bone);
                }
            }

            for (int i = 0; i < BonesList.Count; i++)
            {
                var newLbl3 = Copy.Label(Bone_Label);
                var newLbl4 = Copy.Label(Reassign_Label);
                var newCombo1 = FillComboBox(Copy.ComboBox(Bone_ComboBox), child);
                var newTextbox3 = Copy.TextBox(OldBone_Textbox);

                newLbl3.Text = $"Bone {i + 1} assigned to:";
                newLbl4.Name = "ReassignBone_" + i;
                newTextbox3.Name = "OldBone_" + i;

                newLbl3.Location = new Point(newLbl3.Location.X, newLbl3.Location.Y + 27 * i);
                newLbl4.Location = new Point(newLbl4.Location.X, newLbl4.Location.Y + 27 * i);
                newCombo1.Location = new Point(newCombo1.Location.X, newCombo1.Location.Y + 27 * i);
                newTextbox3.Location = new Point(newTextbox3.Location.X, newTextbox3.Location.Y + 27 * i);

                if (i != BonesList.Count - 1)
                    box.Size = new Size(box.Size.Width, box.Size.Height + 27);

                int bone_id = OGF.bonedata.GetBoneID(LoadedOGF.bonedata.bones[(int)BonesList[i]].name);
                if (bone_id != -1)
                    newCombo1.SelectedIndex = bone_id;
                else
                    newLbl4.ForeColor = Color.FromArgb(255, 255, 0, 0);

                newCombo1.SelectedIndexChanged += new System.EventHandler(this.ComboBoxIndexChanged);
                newCombo1.Name = "BoneComboBox_" + idx + "_" + i;

                newTextbox3.Text = LoadedOGF.bonedata.bones[(int)BonesList[i]].name;

                box.Controls.Add(newLbl3);
                box.Controls.Add(newLbl4);
                box.Controls.Add(newCombo1);
                box.Controls.Add(newTextbox3);
            }

            last_height += box.Size.Height + 2;
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
            int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);
            GroupBox groupBox = MeshPanel.Controls["MeshGrpBox_" + idx.ToString()] as GroupBox;
            bool can_add = true;

            for (int i = 0; i < groupBox.Controls.Count; i++)
            {
                if (groupBox.Controls[i] is ComboBox)
                {
                    ComboBox cmb = (ComboBox)groupBox.Controls[i];
                    if (cmb.SelectedIndex == -1)
                    {
                        can_add = false;
                        break;
                    }
                }
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
            else
                AutoClosingMessageBox.Show("Please, reassign bones to add mesh!", "", 1300, MessageBoxIcon.Warning);
        }

        private void ComboBoxIndexChanged(object sender, EventArgs e)
        {
            ComboBox curBox = sender as ComboBox;
            string currentField = curBox.Name.ToString().Split('_')[0];
            int idx = Convert.ToInt32(curBox.Name.ToString().Split('_')[1]);
            int i = Convert.ToInt32(curBox.Name.ToString().Split('_')[2]);

            GroupBox groupBox = MeshPanel.Controls["MeshGrpBox_" + idx.ToString()] as GroupBox;

            if (curBox.SelectedIndex != -1)
                groupBox.Controls["ReassignBone_" + i].ForeColor = SystemColors.ControlText;
            else
                groupBox.Controls["ReassignBone_" + i].ForeColor = Color.FromArgb(255, 255, 0, 0);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            res = true;

            for (int i = 0; i < mesh_to_add_list.Count; i++)
            {
                if (mesh_to_add_list[i])
                {
                    OGF_Child old_child = LoadedOGF.childs[i];
                    GroupBox groupBox = MeshPanel.Controls["MeshGrpBox_" + i.ToString()] as GroupBox;

                    List<Reassign> reassigns = new List<Reassign>();

                    for (int j = 0; j < groupBox.Controls.Count; j++)
                    {
                        if (groupBox.Controls[j] is ComboBox)
                        {
                            ComboBox cmb = (ComboBox)groupBox.Controls[j];
                            if (cmb.SelectedIndex != -1)
                            {
                                Reassign reassign = new Reassign();

                                int idx = Convert.ToInt32(cmb.Name.ToString().Split('_')[2]); // id ряда с настройками
                                TextBox OldBoneBox = groupBox.Controls["OldBone_" + idx] as TextBox; // Хранит текстовое название кости к которой привязаны вертексы

                                reassign.old_bone = LoadedOGF.bonedata.GetBoneID(OldBoneBox.Text); // Получили id кости ряда
                                reassign.new_bone = cmb.SelectedIndex; // Меняем старую кость на выбранный индекс
                                reassigns.Add(reassign);
                            }
                        }
                    }

                    for (int j = 0; j < old_child.Vertices.Count; j++)
                    {
                        for (int r = 0; r < old_child.LinksCount(); r++)
                        {
                            foreach (var reass in reassigns)
                            {
                                if (old_child.Vertices[j].bones_id[r] == reass.old_bone)
                                    old_child.Vertices[j].bones_id[r] = (uint)reass.new_bone;
                            }
                        }
                    }

                    OGF.childs.Add(old_child);
                }
            }

            Close();
        }
    }
}
