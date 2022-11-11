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
            Identity();
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

        public void Identity()
        {
            min = new float[3];
            max = new float[3];
        }

        public void Invalidate()
        {
            min = new float[3] { float.MaxValue, float.MaxValue , float.MaxValue };
            max = new float[3] { float.MinValue, float.MinValue, float.MinValue };
        }

        public void Set(float[] min, float[] max)
        {
            this.min = min;
            this.max = max;
        }

        public void Modify(float[] vec)
        {
            for (int i = 0; i < 3; i++)
            {
                min[i] = Math.Min(min[i], vec[i]);
                max[i] = Math.Max(max[i], vec[i]);
            }
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
            Invalidate();
            for (int k = 0; k < Vertices.Count; k++)
                Modify(Vertices[k].Offset());
        }

        public List<SSkelVert> GetVisualVerts()
        {
            List<SSkelVert> verts = new List<SSkelVert>();

            float[] _min = min;
            float[] _max = FVec.Mul(max, 2.0f);

            SSkelVert vert1 = new SSkelVert();
            vert1.offs = new float[3] { _min[0], _min[1], _min[2] };

            SSkelVert vert2 = new SSkelVert();
            vert2.offs = new float[3] { _min[0] + _max[0], _min[1], _min[2] };

            SSkelVert vert3 = new SSkelVert();
            vert3.offs = new float[3] { _min[0], _min[1] + _max[1], _min[2] };

            SSkelVert vert4 = new SSkelVert();
            vert4.offs = new float[3] { _min[0] + _max[0], _min[1] + _max[1], _min[2] };

            SSkelVert vert5 = new SSkelVert();
            vert5.offs = new float[3] { _min[0], _min[1], _min[2] + _max[2] };

            SSkelVert vert6 = new SSkelVert();
            vert6.offs = new float[3] { _min[0] + _max[0], _min[1], _min[2] + _max[2] };

            SSkelVert vert7 = new SSkelVert();
            vert7.offs = new float[3] { _min[0], _min[1] + _max[1], _min[2] + _max[2] };

            SSkelVert vert8 = new SSkelVert();
            vert8.offs = new float[3] { _min[0] + _max[0], _min[1] + _max[1], _min[2] + _max[2] };

            verts.Add(vert1);
            verts.Add(vert2);
            verts.Add(vert3);
            verts.Add(vert4);
            verts.Add(vert5);
            verts.Add(vert6);
            verts.Add(vert7);
            verts.Add(vert8);

            SSkelVert.GenerateNormals(ref verts, GetVisualFaces(verts));

            return verts;
        }

        public List<SSkelFace> GetVisualFaces(List<SSkelVert> Verts)
        {
            List<SSkelFace> faces = new List<SSkelFace>();

            /*
             * 1 face - 8
             * 2 face - 7
             * 3 face - 6
             * 4 face - 5
             * 5 face - 4
             * 6 face - 3
             * 7 face - 2
             * 8 face - 1
             */

            // Front
            SSkelFace face1 = new SSkelFace();
            face1.v[0] = (ushort)(Verts.Count - 2); // 7
            face1.v[1] = (ushort)(Verts.Count - 4); // 5
            face1.v[2] = (ushort)(Verts.Count - 1); // 8

            SSkelFace face2 = new SSkelFace();
            face2.v[0] = (ushort)(Verts.Count - 4); // 5
            face2.v[1] = (ushort)(Verts.Count - 3); // 6
            face2.v[2] = (ushort)(Verts.Count - 1); // 8

            // Back
            SSkelFace face3 = new SSkelFace();
            face3.v[0] = (ushort)(Verts.Count - 6); // 3
            face3.v[1] = (ushort)(Verts.Count - 8); // 1
            face3.v[2] = (ushort)(Verts.Count - 5); // 4

            SSkelFace face4 = new SSkelFace();
            face4.v[0] = (ushort)(Verts.Count - 8); // 1
            face4.v[1] = (ushort)(Verts.Count - 7); // 2
            face4.v[2] = (ushort)(Verts.Count - 5); // 4

            // Down
            SSkelFace face5 = new SSkelFace();
            face5.v[0] = (ushort)(Verts.Count - 6); // 3
            face5.v[1] = (ushort)(Verts.Count - 2); // 7
            face5.v[2] = (ushort)(Verts.Count - 5); // 4

            SSkelFace face6 = new SSkelFace();
            face6.v[0] = (ushort)(Verts.Count - 2); // 7
            face6.v[1] = (ushort)(Verts.Count - 1); // 8
            face6.v[2] = (ushort)(Verts.Count - 5); // 4

            // Up
            SSkelFace face7 = new SSkelFace();
            face7.v[0] = (ushort)(Verts.Count - 8); // 1
            face7.v[1] = (ushort)(Verts.Count - 4); // 5
            face7.v[2] = (ushort)(Verts.Count - 7); // 2

            SSkelFace face8 = new SSkelFace();
            face8.v[0] = (ushort)(Verts.Count - 4); // 5
            face8.v[1] = (ushort)(Verts.Count - 3); // 6
            face8.v[2] = (ushort)(Verts.Count - 7); // 2

            // Left
            SSkelFace face9 = new SSkelFace();
            face9.v[0] = (ushort)(Verts.Count - 6); // 3
            face9.v[1] = (ushort)(Verts.Count - 8); // 1
            face9.v[2] = (ushort)(Verts.Count - 2); // 7

            SSkelFace face10 = new SSkelFace();
            face10.v[0] = (ushort)(Verts.Count - 8); // 1
            face10.v[1] = (ushort)(Verts.Count - 4); // 5
            face10.v[2] = (ushort)(Verts.Count - 2); // 7

            // Right
            SSkelFace face11 = new SSkelFace();
            face11.v[0] = (ushort)(Verts.Count - 5); // 4
            face11.v[1] = (ushort)(Verts.Count - 7); // 2
            face11.v[2] = (ushort)(Verts.Count - 1); // 8

            SSkelFace face12 = new SSkelFace();
            face12.v[0] = (ushort)(Verts.Count - 7); // 2
            face12.v[1] = (ushort)(Verts.Count - 3); // 6
            face12.v[2] = (ushort)(Verts.Count - 1); // 8

            faces.Add(face1);
            faces.Add(face2);
            faces.Add(face3);
            faces.Add(face4);
            faces.Add(face5);
            faces.Add(face6);
            faces.Add(face7);
            faces.Add(face8);
            faces.Add(face9);
            faces.Add(face10);
            faces.Add(face11);
            faces.Add(face12);

            return faces;
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
