using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class OGF_Child
    {
        public string m_texture;
        public string m_shader;

        public OGF_Child(long _pos, long _parent_pos, uint _chunk_size, int _old_size, string texture, string shader)
        {
            Vertices = new List<SSkelVert>();
            Faces = new List<SSkelFace>();
            SWI = new List<VIPM_SWR>();
            pos = _pos;
            parent_pos = _parent_pos;
            m_texture = texture;
            m_shader = shader;
            old_size = _old_size;
            chunk_size = _chunk_size;
            links = 0;
            to_delete = false;
            old_verts_size = 0;
        }

        public long pos;
        public long parent_pos;
        public uint chunk_size;
        public int old_size;
        public int old_verts_size;

        public uint links;
        public bool to_delete;

        public List<SSkelVert> Vertices;
        public List<SSkelFace> Faces;
        public List<VIPM_SWR> SWI;

        public long GetVertsSize(uint vers)
        {
            int size = 0;
            switch (LinksCount())
            {
                case 1:
                    size = 12 + 12 + (vers == 4 ? 12 + 12 : 0) + 8 + 4;
                    break;
                case 2:
                    size = 2 + 2 + 12 + 12 + 12 + 12 + 4 + 8;
                    break;
                case 3:
                case 4:
                    for (int j = 0; j < LinksCount(); j++)
                        size += 2;

                    size += 12 + 12 + 12 + 12;

                    for (int j = 0; j < LinksCount() - 1; j++)
                        size += 4;

                    size += 8;
                    break;
            }

            return size * Vertices.Count + 4 + 4;
        }

        private int CalcLod(float lod)
        {
            return (int)Math.Floor(lod * (SWI.Count - 1) + 0.5f);
        }

        public List<SSkelFace> Faces_SWI(float lod)
        {
            if (SWI.Count == 0) return Faces;

            List<SSkelFace> sSkelFaces = new List<SSkelFace>();

            VIPM_SWR SWR = SWI[CalcLod(lod)];

            for (int i = (int)SWR.offset / 3; i < ((int)SWR.offset / 3) + SWR.num_tris; i++)
            {
                sSkelFaces.Add(Faces[i]);
            }

            return sSkelFaces;
        }

        public uint LinksCount()
        {
            uint temp_links;
            if (links >= 0x12071980)
                temp_links = links / 0x12071980;
            else
                temp_links = links;

            return temp_links;
        }

        public void SetLinks(uint count)
        {
            if (links >= 0x12071980)
                links = count * 0x12071980;
            else
                links = count;
        }

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
