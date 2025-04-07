using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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

        private List<string> MyList = new List<string>();
        private string SequencerOut = "";   // used when running sequencer instead of output to text box
        private bool bInSequencer = false;
        private string sOutInfo = "";
        private string sOutHdrs = "";

        private string sTotals;
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
        private CancellationTokenSource cts;
        private Uri myUri;
        private WebClient client;
        private string RawPage;
        private string[] RawTable;
        private string[] RawLines;
        private int MaximumRecsToRead = 100;
        private string MyComputerID = "";
        private string sTermDelim = "";
        public bool CanPageValids = true;
        private int TagOfProject = -1;
        private int RecordsPerPage = 0;
        private int MaxShortSize = 8;  // will be pixel with times character count
        TabPage hiddenTabPage;

        private static string[] FindHdr = { "All (", "Valid (", "Invalid (", "Error (" };
        private static string[] FindHdrA = { "All</a> (", "Valid</a> (", "Invalid</a> (", "Error</a> (" };
        private static string[] FindHdrB = { "All</a> ", "Valid</b> ", "Invalid</font> ", "Error</a> " };
        private static string[] FindBTrm = { "|", "|", "|", "<" };
        private static string[] FindHdrC = { ">All ", ">Valid ", ">Invalid ", ">To late " };
        private static string[] FindCTrm = { "</a>", "</a>", "</a>", "</a>" };

        private List<string>defaultNameHost = new List<string>();
        public CreditStatistics()
        {
            InitializeComponent(); 
            ProjUrl.Text = Properties.Settings.Default.InitialUrl;
            lbVersion.Text = "Build Date:" + GetSimpleDate(Properties.Resources.BuildDate);
            FormProjectRB();
            //dgv.Columns[0].FillWeight = (MaxShortSize+4)*2 ; // pix width?
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
#if DEBUG
            btnCreateIDs.Enabled = true;
            btnSaveDefIDs.Enabled = true;
#else
            btnCreateIDs.Enabled = false;
            btnSaveDefIDs.Enabled = false;
#endif
            this.Shown += InitialLoad;
            
        }

        private void InitialLoad(object sendler, EventArgs e)
        {
            Refresh();
            Task.Delay(1);
            Application.DoEvents();
            SetRB(0,false); // set default to first project
            hiddenTabPage = tcProj.TabPages["lbViewRaw"]; 
            tcProj.TabPages.Remove(hiddenTabPage);
            TryGetHostSets();
        }

        private void FormProjectRB()
        {
            ProjectStats.Init();
            RunGetBoinc();
            sNames = ProjectStats.GetNames();
            defaultNameHost.Clear();
            int iRow = 0, iCol = 0;
            bool bHasID = false;
            int i = 0;
            foreach (string s in sNames)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;                
                string t = ProjectStats.GetIDfromName(s);
                rb.Text = ProjectStats.ShortName(i);
                cbSelProj.Items.Add(rb.Text);
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);  
                defaultNameHost.Add(rb.Text + ": " + t);
                bHasID = t != "";
                rb.AutoSize = true;
                rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(10 + iCol * 120, 80 + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                gbSamURL.Controls.Add(rb);
                iRow++;
                if (iRow > 9)
                {
                    iRow = 0;
                    iCol++;
                }
                i++;
            }
        }

        private void UpdateRB()
        {
            foreach(Control c in gbSamURL.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    int i = (int)rb.Tag;
                    string s = ProjectStats.ProjectList[i].name;
                    string t = ProjectStats.GetIDfromName(s);
                    bool bHasID = t != "";
                    rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                }
            }
        }

        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;            
            TagOfProject = (int)rb.Tag;
            SelectedProject = ProjectStats.ProjectList[TagOfProject].name;
            tbPage.Text = "0";
            tbHdrInfo.Clear();
            tbInfo.Clear();

            if (ProjectStats.LocalHosts.Count > 0)
            {
                tbHOSTID.Text = ProjectStats.GetIDfromName(SelectedProject);
                ApplyName();
            }
        }

        private void SetRB(int iTag, bool b)
        {
            TagOfProject = iTag;
            foreach (Control c in gbSamURL.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if ((int)rb.Tag == iTag)
                    {
                        if(b)rb.CheckedChanged -= new System.EventHandler(this.rbProject_CheckedChanged);
                        rb.Checked = true;
                        if(b)rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                        return;
                    }
                }
            }
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            Process.Start(ProjUrl.Text.ToString());
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
                cNAS.data = null;
                cNAS.outlierIndexes = null;
                (cNAS.data, cNAS.outlierIndexes) = RemoveOutliersWithIndexes(ref mCPU, 2.0);
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
            double dH = 0;  // hours
            long Sd = 0;
            long Ed = 0;  
            SumC.Init();
            tbInfo.Clear();
            ApplyFilter();
            sOutHdrs = "Num" + Rp("     Date Completed", 22) + "    Credit     RunTime     RunTime     CPUtime     CPUtime  Valid" + Environment.NewLine;
            sOutHdrs += "   " + Rp(" ", 22) + "    Points        Secs     Credits        Secs     Credits";
            lbHdr.Text = sOutHdrs;
            sOutInfo = "";
            for (int i = 0; i < CreditInfo.Count; i++)
            {
                cCreditInfo ci = CreditInfo[i];
                string dtS = ci.tCompleted.ToString("yyyy-MM-dd HH:mm:ss");
                if (i == 0) Ed = (long)ci.tCompleted.Ticks;
                if (i == (CreditInfo.Count - 1))
                {
                    Sd = (long)ci.tCompleted.Ticks;
                    dH = (double) ((double)(Ed - Sd) / TimeSpan.TicksPerHour);
                }
                    
                sOutInfo += Lp((i + 1).ToString(), 2) + " "
                    + Rp(dtS, 22)
                    + Lp(ci.Credits.ToString("F2"), 10)
                    + Lp(ci.ElapsedSecs.ToString("F2"), 12)
                    + Lp(ci.mELA.ToString("F4"), 12)
                    + Lp(ci.CPUtimeSecs.ToString("F2"), 12)
                    + Lp(ci.mCPU.ToString("F4"), 12)
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
            sOutInfo += Environment.NewLine;
            sTotals = Lp(SumC.nCnt.ToString(), 2)
                + Rp("   Hours:" + dH.ToString("F2"), 22) + " "
                + Lp(SumC.Credits.ToString("F2"), 10)
                + Lp(SumC.ElapsedSecs.ToString("F2"), 12)
                + Lp(SumC.mELA.ToString("F4"), 12)
                + Lp(SumC.CPUtimeSecs.ToString("F2"), 12)
                + Lp(SumC.mCPU.ToString("F4"), 12)
                + "\r\n";
            sTotals += Lp(MyComputerID,25) + Lp("Total", 10) // put hostname here
                + Lp("Avg", 12)
                + Lp("Avg", 12)
                + Lp("Avg", 12)
                + Lp("Avg", 12);
            if(bInSequencer)
            {                
                SequencerOut += sOutHdrs + sOutInfo + sTotals;
            }
            else
            {
                tbInfo.Text = sOutHdrs + sOutInfo + sTotals;
            }
               
        }

        private void TaskStart()
        {
            cts = new CancellationTokenSource();
            TaskTimer.Start();
  
            Task longRunningTask = Task.Run(async () =>
            {
               await ReadOnePage();
            }, cts.Token);
        }

        private void RunNext()
        {
            if (ProjectStats.IncrementReader() && CreditInfo.Count < MaximumRecsToRead)
            {
                tbPage.Text = ProjectStats.sTaskOffset;
                TaskStart();
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            RunNext();
        }

        private async Task ReadOnePage()
        {
            ProjectStats.TaskBusy = true;
            //while(ProjectStats.TaskBusy)
            {
                ProjectStats.RawPage = "";
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Add("Referer", ProjectStats.TaskUrl);
                    //client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
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
                string sDFT = ")";  // default terminator

                string sB = FindHdrB[k];
                string tB = FindBTrm[k];
                string sC = FindHdrC[k];
                string tC = FindCTrm[k];


                string sA = FindHdrA[k];
                string s = FindHdr[k];
                int i = RawPage.IndexOf(s);
                int n = s.Length;
                if (i < 0)
                {
                    i = RawPage.IndexOf(sA);
                    n = sA.Length;
                }
                if (i < 0)
                {
                    i = RawPage.IndexOf(sB);
                    n = sB.Length;
                    sDFT = tB;
                }
                if (i < 0)
                {
                    i = RawPage.IndexOf(sC);
                    n = sC.Length;
                    sDFT = tC;
                }
                if (i < 0) return 0;

                int j = RawPage.IndexOf(sDFT, i + n);
                if (j < 0)  return 0;

                string t = RawPage.Substring(i + n, j - i - n).Trim();
                if (t == "") t = "0";
                if (k == 1) NumValid = Convert.ToInt32(t);
                tbHdrInfo.Text += FindHdr[k] + t + ")\r\n";
            }
            return NumValid;
        }

        // type is HDR or SEQ for header or sequencer
        private void TaskLoadHeader(string sType)
        {
            ProjectStats.ConfigureTask(TagOfProject, tbHOSTID.Text.ToString().Trim(),
                tbPage.Text.ToString().Trim(), sType);
            CreditInfo = ProjectStats.LCreditInfo;
            mCPU = ProjectStats.mCPU;
            mELA = ProjectStats.mELA;
            mCPU.Clear();
            mELA.Clear();
            CreditInfo.Clear();
            TaskStart();
        }

        private void LoadTask(string TaskType, string sUrl)
        {
            ProjectStats.sTaskType = TaskType;
            ProjectStats.TaskUrl = sUrl;
            ProjectStats.StopTask = false;
            ProjectStats.TaskBusy = false;
            ProjectStats.TaskDone = false;
            switch (TaskType)
            {
                case "FetchHostIDs":
                    AllowGS(false);
                    tbHdrInfo.Clear();
                    break;
            }
            TaskStart();
        }
   
        private int ProcessBody()
        {
            int n = 0;
            int nErr = 0;
            switch(ProjectStats.sCountValids)
            {
                case " Valid () .":
                case ">Valid ()</span>.":
                case "<b>Valid</b> 0 |.":
                case ">Valid 0 </a>.":
                    nErr = ProjectStats.GetTableFromRaw();
                    CreditInfo = ProjectStats.LCreditInfo;
                    n = ProjectStats.NumberRecordsRead;
                    GetResults();
                    return n;
                case ">Valid 0 </a>cc.":
                    break;
                case "null":
                    nErr = ProjectStats.GetTableFromRaw();
                    GetResults();
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
            return "                                               ".Substring(0, i) + strIn;
        }

        private void AllowGS(bool b)
        {
            btnClear.Enabled = b;
            btnStart.Enabled = b;
        }

        private void StartRun(string sType)
        {
            AllowGS(false);
            tbHdrInfo.Clear();
            TaskLoadHeader(sType);
        }

        private void btnRunHdr_Click(object sender, EventArgs e)
        {
            StartRun("HDR");
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
            AllowGS(false);
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
                string sURL = ProjectStats.GetURL0(SelectedProject, sID, ref CanPageValids);
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
            MyComputerID = ProjectStats.GetHosts(tbBoincLoc.Text);
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

        // check url for errors and reform url to simplify
        public bool ParseUrl(string s, ref string sOut, ref int ProjectIndex,
            ref string sHost, ref string sSelectedProject)
        {
            string sPageOffset = "";    // page: 0,1,2 or offset: 0,20,40 etc value + increment of 20
                                        // includes the directive (?offset=) or (page)

            string tURL = "";
            string tHid = "";
            string tValid = "";
            string tPage = "";
            string tCountValids = "";
            string tStudy = "";
            string t;
            int i, j;

            s = s.Trim();
            if (s == "") return false;
            string sUrl = s.ToLower();

            i = sUrl.IndexOf("http");
            if (i < 0)
            {
                MessageBox.Show("badly formed url: missing http");
                return false;
            }
            sUrl = sUrl.Substring(i);

            ProjectIndex = ProjectStats.GetNameIndex(sUrl);
            if(ProjectIndex < 0)
            {
                MessageBox.Show("Project not found in url");
                return false;
            }

            // determine if the project uses an application ID and if not then flag default
            if(ProjectStats.ProjectList[ProjectIndex].sStudy == "null")
            {
                tStudy = "null";
                ProjectStats.ProjectList[ProjectIndex].UseDefault = true;
            }
            else
            {
                string ProjectName = ProjectStats.ProjectList[ProjectIndex].name;
                if (ProjectName == "einstein")
                {
                    i = sUrl.IndexOf("/tasks/4/");
                    if(i < 0)
                    {
                        MessageBox.Show("Task ID not found in url");
                        return false;
                    }
                    j = FirstNonInteger(sUrl, i + 9);
                    t = sUrl.Substring(i + 9, j - i - 9);
                    if (t == "0")
                    {
                        ProjectStats.ProjectList[ProjectIndex].UseDefault = true;
                    }
                    else
                        ProjectStats.ProjectList[ProjectIndex].UseDefault = false;
                        ProjectStats.ProjectList[ProjectIndex].sStudyV = t;
                }
                else
                {
                    i = sUrl.IndexOf("?appid=");
                    if(i > 0)
                    {
                        j = FirstNonInteger(sUrl, i + 7);
                        t = sUrl.Substring(i + 7, j - i - 7);
                        if (t == "0")
                        {
                            ProjectStats.ProjectList[ProjectIndex].sStudyV = t;
                            ProjectStats.ProjectList[ProjectIndex].UseDefault = true;
                        }
                        else ProjectStats.ProjectList[ProjectIndex].UseDefault = false;
                    }
                    else
                    {
                        ProjectStats.ProjectList[ProjectIndex].UseDefault = false;
                    }
                }                
            }

            ProjectStats.GetBaseInfo(ProjectIndex, ref tURL, ref tHid, ref tValid, ref tPage, ref tStudy, ref tCountValids);

            //need to get the value assigned to tHid and to tPage
            i = sUrl.IndexOf(tHid);
            if (i < 0)
            {
                MessageBox.Show(tHid + " not found in url");
                return false;
            }

            j = FirstNonInteger(sUrl, i + tHid.Length);
            sHost = s.Substring(i + tHid.Length, j - i - tHid.Length);
            string sPage = "0";
            //need to get the page offset indexing into the table if it exists
            if (tPage != "null")
            {
                i = sUrl.IndexOf(tPage);
                if (i < 0)
                {
                    sPageOffset = tPage + "0";
                }
                else
                {
                    j = FirstNonInteger(s, i + tPage.Length);
                    sPage = s.Substring(i + tPage.Length, j - i - tPage.Length);
                    sPageOffset = tPage + sPage;
                }                
            }
            else sPageOffset = "";
            ProjectStats.sTaskOffset = sPage;
            sOut = tURL + tHid + sHost + tValid +  ((tStudy=="null") ? "" : tStudy) + sPageOffset;
            //sOut = tURL + tHid + sHost + tValid + tStudy + sPageOffset;
            sSelectedProject = ProjectStats.ProjectList[ProjectIndex].name;
            return true;
            if (tStudy != "null")
            {
                i = sUrl.IndexOf(tStudy); // the /0 or /56 of the /4/xxg or ?appid=xx the x part
                if (i < 0)
                {
                    sOut = tURL + tHid + sHost + tValid + tStudy + sPageOffset;
                }
                else
                {
                    j = FirstNonInteger(s, i + tStudy.Length);
                    string sStudyV = s.Substring(i + tStudy.Length, j - i - tStudy.Length);
                    // need to put sStudyV where the default study value is
                    // default is like &appid=13 or like /0
                    int k = FindFirstNonNumericFromEnd(tStudy);
                    Debug.Assert(k > 0);
                    string sStudy = tStudy.Substring(0, k) + sStudyV;
                    sOut = tURL + tHid + sHost + tValid + sStudy + sPageOffset;
                }
            }
            else
            {
                sOut = tURL + tHid + sHost + tValid + sPageOffset;
            }
            sSelectedProject = ProjectStats.ProjectList[ProjectIndex].name;
            return true;
        }

        private int FindFirstNonNumericFromEnd(string str)
        {
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(str[i]))
                {
                    return i;
                }
            }
            return -1; // All characters are digits
        }
        private void btnExtract_Click(object sender, EventArgs e)
        {
            string sOut = "";
            string sHost = "";
            int ProjectID = -1;
            if (ParseUrl(ProjUrl.Text, ref sOut, ref ProjectID, ref sHost, ref SelectedProject))
            {
                ProjUrl.Text = sOut;
                tbHOSTID.Text = sHost;
                SetRB(ProjectID,true);
            }
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


        private void RunCancel(bool bErr)
        {
            cts.Cancel();
            ProjectStats.TaskDone = true;
            ProjectStats.TaskError = bErr;
            bInSequencer = false;
            pbTask.Value = 0;
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            RunCancel(true);
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            ProjectStats.TaskDone = true;
        }

        private void lbSelectDemo_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string sOut = "";
            int col = 0;
            int w = tbSelected.Width / 10;
            string lOut = "";
            foreach(var item in lbSelectDemo.SelectedItems)
            {
                string s = item.ToString();
                Clipboard.SetText(s);
                s = s.ToLower();
                int i = ProjectStats.GetNameIndex(s);
                s = ProjectStats.ShortName(i);
                if (s.Length + lOut.Length > w)
                {
                    sOut += lOut + Environment.NewLine;
                    lOut = s;
                }
                else lOut += " " + s;
            }
            sOut += lOut;
            tbSelected.Text = sOut;
        }

        private void btnClearURL_Click(object sender, EventArgs e)
        {
            ProjUrl.Clear();
        }

        private void InstallURL(string sIn)
        {
            string sOut = "";
            int ProjectID = -1;
            string sHost = "";
            string sName = "";
            ProjUrl.Text = sIn;
            if (ParseUrl(sIn, ref sOut, ref ProjectID, ref sHost, ref sName))
            {
                ProjUrl.Text = sOut;
                tbHOSTID.Text = sHost;
                TagOfProject = ProjectID;
                SetRB(ProjectID,false);
            }
        }
        private void btnPaste_Click(object sender, EventArgs e)
        {
            InstallURL(Clipboard.GetText().Trim());
        }

        private void InstallUrlNoEvent(string sIn)
        {
            string sOut = "";
            int ProjectID = -1;
            string sHost = "";
            string sName = "";
            ProjUrl.Text = sIn;
            if (ParseUrl(sIn, ref sOut, ref ProjectID, ref sHost, ref sName))
            {
                ProjUrl.Text = sOut;
                tbHOSTID.Text = sHost;
                TagOfProject = ProjectID;
                SetRB(ProjectID, true);
                SelectedProject = ProjectStats.ProjectList[TagOfProject].name;
                tbPage.Text = "0";
                tbHdrInfo.Clear();
                tbInfo.Clear();
                if (ProjectStats.LocalHosts.Count > 0)
                {
                    tbHOSTID.Text = ProjectStats.GetIDfromName(SelectedProject);
                    ApplyName();
                }
            }
        }

        private void TaskTimer_Tick(object sender, EventArgs e)
        {
            pbTask.Value++;
            if (pbTask.Value >= pbTask.Maximum || ProjectStats.TaskDone)
            {
                TaskTimer.Stop();
                if(!bInSequencer)
                    pbTask.Value = 0;
                if(ProjectStats.TaskDone == false)
                {
                    RunCancel(true);
                }
                switch (ProjectStats.sTaskType)
                {
                    case "HDR":
                        if (ProjectStats.sCountValids != "null")
                        {
                            ProjectStats.NumValid = ProcessHDR();
                            if (ProjectStats.NumValid == 0) return;
                        }
                        AllowGS(!ProjectStats.TaskError);
                        RecordsPerPage = ProcessBody();
                        ProjectStats.sTaskType = "BODY";
                        break;
                    case "BODY":
                        RecordsPerPage = ProcessBody();
                        break;
                    case "SEQBODY":
                        RecordsPerPage = ProcessBody();
                        Sequencer((RecordsPerPage == 0) ? "VOID" : "NEXT");
                        break;
                    case "SEQ":
                        if(ProjectStats.TaskError)
                        {
                            Sequencer("ERROR");
                            return;
                        }
                        if (ProjectStats.sCountValids != "null")
                        {
                            ProjectStats.NumValid = ProcessHDR();
                            if (ProjectStats.NumValid > 0)
                            {
                                RecordsPerPage = ProcessBody();
                                ProjectStats.sTaskType = "SEQBODY";
                                Sequencer((RecordsPerPage == 0) ? "VOID" : "NEXT");
                                return;
                            }
                            else
                            {

                            }
                        }
                        Sequencer("VOID");
                        break;
                }
            }
        }

        /*
         * projectname: i1, id2 id3 id4; id5, id6  etc delim:,; or space
         */


        private void btnSaveAS_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save List As";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            foreach (var item in lbSelectDemo.Items)
                            {
                                writer.WriteLine(item.ToString());
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ReadStringsIntoList(ref List<string> sList)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Open Text File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(openFileDialog.FileName);
                        Properties.Settings.Default.HostList = lines;
                        Properties.Settings.Default.Save();
                        sList.Clear(); // Clear current items
                        sList.AddRange(lines); // Add all lines
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnLoadFrom_Click(object sender, EventArgs e)
        {
            ReadStringsIntoList(ref MyList);
            lbSelectDemo.Items.Clear();
            foreach (string s in MyList)
            {
                string sOut = "";
                string sHost = "";
                int ProjectID = -1;
                string sName = "";
                if (ParseUrl(s.Trim(), ref sOut, ref ProjectID, ref sHost, ref sName))
                    lbSelectDemo.Items.Add(sOut);
                else break;
            }   
        }

        private int TryGetHostSets()
        {
            string[] SavedHostList = Properties.Settings.Default.HostList;
            if (SavedHostList == null || SavedHostList.Length == 0) return  -1;
            MyList.Clear();
            MyList.AddRange(SavedHostList);
            if (MyList.Count == 0) return -1;
            return FormHostList();
        }

        private int FormHostList()
        {

            if (MyList.Count == 0) return 0;
            int i, n = 0;
            cbComputerList.Items.Clear();
            foreach (string s in MyList)
            {
                i = s.IndexOf(":");
                if (i < 0)
                {
                    MessageBox.Show("Expected format of project name: id1 id2 .. not found!");
                    continue;
                }
                string sName = s.Substring(0, i).Trim();
                if (i >= s.Length) continue;

                string sRem = s.Substring(i + 1).Trim();
                int iLoc = ProjectStats.GetNameIndex(sName);
                if (iLoc < 0)
                {
                    MessageBox.Show("Unknown project found: ", sName);
                    return n;
                }
                ProjectStats.ProjectList[iLoc].Hosts.Clear();
                ProjectStats.ProjectList[iLoc].HostNames.Clear();

                string[] sPairHosts = sRem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (i = 0; i < sPairHosts.Length; i++)
                {
                    string[] sPair = sPairHosts[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sPair.Length != 2)
                    {
                        MessageBox.Show("Expected format of (hostname hostid)");
                        return n;
                    }
                    string sPC = sPair[0].Trim();
                    if (!cbComputerList.Items.Contains(sPC))
                        cbComputerList.Items.Add(sPC);
                    ProjectStats.ProjectList[iLoc].HostNames.Add(sPC);
                    ProjectStats.ProjectList[iLoc].Hosts.Add(sPair[1].Trim());
                    n++;
                }                
            }
            if(n>0)
            {
                gbAllowSeq.Enabled = true;
                btnSaveIDs.Enabled = true;
                btnSetAll.Enabled = true;
                btnClearAll.Enabled = true;
                cbSelProj.SelectedIndex = 0;
            }
            return n;
        }

        private int ReadHostsSets()
        {
            MyList.Clear();
            ReadStringsIntoList(ref MyList);
            return FormHostList();
        }

        private void ReadAllList()
        {            
            string sOut = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Open Text File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if(line.Contains("</domain_name>"))
                                {
                                    line = line.Replace("<domain_name>", Environment.NewLine);
                                    line = line.Replace("</domain_name>", ",");
                                    sOut += line;
                                }
                                if(line.Contains("</master_url>"))
                                {
                                    line = line.Replace("<master_url>", "");
                                    line = line.Replace("</master_url>", "; ");
                                    sOut += line;
                                }
                                if(line.Contains("</hostid>"))
                                {
                                    line = line.Replace("<hostid>", " ");
                                    line = line.Replace("</hostid>", " ");
                                    sOut += line;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            string[] lines = sOut.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < ProjectStats.ProjectList.Count(); i++)
            {
                ProjectStats.ProjectList[i].Hosts.Clear();
                ProjectStats.ProjectList[i].HostNames.Clear();
            }

            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    string computerid = parts[0].Trim();
                    for (int i = 1; i < parts.Length-1; i += 2)
                    {
                        string projectName = parts[i].Trim().ToLower();
                        string hostIDs = parts[i + 1].Trim();
                        int iLoc = ProjectStats.GetNameIndex(projectName);
                        if (iLoc < 0)
                        {
                            MessageBox.Show("Project name not found: " + projectName);
                            continue;
                        }
                        ProjectStats.ProjectList[iLoc].Hosts.Add(hostIDs);
                        ProjectStats.ProjectList[iLoc].HostNames.Add(computerid);
                    }                    
                }
            }   
        }

        private void btFetchID_Click(object sender, EventArgs e)
        {
            int n = ReadHostsSets();
            if (n > 0)
            {
                gbAllowSeq.Enabled = true;
                btnSaveIDs.Enabled = true;
                btnSetAll.Enabled = true;
                btnClearAll.Enabled = true;
                cbSelProj.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No valid project found");
                return;
            }   

        }
        private void btnSaveIDs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save List As";
                int iLoc = cbSelProj.SelectedIndex;
                cPSlist cPS = ProjectStats.ProjectList[iLoc];

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    cPS.Hosts.Clear();
                    cPS.HostNames.Clear();
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (dgv.Rows[i].Cells[0].Value == null) continue;
                        if (dgv.Rows[i].Cells[1].Value == null) continue;
                        cPS.Hosts.Add(dgv.Rows[i].Cells[1].Value.ToString());
                        cPS.HostNames.Add(dgv.Rows[i].Cells[0].Value.ToString());
                    }
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            string sOut;
                            foreach(cPSlist cP in ProjectStats.ProjectList)
                            {
                                sOut = cP.name + ": ";
                                for(int i = 0; i < cP.Hosts.Count; i++)
                                {
                                    sOut += cP.HostNames[i] + " " + cP.Hosts[i] + ",";
                                }
                                sOut = sOut.TrimEnd(',');
                                writer.WriteLine(sOut);
                            }   
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnLoadDefIDs_Click(object sender, EventArgs e)
        {

        }
        private void btnSaveDefIDs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save List As";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            foreach (string sOut in defaultNameHost)
                            {
                                writer.WriteLine(sOut);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void FillInDGV( int iLoc)
        {
            dgv.Rows.Clear();
            cPSlist cp = ProjectStats.ProjectList[iLoc];

            for (int i = 0; i < cp.Hosts.Count; i++)
            {
                dgv.Rows.Add();
                string s = cp.HostNames[i].ToString();
                string t = cp.Hosts[i].ToString();
                dgv.Rows[i].Cells[0].Value = s;
                dgv.Rows[i].Cells[1].Value = t;
            }
            if (cbComputerList.Items.Count == 0) return;
            cbComputerList.SelectedIndex = 0;            
        }

        private void btnCreateIDs_Click(object sender, EventArgs e)
        {
            ReadAllList();
            FillInDGV(0);
        }

        private void cbSelProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillInDGV(cbSelProj.SelectedIndex);
        }

        private void btnApplyNewPC_Click(object sender, EventArgs e)
        {
            string sPC = cbComputerList.SelectedItem.ToString();
            ProjectStats.LocalHosts.Clear();
            foreach (cPSlist cP in ProjectStats.ProjectList)
            {
                int i = cP.HostNames.IndexOf(sPC);
                if (i >= 0)
                {
                    cLHe c = new cLHe();
                    c.HostID = cP.Hosts[i];
                    c.name = cP.name;
                    c.ComputerID = sPC;
                    ProjectStats.LocalHosts.Add(c);
                }
            }
            if (ProjectStats.LocalHosts.Count > 0)
            {
                tbProjHostList.Clear();
                foreach (cLHe c in ProjectStats.LocalHosts)
                {
                    tbProjHostList.AppendText(c.name + " " + c.HostID + Environment.NewLine);
                }
                MyComputerID = sPC;
                UpdateRB();
                ProjUrl.Clear();
                lbViewRaw.Show();
                tabPage1.Show();
            }
            else
            {
                MessageBox.Show("No valid project found");
            }
        }

        private void btnRunCmp_Click(object sender, EventArgs e)
        {
            lbURLtoSequence.Items.Clear();
            lbViewRawH.Items.Clear();
            SelectedProject = cbSelProj.Text;
            int n = 0;
            foreach ( DataGridViewRow r in dgv.SelectedRows)
            {
                if (r.Cells[0].Value == null) continue;
                if (r.Cells[1].Value == null) continue;
                string sPCname = r.Cells[0].Value.ToString();
                tbPage.Text = "0";
                tbHdrInfo.Clear();
                tbInfo.Clear();
                lbViewRawH.Items.Add(r.Cells[0].Value.ToString());
                tbHOSTID.Text = r.Cells[1].Value.ToString();
                ApplyName();
                lbURLtoSequence.Items.Add(ProjUrl.Text);
                n++;
            }
            if (n > 0)
            {
                tcProj.SelectedTab = lbViewRaw;
                lbViewRawH.Items.Add("Totals only");
                lbXX.Text = ProjectStats.GetStudy(ProjUrl.Text);
                bool b = lbXX.Text.ToString().Count() > 0;
                btnApplyAPxx.Enabled = b;
                gbXX.Enabled = b;
                if(!tcProj.TabPages.Contains(lbViewRaw))
                    tcProj.TabPages.Add(hiddenTabPage);                       
            }
            else tcProj.TabPages.Remove(hiddenTabPage);
        }


        private void btnSetAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Selected = true;
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Selected = false;
            }
        }

        
        private void lbURLtoSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbURLtoSequence.SelectedItems == null) return;
            if(lbURLtoSequence.SelectedItems.Count == 0) return;
            string sUrl = lbURLtoSequence.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(sUrl))
            {
                ProjUrl.Text = sUrl;
                btnViewUrl.Enabled = true;
            }
            
        }
        cSequencer ts;
        private class cSequencer
        {
            public List<string>SeqResults = new List<string>();
            public List<string> SeqTotals = new List<string>();
            public int NumPagesToRead;
            public int CurrentPage;
            public int UrlIndex;
            public string sProject;
            public string sHostName;
            
        }
        private int mHover=0;
        private void StartSEQ()
        {
            tbInfo.Clear();
            if(lbURLtoSequence.Items.Count == 0) return;    
            string sUrl = lbURLtoSequence.Items[ts.UrlIndex].ToString(); 
            ts.sHostName = lbViewRawH.Items[ts.UrlIndex].ToString();
            MyComputerID = ts.sHostName;
            InstallUrlNoEvent(sUrl);
            StartRun("SEQ");
        }


        private void btnRunSeq_Click(object sender, EventArgs e)
        {
            ts = new cSequencer()
            {
                SeqResults = new List<string>(),
                SeqTotals = new List<string>(),
                NumPagesToRead = (int)nudPages.Value,
                CurrentPage = 0,
                UrlIndex = 0
            };
            ts.sProject = SelectedProject;
            bInSequencer = true;
            StartSEQ();
        }

        private void btnViewUrl_Click(object sender, EventArgs e)
        {
            Process.Start(ProjUrl.Text.ToString());
        }

        private void SeqFinished()
        {
            tbInfo.Text = string.Join(Environment.NewLine, ts.SeqTotals);
        }

        private void Sequencer(string sCmd)
        {
            switch(sCmd)
            {
                case "NEXT":
                    ts.CurrentPage++;
                    if (ts.CurrentPage >= ts.NumPagesToRead)
                    {
                        ts.SeqResults.Add(SequencerOut);
                        SequencerOut = "";
                        ts.SeqTotals.Add(sTotals + Environment.NewLine);
                        ts.UrlIndex++;
                        if (ts.UrlIndex >= lbURLtoSequence.Items.Count)
                        {
                            RunCancel(false);   // did all required
                            SeqFinished();
                            return;
                        }
                        ts.CurrentPage = 0;
                        StartSEQ();
                        break;
                    }
                    else SequencerOut = "";
                    RunNext();
                    break;
                case "VOID":
                    SequencerOut = "No valid " + ts.sProject + " records found for " + ts.sHostName;
                    sTotals = SequencerOut;
                    Sequencer("NEXT");
                    break;
                case "ERROR":;
                    SequencerOut = "Error in " + ts.sProject + " problem with host " + ts.sHostName;
                    sTotals = SequencerOut;
                    RunCancel(true);
                    SeqFinished();
                    break;
            }
        }

        private void lbURLtoSequence_DoubleClick(object sender, EventArgs e)
        {
            
            MouseEventArgs me = (MouseEventArgs)e;
            int index = lbURLtoSequence.IndexFromPoint(me.Location);
            if (index != ListBox.NoMatches)
            {
                string selectedUrl = lbURLtoSequence.Items[index].ToString();
                if (!string.IsNullOrEmpty(selectedUrl))
                {
                    Process.Start(selectedUrl);
                }
            }
        }

        private void lbURLtoSequence_MouseClick(object sender, MouseEventArgs e)
        {
            int index = lbURLtoSequence.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                lbNameHost.Text = lbViewRawH.Items[index].ToString();
            }            
        }

        private void lbViewRawH_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = lbViewRawH.SelectedIndex;
            if(n < 0) return;
            if (ts == null) return;
            if (ts.SeqResults == null) return;
            if (lbViewRawH == null) return;            
            if (ts.SeqResults.Count == 0 || lbViewRawH.Items.Count==0) return;
            if(n >= lbViewRawH.Items.Count) return;

            if (n == lbViewRawH.Items.Count-1)
            {
                tbInfo.Text = string.Join(Environment.NewLine, ts.SeqTotals);
            }
            else
            {
                tbInfo.Text = ts.SeqResults[n];
            }
        }

        private void btnApplyAPxx_Click(object sender, EventArgs e)
        {
            string t = lbXX.Text.ToString().Replace("XX", tbXX.Text.Trim());
            string s="";
            string sOld, sNew;

            for (int j = 0; j < lbURLtoSequence.Items.Count; j++)
            {
                sNew = t;
                s = lbURLtoSequence.Items[j].ToString();
                int i = s.IndexOf("/tasks/4/");
                if (i >= 0)
                {
                    int k = FirstNonInteger(s, i + 9);
                    Debug.Assert(k > 0);
                    sOld = s.Substring(i, k - i);
                }
                else
                {
                    i = s.IndexOf("?appid=");
                    if ((i > 0))
                    {
                        int k = FirstNonInteger(s, i + 7);
                        sOld = s.Substring(i, k - i);
                    }
                    else
                    {
                        i = s.IndexOf("&offset=");
                        Debug.Assert(i > 0);
                        sOld = "&offset=";
                        sNew = sNew + "&offset=";
                        return;
                    }
                }
                string u = s.Replace(sOld, sNew);
                lbURLtoSequence.Items[j] = u;
            }
        }

        private void lbURLtoSequence_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(lbURLtoSequence, lbViewRawH.Items[mHover].ToString());
        }

        private void lbURLtoSequence_MouseMove(object sender, MouseEventArgs e)
        {
            int index = lbURLtoSequence.IndexFromPoint(e.Location);

            if (index >= 0)
            {
                mHover = index;
                toolTip1.SetToolTip(lbURLtoSequence, lbViewRawH.Items[mHover].ToString());
            }
        }
    }
}
