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

        public ImportParams(OGF_Children OGF, OGF_Children ImportedOGF)
        {
            InitializeComponent();
            ConstructUI(OGF, ImportedOGF);
        }

        public void ConstructUI(OGF_Children OGF, OGF_Children ImportedOGF)
        {
            bool CanMergeTextures = (OGF.childs.Count == ImportedOGF.childs.Count);
            bool CanMergeRefs = (OGF.IsSkeleton() && ImportedOGF.motion_refs != null);
            bool CanMergeOMF = (OGF.IsSkeleton() && ImportedOGF.motions.data() != null);
            bool CanMergeUserdata = (OGF.IsSkeleton() && ImportedOGF.userdata != null);
            bool CanMergeLod = (OGF.IsSkeleton() && ImportedOGF.lod != null);
            bool CanMergeIkData = (OGF.IsSkeleton() && OGF.ikdata.materials.Count == ImportedOGF.ikdata.materials.Count);

            TexturesChbx.Enabled = CanMergeTextures;
            MotionRefsChbx.Enabled = CanMergeRefs;
            MotionsChbx.Enabled = CanMergeOMF;
            UserdataChbx.Enabled = CanMergeUserdata;
            LodPathChbx.Enabled = CanMergeLod;
            IKdataChbx.Enabled = CanMergeIkData;
        }
        private void ClosingForm(object sender, FormClosingEventArgs e)
        {
            Textures = TexturesChbx.Checked;
            MotionRefs = MotionRefsChbx.Checked;
            Motions = MotionsChbx.Checked;
            Userdata = UserdataChbx.Checked;
            Lod = LodPathChbx.Checked;
            Materials = IKdataChbx.Checked;

            Remove = RemoveChbx.Checked;
        }

        public bool Valid()
        {
            return Textures || MotionRefs || Motions || Userdata || Lod || Materials;
        }
    }
}
