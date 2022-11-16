using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OGF_tool
{
    public partial class MoveMesh : Form
    {
        public bool res = false;
        public float[] offset = new float[3];
        public float[] rotation = new float[3];
        private bool bKeyIsDown = false;
        public bool LocalRotation = false;

        public MoveMesh(float[] offset, float[] rotation, bool local_rotation, bool show_local)
        {
            InitializeComponent();

            PositionXTextBox.Text = ((decimal)offset[0]).ToString();
            PositionYTextBox.Text = ((decimal)offset[1]).ToString();
            PositionZTextBox.Text = ((decimal)offset[2]).ToString();

            RotationXTextBox.Text = ((decimal)rotation[0]).ToString();
            RotationYTextBox.Text = ((decimal)rotation[1]).ToString();
            RotationZTextBox.Text = ((decimal)rotation[2]).ToString();

            LocalRotationBox.Checked = local_rotation;

            if (!show_local)
            {
                LocalRotationBox.Visible = false;
                Size = new System.Drawing.Size(Width, 136);
            }

            ActiveControl = PositionLabelEx;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            res = true;
            LocalRotation = LocalRotationBox.Checked;

            if (PositionXTextBox.Text.Length == 0)
                PositionXTextBox.Text = "0";

            if (PositionYTextBox.Text.Length == 0)
                PositionYTextBox.Text = "0";

            if (PositionZTextBox.Text.Length == 0)
                PositionZTextBox.Text = "0";

            if (RotationXTextBox.Text.Length == 0)
                RotationXTextBox.Text = "0";

            if (RotationYTextBox.Text.Length == 0)
                RotationYTextBox.Text = "0";

            if (RotationZTextBox.Text.Length == 0)
                RotationZTextBox.Text = "0";

            offset[0] = Convert.ToSingle(PositionXTextBox.Text);
            offset[1] = Convert.ToSingle(PositionYTextBox.Text);
            offset[2] = Convert.ToSingle(PositionZTextBox.Text);
            rotation[0] = Convert.ToSingle(RotationXTextBox.Text);
            rotation[1] = Convert.ToSingle(RotationYTextBox.Text);
            rotation[2] = Convert.ToSingle(RotationZTextBox.Text);

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
                            case "RotationXTextBox": curBox.Text = rotation[0].ToString(); break;
                            case "RotationYTextBox": curBox.Text = rotation[1].ToString(); break;
                            case "RotationZTextBox": curBox.Text = rotation[2].ToString(); break;
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
                        case "RotationXTextBox": rotation[0] = Convert.ToSingle(curBox.Text); break;
                        case "RotationYTextBox": rotation[1] = Convert.ToSingle(curBox.Text); break;
                        case "RotationZTextBox": rotation[2] = Convert.ToSingle(curBox.Text); break;
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
