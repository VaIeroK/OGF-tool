using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGF_tool
{
    public class Description
    {
        public long pos;
        public int old_size;
        public bool four_byte;

        public string m_source;
        public string m_export_tool;
        public long m_export_time;
        public string m_owner_name;
        public long m_creation_time;
        public string m_export_modif_name_tool;
        public long m_modified_time;

        public Description()
        {
            this.pos = 0;
            this.old_size = 0;
            this.four_byte = false;

            this.m_source = "";
            this.m_export_tool = "";
            this.m_export_time = 0;
            this.m_owner_name = "";
            this.m_creation_time = 0;
            this.m_export_modif_name_tool = "";
            this.m_modified_time = 0;

        }

        public byte Load(XRayLoader xr_loader, uint chunk_size)
        {
            byte broken_type = 0;

            pos = xr_loader.chunk_pos;

            // Читаем таймеры в 8 байт
            long reader_start_pos = xr_loader.reader.BaseStream.Position;
            m_source = xr_loader.read_stringZ();
            m_export_tool = xr_loader.read_stringZ();
            m_export_time = xr_loader.ReadInt64();
            m_owner_name = xr_loader.read_stringZ();
            m_creation_time = xr_loader.ReadInt64();
            m_export_modif_name_tool = xr_loader.read_stringZ();
            m_modified_time = xr_loader.ReadInt64();
            long description_end_pos = xr_loader.reader.BaseStream.Position;

            old_size = m_source.Length + 1 + m_export_tool.Length + 1 + 8 + m_owner_name.Length + 1 + 8 + m_export_modif_name_tool.Length + 1 + 8;

            if ((description_end_pos - reader_start_pos) != chunk_size) // Размер не состыковывается, пробуем читать 4 байта
            {
                xr_loader.reader.BaseStream.Position = reader_start_pos;
                m_source = xr_loader.read_stringZ();
                m_export_tool = xr_loader.read_stringZ();
                m_export_time = xr_loader.ReadUInt32();
                m_owner_name = xr_loader.read_stringZ();
                m_creation_time = xr_loader.ReadUInt32();
                m_export_modif_name_tool = xr_loader.read_stringZ();
                m_modified_time = xr_loader.ReadUInt32();
                description_end_pos = xr_loader.reader.BaseStream.Position;

                old_size = m_source.Length + 1 + m_export_tool.Length + 1 + 4 + m_owner_name.Length + 1 + 4 + m_export_modif_name_tool.Length + 1 + 4;
                four_byte = true; // Ставим флаг на то что мы прочитали чанк с 4х байтными таймерами, если модель будет сломана то чинить чанк будем в 8 байт

                if ((description_end_pos - reader_start_pos) != chunk_size) // Все равно разный размер? Походу модель сломана
                {
                    broken_type = 1;

                    // Чистим таймеры, так как прочитаны битые байты
                    m_export_time = 0;
                    m_creation_time = 0;
                    m_modified_time = 0;
                }
            }

            return broken_type;
        }

        public byte[] data()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(Encoding.Default.GetBytes(m_source));
            temp.Add(0);
            temp.AddRange(Encoding.Default.GetBytes(m_export_tool));
            temp.Add(0);
            if (!four_byte)
                temp.AddRange(BitConverter.GetBytes(m_export_time));
            else
                temp.AddRange(BitConverter.GetBytes((uint)m_export_time));
            temp.AddRange(Encoding.Default.GetBytes(m_owner_name));
            temp.Add(0);
            if (!four_byte)
                temp.AddRange(BitConverter.GetBytes(m_creation_time));
            else
                temp.AddRange(BitConverter.GetBytes((uint)m_creation_time));
            temp.AddRange(Encoding.Default.GetBytes(m_export_modif_name_tool));
            temp.Add(0);
            if (!four_byte)
                temp.AddRange(BitConverter.GetBytes(m_modified_time));
            else
                temp.AddRange(BitConverter.GetBytes((uint)m_modified_time));

            return temp.ToArray();
        }
    }
}
