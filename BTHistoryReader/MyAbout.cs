using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTHistoryReader
{
    public partial class MyAbout : Form
    {
       
        public MyAbout()
        {
            InitializeComponent();
            labelBuild.Text = "Build Date:" + Properties.Resources.BuildDate;

        }
    }
}
