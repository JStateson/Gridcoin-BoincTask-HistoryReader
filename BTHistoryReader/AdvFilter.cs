using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTHistoryReader
{
    public partial class AdvFilter : Form
    {

        cAdvFilter MyADF;

        public AdvFilter(ref cAdvFilter rADF)
        {
            MyADF = rADF;
            InitializeComponent();
            if(MyADF.strPhrase != "")
            {
                tbFilPhrase.Text = MyADF.strPhrase;
                rbContain.Checked = MyADF.bContains;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MyADF.strPhrase = tbFilPhrase.Text;
            MyADF.bContains = rbContain.Checked;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MyADF.strPhrase = "";
            this.Close();
        }
    }
}
