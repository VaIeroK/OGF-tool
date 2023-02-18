using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGF_tool
{
    public partial class DmData : Form
    {
        private bool bKeyIsDown;
        public float fMinScale;
        public float fMaxScale;
        public uint iFlags;
        public DmData(float min, float max, uint flags)
        {
            InitializeComponent();
            bKeyIsDown = false;
            fMinScale = min;
            fMaxScale = max;
            iFlags = flags;

            MinScale.Text = fMinScale.ToString();
            MaxScale.Text = fMaxScale.ToString();
            checkBox1.Checked = flags == 1;
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            bKeyIsDown = true;
        }

        void ReloadControlText(Control control, int cursor_pos)
        {
            string currentField = control.Name;

            switch (currentField)
            {
                case "MinScale": control.Text = ((decimal)fMinScale).ToString(); break;
                case "MaxScale": control.Text = ((decimal)fMaxScale).ToString(); break;
            }

            if (control is TextBox)
            {
                TextBox curBox = control as TextBox;

                if (curBox.SelectionStart < 1)
                    curBox.SelectionStart = control.Text.Length;

                curBox.SelectionStart = cursor_pos - 1;
            }
        }

        private void TextBoxFilter(object sender, EventArgs e)
        {
            Control curControl = sender as Control;

            string currentField = curControl.Name;

            switch (curControl.Tag.ToString())
            {
                case "float":
                    {
                        if (bKeyIsDown)
                        {
                            TextBox curBox = sender as TextBox;

                            if (curControl.Text.Length == 0)
                                return;

                            int temp = curBox.SelectionStart;

                            Regex.Match(curControl.Text, @"^[0-9.]*$");

                            try
                            {
                                Convert.ToSingle(curControl.Text);
                            }
                            catch (Exception)
                            {
                                ReloadControlText(curControl, temp);
                            }
                        }
                    }
                    break;
            }

            switch (currentField)
            {
                case "MinScale": fMinScale = Convert.ToSingle(curControl.Text); break;
                case "MaxScale": fMaxScale = Convert.ToSingle(curControl.Text); break;
            }

            bKeyIsDown = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            iFlags = (uint)(checkBox1.Checked ? 1 : 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
