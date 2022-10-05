using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class UserData
    {
        public long pos;
        public int old_size;
        public string userdata;

        public UserData()
        {
            this.pos = 0;
            this.old_size = 0;
            this.userdata = "";
        }

        public byte[] data(uint ogf_ver)
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(userdata));
            if (ogf_ver == 4)
                temp.Add(0);

            return temp.ToArray();
        }

        public uint chunk_size(uint ogf_ver)
        {
            return (uint)userdata.Length + (uint)(ogf_ver == 4 ? 1 : 0);
        }
    }
}
