using System;
using System.Collections.Generic;
using System.Text;

namespace OGF_tool
{
    public class Bone
    {
        public string name;
        public string parent_name;
        public byte[] fobb;

        public List<int> childs_id;

        public Bone()
        {
            childs_id = new List<int>();
        }
    }

    public class BoneData
    {
        public long pos;
        public int old_size;
        public List<Bone> bones;

        public BoneData()
        {
            pos = 0;
            old_size = 0;
            bones = new List<Bone>();
        }

        public int GetBoneID(string bone)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                if (bones[i].name == bone)
                    return i;
            }
            return -1;
        }

        public string GetBoneName(int bone)
        {
            if (bones.Count > bone)
                return bones[bone].name;

            return "";
        }

        public void RemoveBone(int bone)
        {
            bones.RemoveAt(bone);
        }

        public void RecalcChilds()
        {
            for (int i = 0; i < bones.Count; i++)
            {
                bones[i].childs_id.Clear();
                for (int j = 0; j < bones.Count; j++)
                {
                    if (bones[j].parent_name == bones[i].name)
                        bones[i].childs_id.Add(j);
                }
            }
        }

        public void Load(XRayLoader xr_loader)
        {
            pos = xr_loader.chunk_pos;

            uint count = xr_loader.ReadUInt32();

            for (; count != 0; count--)
            {
                Bone bone = new Bone();
                bone.name = xr_loader.read_stringZ();
                bone.parent_name = xr_loader.read_stringZ();
                bone.fobb = xr_loader.ReadBytes(60);
                bones.Add(bone);
            }

            RecalcChilds();

            old_size = data(false).Length;
        }

        public byte[] data(bool repair)
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes(bones.Count));

            for (int i = 0; i < bones.Count; i++)
            {
                temp.AddRange(Encoding.Default.GetBytes(bones[i].name));
                temp.Add(0);
                temp.AddRange(Encoding.Default.GetBytes(bones[i].parent_name));
                temp.Add(0);

                if (repair)
                {
                    for (int j = 0; j < 60; j++)
                        temp.Add(0);
                }
                else
                    temp.AddRange(bones[i].fobb);
            }

            return temp.ToArray();
        }
    }
}
