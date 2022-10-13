using System.Collections.Generic;
using System.IO;

namespace OGF_tool
{
    public class OMF
    {
        public class Anim
        {
            public byte flags;

            public Anim()
            {
                flags = 0;
            }
        }

        private byte[] omf_data;
        private string omf_text;
        public List<Anim> Anims;

        public OMF()
        {
            omf_data = null;
            Anims = null;
            omf_text = "";
        }

        public bool SetData(byte[] _data)
        {
            if (_data != null)
            {
                omf_data = _data;

                XRayLoader xr_loader = new XRayLoader();

                using (var r = new BinaryReader(new MemoryStream(data())))
                {
                    xr_loader.SetStream(r.BaseStream);

                    if (xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk((int)OGF.OGF_S_MOTIONS, false, true)))
                    {
                        omf_text = "";
                        Anims = new List<Anim>();

                        int id = 0;

                        while (true)
                        {
                            if (!xr_loader.find_chunk(id)) break;

                            Stream temp = xr_loader.reader.BaseStream;

                            if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

                            Anim anim = new Anim();

                            if (id == 0)
                                omf_text += $"Motions count : {xr_loader.ReadUInt32()}\n";
                            else
                            {
                                omf_text += $"\n{id}. {xr_loader.read_stringZ()}";
                                xr_loader.ReadUInt32();
                                anim.flags = xr_loader.ReadByte();
                            }

                            id++;
                            xr_loader.SetStream(temp);
                            Anims.Add(anim);
                        }

                        return true;
                    }
                }
            }
            else
            {
                omf_data = null;
                Anims = null;
                omf_text = "";
            }

            return false;
        }

        public byte[] data()
        {
            return omf_data;
        }

        public override string ToString()
        {
            return omf_text;
        }
    }
}
