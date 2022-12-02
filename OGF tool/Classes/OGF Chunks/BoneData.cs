using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        public string GetNotNullName()
        {
            if (name == "")
                return "noname_bone";
            else
                return name;
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
    public struct BoneRenderTransform
    {
        public float PosX, PosY, PosZ;
        public float RotX, RotY, RotZ;
        public float OutPosX, OutPosY, OutPosZ;
        public float OutRotX, OutRotY, OutRotZ;

        public float[] OutPos()
        {
            return new float[3] { OutPosX, OutPosY, OutPosZ };
        }

        public float[] OutRot()
        {
            return new float[3] { OutRotX, OutRotY, OutRotZ };
        }

        public static BoneRenderTransform[] Setup(OGF_Model OGF_V, out string child_list)
        {
            BoneRenderTransform[] transforms = new BoneRenderTransform[OGF_V.bonedata.bones.Count];
            child_list = "";

            for (int i = 0; i < OGF_V.bonedata.bones.Count; i++)
            {
                float[] pos, rot; 

                if (OGF_V.ikdata.chunk_version == 2)
                {
                    pos = OGF_V.ikdata.bones[i].fixed_position;
                    rot = OGF_V.ikdata.bones[i].fixed_rotation;
                }
                else
                {
                    pos = OGF_V.ikdata.bones[i].position;
                    rot = OGF_V.ikdata.bones[i].rotation;
                }

                transforms[i].PosX = pos[0];
                transforms[i].PosY = pos[1];
                transforms[i].PosZ = pos[2];

                transforms[i].RotX = rot[0];
                transforms[i].RotY = rot[1];
                transforms[i].RotZ = rot[2];

                if (i != 0)
                    child_list += "-";

                for (int j = 0; j < OGF_V.bonedata.bones[i].childs_id.Count; j++)
                    child_list += $"{OGF_V.bonedata.bones[i].childs_id[j]},";

                if (OGF_V.bonedata.bones[i].parent_name != "")
                    child_list += $"{OGF_V.bonedata.GetBoneID(OGF_V.bonedata.bones[i].parent_name)}";
                else
                    child_list += "9999";
            }

            child_list += "-";

            return transforms;
        }
    }
}
