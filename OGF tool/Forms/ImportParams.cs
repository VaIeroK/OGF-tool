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
    public partial class ImportParams : Form
    {
        public bool Textures = false;
        public bool MotionRefs = false;
        public bool Motions = false;
        public bool Userdata = false;
        public bool Lod = false;
        public bool Materials = false;
        public bool Remove = false;

        public bool res = false;

        public ImportParams(OGF_Model OGF, OGF_Model ImportedOGF)
        {
            InitializeComponent();
            ConstructUI(OGF, ImportedOGF);
        }

        public void ConstructUI(OGF_Model OGF, OGF_Model ImportedOGF)
        {
            bool CanMergeTextures = (OGF.childs.Count == ImportedOGF.childs.Count);
            bool CanMergeRefs = (OGF.Header.IsSkeleton() && ImportedOGF.motion_refs != null);
            bool CanMergeOMF = (OGF.Header.IsSkeleton() && ImportedOGF.motions.data() != null);
            bool CanMergeUserdata = (OGF.Header.IsSkeleton() && ImportedOGF.userdata != null);
            bool CanMergeLod = (OGF.Header.IsSkeleton() && ImportedOGF.lod != null);
            bool CanMergeIkData = (OGF.Header.IsSkeleton() && OGF.ikdata != null && ImportedOGF.ikdata != null && OGF.ikdata.bones.Count == ImportedOGF.ikdata.bones.Count);

            TexturesChbx.Enabled = CanMergeTextures;
            MotionRefsChbx.Enabled = CanMergeRefs;
            MotionsChbx.Enabled = CanMergeOMF;
            UserdataChbx.Enabled = CanMergeUserdata;
            LodPathChbx.Enabled = CanMergeLod;
            IKdataChbx.Enabled = CanMergeIkData;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            res = true;

            Textures = TexturesChbx.Checked;
            MotionRefs = MotionRefsChbx.Checked;
            Motions = MotionsChbx.Checked;
            Userdata = UserdataChbx.Checked;
            Lod = LodPathChbx.Checked;
            Materials = IKdataChbx.Checked;

            Remove = RemoveChbx.Checked;

            Close();
        }
    }
}
