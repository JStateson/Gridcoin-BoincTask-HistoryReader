using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace InvalidAnalysis
{
    

    public partial class ErrorAnalysis : Form
    {

        private Uri myUri;
        private WebClient client;
        private string RawPage;
        private string[] RawTable;
        private string[] RawLines;
        private int NumberRecsToRead = 20;
        private string MyComputerID = "";
        public List<cProjPage> ProjectPages;

        public ErrorAnalysis()
        {
            InitializeComponent();
            nudRecsToRead.Value = Properties.Settings.Default.MaxRecords;
            ProjUrl.Text = Properties.Settings.Default.InitialUrl;
            lbVersion.Text = "Build Date:" + GetSimpleDate(Properties.Resources.BuildDate);
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            Process.Start(ProjUrl.Text);
        }


        private string GetSimpleDate(string sDT)
        {
            //Sun 06/09/2019 23:33:53.18 
            int i = sDT.IndexOf(' ');
            i++;
            int j = sDT.LastIndexOf('.');
            return sDT.Substring(i, j - i);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnShowUT.Enabled = false;
            LoadProjectPage();
            int i, j, n;
            int iPage = 1;
            lvWorkUnits.Items.Clear();
            tbInfo.Clear();

            foreach(cProjPage pp in ProjectPages)
            {
                myUri = new Uri(pp.urlWorkUnit);
                client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705");
                RawPage = client.DownloadString(myUri);
                string[] FiveTables = RawPage.Split(new string[] { "<div class=\"table\">", "</table>" }, StringSplitOptions.RemoveEmptyEntries);
                string [] aLine = FiveTables[1].Split(new string[] { "<td ", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                cWorkUnit wu = new cWorkUnit();
                wu.HeaderInit(ref aLine[3], ref aLine[31]);
                i = FiveTables[3].IndexOf("Application</th></tr>");  // should be able split using <tr> and </tr>
                aLine = FiveTables[3].Substring(i).Split(new string[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
                j = 2;
                n = (aLine.Count() - 2) / 2;
                pp.Wingmen = new List<cWorkUnit>();
                for(i = 0; i < n; i++)
                {
                    wu.AddData(ref aLine[j]);
                    pp.Wingmen.Add(wu);
                    j += 2;
                    wu = new cWorkUnit();
                    wu.name = pp.Wingmen[0].name;
                    wu.ets = pp.Wingmen[0].ets;
                }
                tbInfo.Text += iPage.ToString("D").PadLeft(4) + " " + pp.strWorkUnit + "\r\n";
                ListViewItem lvi = new ListViewItem();
                lvi.Text = pp.strWorkUnit;
                lvi.Tag = pp.urlWorkUnit;
                lvWorkUnits.Items.Add(lvi);
                tbInfo.Refresh();
                lvWorkUnits.Refresh();
                iPage++;
            }
            btnShowUT.Enabled = true;
        }




        private void LoadProjectPage()
        {
            ProjectPages = new List<cProjPage>();
            cProjPage pp = new cProjPage();
            int i, j;
            NumberRecsToRead = Convert.ToInt32(nudRecsToRead.Value); 
            myUri = new Uri(ProjUrl.Text);
            client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705");
            RawPage = client.DownloadString(myUri);
            i = RawPage.IndexOf("Application</th></tr>");
            if(i < 0)
            {
                tbInfo.Text = "unable to read first page\r\n";
                return;
            }
            j = RawPage.IndexOf("</table>");
            RawTable = RawPage.Substring(i, (j - i)).Split(new string[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
            j = 2;
            for(i = 0; i < NumberRecsToRead; i++)
            {
                pp = new cProjPage();
                if(i == 0)
                    MyComputerID = pp.InitProject("https://milkyway.cs.rpi.edu/milkyway", ref RawPage, "tasks for computer ");
                else
                {
                    pp.strProject = ProjectPages[0].strProject; // could not figure another way to get url info here
                }
                pp.SetData("Milkyway@home", MyComputerID, ref RawTable[j]);
                ProjectPages.Add(pp);
                j += 2;
            }
        }

        private void btnShowUT_Click(object sender, EventArgs e)
        {
            InfoForm MyInfo = new InfoForm(this, "UserTasks");
            MyInfo.ShowDialog();
            MyInfo.Dispose();
        }

        private void ErrorAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.InitialUrl = ProjUrl.Text;
            Properties.Settings.Default.MaxRecords = (int)nudRecsToRead.Value;
            Properties.Settings.Default.Save();
        }

    

        private void lvWorkUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = lvWorkUnits.SelectedItems[0];
            Process.Start((string)lvi.Tag);
        }
    }
}
