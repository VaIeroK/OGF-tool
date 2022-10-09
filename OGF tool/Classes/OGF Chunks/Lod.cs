using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class Lod
    {
        public long pos;
        public int old_size;
        public string lod_path;
        private bool data_str;

        public Lod()
        {
            pos = 0;
            old_size = 0;
            lod_path = "";
            data_str = true;
        }

        public void Load(XRayLoader xr_loader)
        {
            pos = xr_loader.chunk_pos;
            lod_path = xr_loader.read_stringData(ref data_str);
            old_size = data().Length;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(lod_path));
            if (data_str)
            {
                temp.Add(0xD);
                temp.Add(0xA);
            }
            else
                temp.Add(0);

            return temp.ToArray();
        }
    }
}
