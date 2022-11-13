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
            min = new float[3] { float.MaxValue, float.MaxValue, float.MaxValue };
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

            float x_diff = max[0]-min[0];
            float y_diff = max[1]-min[1];
            float z_diff = max[2]-min[2];

            // Back start
            {
                SSkelVert vert1 = new SSkelVert();
                vert1.offs = new float[3] { min[0], min[1], min[2] };
                vert1.norm = new float[3] { 0.0f, 0.0f, -1.0f };

                SSkelVert vert2 = new SSkelVert();
                vert2.offs = new float[3] { min[0] + x_diff, min[1], min[2] };
                vert2.norm = new float[3] { 0.0f, 0.0f, -1.0f };

                SSkelVert vert3 = new SSkelVert();
                vert3.offs = new float[3] { min[0], min[1] + y_diff, min[2] };
                vert3.norm = new float[3] { 0.0f, 0.0f, -1.0f };

                SSkelVert vert4 = new SSkelVert();
                vert4.offs = new float[3] { min[0] + x_diff, min[1] + y_diff, min[2] };
                vert4.norm = new float[3] { 0.0f, 0.0f, -1.0f };

                verts.Add(vert1);
                verts.Add(vert2);
                verts.Add(vert3);
                verts.Add(vert4);
            }
            // Back end 

            // Left start
            {
                SSkelVert vert1 = new SSkelVert();
                vert1.offs = new float[3] { min[0], min[1], min[2] };
                vert1.norm = new float[3] { -1.0f, 0.0f, 0.0f };

                SSkelVert vert2 = new SSkelVert();
                vert2.offs = new float[3] { min[0], min[1] + y_diff, min[2] };
                vert2.norm = new float[3] { -1.0f, 0.0f, 0.0f };

                SSkelVert vert3 = new SSkelVert();
                vert3.offs = new float[3] { min[0], min[1], min[2] + z_diff };
                vert3.norm = new float[3] { -1.0f, 0.0f, 0.0f };

                SSkelVert vert4 = new SSkelVert();
                vert4.offs = new float[3] { min[0], min[1] + y_diff, min[2] + z_diff };
                vert4.norm = new float[3] { -1.0f, 0.0f, 0.0f };

                verts.Add(vert1);
                verts.Add(vert2);
                verts.Add(vert3);
                verts.Add(vert4);
            }
            // Left end

            // Right start
            {
                SSkelVert vert1 = new SSkelVert();
                vert1.offs = new float[3] { min[0] + x_diff, min[1], min[2] };
                vert1.norm = new float[3] { 1.0f, 0.0f, 0.0f };

                SSkelVert vert2 = new SSkelVert();
                vert2.offs = new float[3] { min[0] + x_diff, min[1], min[2] + z_diff };
                vert2.norm = new float[3] { 1.0f, 0.0f, 0.0f };

                SSkelVert vert3 = new SSkelVert();
                vert3.offs = new float[3] { min[0] + x_diff, min[1] + y_diff, min[2] };
                vert3.norm = new float[3] { 1.0f, 0.0f, 0.0f };

                SSkelVert vert4 = new SSkelVert();
                vert4.offs = new float[3] { max[0], max[1], max[2] };
                vert4.norm = new float[3] { 1.0f, 0.0f, 0.0f };

                verts.Add(vert1);
                verts.Add(vert2);
                verts.Add(vert3);
                verts.Add(vert4);
            }
            // Right end

            // Front start
            {
                SSkelVert vert1 = new SSkelVert();
                vert1.offs = new float[3] { min[0], min[1], min[2] + z_diff };
                vert1.norm = new float[3] { 0.0f, 0.0f, 1.0f };

                SSkelVert vert2 = new SSkelVert();
                vert2.offs = new float[3] { min[0] + x_diff, min[1], min[2] + z_diff };
                vert2.norm = new float[3] { 0.0f, 0.0f, 1.0f };

                SSkelVert vert3 = new SSkelVert();
                vert3.offs = new float[3] { min[0], min[1] + y_diff, min[2] + z_diff };
                vert3.norm = new float[3] { 0.0f, 0.0f, 1.0f };

                SSkelVert vert4 = new SSkelVert();
                vert4.offs = new float[3] { max[0], max[1], max[2] };
                vert4.norm = new float[3] { 0.0f, 0.0f, 1.0f };

                verts.Add(vert1);
                verts.Add(vert2);
                verts.Add(vert3);
                verts.Add(vert4);
            }
            // Front end 

            // Up start
            {
                SSkelVert vert1 = new SSkelVert();
                vert1.offs = new float[3] { min[0], min[1] + y_diff, min[2] };
                vert1.norm = new float[3] { 0.0f, 1.0f, 0.0f };

                SSkelVert vert2 = new SSkelVert();
                vert2.offs = new float[3] { min[0] + x_diff, min[1] + y_diff, min[2] };
                vert2.norm = new float[3] { 0.0f, 1.0f, 0.0f };

                SSkelVert vert3 = new SSkelVert();
                vert3.offs = new float[3] { min[0], min[1] + y_diff, min[2] + z_diff };
                vert3.norm = new float[3] { 0.0f, 1.0f, 0.0f };

                SSkelVert vert4 = new SSkelVert();
                vert4.offs = new float[3] { max[0], max[1], max[2] };
                vert4.norm = new float[3] { 0.0f, 1.0f, 0.0f };

                verts.Add(vert1);
                verts.Add(vert2);
                verts.Add(vert3);
                verts.Add(vert4);
            }
            // Up end 

            // Down start
            {
                SSkelVert vert1 = new SSkelVert();
                vert1.offs = new float[3] { min[0], min[1], min[2] };
                vert1.norm = new float[3] { 0.0f, -1.0f, 0.0f };

                SSkelVert vert2 = new SSkelVert();
                vert2.offs = new float[3] { min[0] + x_diff, min[1], min[2] };
                vert2.norm = new float[3] { 0.0f, -1.0f, 0.0f };

                SSkelVert vert3 = new SSkelVert();
                vert3.offs = new float[3] { min[0], min[1], min[2] + z_diff };
                vert3.norm = new float[3] { 0.0f, -1.0f, 0.0f };

                SSkelVert vert4 = new SSkelVert();
                vert4.offs = new float[3] { min[0] + x_diff, min[1], min[2] + z_diff };
                vert4.norm = new float[3] { 0.0f, -1.0f, 0.0f };

                verts.Add(vert1);
                verts.Add(vert2);
                verts.Add(vert3);
                verts.Add(vert4);
            }
            // Down end 

            return verts;
        }

        public List<SSkelFace> GetVisualFaces(List<SSkelVert> Verts)
        {
            List<SSkelFace> faces = new List<SSkelFace>();
 
            int VertsCount = GetVisualVerts().Count; // 24
            int[,] FaceVertList = new int[,]
            {
                { 1, 3, 4 },
                { 4, 2, 1 },
                { 7, 8, 6 },
                { 6, 5, 7 },
                { 9, 11, 12 },
                { 12, 10, 9 },
                { 16, 15, 13 },
                { 13, 14, 16 },
                { 17, 19, 20 },
                { 20, 18, 17 },
                { 24, 23, 21 },
                { 21, 22, 24 }
            };

            for (int i = 0; i < FaceVertList.Length / 3; i++)
            {
                SSkelFace face = new SSkelFace();

                for (int j = 0; j < 3; j++)
                {
                    int vert_idx = VertsCount - FaceVertList[i, j] + 1;
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
