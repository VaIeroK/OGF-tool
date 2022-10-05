using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;


namespace OGF_tool
{
    public class IniFile   // revision 11
    {
        private FileInfo Ini;
        private string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            if (!File.Exists(IniPath))
            {
                var myFile = File.Create(IniPath);
                myFile.Close();
            }
            string file_name = (IniPath ?? EXE + ".ini");
            Ini = new FileInfo(file_name);
        }

        public IniFile(string IniPath = null, string init_write = "")
        {
            if (!File.Exists(IniPath))
                File.WriteAllText(IniPath, init_write);

            string file_name = (IniPath ?? EXE + ".ini");
            Ini = new FileInfo(file_name);
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
