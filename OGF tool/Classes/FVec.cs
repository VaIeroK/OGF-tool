using System;

namespace OGF_tool
{
    static internal class FVec
    {
        static public float[] CrossProduct(float[] v1, float[] v2)
        {
            var vec = new float[3] 
            {
                v1[1] * v2[2] - v2[1] * v1[2],
                (v1[0] * v2[2] - v2[0] * v1[2]) * -1,
                v1[0] * v2[1] - v2[0] * v1[1]
            };

            return vec;
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
            v = Mul(v, (1.0f / (float)Math.Sqrt(v[0]*v[0] + v[1]*v[1] + v[2]*v[2])));
            return v;
        }

        static public float[] MirrorZ(float[] v)
        {
            v[0] = v[0];
            v[1] = v[1];
            v[2] = -v[2];

            return v;
        }

        static public float[] RotateZ(float[] v)
        {
            v[0] = -v[0];
            v[1] = v[1];
            v[2] = -v[2];

            return v;
        }

        static public string vPUSH(float[] vec, string format = null)
        {
            if (format != null)
                return vec[0].ToString(format) + " " + vec[1].ToString(format) + " " + vec[2].ToString(format);
            else
                return vec[0].ToString() + " " + vec[1].ToString() + " " + vec[2].ToString();
        }
    }
}
