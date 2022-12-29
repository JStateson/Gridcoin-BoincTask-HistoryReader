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
        cGpuReassigned GpuRe;

        public AdvFilter(ref cAdvFilter rADF, ref cGpuReassigned GpuReassignment)
        {
            MyADF = rADF;
            GpuRe = GpuReassignment;
            InitializeComponent();
            if(MyADF.strPhrase != "")
            {
                tbFilPhrase.Text = MyADF.strPhrase;
                rbContain.Checked = MyADF.bContains;
                rbDoNotContain.Checked = !rbContain.Checked;
            }
            cbUnkGpuSelect.Items.Clear();
            cbUnkGpuSelect.Items.Add("Use last known GPU");
            if (GpuRe.NumGPUs > 1)
            {
                for(int i = 0; i < GpuRe.NumGPUs; i++)
                {
                    cbUnkGpuSelect.Items.Add("use GPU# " + i.ToString());
                }
            }

            cbUnkGpuSelect.SelectedItem = GpuRe.ReassignedGPU + 1;
            cbUnkGpuSelect.SelectedIndex = GpuRe.ReassignedGPU + 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MyADF.strPhrase = tbFilPhrase.Text;
            MyADF.bOKreturn = tbFilPhrase.Text != "";
            MyADF.bContains = rbContain.Checked;
            GpuRe.ReassignedGPU = cbUnkGpuSelect.SelectedIndex - 1;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //MyADF.strPhrase = ""; may want to restore original phrase change mind but have not saved)
            MyADF.bOKreturn = false;
            this.Close();
        }
    }
}
