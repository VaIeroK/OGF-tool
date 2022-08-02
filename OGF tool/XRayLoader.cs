using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OGF_Tool
{
    public enum OBJECT
    {
        EOBJ_CURRENT_VERSION = 0x0010,
        EOBJ_CHUNK_OBJECT_BODY = 0x7777,
        EOBJ_CHUNK_VERSION = 0x0900,
        EOBJ_CHUNK_REFERENCE = 0x0902,
        EOBJ_CHUNK_FLAGS = 0x0903,
        EOBJ_CHUNK_SURFACES = 0x0905,
        EOBJ_CHUNK_SURFACES2 = 0x0906,
        EOBJ_CHUNK_SURFACES3 = 0x0907,
        EOBJ_CHUNK_EDITMESHES = 0x0910,
        EOBJ_CHUNK_CLASSSCRIPT = 0x0912,
        EOBJ_CHUNK_BONES = 0x0913,
        EOBJ_CHUNK_SMOTIONS = 0x0916,
        EOBJ_CHUNK_SURFACES_XRLC = 0x0918,
        EOBJ_CHUNK_BONEPARTS = 0x0919,
        EOBJ_CHUNK_ACTORTRANSFORM = 0x0920,
        EOBJ_CHUNK_BONES2 = 0x0921,
        EOBJ_CHUNK_DESC = 0x0922,
        EOBJ_CHUNK_BONEPARTS2 = 0x0923,
        EOBJ_CHUNK_SMOTIONS2 = 0x0924,
        EOBJ_CHUNK_LODS = 0x0925,
        EOBJ_CHUNK_SMOTIONS3 = 0x0926
    };

    public enum MESH
    {
        EMESH_CURRENT_VERSION = 0x0011,
        EMESH_CHUNK_VERSION = 0x1000,
        EMESH_CHUNK_MESHNAME = 0x1001,
        EMESH_CHUNK_FLAGS = 0x1002,
        EMESH_CHUNK_NOT_USED_0 = 0x1003,
        EMESH_CHUNK_BBOX = 0x1004,
        EMESH_CHUNK_VERTS = 0x1005,
        EMESH_CHUNK_FACES = 0x1006,
        EMESH_CHUNK_VMAPS_0 = 0x1007,
        EMESH_CHUNK_VMREFS = 0x1008,
        EMESH_CHUNK_SFACE = 0x1009,
        EMESH_CHUNK_BOP = 0x1010,
        EMESH_CHUNK_VMAPS_1 = 0x1011,
        EMESH_CHUNK_VMAPS_2 = 0x1012,
        EMESH_CHUNK_SG = 0x1013,
        EMESH_CHUNK_NORMALS = 0x1014
    };


    public enum BONE
    {
        BONE_VERSION = 0x0002,
        BONE_CHUNK_VERSION = 0x0001,
        BONE_CHUNK_DEF = 0x0002,
        BONE_CHUNK_BIND_POSE = 0x0003,
        BONE_CHUNK_MATERIAL = 0x0004,
        BONE_CHUNK_SHAPE = 0x0005,
        BONE_CHUNK_IK_JOINT = 0x0006,
        BONE_CHUNK_MASS = 0x0007,
        BONE_CHUNK_FLAGS = 0x0008,
        BONE_CHUNK_IK_JOINT_BREAK = 0x0009,
        BONE_CHUNK_IK_JOINT_FRICTION = 0x0010
    };

    public enum MTL
    {
        GAMEMTL_CURRENT_VERSION = 0x0001,
        GAMEMTLS_CHUNK_VERSION = 0x1000,
        GAMEMTLS_CHUNK_AUTOINC = 0x1001,
        GAMEMTLS_CHUNK_MTLS = 0x1002,
        GAMEMTLS_CHUNK_MTLS_PAIR = 0x1003,
        GAMEMTL_CHUNK_MAIN = 0x1000,
        GAMEMTL_CHUNK_FLAGS = 0x1001,
        GAMEMTL_CHUNK_PHYSICS = 0x1002,
        GAMEMTL_CHUNK_FACTORS = 0x1003,
        GAMEMTL_CHUNK_FLOTATION = 0x1004,
        GAMEMTL_CHUNK_DESC = 0x1005,
        GAMEMTL_CHUNK_INJURIOUS = 0x1006,
        GAMEMTL_CHUNK_DENSITY = 0x1007,
        GAMEMTL_CHUNK_FACTORS_MP = 0x1008,
        GAMEMTLPAIR_CHUNK_PAIR = 0x1000,
        GAMEMTLPAIR_CHUNK_BREAKING = 0x1002,
        GAMEMTLPAIR_CHUNK_STEP = 0x1003,
        GAMEMTLPAIR_CHUNK_COLLIDE = 0x1005
    }

    public enum OGF
    {
        OGF4_HEADER = 1,
        OGF4_TEXTURE = 2,
        OGF4_VERTICES = 3,
        OGF4_INDICES = 4,
        OGF4_P_MAP = 5,  //---------------------- unused
        OGF4_SWIDATA = 6,
        OGF4_VCONTAINER = 7, // not used ??
        OGF4_ICONTAINER = 8, // not used ??
        OGF4_CHILDREN = 9,   // * For skeletons only
        OGF4_CHILDREN_L = 10,    // Link to child visuals
        OGF4_LODDEF2 = 11,   // + 5 channel data
        OGF4_TREEDEF2 = 12,  // + 5 channel data
        OGF4_S_BONE_NAMES = 13,  // * For skeletons only
        OGF4_S_MOTIONS = 14, // * For skeletons only
        OGF4_S_SMPARAMS = 15,    // * For skeletons only
        OGF4_S_IKDATA = 16,  // * For skeletons only
        OGF4_S_USERDATA = 17,    // * For skeletons only (Ini-file)
        OGF4_S_DESC = 18,    // * For skeletons only
        OGF4_S_MOTION_REFS = 19, // * For skeletons only
        OGF4_SWICONTAINER = 20,  // * SlidingWindowItem record container
        OGF4_GCONTAINER = 21,    // * both VB&IB
        OGF4_FASTPATH = 22,  // * extended/fast geometry
        OGF4_S_LODS = 23,    // * For skeletons only (Ini-file)
        OGF4_S_MOTION_REFS2 = 24,    // * changes in format
        OGF4_COLLISION_VERTICES = 25,
        OGF4_COLLISION_INDICES = 26,
    };

    public class XRayLoader
    {
        public long chunk_pos = 0;

        uint CHUNK_COMPRESSED = 0x80000000;

        public MemoryStream mem_stream;
        public BinaryReader reader;


        public void Destroy()
        {
            mem_stream.Dispose();
            reader.Dispose();
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public int ReadInt32()
        {
            return reader.ReadInt32();
        }

        public long ReadInt64()
        {
            return reader.ReadInt64();
        }

        public float ReadFloat()
        {
            return reader.ReadSingle();
        }

        public uint ReadUInt16()
        {
            return reader.ReadUInt16();
        }

        public uint ReadUInt32()
        {
            return reader.ReadUInt32();
        }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(count);
        }

        public Fvector ReadVector()
        {
            Fvector vector = new Fvector();
            vector.x = reader.ReadSingle();
            vector.y = reader.ReadSingle();
            vector.z = reader.ReadSingle();
            return vector;
        }

        public bool SetData(byte[] input)
        {
            if (input == null) return false;
            mem_stream = new MemoryStream(input);
            reader = new BinaryReader(mem_stream);
            return true;
        }

        public void SetStream(Stream stream)
        {
            reader = new BinaryReader(stream);
        }

        public void SetReader(BinaryReader rd)
        {
            reader = rd;
        }

        public bool find_chunk(int chunkId, bool skip = false, bool reset = false)
        {
            return find_chunkSize(chunkId, skip, reset) != 0;
        }

        public byte[] find_and_return_chunk_in_chunk(int chunkId, bool skip = false, bool reset = false)
        {
            int size = (int)find_chunkSize(chunkId, skip, reset);

            if (size > 0)
            {
                return ReadBytes(size);
            }
            else
                return null;
        }

        public uint find_chunkSize(int chunkId, bool skip = false, bool reset = false)
        {
            chunk_pos = 0;

            if (reset) reader.BaseStream.Position = 0;
            if (skip) ReadInt64();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                if (reader.BaseStream.Position + 8 > reader.BaseStream.Length)
                    return 0;

                uint dwType = reader.ReadUInt32();
                uint dwSize = reader.ReadUInt32();

                if (dwType == chunkId || (dwType ^ CHUNK_COMPRESSED) == chunkId)
                {
                    chunk_pos = reader.BaseStream.Position - 8;
                    return dwSize;
                }
                else
                {
                    if (reader.BaseStream.Position + dwSize < reader.BaseStream.Length)
                        reader.BaseStream.Position += dwSize;
                    else if (reader.BaseStream.Position + 8 < reader.BaseStream.Length)
                        reader.BaseStream.Position += 4;
                    else
                        return 0;
                }
            }

            return 0;
        }

        public void open_chunk(BinaryWriter w, int chunkId)
        {
            w.Write(chunkId);
            chunk_pos = w.BaseStream.Position;
            w.Write(0);     // the place for 'size'
        }

        public void close_chunk(BinaryWriter w)
        {
            if (chunk_pos == 0)
            {
                throw new InvalidOperationException("no chunk!");
            }

            long pos = w.BaseStream.Position;
            w.BaseStream.Position = chunk_pos;
            w.Write((int)(pos - chunk_pos - 4));
            w.BaseStream.Position = pos;
            chunk_pos = 0;
        }

        public string read_stringZ()
        {
            string str = "";

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte[] one = { reader.ReadByte() };
                if (one[0] != 0)
                {
                    str += Encoding.Default.GetString(one);
                }
                else
                {
                    break;
                }
            }
            return str;
        }

        public string read_stringData(ref bool data)
        {
            string str = "";
            data = false;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte[] one = { reader.ReadByte() };
                if (one[0] != 0 && one[0] != 0xA && one[0] != 0xD)
                {
                    str += Encoding.Default.GetString(one);
                }
                else
                {
                    if (one[0] == 0xD)
                    {
                        reader.ReadByte();
                        data = true;
                    }
                    break;
                }
            }
            return str;
        }

        public string read_motion_mark_string()
        {
            string str = "";

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte[] one = { reader.ReadByte() };
                if (one[0] != 0xA)
                {
                    str += Encoding.Default.GetString(one);
                }
                else
                {
                    str += Encoding.Default.GetString(one);
                    break;
                }
            }
            return str;
        }

        public void write_stringZ(BinaryWriter w, string str)
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(str));
            temp.Add(0);

            w.Write(temp.ToArray());
        }

        public void write_u32(BinaryWriter w, uint num)
        {
            w.Write(num);
        }
    }

    public class Fvector
    {
        public float x, y, z;
        public Fvector()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }
        public Fvector(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public void set(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public void setX(float _x)
        {
            x = _x;
        }

        public void setY(float _y)
        {
            y = _y;
        }

        public void setZ(float _z)
        {
            z = _z;
        }

        public void set(Fvector vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }
}
