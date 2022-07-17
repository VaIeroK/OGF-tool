using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OGF_Tool
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public Description descr = new Description();
        public bool res = false;
        public InfoWindow(Description init_descr, byte vers, byte type)
        {
            InitializeComponent();

            OgfVersLabel.Content = vers.ToString();
            ModelTypeLabel.Content = (type == 1 ? "Object" : type == 3 ? "Animated" : "Rigid");
            ByteLabel.Text = "Timers: " + (init_descr.four_byte ? "4 byte" : "8 byte");
            RepairTimersButton.IsEnabled = init_descr.four_byte;

            SourceTextBox.Text = init_descr.m_source;
            ConverterTextBox.Text = init_descr.m_export_tool;
            CreatorTextBox.Text = init_descr.m_owner_name;
            EditorTextBox.Text = init_descr.m_export_modif_name_tool;

            System.DateTime dt_e = new System.DateTime(1970, 1, 1).AddSeconds(init_descr.m_export_time);
            System.DateTime dt_c = new System.DateTime(1970, 1, 1).AddSeconds(init_descr.m_creation_time);
            System.DateTime dt_m = new System.DateTime(1970, 1, 1).AddSeconds(init_descr.m_modified_time);

            //ExportTimeDate.Text = dt_e;
            //CreationTimeDate.Text = dt_c;
            //ModifedTimeDate.Text = dt_m;
        }

        private void RepairTimersButton_Click(object sender, EventArgs e)
        {
            descr.four_byte = false;
            RepairTimersButton.IsEnabled = false;
            ByteLabel.Text = "Timers: 8 byte";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            descr.m_source = SourceTextBox.Text;

            descr.m_source = SourceTextBox.Text;
            descr.m_export_tool = ConverterTextBox.Text;
            descr.m_owner_name = CreatorTextBox.Text;
            descr.m_export_modif_name_tool = EditorTextBox.Text;

            //descr.m_export_time = Convert.ToUInt32(ExportTimeDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            //descr.m_creation_time = Convert.ToUInt32(CreationTimeDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            //descr.m_modified_time = Convert.ToUInt32(ModifedTimeDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            res = true;
        }
    }
}
