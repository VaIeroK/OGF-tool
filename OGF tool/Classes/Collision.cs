using System;
using System.Collections.Generic;

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
            min = xr_loader.ReadVector();
            max = xr_loader.ReadVector();
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(FVec.GetBytes(min));
            temp.AddRange(FVec.GetBytes(max));

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
            c = xr_loader.ReadVector();
            r = xr_loader.ReadFloat();
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(FVec.GetBytes(c));
            temp.AddRange(BitConverter.GetBytes(r));

            return temp.ToArray();
        }
    };
}
