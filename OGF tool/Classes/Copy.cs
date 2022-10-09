using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGF_tool
{
    internal static class Copy
    {
        public static Label Label(Label label)
        {
            var newLbl = new Label();
            newLbl.Name = label.Name;
            newLbl.Text = label.Text;
            newLbl.Size = label.Size;
            newLbl.Location = label.Location;
            newLbl.Anchor = label.Anchor;
            newLbl.TextAlign = label.TextAlign;
            newLbl.Dock = label.Dock;
            newLbl.Tag = newLbl.Tag;

            return newLbl;
        }

        public static ComboBox ComboBox(ComboBox box)
        {
            var newBox = new ComboBox();
            newBox.Name = box.Name;
            newBox.Text = box.Text;
            newBox.Size = box.Size;
            newBox.Location = box.Location;
            newBox.Anchor = box.Anchor;
            newBox.Dock = box.Dock;
            newBox.DropDownStyle = box.DropDownStyle;
            newBox.Tag = box.Tag;

            return newBox;
        }

        public static Button Button(Button box)
        {
            var newBox = new Button();
            newBox.Name = box.Name;
            newBox.Text = box.Text;
            newBox.Size = box.Size;
            newBox.Location = box.Location;
            newBox.Anchor = box.Anchor;
            newBox.Dock = box.Dock;
            newBox.Tag = box.Tag;

            return newBox;
        }

        public static TextBox TextBox(TextBox box)
        {
            var newBox = new TextBox();
            newBox.Name = box.Name;
            newBox.Text = box.Text;
            newBox.Size = box.Size;
            newBox.Location = box.Location;
            newBox.Anchor = box.Anchor;
            newBox.Dock = box.Dock;
            newBox.ReadOnly = box.ReadOnly;
            newBox.Tag = box.Tag;

            return newBox;
        }

        public static CheckBox CheckBox(CheckBox box)
        {
            var newBox = new CheckBox();
            newBox.Name = box.Name;
            newBox.Text = box.Text;
            newBox.Size = box.Size;
            newBox.Location = box.Location;
            newBox.Anchor = box.Anchor;
            newBox.Dock = box.Dock;
            newBox.Tag = box.Tag;
            newBox.Checked = box.Checked;

            return newBox;
        }

        public static TableLayoutPanel TableLayoutPanel(TableLayoutPanel box)
        {
            var newBox = new TableLayoutPanel();
            newBox.Name = box.Name;
            newBox.Text = box.Text;
            newBox.Size = box.Size;
            newBox.Location = box.Location;
            newBox.Anchor = box.Anchor;
            newBox.Dock = box.Dock;
            newBox.Tag = box.Tag;

            newBox.RowCount = box.RowCount;
            for (int x = 0; x < box.RowCount; x++)
                newBox.RowStyles.Add(new RowStyle() { Height = box.RowStyles[x].Height, SizeType = box.RowStyles[x].SizeType });

            newBox.ColumnCount = box.ColumnCount;
            for (int x = 0; x < box.ColumnCount; x++)
                newBox.ColumnStyles.Add(new ColumnStyle() { Width = box.ColumnStyles[x].Width, SizeType = box.ColumnStyles[x].SizeType });

            return newBox;
        }
    }
}
