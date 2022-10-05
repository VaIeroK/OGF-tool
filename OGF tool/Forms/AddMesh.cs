using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGF_tool
{
    public partial class AddMesh : Form
    {
        private OGF_Children OGF, LoadedOGF;
        public AddMesh(ref OGF_Children Main_OGF, OGF_Children Loaded_OGF)
        {
            InitializeComponent();

            OGF = Main_OGF;
            LoadedOGF = Loaded_OGF;
        }
    }
}
