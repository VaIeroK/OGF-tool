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
            Invalidate();
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

        public void Invalidate()
        {
            min = new float[3];
            max = new float[3];
        }

        public void Set(float[] min, float[] max)
        {
            this.min = min;
            this.max = max;
        }

        public void Modify(float[] vec)
        {
            for (int i = 0; i < 3; i++)
                min[i] = Math.Min(min[i], vec[i]);

            for (int i = 0; i < 3; i++)
                max[i] = Math.Max(max[i], vec[i]);
        }

        public void Merge(BBox box)
        {
            Modify(box.min);
            Modify(box.max);
        }

        public float[] GetCenter()
        {
            float[] C = new float[3];
            for (int i = 0; i < 3; i++)
                C[i] = (min[i] + max[i]) * 0.5f;

            return C;
        }

        public void CreateBox(List<SSkelVert> Vertices)
        {
            if (0 == Vertices.Count)
            {
                Set(new float[3], new float[3]);
                return;
            }
            Set(Vertices[0].offs, Vertices[0].offs);
            for (int k = 1; k < Vertices.Count; k++)
                Modify(Vertices[k].offs);
        }

        public List<SSkelVert> GetVisualVerts()
        {
            List<SSkelVert> verts = new List<SSkelVert>();

            SSkelVert vert1 = new SSkelVert();
            vert1.offs = min;

            SSkelVert vert2 = new SSkelVert();
            vert1.offs = min;

            return verts;
        }
    };

    public class BSphere
    {
        public float[] c;
        public float r;

        public BSphere()
        {
            Invalidate();
        }

        public void Invalidate()
        {
            c = new float[3];
            r = 0.0f;
        }

        public void Load(XRayLoader xr_loader)
        {
            c = xr_loader.ReadVector();
            r = xr_loader.ReadFloat();
        }

        public void CreateSphere(BBox box)
        {
            c = box.GetCenter();
            r = FVec.DistanceTo(c, box.max);
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
