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

            return newBox;
        }
    }
}
