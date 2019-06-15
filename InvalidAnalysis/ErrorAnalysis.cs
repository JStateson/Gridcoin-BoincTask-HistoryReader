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
        private List<cProjPage> ProjectPages;

        public ErrorAnalysis()
        {
            InitializeComponent();
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            Process.Start(ProjUrl.Text);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            LoadProjectPage();
            int i, j, n;
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
                
                for(i = 0; i < n; i++)
                {
                    wu.AddData(ref aLine[j]);
                    pp.Wingmen.Add(wu);
                    j += 2;
                }
            }
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
                pp.SetData("Milkyway@home", MyComputerID, ref RawTable[j]);
                ProjectPages.Add(pp);
                j += 2;
            }
        }

    }
}
