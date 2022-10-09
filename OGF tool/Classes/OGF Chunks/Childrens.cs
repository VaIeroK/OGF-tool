using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void Load(XRayLoader xr_loader)
        {
            min[0] = xr_loader.ReadFloat();
            min[1] = xr_loader.ReadFloat();
            min[2] = xr_loader.ReadFloat();

            max[0] = xr_loader.ReadFloat();
            max[1] = xr_loader.ReadFloat();
            max[2] = xr_loader.ReadFloat();
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

        public void Load(XRayLoader xr_loader)
        {
            c[0] = xr_loader.ReadFloat();
            c[1] = xr_loader.ReadFloat();
            c[2] = xr_loader.ReadFloat();
            r = xr_loader.ReadFloat();
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

        public void Load(XRayLoader xr_loader)
        {
            format_version = xr_loader.ReadByte();
            type = xr_loader.ReadByte();
            shader_id = (short)xr_loader.ReadUInt16();

            if (format_version == 4)
            {
                bb.Load(xr_loader);
                bs.Load(xr_loader);
            }
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(BitConverter.GetBytes((uint)OGF.OGF_HEADER));
            temp.AddRange(BitConverter.GetBytes((format_version == 4 ? 44 : 4)));

            temp.Add(format_version);
            temp.Add(type);
            temp.AddRange(BitConverter.GetBytes(shader_id));

            if (format_version == 4)
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

        public OGF_Child()
        {
            Vertices = new List<SSkelVert>();
            Faces = new List<SSkelFace>();
            SWI = new List<VIPM_SWR>();
            old_size = 0;
            links = 0;
            to_delete = false;
            FVF = 0;
        }

        public int old_size;

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

        public void LoadDM(XRayLoader xr_loader)
        {
            m_shader = xr_loader.read_stringZ();
            m_texture = xr_loader.read_stringZ();
            xr_loader.ReadUInt32();
            xr_loader.ReadFloat();
            xr_loader.ReadFloat();

            old_size = m_shader.Length + m_texture.Length + 2;

            uint verts = xr_loader.ReadUInt32();
            uint faces = xr_loader.ReadUInt32() / 3;

            for (int i = 0; i < verts; i++)
            {
                SSkelVert Vert = new SSkelVert();
                Vert.offs = xr_loader.ReadVector();
                Vert.uv = xr_loader.ReadVector2();
                Vertices.Add(Vert);
            }

            for (int i = 0; i < faces; i++)
            {
                SSkelFace Face = new SSkelFace();
                Face.v[0] = (ushort)xr_loader.ReadUInt16();
                Face.v[1] = (ushort)xr_loader.ReadUInt16();
                Face.v[2] = (ushort)xr_loader.ReadUInt16();
                Faces.Add(Face);
            }
        }

        public bool Load(XRayLoader xr_loader)
        {
            // Header start
            if (!xr_loader.find_chunk((int)OGF.OGF_HEADER, false, true)) 
                return false;

            Header = new OGF_Header();
            Header.Load(xr_loader);
            // Header end

            // Texture start
            if (!xr_loader.find_chunk((int)OGF.OGF_TEXTURE, false, true))
                return false;

            m_texture = xr_loader.read_stringZ();
            m_shader = xr_loader.read_stringZ();
            // Texture end

            // Verts start
            int VertsChunk = (Header.format_version == 3 ? (int)OGF.OGF3_VERTICES : (int)OGF.OGF4_VERTICES);

            if (!xr_loader.find_chunk(VertsChunk, false, true))
                return false;

            FVF = links = xr_loader.ReadUInt32();
            uint verts = xr_loader.ReadUInt32();

            for (int i = 0; i < verts; i++)
            {
                SSkelVert Vert = new SSkelVert();
                switch (LinksCount())
                {
                    case 1:
                        Vert.offs = xr_loader.ReadVector();
                        Vert.norm = xr_loader.ReadVector();
                        if (Header.format_version == 4)
                        {
                            Vert.tang = xr_loader.ReadVector();
                            Vert.binorm = xr_loader.ReadVector();
                        }
                        Vert.uv = xr_loader.ReadVector2();

                        Vert.bones_id[0] = xr_loader.ReadUInt32();
                        break;
                    case 2:
                        Vert.bones_id[0] = xr_loader.ReadUInt16();
                        Vert.bones_id[1] = xr_loader.ReadUInt16();

                        Vert.offs = xr_loader.ReadVector();
                        Vert.norm = xr_loader.ReadVector();
                        Vert.tang = xr_loader.ReadVector();
                        Vert.binorm = xr_loader.ReadVector();
                        Vert.bones_infl[0] = xr_loader.ReadFloat();
                        Vert.uv = xr_loader.ReadVector2();
                        break;
                    case 3:
                    case 4:
                        for (int j = 0; j < LinksCount(); j++)
                            Vert.bones_id[j] = xr_loader.ReadUInt16();

                        Vert.offs = xr_loader.ReadVector();
                        Vert.norm = xr_loader.ReadVector();
                        Vert.tang = xr_loader.ReadVector();
                        Vert.binorm = xr_loader.ReadVector();

                        for (int j = 0; j < LinksCount() - 1; j++)
                            Vert.bones_infl[j] = xr_loader.ReadFloat();

                        Vert.uv = xr_loader.ReadVector2();
                        break;
                    default:
                        Vert.offs = xr_loader.ReadVector();
                        Vert.norm = xr_loader.ReadVector();
                        Vert.uv = xr_loader.ReadVector2();
                        links = 0;
                        break;
                }
                Vertices.Add(Vert);
            }
            // Verts end

            // Indices start
            int FacesChunk = (Header.format_version == 3 ? (int)OGF.OGF3_INDICES : (int)OGF.OGF4_INDICES);

            if (!xr_loader.find_chunk(FacesChunk, false, true))
                return false;

            uint faces = xr_loader.ReadUInt32() / 3;

            for (uint i = 0; i < faces; i++)
            {
                SSkelFace Face = new SSkelFace();
                Face.v[0] = (ushort)xr_loader.ReadUInt16();
                Face.v[1] = (ushort)xr_loader.ReadUInt16();
                Face.v[2] = (ushort)xr_loader.ReadUInt16();
                Faces.Add(Face);
            }
            // Indices end

            // SWR start
            if (Header.format_version == 4)
            {
                if (xr_loader.find_chunk((int)OGF.OGF4_SWIDATA, false, true))
                {
                    xr_loader.ReadUInt32();
                    xr_loader.ReadUInt32();
                    xr_loader.ReadUInt32();
                    xr_loader.ReadUInt32();

                    uint swi_size = xr_loader.ReadUInt32();

                    for (uint i = 0; i < swi_size; i++)
                    {
                        VIPM_SWR SWR = new VIPM_SWR();
                        SWR.offset = xr_loader.ReadUInt32();
                        SWR.num_tris = (ushort)xr_loader.ReadUInt16();
                        SWR.num_verts = (ushort)xr_loader.ReadUInt16();
                        SWI.Add(SWR);
                    }
                }
            }

            old_size = data().Length;

            // SWR end
            return true;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            // Header start
            temp.AddRange(Header.data());
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
            uint VertsChunk = (Header.format_version == 4 ? (uint)OGF.OGF4_VERTICES : (uint)OGF.OGF3_VERTICES);
            temp.AddRange(BitConverter.GetBytes(VertsChunk));
            temp.AddRange(BitConverter.GetBytes((uint)GetVertsChunk().Length));

            temp.AddRange(GetVertsChunk());

            // Verts end

            // Indices start
            uint FacesChunk = (Header.format_version == 4 ? (uint)OGF.OGF4_INDICES : (uint)OGF.OGF3_INDICES);
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

        private byte[] GetVertsChunk()
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

                        if (Header.format_version == 4)
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
