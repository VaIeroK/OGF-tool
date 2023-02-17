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

        public ImportParams(XRay_Model Model, XRay_Model ImportedModel)
        {
            InitializeComponent();
            ConstructUI(Model, ImportedModel);
        }

        public void ConstructUI(XRay_Model Model, XRay_Model ImportedModel)
        {
            bool CanMergeTextures = (Model.childs.Count == ImportedModel.childs.Count);
            bool CanMergeRefs = (Model.Header.IsSkeleton() && ImportedModel.motion_refs != null);
            bool CanMergeOMF = (Model.Header.IsSkeleton() && ImportedModel.motions.data() != null);
            bool CanMergeUserdata = (Model.Header.IsSkeleton() && ImportedModel.userdata != null);
            bool CanMergeLod = (Model.Header.IsSkeleton() && ImportedModel.lod != null);
            bool CanMergeIkData = (Model.Header.IsSkeleton() && Model.ikdata != null && ImportedModel.ikdata != null && Model.ikdata.bones.Count == ImportedModel.ikdata.bones.Count);

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
