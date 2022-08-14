using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OGF_Tool
{
    public class OGF_Children
    {
        public byte m_version;
        public byte m_model_type; // 1 - Without bones, 3 - Animated, 10 - Rigid
        public bool IsDM;
        public uint BrokenType;

        public Description description;
        public List<OGF_Child> childs;
        public BoneData bones;
        public IK_Data ikdata;
        public UserData userdata;
        public Lod lod;
        public MotionRefs motion_refs;
        public string motions;

        public uint chunk_size;
        public long pos;

        public OGF_Children()
        {
            this.pos = 0;
            this.chunk_size = 0;
            this.BrokenType = 0;
            this.motions = "";
            this.IsDM = false;
            this.m_model_type = 0;
            this.m_version = 0;
            this.description = null;
            this.childs = new List<OGF_Child>();
            this.bones = null;
            this.ikdata = null;
            this.userdata = null;
            this.lod = null;
            this.motion_refs = null;
        }

        public bool IsSkeleton()
        {
            return m_model_type == 3 || m_model_type == 10;
        }

        public bool IsAnimated()
        {
            return m_model_type == 3;
        }
    }

    public class MotionRefs
    {
        public long pos;
        public List<string> refs;
        public bool v3;
        public int old_size;

        public MotionRefs()
        {
            this.pos = 0;
            this.old_size = 0;
            this.refs = new List<string>();
            this.v3 = false;
        }

        public uint chunk_size(bool v3)
        {
            uint temp = (uint)(v3 ? 0 : 4);
            foreach (var text in refs)
                temp += (uint)text.Length + 1;
            return temp;
        }
        public byte[] count()
        {
            return BitConverter.GetBytes(refs.Count);
        }
        public byte[] data(bool v3)
        {
            List<byte> temp = new List<byte>();

            if (!v3)
            {
                foreach (var str in refs)
                {
                    temp.AddRange(Encoding.Default.GetBytes(str));
                    temp.Add(0);
                }
            }
            else
            {
                string strref = refs[0];
                if (refs.Count > 1)
                {
                    for (int i = 1; i < refs.Count; i++)
                        strref += refs[i] + ",";
                }

                temp.AddRange(Encoding.Default.GetBytes(strref));
                temp.Add(0);
            }

            return temp.ToArray();
        }
    }

    public class UserData
    {
        public long pos;
        public int old_size;
        public string userdata;

        public UserData()
        {
            this.pos = 0;
            this.old_size = 0;
            this.userdata = "";
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(userdata));
            temp.Add(0);

            return temp.ToArray();
        }

        public uint chunk_size()
        {
            return (uint)userdata.Length + 1;
        }
    }

    public class Lod
    {
        public long pos;
        public int old_size;
        public string lod_path;
        public bool data_str;

        public Lod()
        {
            this.pos = 0;
            this.old_size = 0;
            this.lod_path = "";
            this.data_str = true;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(lod_path));
            if (data_str)
            {
                temp.Add(0xD);
                temp.Add(0xA);
            }
            else
                temp.Add(0);

            return temp.ToArray();
        }

        public uint chunk_size()
        {
            return (uint)lod_path.Length + (uint)(data_str ? 2 : 1);
        }
    }

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

        public uint chunk_size()
        {
            uint temp = 4;                                  // count byte

            for (int i = 0; i < bone_names.Count; i++)
            {
                temp += (uint)bone_names[i].Length + 1;          // bone name
                temp += (uint)parent_bone_names[i].Length + 1;   // parent bone name
                temp += 60;                                 // obb
            }

            return temp;
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

    public class IK_Data
    {
        public int old_size;

        public List<string> materials;
        public List<float> mass;
        public List<uint> version;
        public List<Fvector> center_mass;
        public List<Fvector> position;
        public List<Fvector> rotation;
        public List<List<byte[]>> bytes_1;

        public IK_Data()
        {
            this.old_size = 0;
            this.materials = new List<string>();
            this.mass = new List<float>();
            this.version = new List<uint>();
            this.center_mass = new List<Fvector>();
            this.position = new List<Fvector>();
            this.rotation = new List<Fvector>();
            this.bytes_1 = new List<List<byte[]>>();
        }

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
                temp.AddRange(Encoding.Default.GetBytes(materials[i]));
                temp.Add(0);
                for (int j = 0; j < bytes_1[i].Count; j++)
                    temp.AddRange(bytes_1[i][j]);

                temp.AddRange(BitConverter.GetBytes(rotation[i].x));
                temp.AddRange(BitConverter.GetBytes(rotation[i].y));
                temp.AddRange(BitConverter.GetBytes(rotation[i].z));

                temp.AddRange(BitConverter.GetBytes(position[i].x));
                temp.AddRange(BitConverter.GetBytes(position[i].y));
                temp.AddRange(BitConverter.GetBytes(position[i].z));

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
        public bool four_byte;

        public string m_source;
        public string m_export_tool;
        public long m_export_time;
        public string m_owner_name;
        public long m_creation_time;
        public string m_export_modif_name_tool;
        public long m_modified_time;

        public Description()
        {
            this.pos = 0;
            this.old_size = 0;
            this.four_byte = false;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(m_source));
            temp.Add(0);
            temp.AddRange(Encoding.Default.GetBytes(m_export_tool));
            temp.Add(0);
            if (!four_byte)
                temp.AddRange(BitConverter.GetBytes(m_export_time));
            else
                temp.AddRange(BitConverter.GetBytes((uint)m_export_time));
            temp.AddRange(Encoding.Default.GetBytes(m_owner_name));
            temp.Add(0);
            if (!four_byte)
                temp.AddRange(BitConverter.GetBytes(m_creation_time));
            else
                temp.AddRange(BitConverter.GetBytes((uint)m_creation_time));
            temp.AddRange(Encoding.Default.GetBytes(m_export_modif_name_tool));
            temp.Add(0);
            if (!four_byte)
                temp.AddRange(BitConverter.GetBytes(m_modified_time));
            else
                temp.AddRange(BitConverter.GetBytes((uint)m_modified_time));

            return temp.ToArray();
        }

        public uint chunk_size()
        {
            uint time_size = (uint)(four_byte ? 4 : 8);
            uint size = 0;
            size += (uint)m_source.Length + 1;
            size += (uint)m_export_tool.Length + 1;
            size += time_size;
            size += (uint)m_owner_name.Length + 1;
            size += time_size;
            size += (uint)m_export_modif_name_tool.Length + 1;
            size += time_size;
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

            temp.AddRange(Encoding.Default.GetBytes(m_texture));
            temp.Add(0);
            temp.AddRange(Encoding.Default.GetBytes(m_shader));
            temp.Add(0);

            return temp.ToArray();

        }
    }
}
