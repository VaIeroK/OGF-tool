using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OGF_tool
{
    public class XRayLoader
    {
        public long chunk_pos = 0;

        uint CHUNK_COMPRESSED = 0x80000000;

        public MemoryStream mem_stream;
        public BinaryReader reader;


        public void Destroy()
        {
            mem_stream.Dispose();
            reader.Dispose();
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public int ReadInt32()
        {
            return reader.ReadInt32();
        }

        public long ReadInt64()
        {
            return reader.ReadInt64();
        }

        public float ReadFloat()
        {
            return reader.ReadSingle();
        }

        public float[] ReadVector()
        {
            float[] vec = new float[3];

            vec[0] = reader.ReadSingle();
            vec[1] = reader.ReadSingle();
            vec[2] = reader.ReadSingle();

            return vec;
        }

        public float[] ReadVector2()
        {
            float[] vec = new float[2];

            vec[0] = reader.ReadSingle();
            vec[1] = reader.ReadSingle();

            return vec;
        }

        public uint ReadUInt16()
        {
            return reader.ReadUInt16();
        }

        public uint ReadUInt32()
        {
            return reader.ReadUInt32();
        }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(count);
        }

        public bool SetData(byte[] input)
        {
            if (input == null) return false;
            mem_stream = new MemoryStream(input);
            reader = new BinaryReader(mem_stream);
            return true;
        }

        public void SetStream(Stream stream)
        {
            reader = new BinaryReader(stream);
        }

        public void SetReader(BinaryReader rd)
        {
            reader = rd;
        }

        public bool find_chunk(int chunkId, bool skip = false, bool reset = false)
        {
            return find_chunkSize(chunkId, skip, reset) != 0;
        }

        public byte[] find_and_return_chunk_in_chunk(int chunkId, bool skip = false, bool reset = false)
        {
            int size = (int)find_chunkSize(chunkId, skip, reset);

            if (size > 0)
            {
                return ReadBytes(size);
            }
            else
                return null;
        }

        public uint find_chunkSize(int chunkId, bool skip = false, bool reset = false)
        {
            chunk_pos = 0;

            if (reset) reader.BaseStream.Position = 0;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                if (reader.BaseStream.Position + 8 > reader.BaseStream.Length)
                    return 0;

                uint dwType = reader.ReadUInt32();
                uint dwSize = reader.ReadUInt32();

                if ((dwType == chunkId || (dwType ^ CHUNK_COMPRESSED) == chunkId) && ( reader.BaseStream.Position - 8 + dwSize <= reader.BaseStream.Length ) )
                {
                    chunk_pos = reader.BaseStream.Position - 8;
                    return dwSize;
                }
                else
                {
                    if (reader.BaseStream.Position + dwSize < reader.BaseStream.Length)
                        reader.BaseStream.Position += dwSize;
                    else if (reader.BaseStream.Position + 8 < reader.BaseStream.Length)
                        reader.BaseStream.Position += 4;
                    else
                        return 0;
                }
            }

            return 0;
        }

        public void open_chunk(BinaryWriter w, int chunkId)
        {
            w.Write(chunkId);
            chunk_pos = w.BaseStream.Position;
            w.Write(0);     // the place for 'size'
        }

        public void close_chunk(BinaryWriter w)
        {
            if (chunk_pos == 0)
            {
                throw new InvalidOperationException("no chunk!");
            }

            long pos = w.BaseStream.Position;
            w.BaseStream.Position = chunk_pos;
            w.Write((int)(pos - chunk_pos - 4));
            w.BaseStream.Position = pos;
            chunk_pos = 0;
        }

        public string read_stringData(ref bool data)
        {
            string str = "";
            data = false;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte[] one = { reader.ReadByte() };
                if (one[0] != 0 && one[0] != 0xA && one[0] != 0xD)
                {
                    str += Encoding.Default.GetString(one);
                }
                else
                {
                    if (one[0] == 0xD)
                    {
                        reader.ReadByte();
                        data = true;
                    }
                    break;
                }
            }
            return str;
        }

        public string read_stringZ()
        {
            string str = "";

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte[] one = { reader.ReadByte() };
                if (one[0] != 0 && one[0] != 0xE)
                {
                    str += Encoding.Default.GetString(one);
                }
                else
                {
                    break;
                }
            }
            return str;
        }

        public string read_stringSize(uint size)
        {
            string str = "";

            for (uint i = 0; i < size; i++)
            {
                byte[] one = { reader.ReadByte() };
                str += Encoding.Default.GetString(one);
            }
            return str;
        }


        public void write_stringZ(BinaryWriter w, string str)
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(str));
            temp.Add(0);

            w.Write(temp.ToArray());
        }

        public void write_u32(BinaryWriter w, uint num)
        {
            w.Write(num);
        }
    }
}
