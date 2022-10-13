using System;
using System.Collections.Generic;
using System.Text;

namespace OGF_tool
{
    public class IK_Bone
    {
        public string material;
        public float mass;
        public uint version;
        public float[] center_mass;
        public float[] position;
        public float[] rotation;
        public byte[] kinematic_data;

        public IK_Bone()
        {
            version = 0;
            mass = 0.0f;
        }
    }

    public class IK_Data
    {
        public long pos;
        public int old_size;
        public byte chunk_version;

        public List<IK_Bone> bones;

        public IK_Data()
        {
            pos = 0;
            old_size = 0;
            chunk_version = 0;
            bones = new List<IK_Bone>();
        }

        public void RemoveBone(int bone)
        {
            bones.RemoveAt(bone);
        }

        public void Load(XRayLoader xr_loader, int bones_count, byte chunk_ver)
        {
            pos = xr_loader.chunk_pos;
            chunk_version = chunk_ver;

            for (int i = 0; i < bones_count; i++)
            {
                IK_Bone bone = new IK_Bone();

                List<byte> kinematic_data = new List<byte>();

                byte[] temp_byte;

                if (chunk_version == 4)
                    bone.version = xr_loader.ReadUInt32();

                bone.material = xr_loader.read_stringZ();

                temp_byte = xr_loader.ReadBytes(112);   // struct SBoneShape
                kinematic_data.AddRange(temp_byte);

                int ImportBytes = ((chunk_version == 4) ? 76 : ((chunk_version == 3) ? 72 : 60));
                temp_byte = xr_loader.ReadBytes(ImportBytes); // Import
                kinematic_data.AddRange(temp_byte);

                bone.kinematic_data = kinematic_data.ToArray();

                bone.rotation = xr_loader.ReadVector();
                bone.position = xr_loader.ReadVector();

                bone.mass = xr_loader.ReadFloat();
                bone.center_mass = xr_loader.ReadVector();

                bones.Add(bone);
            }

            old_size = data().Length;
        }

        public uint ChunkID(byte vers)
        {
            switch (chunk_version)
            {
                case 4:
                    return (vers == 4 ? (uint)OGF.OGF4_S_IKDATA : (uint)OGF.OGF3_S_IKDATA_2);
                case 3:
                    return (uint)OGF.OGF3_S_IKDATA;
                case 2:
                    return (uint)OGF.OGF3_S_IKDATA_0;
            }

            return 4;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            for (int i = 0; i < bones.Count; i++)
            {
                if (chunk_version == 4)
                    temp.AddRange(BitConverter.GetBytes(bones[i].version));

                temp.AddRange(Encoding.Default.GetBytes(bones[i].material));
                temp.Add(0);

                temp.AddRange(bones[i].kinematic_data);

                temp.AddRange(BitConverter.GetBytes(bones[i].rotation[0]));
                temp.AddRange(BitConverter.GetBytes(bones[i].rotation[1]));
                temp.AddRange(BitConverter.GetBytes(bones[i].rotation[2]));

                temp.AddRange(BitConverter.GetBytes(bones[i].position[0]));
                temp.AddRange(BitConverter.GetBytes(bones[i].position[1]));
                temp.AddRange(BitConverter.GetBytes(bones[i].position[2]));

                temp.AddRange(BitConverter.GetBytes(bones[i].mass));

                temp.AddRange(BitConverter.GetBytes(bones[i].center_mass[0]));
                temp.AddRange(BitConverter.GetBytes(bones[i].center_mass[1]));
                temp.AddRange(BitConverter.GetBytes(bones[i].center_mass[2]));
            }

            return temp.ToArray();
        }
    }
}
