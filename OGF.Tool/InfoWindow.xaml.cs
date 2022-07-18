using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OGF_Tool
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : UserControl
    {
        public Description descr = new Description();
        public bool res = false;
        public InfoWindow()
        {
            InitializeComponent();

            Visibility = Visibility.Hidden;
            this.DataContext = this;

            
        }

        private bool _hideRequest = false;
        private bool _result = false;

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message", typeof(string), typeof(InfoWindow), new UIPropertyMetadata(string.Empty));


        public bool ShowHandlerDialog(Description init_descr, byte vers, byte type, ref Description out_d)
        {
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

            ExportTimeDate.SelectedDate = dt_e;
            CreationTimeDate.SelectedDate = dt_c;
            ModifedTimeDate.SelectedDate = dt_m;

            Visibility = Visibility.Visible;
            asd.Visibility = Visibility.Visible;
            //_parent.IsEnabled = false;

            _hideRequest = false;
            while (!_hideRequest)
            {
                // HACK: Stop the thread if the application is about to close
                if (this.Dispatcher.HasShutdownStarted ||
                    this.Dispatcher.HasShutdownFinished)
                {
                    break;
                }

                // HACK: Simulate "DoEvents"
                this.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new ThreadStart(delegate { }));
                Thread.Sleep(20);
            }
            out_d = descr;
            return _result;
        }

        private void RepairTimersButton_Click(object sender, EventArgs e)
        {
            descr.four_byte = false;
            RepairTimersButton.IsEnabled = false;
            ByteLabel.Text = "Timers: 8 byte";
        }

        private void HideHandlerDialog()
        {
            _hideRequest = true;
            Visibility = Visibility.Hidden;

            descr.m_source = SourceTextBox.Text;

            descr.m_source = SourceTextBox.Text;
            descr.m_export_tool = ConverterTextBox.Text;
            descr.m_owner_name = CreatorTextBox.Text;
            descr.m_export_modif_name_tool = EditorTextBox.Text;

            descr.m_export_time = Convert.ToUInt32(ExportTimeDate.SelectedDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            descr.m_creation_time = Convert.ToUInt32(CreationTimeDate.SelectedDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            descr.m_modified_time = Convert.ToUInt32(ModifedTimeDate.SelectedDate.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

            //_parent.IsEnabled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _result = true;
            HideHandlerDialog();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _result = false;
            HideHandlerDialog();
        }
    }
}
