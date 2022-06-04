using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OGF_tool
{
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
            List<char> str = new List<char>();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte one = reader.ReadByte();
                if (one != 0)
                {
                    str.Add((char)one);
                }
                else
                {
                    break;
                }
            }

            return new string(str.ToArray());
        }

        public void write_stringZ(BinaryWriter w, string str)
        {
            foreach (char c in str)
            {
                byte b = (byte)c;
                w.Write(b);
            }

            w.Write((byte)0);
        }
    }

    public class OGF_Children
    {
        public List<OGF_Child> childs = new List<OGF_Child>();

        public uint chunk_size;

        public long pos;

        public MotionRefs refs = new MotionRefs();

        public UserData usertdata = null;

        public BoneData bones = new BoneData();

        public IK_Data ikdata = new IK_Data();

        public Description descr = null;

        public OGF_Children()
        {
            refs.pos = -1;
        }

    }

    public struct MotionRefs
    {
        public long pos;
        public List<string> refs0;
        public uint chunk_size()
        {
            uint temp = 4;
            foreach (var text in refs0)
                temp += (uint)text.Length + 1;
            return temp;
        }
        public byte[] count()
        {
            return BitConverter.GetBytes(refs0.Count);
        }
        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            foreach (var str in refs0)
            {
                temp.AddRange(Encoding.ASCII.GetBytes(str));
                temp.Add(0);
            }

            return temp.ToArray();
        }
    }

    public class UserData
    {
        public long pos;
        public string data;

        public UserData()
        {
            this.pos = 0;
            this.data = "";
        }

        public byte[] data_all()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.ASCII.GetBytes(data));
            temp.Add(0);

            return temp.ToArray();
        }

        public uint chunk_size()
        {
            return (uint)data.Length +1;
        }
    }

    public struct BoneData
    {
        public long pos;
        public int old_size;

        public List<string> bones;
        public List<string> parent_bones;
        public List<byte[]> fobb;

        public uint chunk_size()
        {
            uint temp = 4;                                  // count byte

            for (int i = 0; i < bones.Count; i++)
            {
                temp += (uint)bones[i].Length + 1;          // bone name
                temp += (uint)parent_bones[i].Length + 1;   // parent bone name
                temp += 60;                                 // obb
            }

            return temp;
        }
        public byte[] count()
        {
            return BitConverter.GetBytes(bones.Count);
        }
        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            for (int i = 0; i < bones.Count; i++)
            {
                temp.AddRange(Encoding.ASCII.GetBytes(bones[i]));       // bone name
                temp.Add(0);
                temp.AddRange(Encoding.ASCII.GetBytes(parent_bones[i]));// parent bone name
                temp.Add(0);
                temp.AddRange(fobb[i]);                                 // obb
            }

            return temp.ToArray();
        }
    }

    public struct Fvector
    {
        public float x;
        public float y;
        public float z;
    }

    public struct IK_Data
    {
        public long pos;
        public int old_size;

        public List<string> materials;
        public List<float> mass;
        public List<uint> version;
        public List<Fvector> center_mass;
        public List<List<byte[]>> bytes_1;

        public uint chunk_size()
        {
            uint temp = 0;
            for (int i = 0; i < materials.Count; i++)
            {
                temp += 4;
                temp += (uint)materials[i].Length + 1;       // bone name
                temp += 112;

                temp += 4;
                temp += 16 * 3;
                temp += 4;
                temp += 4;
                temp += 4;
                temp += 4;
                temp += 4;

                if (version[i] > 0)
                    temp += 4;

                temp += 12;
                temp += 12;
                temp += 4;
                temp += 12;
            }

            return temp;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            for (int i = 0; i < materials.Count; i++)
            {
                temp.AddRange(BitConverter.GetBytes(version[i]));
                temp.AddRange(Encoding.ASCII.GetBytes(materials[i]));
                temp.Add(0);
                for (int j = 0; j < bytes_1[i].Count; j++)
                    temp.AddRange(bytes_1[i][j]);
                temp.AddRange(BitConverter.GetBytes(mass[i]));

                temp.AddRange(BitConverter.GetBytes(center_mass[i].x));
                temp.AddRange(BitConverter.GetBytes(center_mass[i].y));
                temp.AddRange(BitConverter.GetBytes(center_mass[i].z));
            }

            return temp.ToArray();
        }
    }

    public class Description
    {
        public long pos;
        public int old_size;

        public string m_source;
        public string m_export_tool;
        public uint m_export_time;
        public string m_owner_name;
        public uint m_creation_time;
        public string m_export_modif_name_tool;
        public uint m_modified_time;

        public Description()
        {
            this.pos = 0;
            this.old_size = 0;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.ASCII.GetBytes(m_source));
            temp.Add(0);
            temp.AddRange(Encoding.ASCII.GetBytes(m_export_tool));
            temp.Add(0);
            temp.AddRange(BitConverter.GetBytes(m_export_time));
            temp.AddRange(Encoding.ASCII.GetBytes(m_owner_name));
            temp.Add(0);
            temp.AddRange(BitConverter.GetBytes(m_creation_time));
            temp.AddRange(Encoding.ASCII.GetBytes(m_export_modif_name_tool));
            temp.Add(0);
            temp.AddRange(BitConverter.GetBytes(m_modified_time));

            return temp.ToArray();
        }

        public uint chunk_size()
        {
            uint size = 0;
            size += (uint)m_source.Length + 1;
            size += (uint)m_export_tool.Length + 1;
            size += 4;
            size += (uint)m_owner_name.Length + 1;
            size += 4;
            size += (uint)m_export_modif_name_tool.Length + 1;
            size += 4;
            return size;
        }
    }

    public class OGF_Child
    {
        public string m_texture;

        public string m_shader;

        public OGF_Child(long _pos, int _parent_id, long _parent_pos, int _old_size, string texture, string shader)
        {
            pos = _pos;
            parent_id = _parent_id;
            parent_pos = _parent_pos;
            m_texture = texture;
            m_shader = shader;
            old_size = _old_size;
        }

        public long pos;

        public long parent_pos;
        public int parent_id;

        public int old_size;

        public uint NewSize()
        {
            return (uint)(m_texture.Length + m_shader.Length + 2 - old_size);
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes(2));
            temp.AddRange(BitConverter.GetBytes(m_texture.Length + m_shader.Length + 2));

            foreach (char c in m_texture)
            {
                byte b = (byte)c;
                temp.Add(b);
            }

            temp.Add(0);

            foreach (char c in m_shader)
            {
                byte b = (byte)c;
                temp.Add(b);
            }

            temp.Add(0);

            return temp.ToArray();

        }
    }
}
