using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    static internal class FVec
    {
        static public float[] CrossProduct(float[] v1, float[] v2)
        {
            float x, y, z;
            x = v1[1] * v2[2] - v2[1] * v1[2];
            y = (v1[0] * v2[2] - v2[0] * v1[2]) * -1;
            z = v1[0] * v2[1] - v2[0] * v1[1];

            var rtnvector = new float[3] { x, y, z };
            return rtnvector;
        }

        static public float[] Sub(float[] v1, float[] v2)
        {
            float x, y, z;

            x = v1[0] - v2[0];
            y = v1[1] - v2[1];
            z = v1[2] - v2[2];

            var rtnvector = new float[3] { x, y, z };
            return rtnvector;
        }

        static public float[] Add(float[] v1, float[] v2)
        {
            float x, y, z;

            x = v1[0] + v2[0];
            y = v1[1] + v2[1];
            z = v1[2] + v2[2];

            var rtnvector = new float[3] { x, y, z };
            return rtnvector;
        }

        static public float[] Mul(float[] v1, float val)
        {
            float x, y, z;

            x = v1[0] * val;
            y = v1[1] * val;
            z = v1[2] * val;

            var rtnvector = new float[3] { x, y, z };
            return rtnvector;
        }

        static public float[] Normalize(float[] v)
        {
            v = Mul(v, (1.0f / (float)Math.Sqrt(v[0]*v[0] + v[1]*v[1] + v[2]*v[2])));
            return v;
        }
    }
}
