using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class BBox
    {
        public float[] min;
        public float[] max;

        public BBox()
        {
            min = new float[3];
            max = new float[3];
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes(min[0]));
            temp.AddRange(BitConverter.GetBytes(min[1]));
            temp.AddRange(BitConverter.GetBytes(min[2]));

            temp.AddRange(BitConverter.GetBytes(max[0]));
            temp.AddRange(BitConverter.GetBytes(max[1]));
            temp.AddRange(BitConverter.GetBytes(max[2]));

            return temp.ToArray();
        }
    };

    public class BSphere
    {
        public float[] c;
        public float r;

        public BSphere()
        {
            c = new float[3];
            r = 0.0f;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes(c[0]));
            temp.AddRange(BitConverter.GetBytes(c[1]));
            temp.AddRange(BitConverter.GetBytes(c[2]));
            temp.AddRange(BitConverter.GetBytes(r));

            return temp.ToArray();
        }
    };

    public class OGF_Header
    {
        public byte format_version;
        public byte type;
        public short shader_id;
        public BBox bb;
        public BSphere bs;

        public OGF_Header()
        {
            bb = new BBox();
            bs = new BSphere();
            format_version = 4;
            type = 0;
            shader_id = 0;
        }

        public byte[] data(byte version)
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes((uint)OGF.OGF_HEADER));
            temp.AddRange(BitConverter.GetBytes((version == 4 ? 44 : 4)));

            temp.Add(format_version);
            temp.Add(type);
            temp.AddRange(BitConverter.GetBytes(shader_id));

            if (version == 4)
            {
                temp.AddRange(bb.data());
                temp.AddRange(bs.data());
            }

            return temp.ToArray();
        }
    };

    public class OGF_Child
    {
        public string m_texture;
        public string m_shader;

        public OGF_Child(int _old_size, string texture, string shader)
        {
            Vertices = new List<SSkelVert>();
            Faces = new List<SSkelFace>();
            SWI = new List<VIPM_SWR>();
            m_texture = texture;
            m_shader = shader;
            old_size = _old_size;
            links = 0;
            to_delete = false;
            old_verts_size = 0;
            FVF = 0;
        }

        public int old_size;
        public int old_verts_size;

        public uint links;
        public uint FVF;
        public bool to_delete;

        public List<SSkelVert> Vertices;
        public List<SSkelFace> Faces;
        public List<VIPM_SWR> SWI;
        public OGF_Header Header;

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

        public byte[] data(byte version)
        {
            List<byte> temp = new List<byte>();

            // Header start
            if (Header != null)
                temp.AddRange(Header.data(version));
            // Header end

            // Texture start
            temp.AddRange(BitConverter.GetBytes((uint)OGF.OGF_TEXTURE));
            temp.AddRange(BitConverter.GetBytes(m_texture.Length + m_shader.Length + 2));

            temp.AddRange(Encoding.Default.GetBytes(m_texture));
            temp.Add(0);
            temp.AddRange(Encoding.Default.GetBytes(m_shader));
            temp.Add(0);
            // Texture end

            // Verts start
            uint VertsChunk = (version == 4 ? (uint)OGF.OGF4_VERTICES : (uint)OGF.OGF3_VERTICES);
            temp.AddRange(BitConverter.GetBytes(VertsChunk));
            temp.AddRange(BitConverter.GetBytes((uint)GetVertsChunk(version).Length));

            temp.AddRange(GetVertsChunk(version));

            // Verts end

            // Indices start
            uint FacesChunk = (version == 4 ? (uint)OGF.OGF4_INDICES : (uint)OGF.OGF3_INDICES);
            temp.AddRange(BitConverter.GetBytes(FacesChunk));
            temp.AddRange(BitConverter.GetBytes(Faces.Count * 3 * 2 + 4));

            temp.AddRange(BitConverter.GetBytes(Faces.Count * 3));
            for (int i = 0; i < Faces.Count; i++)
            {
                temp.AddRange(BitConverter.GetBytes(Faces[i].v[0]));
                temp.AddRange(BitConverter.GetBytes(Faces[i].v[1]));
                temp.AddRange(BitConverter.GetBytes(Faces[i].v[2]));
            }
            // Indices end

            // SWR start
            if (SWI.Count > 0)
            {
                temp.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_SWIDATA));
                temp.AddRange(BitConverter.GetBytes(4 + 4 + 4 + 4 + 4 + SWI.Count * 8));

                temp.AddRange(BitConverter.GetBytes(0));
                temp.AddRange(BitConverter.GetBytes(0));
                temp.AddRange(BitConverter.GetBytes(0));
                temp.AddRange(BitConverter.GetBytes(0));
                temp.AddRange(BitConverter.GetBytes(SWI.Count));

                for (int i = 0; i < SWI.Count; i++)
                {
                    temp.AddRange(BitConverter.GetBytes(SWI[i].offset));
                    temp.AddRange(BitConverter.GetBytes(SWI[i].num_tris));
                    temp.AddRange(BitConverter.GetBytes(SWI[i].num_verts));
                }
            }
            // SWR end

            return temp.ToArray();
        }

        private byte[] GetVertsChunk(byte version)
        {
            List<byte> temp = new List<byte>();

            if (LinksCount() == 0)
                temp.AddRange(BitConverter.GetBytes(FVF));
            else
                temp.AddRange(BitConverter.GetBytes(links));
            temp.AddRange(BitConverter.GetBytes(Vertices.Count));

            for (int i = 0; i < Vertices.Count; i++)
            {
                SSkelVert vert = Vertices[i];

                switch (LinksCount())
                {
                    case 1:
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.offs));
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.norm));

                        if (version == 4)
                        {
                            temp.AddRange(OGF_Editor.GetVec3Bytes(vert.tang));
                            temp.AddRange(OGF_Editor.GetVec3Bytes(vert.binorm));
                        }

                        temp.AddRange(OGF_Editor.GetVec2Bytes(vert.uv));
                        temp.AddRange(BitConverter.GetBytes(vert.bones_id[0]));
                        break;
                    case 2:
                        temp.AddRange(BitConverter.GetBytes((short)vert.bones_id[0]));
                        temp.AddRange(BitConverter.GetBytes((short)vert.bones_id[1]));

                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.offs));
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.norm));

                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.tang));
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.binorm));

                        temp.AddRange(BitConverter.GetBytes(vert.bones_infl[0]));
                        temp.AddRange(OGF_Editor.GetVec2Bytes(vert.uv));
                        break;
                    case 3:
                    case 4:
                        for (int j = 0; j < LinksCount(); j++)
                            temp.AddRange(BitConverter.GetBytes((short)vert.bones_id[j]));

                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.offs));
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.norm));

                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.tang));
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.binorm));

                        for (int j = 0; j < LinksCount() - 1; j++)
                            temp.AddRange(BitConverter.GetBytes(vert.bones_infl[j]));

                        temp.AddRange(OGF_Editor.GetVec2Bytes(vert.uv));
                        break;
                    default: // Static
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.offs));
                        temp.AddRange(OGF_Editor.GetVec3Bytes(vert.norm));
                        temp.AddRange(OGF_Editor.GetVec2Bytes(vert.uv));
                        break;
                }
            }

            return temp.ToArray();
        }
    }
}
