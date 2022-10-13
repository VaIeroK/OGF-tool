using System;
using System.Collections.Generic;
using System.Text;

namespace OGF_tool
{
    public class MotionRefs
    {
        public long pos;
        public List<string> refs;
        public bool soc;
        public int old_size;

        public MotionRefs()
        {
            pos = 0;
            old_size = 0;
            refs = new List<string>();
            soc = false;
        }

        public void Load(XRayLoader xr_loader, bool string_refs)
        {
            pos = xr_loader.chunk_pos;

            if (string_refs)
            {
                soc = true;
                string motions = xr_loader.read_stringZ();
                string motion = "";
                for (int i = 0; i < motions.Length; i++)
                {
                    if (motions[i] != ',')
                        motion += motions[i];
                    else
                    {
                        refs.Add(motion);
                        motion = "";
                    }

                }

                if (motion != "")
                    refs.Add(motion);
            }
            else
            {
                uint count = xr_loader.ReadUInt32();

                for (int i = 0; i < count; i++)
                    refs.Add(xr_loader.read_stringZ());
            }

            old_size = data(soc).Length;
        }

        public byte[] data(bool v3)
        {
            List<byte> temp = new List<byte>();

            if (!v3)
            {
                temp.AddRange(BitConverter.GetBytes(refs.Count));

                foreach (var str in refs)
                {
                    temp.AddRange(Encoding.Default.GetBytes(str));
                    temp.Add(0);
                }
            }
            else
            {
                string strref = refs[0];
                if (refs.Count > 1)
                {
                    for (int i = 1; i < refs.Count; i++)
                        strref += "," + refs[i];
                }

                temp.AddRange(Encoding.Default.GetBytes(strref));
                temp.Add(0);
            }

            return temp.ToArray();
        }
    }
}
