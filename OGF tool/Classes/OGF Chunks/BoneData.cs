using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class BoneData
    {
        public long pos;
        public int old_size;

        public List<string> bone_names;
        public List<string> parent_bone_names;
        public List<byte[]> fobb;
        public List<List<int>> bone_childs;

        public BoneData()
        {
            this.pos = 0;
            this.old_size = 0;
            this.bone_names = new List<string>();
            this.parent_bone_names = new List<string>();
            this.fobb = new List<byte[]>();
            this.bone_childs = new List<List<int>>();
        }

        public int GetBoneID(string bone)
        {
            for (int i = 0; i < bone_names.Count; i++)
            {
                if (bone_names[i] == bone)
                    return i;
            }
            return -1;
        }

        public string GetBoneName(int bone)
        {
            if (bone_names.Count > bone)
                return bone_names[bone];

            return "";
        }

        public byte[] data(bool repair)
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes(bone_names.Count));

            for (int i = 0; i < bone_names.Count; i++)
            {
                temp.AddRange(Encoding.Default.GetBytes(bone_names[i]));       // bone name
                temp.Add(0);
                temp.AddRange(Encoding.Default.GetBytes(parent_bone_names[i]));// parent bone name
                temp.Add(0);

                if (repair)
                {
                    for (int j = 0; j < 60; j++)
                        temp.Add(0);
                }
                else
                    temp.AddRange(fobb[i]);                               // obb
            }

            return temp.ToArray();
        }
    }
}
