using System;
using System.Collections.Generic;

namespace OGF_tool
{
    static internal class FVec
    {
        static public float[] CrossProduct(float[] v1, float[] v2)
        {
            var vec = new float[3] 
            {
                v1[1] * v2[2] - v2[1] * v1[2],
                -(v1[0] * v2[2] - v2[0] * v1[2]),
                v1[0] * v2[1] - v2[0] * v1[1]
            };

            return vec;
        }

        static public float DotProduct(float[] v1, float[] v2)
        {
            float product = 0.0f;
            for (int i = 0; i < 3; i++)
                product += v1[i] * v2[i];
            return product;
        }

        static public float[] Sub(float[] v1, float[] v2)
        {
            var vec = new float[3]
            {
                v1[0] - v2[0],
                v1[1] - v2[1],
                v1[2] - v2[2]
            };

            return vec;
        }

        static public float[] Add(float[] v1, float[] v2)
        {
            var vec = new float[3]
            {
                v1[0] + v2[0],
                v1[1] + v2[1],
                v1[2] + v2[2]
            };

            return vec;
        }

        static public float[] Mul(float[] v1, float val)
        {
            var vec = new float[3]
            {
                v1[0] * val,
                v1[1] * val,
                v1[2] * val
            };

            return vec;
        }

        static public float[] Mul(float[] v1, float[] v2)
        {
            var vec = new float[3]
            {
                v1[0] * v2[0],
                v1[1] * v2[1],
                v1[2] * v2[2]
            };

            return vec;
        }

        static public float[] Div(float[] v1, float val)
        {
            var vec = new float[3]
            {
                v1[0] / val,
                v1[1] / val,
                v1[2] / val
            };

            return vec;
        }

        static public float[] Min(float[] v1, float[] v2)
        {
            var vec = new float[3]
            {
                Math.Min(v1[0], v2[0]),
                Math.Min(v1[1], v2[1]),
                Math.Min(v1[2], v2[2])
            };

            return vec;
        }

        static public float[] Max(float[] v1, float[] v2)
        {
            var vec = new float[3]
            {
                Math.Max(v1[0], v2[0]),
                Math.Max(v1[1], v2[1]),
                Math.Max(v1[2], v2[2])
            };

            return vec;
        }

        static public float[] Normalize(float[] v)
        {
            return Mul(v, 1.0f / (float)Math.Sqrt(v[0]*v[0] + v[1]*v[1] + v[2]*v[2]));
        }

        static public float[] MirrorZ(float[] v)
        {
            var vec = new float[3]
            {
                v[0],
                v[1],
               -v[2]
            };

            return vec;
        }

        static public float[] RotateXYZRad(float[] v, float[] rot, float[] center)
        {
            float yaw = rot[0];
            float pitch = rot[1];
            float roll = rot[2];

            return RotateYPR(v, yaw, pitch, roll, center);
        }

        static public float[] RotateXYZRad(float[] v, float[] rot)
        {
            float yaw = rot[0];
            float pitch = rot[1];
            float roll = rot[2];

            return RotateYPR(v, yaw, pitch, roll, new float[3]);
        }

        static public float[] RotateXYZRad(float[] v, float x, float y, float z, float[] center)
        {
            float yaw = z;
            float pitch = y;
            float roll = x;

            return RotateYPR(v, yaw, pitch, roll, center);
        }

        static public float[] RotateXYZRad(float[] v, float x, float y, float z)
        {
            float yaw = z;
            float pitch = y;
            float roll = x;

            return RotateYPR(v, yaw, pitch, roll, new float[3]);
        }

        static public float[] RotateXYZ(float[] v, float[] rot, float[] center)
        {
            float yaw = rot[0] * (float)(Math.PI / 180.0f);
            float pitch = rot[1] * (float)(Math.PI / 180.0f);
            float roll = rot[2] * (float)(Math.PI / 180.0f);

            return RotateYPR(v, yaw, pitch, roll, center);
        }

        static public float[] RotateXYZ(float[] v, float[] rot)
        {
            float yaw = rot[0] * (float)(Math.PI / 180.0f);
            float pitch = rot[1] * (float)(Math.PI / 180.0f);
            float roll = rot[2] * (float)(Math.PI / 180.0f);

            return RotateYPR(v, yaw, pitch, roll, new float[3]);
        }

        static public float[] RotateXYZ(float[] v, float x, float y, float z, float[] center)
        {
            float yaw = z * (float)(Math.PI / 180.0f);
            float pitch = y * (float)(Math.PI / 180.0f);
            float roll = x * (float)(Math.PI / 180.0f);

            return RotateYPR(v, yaw, pitch, roll, center);
        }

        static public float[] RotateXYZ(float[] v, float x, float y, float z)
        {
            float yaw = z * (float)(Math.PI / 180.0f);
            float pitch = y * (float)(Math.PI / 180.0f);
            float roll = x * (float)(Math.PI / 180.0f);

            return RotateYPR(v, yaw, pitch, roll, new float[3]);
        }

        static public float[] RotateYPR(float[] v, float yaw, float pitch, float roll, float[] center)
        {
            float[] rotated = new float[3];

            double cosa = Math.Cos(yaw);
            double sina = Math.Sin(yaw);

            double cosb = Math.Cos(pitch);
            double sinb = Math.Sin(pitch);

            double cosc = Math.Cos(roll);
            double sinc = Math.Sin(roll);

            double Axx = cosa * cosb;
            double Axy = cosa * sinb * sinc - sina * cosc;
            double Axz = cosa * sinb * cosc + sina * sinc;

            double Ayx = sina * cosb;
            double Ayy = sina * sinb * sinc + cosa * cosc;
            double Ayz = sina * sinb * cosc - cosa * sinc;

            double Azx = -sinb;
            double Azy = cosb * sinc;
            double Azz = cosb * cosc;

            double px = v[0] - center[0];
            double py = v[1] - center[1];
            double pz = v[2] - center[2];

            rotated[0] = (float)(Axx * px + Axy * py + Axz * pz) + center[0];
            rotated[1] = (float)(Ayx * px + Ayy * py + Ayz * pz) + center[1];
            rotated[2] = (float)(Azx * px + Azy * py + Azz * pz) + center[2];

            return rotated;
        }

        static public float[] RotateYPR(float[] v, float[] ypr, float[] center)
        {
            return RotateYPR(v, ypr[0], ypr[1], ypr[2], center);
        }

        static public float[] RotateYPR(float[] v, float[] ypr)
        {
            return RotateYPR(v, ypr[0], ypr[1], ypr[2], new float[3]);
        }

        static public bool Similar(float[] v1, float[] v2)
        {
            if (v1[0] != v2[0]) return false;
            if (v1[1] != v2[1]) return false;
            if (v1[2] != v2[2]) return false;

            return true;
        }

        static public bool IsNan(float[] v1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (float.IsNaN(v1[i]))
                    return true;
            }

            return false;
        }

        static public string vPUSH(float[] vec, string format = null)
        {
            if (format != null)
                return vec[0].ToString(format) + " " + vec[1].ToString(format) + " " + vec[2].ToString(format);
            else
                return vec[0].ToString() + " " + vec[1].ToString() + " " + vec[2].ToString();
        }

        static public float DistanceToSqr(float[] from, float[] to)
        {
            return (from[0]-to[0])*(from[0]-to[0]) + (from[1]-to[1])*(from[1]-to[1]) + (from[2]-to[2])*(from[2]-to[2]);
        }

        static public float DistanceTo(float[] from, float[] to)
        {
            return (float)Math.Sqrt(DistanceToSqr(from, to));
        }

        static public byte[] GetBytes(float[] vec)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(vec[0]));
            bytes.AddRange(BitConverter.GetBytes(vec[1]));
            bytes.AddRange(BitConverter.GetBytes(vec[2]));
            return bytes.ToArray();
        }
    }
}
