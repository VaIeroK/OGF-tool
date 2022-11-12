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

        public List<SSkelVert> GetVisualVerts(bool generate_normals = true)
        {
            List<SSkelVert> verts = new List<SSkelVert>();

            float x_diff = max[0]-min[0];
            float y_diff = max[1]-min[1];
            float z_diff = max[2]-min[2];

            SSkelVert vert1 = new SSkelVert();
            vert1.offs = new float[3] { min[0], min[1], min[2] };

            SSkelVert vert2 = new SSkelVert();
            vert2.offs = new float[3] { min[0] + x_diff, min[1], min[2] };

            SSkelVert vert3 = new SSkelVert();
            vert3.offs = new float[3] { min[0], min[1] + y_diff, min[2] };

            SSkelVert vert4 = new SSkelVert();
            vert4.offs = new float[3] { min[0] + x_diff, min[1] + y_diff, min[2] };

            SSkelVert vert5 = new SSkelVert();
            vert5.offs = new float[3] { min[0], min[1], min[2] + z_diff };

            SSkelVert vert6 = new SSkelVert();
            vert6.offs = new float[3] { min[0] + x_diff, min[1], min[2] + z_diff };

            SSkelVert vert7 = new SSkelVert();
            vert7.offs = new float[3] { min[0], min[1] + y_diff, min[2] + z_diff };

            SSkelVert vert8 = new SSkelVert();
            vert8.offs = new float[3] { max[0], max[1], max[2] };

            verts.Add(vert1);
            verts.Add(vert2);
            verts.Add(vert3);
            verts.Add(vert4);
            verts.Add(vert5);
            verts.Add(vert6);
            verts.Add(vert7);
            verts.Add(vert8);

            if (generate_normals)
                SSkelVert.GenerateNormals(ref verts, GetVisualFaces(verts));

            return verts;
        }

        public List<SSkelFace> GetVisualFaces(List<SSkelVert> Verts)
        {
            List<SSkelFace> faces = new List<SSkelFace>();
 
            int VertsCount = GetVisualVerts(false).Count;
            int[,] VertList = new int[,] 
            { 
                { 7, 5, 6 }, // Front
                { 7, 6, 8 }, // Front
                { 3, 1, 4 }, // Back
                { 1, 2, 4 }, // Back
                { 3, 7, 4 }, // Down
                { 7, 8, 4 }, // Down
                { 1, 5, 6 }, // Up
                { 1, 6, 2 }, // Up
                { 3, 1, 5 }, // Left
                { 3, 5, 7 }, // Left
                { 4, 2, 8 }, // Right
                { 2, 6, 8 }  // Right
            };

            for (int i = 0; i < VertList.Length / 3; i++)
            {
                SSkelFace face = new SSkelFace();

                for (int j = 0; j < 3; j++)
                {
                    int vert_idx = VertsCount - VertList[i, j] + 1;
                    face.v[j] = (ushort)(Verts.Count - vert_idx);
                }

                faces.Add(face);
            }

            return faces;
        }
    };

    public class BSphere
    {
        public float[] c;
        public float r;

        public BSphere()
        {
            Identity();
        }

        public void Identity()
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
