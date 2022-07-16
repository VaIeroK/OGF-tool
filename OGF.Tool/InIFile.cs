using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OGF.Tool
{

    class IniFile   // revision 11
    {
        private FileInfo Ini;
        private string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            string file_name = (IniPath ?? EXE + ".ini");
            Ini = new FileInfo(file_name);
            if (!Ini.Exists)
                Ini.Create();
        }

        public IniFile(string IniPath = null, string init_write = null)
        {
            string file_name = (IniPath ?? EXE + ".ini");
            Ini = new FileInfo(file_name);
            if (!Ini.Exists)
            {
                if (init_write != null)
                    File.WriteAllText(file_name, init_write);
                else
                    Ini.Create();
            }
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? this.EXE, Key, "", RetVal, 255, this.Ini.FullName);
            return RetVal.ToString();
        }

        public string ReadDef(string Key, string Section = null, string def = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? this.EXE, Key, "", RetVal, 255, this.Ini.FullName);
            return RetVal.ToString() != "" ? RetVal.ToString() : def;
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? this.EXE, Key, Value, this.Ini.FullName);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? this.EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? this.EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
