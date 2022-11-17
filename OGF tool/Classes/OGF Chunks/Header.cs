using System;
using System.Collections.Generic;

namespace OGF_tool
{
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

        public bool IsProgressive()
        {
            if (format_version == 4)
                return type == (byte)ModelType.MT4_PROGRESSIVE || type == (byte)ModelType.MT4_SKELETON_GEOMDEF_PM;
            else
                return type == (byte)ModelType.MT3_PROGRESSIVE || type == (byte)ModelType.MT3_SKELETON_GEOMDEF_PM;
        }

        public bool IsSkeleton()
        {
            if (format_version == 4)
                return type == (byte)ModelType.MT4_SKELETON_ANIM || type == (byte)ModelType.MT4_SKELETON_RIGID;
            else
                return type == (byte)ModelType.MT3_SKELETON_ANIM || type == (byte)ModelType.MT3_SKELETON_RIGID;
        }

        public bool IsAnimated()
        {
            if (format_version == 4)
                return type == (byte)ModelType.MT4_SKELETON_ANIM;
            else
                return type == (byte)ModelType.MT3_SKELETON_ANIM;
        }

        public bool IsStatic()
        {
            if (format_version == 4)
                return type == (byte)ModelType.MT4_HIERRARHY;
            else
                return type == (byte)ModelType.MT3_HIERRARHY;
        }

        public bool IsStaticSingle()
        {
            if (format_version == 4)
                return type == (byte)ModelType.MT4_NORMAL || type == (byte)ModelType.MT4_PROGRESSIVE;
            else
                return type == (byte)ModelType.MT3_NORMAL || type == (byte)ModelType.MT3_PROGRESSIVE || type == (byte)ModelType.MT3_PROGRESSIVE2;
        }

        public void Skeleton()
        {
            if (format_version == 4)
                type = (byte)ModelType.MT4_SKELETON_RIGID;
            else
                type = (byte)ModelType.MT3_SKELETON_RIGID;
        }


        public void Animated()
        {
            if (format_version == 4)
                type = (byte)ModelType.MT4_SKELETON_ANIM;
            else
                type = (byte)ModelType.MT3_SKELETON_ANIM;
        }

        public void GeomdefST()
        {
            if (format_version == 4)
                type = (byte)ModelType.MT4_SKELETON_GEOMDEF_ST;
            else
                type = (byte)ModelType.MT3_SKELETON_GEOMDEF_ST;
        }

        public void Normal()
        {
            if (format_version == 4)
                type = (byte)ModelType.MT4_NORMAL;
            else
                type = (byte)ModelType.MT3_NORMAL;
        }

        public void Static(List<OGF_Child> childs)
        {
            int childs_count = 0;
            foreach (OGF_Child chld in childs)
            {
                if (!chld.to_delete)
                    childs_count++;
            }

            if (childs_count <= 1)
            {
                StaticSingle(childs);
                return;
            }

            if (format_version == 4)
                type = (byte)ModelType.MT4_HIERRARHY;
            else
                type = (byte)ModelType.MT3_HIERRARHY;
        }

        public void StaticSingle(List<OGF_Child> childs)
        {
            int childs_count = 0;
            foreach (OGF_Child chld in childs)
            {
                if (!chld.to_delete)
                    childs_count++;
            }

            if (childs_count > 1)
            {
                Static(childs);
                return;
            }

            if (format_version == 4)
                type = (childs[0].SWI.Count > 0) ? (byte)ModelType.MT4_PROGRESSIVE : (byte)ModelType.MT4_NORMAL;
            else
                type = (childs[0].SWI.Count > 0) ? (byte)ModelType.MT3_PROGRESSIVE : (byte)ModelType.MT3_NORMAL;
        }
    };
}
