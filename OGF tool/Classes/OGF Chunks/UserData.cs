using System.Collections.Generic;
using System.Text;

namespace OGF_tool
{
    public class UserData
    {
        public long pos;
        public int old_size;
        public string userdata;
        private bool old_format;

        public UserData()
        {
            pos = 0;
            old_size = 0;
            userdata = "";
            old_format = false;
        }

        public void Load(XRayLoader xr_loader, uint chunk_size)
        {
            pos = xr_loader.chunk_pos;

            long UserdataStreamPos = xr_loader.reader.BaseStream.Position;
            userdata = xr_loader.read_stringZ();

            if (userdata.Length + 1 != chunk_size)
            {
                old_format = true;
                xr_loader.reader.BaseStream.Position = UserdataStreamPos;
                userdata = xr_loader.read_stringSize(chunk_size);
            }

            old_size = data().Length;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(userdata));
            if (!old_format)
                temp.Add(0);

            return temp.ToArray();
        }
    }
}
