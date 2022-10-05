using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OGF_tool
{
    internal static class Resources
    {
        public static class SoCSkeleton
        {
            public static float[] InitIK()
            {
                var xr_loader = new XRayLoader();
                List<float> data = new List<float>();

                using (var r = new BinaryReader(new MemoryStream(File.ReadAllBytes(OGF_Editor.AppPath() + "\\res\\soc_npc.ik"))))
                {
                    xr_loader.SetStream(r.BaseStream);

                    if (xr_loader.find_chunk((int)OGF.OGF4_S_IKDATA))
                    {
                        for (int i = 0; i < 45; i++)
                        {
                            data.AddRange(xr_loader.ReadVector());
                            data.AddRange(xr_loader.ReadVector());
                        }
                    }
                }

                return data.ToArray();
            }

            public static float[] Rot(int bone, float[] data)
            {
                float[] vec = new float[3];

                for (int i = 0; i < 3; i++)
                    vec[i] = data[bone * 6 + i];

                return vec;
            }

            public static float[] Pos(int bone, float[] data)
            {
                float[] vec = new float[3];

                for (int i = 3; i < 6; i++)
                    vec[i - 3] = data[bone * 6 + i];

                return vec;
            }
        }

        public static class CoPSkeleton
        {
            public static float[] InitIK()
            {
                var xr_loader = new XRayLoader();
                List<float> data = new List<float>();

                using (var r = new BinaryReader(new MemoryStream(File.ReadAllBytes(OGF_Editor.AppPath() + "\\res\\cop_npc.ik"))))
                {
                    xr_loader.SetStream(r.BaseStream);

                    if (xr_loader.find_chunk((int)OGF.OGF4_S_IKDATA))
                    {
                        for (int i = 0; i < 47; i++)
                        {
                            data.AddRange(xr_loader.ReadVector());
                            data.AddRange(xr_loader.ReadVector());
                        }
                    }
                }

                return data.ToArray();
            }

            public static float[] Rot(int bone, float[] data)
            {
                float[] vec = new float[3];

                for (int i = 0; i < 3; i++)
                    vec[i] = data[bone * 6 + i];

                return vec;
            }

            public static float[] Pos(int bone, float[] data)
            {
                float[] vec = new float[3];

                for (int i = 3; i < 6; i++)
                    vec[i - 3] = data[bone * 6 + i];

                return vec;
            }
        }
    }
}
