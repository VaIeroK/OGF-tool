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
        public bool data_str;

        public Lod()
        {
            this.pos = 0;
            this.old_size = 0;
            this.lod_path = "";
            this.data_str = true;
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

        public uint chunk_size()
        {
            return (uint)lod_path.Length + (uint)(data_str ? 2 : 1);
        }
    }
}
