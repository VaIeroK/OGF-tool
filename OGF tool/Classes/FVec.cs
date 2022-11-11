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

        static public float[] RotateZ(float[] v)
        {
            var vec = new float[3]
            {
                -v[0],
                 v[1],
                -v[2]
            };

            return vec;
        }

        static public float[] Rotate(float[] v, float[] k)
        {
            float cos_theta = (float)Math.Cos(Math.PI);
            float sin_theta = (float)Math.Sin(Math.PI);
            
            float[] rotated = Add(Mul(v, cos_theta), Add(Mul(CrossProduct(k, v), sin_theta), Mul(k, DotProduct(k, v) * (1 - cos_theta))));

            return rotated;
        }

        static public bool Similar(float[] v1, float[] v2)
        {
            if (v1[0] != v2[0]) return false;
            if (v1[1] != v2[1]) return false;
            if (v1[2] != v2[2]) return false;

            return true;
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
