using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace BTHistoryReader
{
    partial class reportbug : Form
    {
        string strWhere;
        public reportbug(string strLastHistoryFiles, string strWhereLooking)
        {
            InitializeComponent();
            tbLastFiles.Text = strLastHistoryFiles;
            strWhere = strWhereLooking;
        }
         

        private void btnEmail_Click(object sender, EventArgs e)
        {
            string bDate = Properties.Resources.BuildDate;
            string mailto = string.Format("mailto:{0}?Subject={1}&Body={2}", "bthistory@stateson.net", "BugReport(v)Date : " + bDate, "Compress files listed in box and attach to this email");

            Process.Start(mailto);
            
        }

        private void btnfindFiles_Click(object sender, EventArgs e)
        {
            if(strWhere == "")
            {
                MessageBox.Show("Error: cannot find history files");
                return;
            }
            Process.Start("explorer.exe", strWhere);
        }
    }
}
