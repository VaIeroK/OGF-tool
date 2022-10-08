using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class IK_Data
    {
        public long pos;
        public int old_size;

        public List<string> materials;
        public List<float> mass;
        public List<uint> version;
        public List<float[]> center_mass;
        public List<float[]> position;
        public List<float[]> rotation;
        public List<List<byte[]>> bytes_1;
        public byte chunk_version;

        public IK_Data()
        {
            this.pos = 0;
            this.old_size = 0;
            this.materials = new List<string>();
            this.mass = new List<float>();
            this.version = new List<uint>();
            this.center_mass = new List<float[]>();
            this.position = new List<float[]>();
            this.rotation = new List<float[]>();
            this.bytes_1 = new List<List<byte[]>>();
            this.chunk_version = 0;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            for (int i = 0; i < materials.Count; i++)
            {
                if (chunk_version == 4)
                    temp.AddRange(BitConverter.GetBytes(version[i]));

                temp.AddRange(Encoding.Default.GetBytes(materials[i]));
                temp.Add(0);

                for (int j = 0; j < bytes_1[i].Count; j++)
                    temp.AddRange(bytes_1[i][j]);

                temp.AddRange(BitConverter.GetBytes(rotation[i][0]));
                temp.AddRange(BitConverter.GetBytes(rotation[i][1]));
                temp.AddRange(BitConverter.GetBytes(rotation[i][2]));

                temp.AddRange(BitConverter.GetBytes(position[i][0]));
                temp.AddRange(BitConverter.GetBytes(position[i][1]));
                temp.AddRange(BitConverter.GetBytes(position[i][2]));

                temp.AddRange(BitConverter.GetBytes(mass[i]));

                temp.AddRange(BitConverter.GetBytes(center_mass[i][0]));
                temp.AddRange(BitConverter.GetBytes(center_mass[i][1]));
                temp.AddRange(BitConverter.GetBytes(center_mass[i][2]));
            }

            return temp.ToArray();
        }
    }
}
