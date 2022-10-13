using System;

namespace OGF_tool
{
    static internal class FVec2
    {
        static public float[] Sub(float[] v1, float[] v2)
        {
            float x, y;

            x = v1[0] - v2[0];
            y = v1[1] - v2[1];

            var rtnvector = new float[2] { x, y };
            return rtnvector;
        }

        static public float[] Add(float[] v1, float[] v2)
        {
            float x, y;

            x = v1[0] + v2[0];
            y = v1[1] + v2[1];

            var rtnvector = new float[2] { x, y };
            return rtnvector;
        }

        static public float[] Mul(float[] v1, float val)
        {
            float x, y;

            x = v1[0] * val;
            y = v1[1] * val;

            var rtnvector = new float[2] { x, y };
            return rtnvector;
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
            v = Mul(v, (1.0f / (float)Math.Sqrt(v[0]*v[0] + v[1]*v[1])));
            return v;
        }

        static public string vPUSH(float[] vec, string format = null)
        {
            if (format != null)
                return vec[0].ToString(format) + " " + vec[1].ToString(format);
            else
                return vec[0].ToString() + " " + vec[1].ToString();
        }
    }
}
