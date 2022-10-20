using System;
using System.Collections.Generic;

namespace OGF_tool
{
    static internal class FVec2
    {
        static public float[] Sub(float[] v1, float[] v2)
        {
            var vec = new float[2]
            {
                v1[0] - v2[0],
                v1[1] - v2[1],
            };

            return vec;
        }

        static public float[] Add(float[] v1, float[] v2)
        {
            var vec = new float[2]
            {
                v1[0] + v2[0],
                v1[1] + v2[1],
            };

            return vec;
        }

        static public float[] Mul(float[] v1, float val)
        {
            var vec = new float[2]
            {
                v1[0] * val,
                v1[1] * val,
            };
            return vec;
        }

        static public float[] Mul(float[] v1, float[] v2)
        {
            var vec = new float[2]
            {
                v1[0] * v2[0],
                v1[1] * v2[1],
            };

            return vec;
        }

        static public float[] Div(float[] v1, float val)
        {
            var vec = new float[2]
            {
                v1[0] / val,
                v1[1] / val,
            };

            return vec;
        }

        static public float[] Normalize(float[] v)
        {
            return Mul(v, 1.0f / (float)Math.Sqrt(v[0]*v[0] + v[1]*v[1]));
        }

        static public string vPUSH(float[] vec, string format = null)
        {
            if (format != null)
                return vec[0].ToString(format) + " " + vec[1].ToString(format);
            else
                return vec[0].ToString() + " " + vec[1].ToString();
        }

        static public byte[] GetBytes(float[] vec)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(vec[0]));
            bytes.AddRange(BitConverter.GetBytes(vec[1]));
            return bytes.ToArray();
        }
    }
}
