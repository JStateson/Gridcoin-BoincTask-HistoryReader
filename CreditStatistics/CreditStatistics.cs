using CreditStatistics.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.cProjectStats;

namespace CreditStatistics
{   

    public partial class CreditStatistics : Form
    {

        public class cCreditInfo
        {
            public DateTime tCompleted;
            public int nCnt;
            public bool bValid;
            public double ElapsedSecs;
            public double CPUtimeSecs;
            public double Credits;
            public double mELA;
            public double mCPU;
            public void Init()
            {
                Credits = 0;
                ElapsedSecs = 0;
                CPUtimeSecs = 0;
                mELA = 0;
                mCPU = 0;
                nCnt = 0;
            }
        }

        public List<string> sNames = new List<string>();
        public cProjectStats ProjectStats = new cProjectStats();
        public string SelectedProject = "";
        public List<double> mCPU;
        public List<double> mELA;
        private string[] formats = { "dd MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, h:mm:ss tt UTC" };

        public class cOutFilter
        {
            public int n;
            public long nWidth;
            public double avg;
            public double std;
            public List<double> data;
            public List<int> outlierIndexes;
        }

        cOutFilter cNAS = new cOutFilter();

        public List<cCreditInfo> CreditInfo;  

        private Uri myUri;
        private WebClient client;
        private string RawPage;
        private string[] RawTable;
        private string[] RawLines;
        private int NumberRecsToRead = 20;
        private string MyComputerID = "";
        private string sTermDelim = "";
        public bool CanPageValids = true;
        private int TagOfProject = -1;
        private int RecordsPerPage = 0;

        private static string[] FindHdr = { "All (", "Valid (", "Invalid (", "Error (" };
        private static string[] FindHdrA = { "All</a> (", "Valid</a> (", "Invalid</a> (", "Error</a> (" };

        public CreditStatistics()
        {
            InitializeComponent();
            ProjUrl.Text = Properties.Settings.Default.InitialUrl;
            lbVersion.Text = "Build Date:" + GetSimpleDate(Properties.Resources.BuildDate);
            FormProjectRB();
        }

        private void FormProjectRB()
        {
            ProjectStats.Init();
            RunGetBoinc();
            sNames = ProjectStats.GetNames();
            int iRow = 0, iCol = 0;
            bool bHasID = false;
            int i = 0;
            foreach (string s in sNames)
            {
                RadioButton rb = new RadioButton();
                rb.Text = s;
                rb.Tag = i;
                string t = ProjectStats.GetIDfromName(s);
                bHasID = t != "";
                rb.AutoSize = true;
                rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(10 + iCol * 120, 80 + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                gbSamURL.Controls.Add(rb);
                iRow++;
                if (iRow > 10)
                {
                    iRow = 0;
                    iCol++;
                }
                i++;
            }
        }

        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            SelectedProject = rb.Text;
            TagOfProject = (int)rb.Tag;
            tbPage.Text = "0";
            if (ProjectStats.LocalHosts.Count > 0)
            {
                tbHOSTID.Text = ProjectStats.GetIDfromName(SelectedProject);
                ApplyName();
            }
        }

        private void SetRB(int iTag)
        {
            TagOfProject = iTag;
            foreach (Control c in gbSamURL.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if ((int)rb.Tag == iTag)
                    {
                        rb.CheckedChanged -= new System.EventHandler(this.rbProject_CheckedChanged);
                        rb.Checked = true;
                        rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                        return;
                    }
                }
            }
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

        private void ApplyFilter()
        {
            if(mCPU.Count == 0)
            {
                return;
            }
            foreach (cCreditInfo ci in CreditInfo)
            {
                ci.bValid = true;
            }
            if (cbfilterSTD.Checked)
            {
                (cNAS.data, cNAS.outlierIndexes) = RemoveOutliersWithIndexes(ref mCPU, 1.0);
                for (int k = 0; k < cNAS.outlierIndexes.Count; k++)
                {
                    CreditInfo[k].bValid = false;
                }
                foreach (int k in cNAS.outlierIndexes)
                {
                    CreditInfo[k].bValid = true;
                }
            }   
        }

        private void GetResults()
        {
            cCreditInfo SumC = new cCreditInfo();
            SumC.Init();
            tbInfo.Clear();
            ApplyFilter();
            string sOut = "Num" + Rp("     Date Completed", 22) + "  Credit   RunTime   RunTime   CPUtime   CPUtime  Valid" + Environment.NewLine;
            sOut += "   " + Rp(" ", 22) + "  Points      Secs   Credits      Secs   Credits";
            lbHdr.Text = sOut;
            sOut = "";
            for (int i = 0; i < CreditInfo.Count; i++)
            {
                cCreditInfo ci = CreditInfo[i];
                string dtS = ci.tCompleted.ToString("yyyy-MM-dd HH:mm:ss");
                sOut += Lp((i + 1).ToString(), 2) + " "
                    + Rp(dtS, 22)
                    + Lp(ci.Credits.ToString("F2"), 8)
                    + Lp(ci.ElapsedSecs.ToString("F2"), 10)
                    + Lp(ci.mELA.ToString("F4"), 10)
                    + Lp(ci.CPUtimeSecs.ToString("F2"), 10)
                    + Lp(ci.mCPU.ToString("F4"), 10)
                    + (ci.bValid ? "" : "   X")
                    + "\r\n";

                if (ci.bValid)
                {
                    SumC.Credits += ci.Credits;
                    SumC.ElapsedSecs += ci.ElapsedSecs;
                    SumC.CPUtimeSecs += ci.CPUtimeSecs;
                    SumC.mELA += ci.mELA;
                    SumC.mCPU += ci.mCPU;
                    SumC.nCnt++;
                }
            }
            int n = SumC.nCnt;
            if (n > 0)
            {
                SumC.mELA /= n;
                SumC.mCPU /= n;
                SumC.ElapsedSecs /= n;
                SumC.CPUtimeSecs /= n;
            }
            sOut += Environment.NewLine;
            sOut += Lp(SumC.nCnt.ToString(), 2)
                + Lp(" ", 22) + " "
                + Lp(SumC.Credits.ToString("F2"), 8)
                + Lp(SumC.ElapsedSecs.ToString("F2"), 10)
                + Lp(SumC.mELA.ToString("F4"), 10)
                + Lp(SumC.CPUtimeSecs.ToString("F2"), 10)
                + Lp(SumC.mCPU.ToString("F4"), 10)
                + "\r\n";
            sOut += Lp(" ",25) + Lp("Total", 8)
                + Lp("Avg", 10)
                + Lp("Avg", 10)
                + Lp("Avg", 10)
                + Lp("Avg", 10);
            tbInfo.Text = sOut;
        }

        private void TaskStart()
        {
            TaskTimer.Start();
            Task.Run(() => ReadOnePage());
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ProjectStats.ConfigureTask(TagOfProject,tbHOSTID.Text.ToString(), tbPage.Text.ToString(),NumberRecsToRead,"BODY");
            if(ProjectStats.RawPage != "")
            {

            }
            TaskStart();
           
        }

        string AsyncContent = "";   
        private async Task ReadOnePage()
        {
            ProjectStats.TaskBusy = true;
            while(ProjectStats.TaskBusy)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(ProjectStats.TaskUrl);
                        ProjectStats.RawPage = await response.Content.ReadAsStringAsync();
                        ProjectStats.TaskBusy = false;
                        ProjectStats.TaskDone = true;
                        ProjectStats.TaskError = false;
                    }
                    catch (HttpRequestException ex)
                    {
                        RawPage = "";
                        ProjectStats.TaskBusy = false;
                        ProjectStats.TaskDone = true;      
                        ProjectStats.TaskError = true;
                    }
                }
            }
        }

        private int ProcessHDR()
        {
            RawPage = ProjectStats.RawPage;
            if (RawPage == null) return 0;
            int NumValid = 0;
            for (int k = 0; k < FindHdr.Length; k++)
            {
                string sA = FindHdrA[k];
                string s = FindHdr[k];
                int i = RawPage.IndexOf(s);
                int n = s.Length;
                if (i < 0)
                {
                    i = RawPage.IndexOf(sA);
                    n = sA.Length;
                }
                if (i < 0) return 0;

                int j = RawPage.IndexOf(")", i + n);
                if (j < 0)  return 0;

                string t = RawPage.Substring(i + n, j - i - n);
                if (k == 1) NumValid = Convert.ToInt32(t);
                tbHdrInfo.Text += FindHdr[k] + t + ")\r\n";
            }
            return NumValid;
        }

        private void TaskLoadHeader()
        {
            ProjectStats.ConfigureTask(TagOfProject, tbHOSTID.Text.ToString().Trim(),
                tbPage.Text.ToString().Trim(), NumberRecsToRead, "HDR");
            CreditInfo = ProjectStats.LCreditInfo;
            mCPU = ProjectStats.mCPU;
            mELA = ProjectStats.mELA;
            mCPU.Clear();
            mELA.Clear();
            CreditInfo.Clear();
            TaskStart();
        }

        private string fBR(string s)
        {
            int i = s.IndexOf('>');
            if (i < 0) return "";
            return s.Substring(i + 1);  
        }

        private int ProcessPage_9_10_11()
        {
            int RecordCount = -1;
            RawPage = ProjectStats.RawPage;
            int i = RawPage.IndexOf("Application</th></tr>");
            if (i < 0)
            {
                tbInfo.Text = "unable to read page\r\n";
                return 0;
            }
            int j = RawPage.IndexOf("</table>");
            RawTable = RawPage.Substring(i, (j - i)).Split(new string[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
            j = 2;
            while (true)
            {
                if (j >= RawTable.Length) return RecordCount;
                string[] InsideTbl = RawTable[j].Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                RecordCount++;
                cCreditInfo ci = new cCreditInfo();
                ci.bValid = true;
                if (DateTime.TryParseExact(InsideTbl[4], formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                {
                    ci.tCompleted = dateTime1;
                }
                else
                {
                    return RecordCount;
                }

                ci.ElapsedSecs = Convert.ToDouble(fBR(InsideTbl[9]));
                ci.CPUtimeSecs = Convert.ToDouble(fBR(InsideTbl[10]));
                ci.Credits = Convert.ToDouble(fBR(InsideTbl[11]));
                if (ci.ElapsedSecs == 0.0) ci.ElapsedSecs = 0.01;
                ci.mELA = ci.Credits / ci.ElapsedSecs;
                mELA.Add(ci.mELA);
                if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                mCPU.Add(ci.mCPU);
                ci.bValid = true;   // do not really know if it is valid yet
                CreditInfo.Add(ci);
                j += 2;
            }
        }

        private int ProcessBody()
        {
            int n = 0;
            switch(ProjectStats.sCountValids)
            {
                case " Valid () .":
                case ">Valid ()</span>.":
                    int nErr = ProjectStats.GetTableFromRaw();
                    n = ProjectStats.NumberRecordsRead;
                    //ProcessPage_9_10_11();
                    GetResults();
                    return n;
                case ">Valid 0 </a>.":
                    break;
                case "<b>Valid</b> 0 |.":
                    break;
                case "null":
                    break;
            }
            return n;
        }

        private string Add20(string s)
        {
            int i = s.IndexOf("offset=");
            if (i < 0) return "";
            i += 7;
            int j = s.IndexOf('&', i);
            if (j < 0) return "";
            string s1 = s.Substring(0, i);
            string s2 = s.Substring(j);
            int n = Convert.ToInt32(s.Substring(i, j - i));
            n += 20;
            return s1 + n.ToString() + s2;
        }

        private void ErrorAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.InitialUrl = ProjUrl.Text;
            //Properties.Settings.Default.MaxRecords = (int)nudRecsToRead.Value;
            Properties.Settings.Default.Save();
        }

    

        // pad right side with spaces to fill
        public static string Rp(string strIn, int cnt)
        {
            int i = cnt - strIn.Length;
            if (i < 0) return strIn.Substring(0, cnt);
            return strIn + "                              ".Substring(0, i);
        }

        // pad left side with spaces to fill
        public static string Lp(string strIn, int cnt)
        {
            int i = cnt - strIn.Length;
            if (i < 0) return strIn.Substring(0, cnt);
            return "                              ".Substring(0, i) + strIn;
        }

        private bool ParseUrl(string s)
        {
            if (s == "") return false;
            if (s.Contains("hostid") && s.Contains("results.php")) return true;
            if(s.Contains("/host/"))
            {
                int i = s.IndexOf("/host/");
                string t = s.Substring(0, i);
            }
            return false;
        }   

        private void btnFind_Click(object sender, EventArgs e)
        {
            gbGetStats.Enabled = false;
            tbHdrInfo.Clear();
            bool bValid = RunExtract();
            if(bValid)
                TaskLoadHeader();
        }

        private (List<double>, List<int>) RemoveOutliersWithIndexes(ref List<double> data, double threshold = 2)
        {            
            double mean = data.Average();
            double stdDev = Math.Sqrt(data.Average(v => Math.Pow(v - mean, 2)));

            // Track indexes of removed outliers
            List<int> outlierIndexes = new List<int>();

            // Filter data while keeping track of original indexes
            List<double> filteredData = data
                .Select((value, index) => new { value, index }) // Store original index
                .Where(item =>
                {
                    bool isOutlier = Math.Abs(item.value - mean) > threshold * stdDev;
                    if (!isOutlier) outlierIndexes.Add(item.index);
                    return !isOutlier;
                })
                .Select(item => item.value)
                .ToList();

            return (filteredData, outlierIndexes);
        }

        private void cbfilterSTD_CheckedChanged(object sender, EventArgs e)
        {
            GetResults();
        }

        private void ProjUrl_TextChanged(object sender, EventArgs e)
        {
            gbGetStats.Enabled = false;
        }

        bool IsInteger(string input)
        {
            return int.TryParse(input, out _);
        }

        private void ApplyName()
        {
            string sID = tbHOSTID.Text.Trim();
            ProjUrl.Text = "";
            if (sID == "") return;
            if(SelectedProject == "" || sID == "12345")
            {
                return;
            }
            if (IsInteger(sID))
            {
                string sURL = ProjectStats.GetURL0(SelectedProject, sID,  ref CanPageValids);
                if (sURL == "")
                {
                    Debug.Assert(false);
                }
                ProjUrl.Text = sURL;
            }
        }
        private void btnApplyName_Click(object sender, EventArgs e)
        {
            ApplyName();
        }

        private void RunGetBoinc()
        {
            ProjectStats.GetHosts(tbBoincLoc.Text);
            tbProjHostList.Text = "";            
            string s = "";
            if (ProjectStats.LocalHosts.Count > 0)
            {
                foreach (cLHe c in ProjectStats.LocalHosts)
                {
                    s += Lp(c.name, 24) + " " + c.HostID + "\r\n";
                }
            }
            tbProjHostList.Text = s;
        }

        private void btnReadBoinc_Click(object sender, EventArgs e)
        {
            RunGetBoinc();
        }

        private bool RunExtract()
        {

            int i, j;
            string s = ProjUrl.Text.ToLower();
            int pLoc = ProjectStats.GetNameIndex(s);
            if (pLoc < 0)
            {
                MessageBox.Show("Project not found in url");
                return false;
            }
            SetRB(pLoc);
            string sLoc = ProjectStats.ProjectList[pLoc].sPage;
            if (sLoc != "null")
            {
                i = s.IndexOf(sLoc);
                if (i < 0)
                {
                    s += sLoc + "0";
                    ProjUrl.Text = s;
                    i = s.IndexOf(sLoc);
                }
                j = FirstNonInteger(s, i + sLoc.Length);
                string sPage = s.Substring(i + sLoc.Length, j - i - sLoc.Length);
                tbPage.Text = sPage;
            }
            else tbPage.Text = "";

            string sHIDlookup = ProjectStats.ProjectList[pLoc].sHid;
            i = s.IndexOf(sHIDlookup);
            if (i < 0)
            {
                MessageBox.Show(sHIDlookup + " not found in url");
                return false;
            }
            j = FirstNonInteger(s, i + sHIDlookup.Length);
            string sHID = s.Substring(i + sHIDlookup.Length, j - i - sHIDlookup.Length);
            tbHOSTID.Text = sHID;

            string sWantValid = ProjectStats.ProjectList[pLoc].sValid;
            if (sWantValid != null)
            {
                i = s.IndexOf(sWantValid);
                if (i < 0)
                {
                    s += sWantValid;
                    ProjUrl.Text = s;
                }
            }
            return true;
        }
        private void btnExtract_Click(object sender, EventArgs e)
        {
            RunExtract();
        }

        private int FirstNonInteger(string s, int iOffset)
        {
            int i = iOffset;
            while (i < s.Length)
            {
                if (s[i] < '0' || s[i] > '9') return i;
                i++;
            }
            return s.Length;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbInfo.Clear();
        }

        private void TaskTimer_Tick(object sender, EventArgs e)
        {
            pbTask.Value++;
            if (pbTask.Value >= pbTask.Maximum || ProjectStats.TaskDone)
            {
                TaskTimer.Stop();
                pbTask.Value = 0;
                switch(ProjectStats.sTaskType)
                {
                    case "HDR":
                        ProjectStats.NumValid = ProcessHDR();
                        if (ProjectStats.NumValid == 0) return;
                        gbGetStats.Enabled = !ProjectStats.TaskError;
                        RecordsPerPage = ProcessBody();
                        break;
                    case "BODY":
                        GetResults();
                        break;
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            ProjectStats.TaskDone = true;
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            ProjectStats.TaskDone = true;
        }
    }
}
