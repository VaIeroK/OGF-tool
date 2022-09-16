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
    public partial class OgfInfo : Form
    {
        public Description descr = new Description();
        public bool res = false;
        public OgfInfo(OGF_Children OGF, bool refs_correct, float lod, List<byte> motions_flags)
        {
            InitializeComponent();

            uint links = 0;
            bool cop_links = false;

            long verts = 0, faces = 0;
            foreach (var ch in OGF.childs)
            {
                if (ch.to_delete) continue;

                if (ch.links >= 0x12071980)
                    links = Math.Max(links, ch.links / 0x12071980);
                else
                {
                    links = Math.Max(links, ch.links);
                    cop_links = true;
                }

                verts += ch.Vertices.Count;
                faces += ch.Faces_SWI(lod).Count;
            }

            OgfVersLabel.Text = OGF.m_version.ToString();
            ModelTypeLabel.Text = (OGF.IsStaticSingle() ? "Single Static" : OGF.IsStatic() ? "Static" : OGF.IsAnimated() ? "Animated" : "Rigid");
            LinksLabel.Text = links > 0 ? links.ToString() + ", " + (cop_links ? "CoP" : "SoC") : "None";
            MotionRefsTypeLabel.Text = (OGF.motion_refs == null || !refs_correct) ? "None" : (OGF.motion_refs.soc ? "SoC" : "CoP");

            bool bit8 = false;
            bool bit16 = false;
            bool no_bit = false;

            if (motions_flags.Count > 0)
            {
                for (int i = 0; i < motions_flags.Count; i++)
                {
                    byte flag = motions_flags[i];

                    bool key16bit = (flag & (int)MotionKeyFlags.flTKey16IsBit) == (int)MotionKeyFlags.flTKey16IsBit;
                    bool keynocompressbit = (flag & (int)MotionKeyFlags.flTKeyFFT_Bit) == (int)MotionKeyFlags.flTKeyFFT_Bit;

                    if (!key16bit && !keynocompressbit && !bit8)
                        bit8 = true;
                    else if (key16bit && !keynocompressbit && !bit16)
                        bit16 = true;
                    else if (keynocompressbit && !no_bit)
                        no_bit = true;
                }

                MotionsLabel.Text = "";

                if (bit8)
                    MotionsLabel.Text += "8 bit";

                if (bit16)
                {
                    if (MotionsLabel.Text != "")
                        MotionsLabel.Text += " | ";

                    MotionsLabel.Text += "16 bit";
                }

                if (no_bit)
                {
                    if (MotionsLabel.Text != "")
                        MotionsLabel.Text += " | ";

                    MotionsLabel.Text += "no compress";
                }
            }
            else
                MotionsLabel.Text = "None";

            VertsLabel.Text = verts.ToString();
            FacesLabel.Text = faces.ToString();

            if (OGF.description != null)
            {
                ByteLabel.Text = OGF.description.four_byte ? "4 byte" : "8 byte";
                RepairTimersButton.Enabled = OGF.description.four_byte;

                SourceTextBox.Text = OGF.description.m_source;
                ConverterTextBox.Text = OGF.description.m_export_tool;
                CreatorTextBox.Text = OGF.description.m_owner_name;
                EditorTextBox.Text = OGF.description.m_export_modif_name_tool;

                System.DateTime dt_e = new System.DateTime(1970, 1, 1).AddSeconds(OGF.description.m_export_time);
                System.DateTime dt_c = new System.DateTime(1970, 1, 1).AddSeconds(OGF.description.m_creation_time);
                System.DateTime dt_m = new System.DateTime(1970, 1, 1).AddSeconds(OGF.description.m_modified_time);

                ExportTimeDate.Value = dt_e;
                CreationTimeDate.Value = dt_c;
                ModifedTimeDate.Value = dt_m;
            }
            else
                RepairTimersButton.Enabled = false;

            SourceTextBox.Enabled = OGF.description != null;
            ConverterTextBox.Enabled = OGF.description != null;
            CreatorTextBox.Enabled = OGF.description != null;
            EditorTextBox.Enabled = OGF.description != null;

            ExportTimeDate.Enabled = OGF.description != null;
            CreationTimeDate.Enabled = OGF.description != null;
            ModifedTimeDate.Enabled = OGF.description != null;
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            descr.m_source = SourceTextBox.Text;

            descr.m_source = SourceTextBox.Text;
            descr.m_export_tool = ConverterTextBox.Text;
            descr.m_owner_name = CreatorTextBox.Text;
            descr.m_export_modif_name_tool = EditorTextBox.Text;

            descr.m_export_time = Convert.ToUInt32(ExportTimeDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            descr.m_creation_time = Convert.ToUInt32(CreationTimeDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            descr.m_modified_time = Convert.ToUInt32(ModifedTimeDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            res = true;
        }

        private void RepairTimersButton_Click(object sender, EventArgs e)
        {
            descr.four_byte = false;
            RepairTimersButton.Enabled = false;
            ByteLabel.Text = "8 byte";
        }
    }
}
