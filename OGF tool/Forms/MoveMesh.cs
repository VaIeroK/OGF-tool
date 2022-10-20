using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OGF_tool
{
    public partial class MoveMesh : Form
    {
        public bool res = false;
        public float[] offset = new float[3];
        private bool bKeyIsDown = false;

        public MoveMesh(float[] offset)
        {
            InitializeComponent();

            PositionXTextBox.Text = ((decimal)offset[0]).ToString();
            PositionYTextBox.Text = ((decimal)offset[1]).ToString();
            PositionZTextBox.Text = ((decimal)offset[2]).ToString();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            res = true;

            Close();
        }

        private void PosTextChanged(object sender, EventArgs e)
        {
            if (bKeyIsDown)
            {
                TextBox curBox = sender as TextBox;

                if (curBox.Text.Length == 0)
                    return;

                int temp = curBox.SelectionStart;

                string number_mask = @"^-[0-9.]*$";
                Regex.Match(curBox.Text, number_mask);

                if (curBox.Text != "-")
                {
                    try
                    {
                        Convert.ToSingle(curBox.Text);
                    }
                    catch (Exception)
                    {
                        switch (curBox.Name)
                        {
                            case "PositionXTextBox": curBox.Text = offset[0].ToString(); break;
                            case "PositionYTextBox": curBox.Text = offset[1].ToString(); break;
                            case "PositionZTextBox": curBox.Text = offset[2].ToString(); break;
                        }

                        if (curBox.SelectionStart < 1)
                            curBox.SelectionStart = curBox.Text.Length;

                        curBox.SelectionStart = temp - 1;
                    }

                    switch (curBox.Name)
                    {
                        case "PositionXTextBox": offset[0] = Convert.ToSingle(curBox.Text); break;
                        case "PositionYTextBox": offset[1] = Convert.ToSingle(curBox.Text); break;
                        case "PositionZTextBox": offset[2] = Convert.ToSingle(curBox.Text); break;
                    }
                }

                bKeyIsDown = false;
            }
        }

        private void PosKeyDown(object sender, KeyEventArgs e)
        {
            bKeyIsDown = true;
        }
    }
}
