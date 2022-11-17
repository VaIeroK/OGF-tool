using System;
using System.Windows.Forms;
using System.IO;

namespace OGF_tool
{
    public class EditorSettings
    {
        private IniFile pSettings = null;
        private string sMainSect = "settings";

        public int SETTINGS_VERS = 1;

        public bool CheckVers()
        {
            int vers = Convert.ToInt32(pSettings.ReadDef("SettingsVersion", sMainSect, "0"));
            return vers == SETTINGS_VERS;
        }

        public EditorSettings(string IniPath = null)
        {
            pSettings = new IniFile(IniPath);
        }

        public EditorSettings(string IniPath = null, string init_write = null)
        {
            pSettings = new IniFile(IniPath, init_write);
        }

        public void SaveVersion()
        {
            pSettings.Write("SettingsVersion", Convert.ToInt32(SETTINGS_VERS).ToString(), sMainSect);
        }

        public void Save(CheckBox box)
        {
            pSettings.Write(box.Name, Convert.ToUInt16(box.Checked).ToString(), sMainSect);
        }

        public void Save(RadioButton box)
        {
            pSettings.Write(box.Name, Convert.ToUInt16(box.Checked).ToString(), sMainSect);
        }

        public void Save(TextBox box)
        {
            pSettings.Write(box.Name, box.Text, sMainSect);
        }

        public void Save(string key, bool var)
        {
            pSettings.Write(key, Convert.ToUInt16(var).ToString(), sMainSect);
        }

        public void Save(string key, string var)
        {
            pSettings.Write(key, var, sMainSect);
        }

        public void Save(LinkLabel box)
        {
            pSettings.Write(box.Name, Convert.ToUInt16(box.LinkVisited).ToString(), sMainSect);
        }

        public void Save(string name, FileDialog dialog)
        {
            pSettings.Write(name, dialog.FileName, sMainSect);
        }

        public void Save(string name, FolderSelectDialog dialog)
        {
            Save(name, dialog.Parent);
        }

        public void Save(string key, int var)
        {
            pSettings.Write(key, var.ToString(), sMainSect);
        }

        public void Save(string key, DateTime var)
        {
            pSettings.Write(key, var.ToString(), sMainSect);
        }

        public bool Load(CheckBox box, bool def = false)
        {
            if (CheckVers())
                box.Checked = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(box.Name, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                box.Checked = def;
            return box.Checked;
        }

        public bool Load(RadioButton box, bool def = false)
        {
            if (CheckVers())
                box.Checked = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(box.Name, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                box.Checked = def;
            return box.Checked;
        }

        public void Load(TextBox box, string def = "")
        {
            if (CheckVers())
                box.Text = pSettings.ReadDef(box.Name, sMainSect, def);
            else
                box.Text = def;
        }

        public bool Load(string key, ref bool var, bool def = false)
        {
            if (CheckVers())
                var = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(key, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                var = def;
            return var;
        }

        public void Load(string key, ref string var, string def = "")
        {
            if (CheckVers())
                var = pSettings.ReadDef(key, sMainSect, def);
            else
                var = def;
        }

        public string Load(string key, string def = "")
        {
            string var;
            if (CheckVers())
                var = pSettings.ReadDef(key, sMainSect, def);
            else
                var = def;

            return var;
        }

        public bool Load(string name, CheckBox box, bool def = false)
        {
            if (CheckVers())
                box.Checked = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(name, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                box.Checked = def;
            return box.Checked;
        }

        public bool Load(string name, RadioButton box, bool def = false)
        {
            if (CheckVers())
                box.Checked = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(name, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                box.Checked = def;
            return box.Checked;
        }

        public bool Load(LinkLabel box, bool def = false)
        {
            if (CheckVers())
                box.LinkVisited = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(box.Name, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                box.LinkVisited = def;
            return box.LinkVisited;
        }

        public DateTime Load(string param, DateTime def)
        {
            DateTime time;
            if (CheckVers())
                time = Convert.ToDateTime(pSettings.ReadDef(param, sMainSect, def.ToString()));
            else
                time = def;
            return time;
        }

        public bool LoadState(string name, ref bool state, bool def = false)
        {
            if (CheckVers())
                state = Convert.ToBoolean(Convert.ToUInt16(pSettings.ReadDef(name, sMainSect, Convert.ToUInt16(def).ToString())));
            else
                state = def;
            return state;
        }

        public string LoadText(string name, ref string text, string def = "")
        {
            if (CheckVers())
                text = pSettings.ReadDef(name, sMainSect, def);
            else
                text = def;
            return text;
        }

        public int Load(string name, ref int text, int def = 0)
        {
            if (CheckVers())
                text = Convert.ToInt32(pSettings.ReadDef(name, sMainSect, def.ToString()));
            else
                text = def;
            return text;
        }

        public string Load(string name, FileDialog dialog, bool is_folder = false)
        {
            if (CheckVers())
            {
                string full_path = pSettings.ReadDef(name, sMainSect, dialog.FileName);
                dialog.FileName = Path.GetFileName(dialog.FileName);
                if (full_path.Length > 0 && full_path.LastIndexOf('\\') > 0 && !is_folder)
                    dialog.InitialDirectory = full_path.Substring(0, full_path.LastIndexOf('\\'));
                else
                    dialog.InitialDirectory = full_path;
            }
            else
            {
                string full_filename = dialog.FileName;
                if (full_filename.Length > 0)
                {
                    dialog.FileName = Path.GetFileName(dialog.FileName);
                    dialog.InitialDirectory = full_filename.Substring(0, full_filename.LastIndexOf('\\'));
                }
            }
            return dialog.FileName;
        }

        public string Load(string name, FolderSelectDialog dialog)
        {
            return Load(name, dialog.Parent, true);
        }
    }
}
