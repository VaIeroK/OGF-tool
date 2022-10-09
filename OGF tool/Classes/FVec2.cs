using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        static public float[] Mul(float[] v1, float v2)
        {
            float x, y;

            x = v1[0] * v2;
            y = v1[1] * v2;

            var rtnvector = new float[2] { x, y };
            return rtnvector;
        }

        static public float[] Normalize(float[] v)
        {
            v = Mul(v, (1.0f / (float)Math.Sqrt(v[0]*v[0] + v[1]*v[1])));
            return v;
        }
    }
}
