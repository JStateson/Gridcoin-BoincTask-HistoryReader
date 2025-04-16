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
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Xml;
using System.Xml.Linq;
using static CreditStatistics.cProjectStats;
using static CreditStatistics.CreditStatistics;
using static System.Windows.Forms.LinkLabel;
using static CreditStatistics.globals.Utils;




namespace CreditStatistics
{
    // debugger cmd line: C:\Users\josep\source\repos\Gridcoin-BoincTask-HistoryReader\CreditStatistics\ClientList_in.txt
    // debugger cmd line: reset

    // as generated using mod'ed boinccmd.exe, systems.txt and script: GetHostIDs.cmd

    public partial class CreditStatistics : Form
    {
        private string SequencerOut = "";   // used when running sequencer instead of output to text box
        private bool bInSequencer = false;

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
        private TabPage tTabP; // Page0;
        private TabPage tTabT; // Page1;
        private TabPage tTabH; // Page2;  
        private TabPage tTabS; // Page3;
   

         

        private List<string>defaultNameHost = new List<string>();
        private string WorkingFolder = "";
        private string WhereEXE = "";
        bool bHaveHostInfo = false; // have hostname, project names and host ID for project
        private HostRPC MYrpc = new HostRPC();
        private string SelectedDemo = "";
        public CreditStatistics(string[] args)
        {
            InitializeComponent();
            WhereEXE = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            ProjectStats.WhereEXE = WhereEXE;
            lbVersion.Text = "Build Date:" + GetSimpleDate(Properties.Resources.BuildDate);
            MyComputerID = Dns.GetHostName().ToLower();
            tTabP = tcProj.TabPages["TabP"];
            tTabT = tcProj.TabPages["TabT"];
            tTabH = tcProj.TabPages["TabH"];
            tTabS = tcProj.TabPages["TabS"];

            ProjectStats.Init();

            if (args.Length > 0 )
            {
                string s = args[0].ToLower();
                if (s == "reset")// && false)
                {
                    Properties.Settings.Default.Reset();
                    Properties.Settings.Default.Save();
                }
                else if(s=="/?" || s=="-h")
                {
                    string t = 
                        "CreditStatistics reset             : remove all client data\r\n" +
                        "CreditStatistics ClientList_in.txt : get client information from file";

                    MessageBox.Show(t);   
                }
                else 
                    bHaveHostInfo = ProjectStats.GetHostsFile(args[0]);
                // produces: Properties.Settings.Default.HostList  FROM 
                // hostname or pc: projname1,id1 projname2,i2  etc using space delim between pairs
                // giving projname1 : pc,pcid  pc,pcid
            }
            if(Properties.Settings.Default.AskForDemo)
            {
                Properties.Settings.Default.AskForDemo = false;
                Properties.Settings.Default.AskForStudy = false;
                Properties.Settings.Default.AskForBoinctasks = false;
                Properties.Settings.Default.Save();
                DialogResult Res = MessageBox.Show("Start in demo mode: Select Yes", "Can be changed in the config menu", MessageBoxButtons.YesNo);
                if (Res == DialogResult.Yes)
                {
                    UseDemoData();
                    GetSavedAppStudy(ref ProjectStats);
                }
            }
            AskToGetAppList(ref ProjectStats);
            bool bWantsRunScheduler = AskToGetClientList(MyComputerID);
            HaveBThostlist();
            if (TryGetHostSets())
            {
                bHaveHostInfo = true;
                ProjectStats.SelectComputer(MyComputerID);
            }

            FormProjectRB();
            btnRestoreID.Enabled = bHaveHostInfo;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Shown += InitialLoad;            
        }

        private void InitialLoad(object sender, EventArgs e)
        {
            //Refresh();
            //Task.Delay(1);
            //Application.DoEvents();
 
            tcProj.TabPages.Remove(tTabS);
            if(bHaveHostInfo)
                cbSelProj.SelectedIndex = 0;
            ParseProjUrl();
            btnRestoreID.Tag = MyComputerID;
            lbPCname.Text = "PC name: " + MyComputerID;
            GetSavedAppStudy(ref ProjectStats);
        }

        private void FormProjectRB()
        {
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
         
        private void RBchanged(bool bIsLocal)
        {
            SelectedProject = ProjectStats.ProjectList[TagOfProject].name;
            tbPage.Text = "0";
            tbHdrInfo.Clear();
            tbInfo.Clear();

            if (ProjectStats.LocalHosts.Count > 0)
            {
                if(bIsLocal)tbHOSTID.Text = ProjectStats.GetIDfromName(SelectedProject);
                ApplyName();
            }
        }

        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender; 
            if(rb.Checked)
                TagOfProject = (int)rb.Tag;
            RBchanged(bIsLocal);
        }
        bool bIsLocal = true;
        private void SetRB(int iTag, bool b)
        {
            bIsLocal = b;
            TagOfProject = iTag;
            foreach (Control c in gbSamURL.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if ((int)rb.Tag == iTag)
                    {
                        //if(b)rb.CheckedChanged -= new System.EventHandler(this.rbProject_CheckedChanged);
                        rb.Checked = true;
                        //if(b)rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                        return;
                    }
                }
            }
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            Process.Start(ProjUrl.Text.ToString());
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
            string sOutInfo = "";
            cCreditInfo SumC = new cCreditInfo();
            double dH = 0;  // hours
            long Sd = 0;
            long Ed = 0;  
            SumC.Init();
            if(!bInSequencer) tbInfo.Clear();
            ApplyFilter();
            string sOutHdrs = "Num" + Rp("     Date Completed", 22) + "    Credit     RunTime     RunTime     CPUtime     CPUtime  Valid" + Environment.NewLine;
            sOutHdrs += "   " + Rp(" ", 22) + "    Points        Secs     Credits        Secs     Credits";
            lbHdr.Text = sOutHdrs;
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
                SequencerOut += sOutHdrs + Environment.NewLine + sOutInfo + sTotals;
            }
            else
            {
                tbInfo.Text = sOutHdrs + Environment.NewLine + sOutInfo + sTotals;
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
            tbInfo.Text = "";
            RunNext();
        }

        // task start taskstart 
        private async Task ReadOnePage()
        {
            ProjectStats.TaskBusy = true;
            ProjectStats.TaskDone = false;
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


        private void ErrorAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.InitialUrl = ProjUrl.Text;
            Properties.Settings.Default.Save();
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
            string PCname = "";
            tbHdrInfo.Clear();
            if (UrlPassed())
            {
                tbPage.Text = "0";
                tbInfo.Text = "";
                StartRun("HDR");
            }
        }

        private void cbfilterSTD_CheckedChanged(object sender, EventArgs e)
        {
            GetResults();
        }

        private void ProjUrl_TextChanged(object sender, EventArgs e)
        {
            AllowGS(false);
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


        private bool HaveBThostlist() // boinctask host list
        {            
            ProjectStats.LocalHostList = Properties.Settings.Default.RemoteHosts;
            return (!(ProjectStats.LocalHostList == null)) ;
        }



        private void ParseProjUrl()
        {
            string sOut = "";
            string sHost = "";
            int ProjectID = -1;
            string sPage = "";
            
            if (ParseUrl( ref ProjectStats, ProjUrl.Text, ref sOut, ref ProjectID, ref sHost, ref SelectedProject, ref sPage))
            {
                ProjUrl.Text = sOut;
                tbHOSTID.Text = sHost;
                SetRB(ProjectID, true);
                tbPage.Text = sPage;
            }
        }

        private bool UrlPassed()
        {
            int i = 0;
            string s = ProjUrl.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(s))
            {
                i = s.IndexOf("http");
                if (i < 0)
                {
                    MessageBox.Show("badly formed url: missing http");
                    return false;
                }
                i = ProjectStats.GetNameIndex(s);
                if (i < 0)
                {
                    MessageBox.Show("Project not found in url");
                    return false;
                }
                return true;
            }
            return false;
        }
        private void btnExtract_Click(object sender, EventArgs e)
        {
            UrlPassed();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbInfo.Clear();
        }

        private void RunCancel(bool bErr)
        {
            if(cts != null)
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

        private void lbSelectDemo_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string sOut = "";
            int col = 0;
            int w = tbSelected.Width / 10;
            string lOut = "";
            foreach(var item in lbSelectDemo.SelectedItems)
            {
                string s = item.ToString();
                SelectedDemo = s;
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
            btnViewTop.Enabled = true;
            btnRunTop.Enabled = true;
        }

        private void btnClearURL_Click(object sender, EventArgs e)
        {
            ProjUrl.Clear();
        }

        // unknown is the PC name
        private void InstallURL(string sIn, string sUnknown)
        {
            string sOut = "";
            int ProjectID = -1;
            string sHost = "";
            string sName = "";
            string sPage = "";
            ProjUrl.Text = sIn;

            if (ParseUrl(ref ProjectStats, sIn, ref sOut, ref ProjectID, ref sHost, ref sName, ref sPage))
            {
                if(sUnknown != "")
                {
                    ApplyOneProject(sHost, sName, sUnknown);
                    lbPCname.Text = "PC name is unknown";
                }

                ProjUrl.Text = sOut;
                tbHOSTID.Text = sHost;
                TagOfProject = ProjectID;
                tbPage.Text = sPage;
                SetRB(ProjectID,false);
            }
        }


        private bool InstallKnownPCurl(string sUrl, ref string sProjectID)
        {
            bool bFound = GetPCnameFromURL(ref ProjectStats, sUrl, ref sProjectID);
            if (bFound)
                InstallURL(sUrl, "");
            else
                InstallURL(sUrl, sProjectID);
            return bFound;
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            string sProjectID = "";
            string sUrl = Clipboard.GetText().Trim().ToLower();
            InstallKnownPCurl(sUrl, ref sProjectID);  
        }

        // from TaskTimer.Start(); TaskStart
        private void TaskTimer_Tick(object sender, EventArgs e)
        {
            pbTask.Value++;
            string sTemp = "";
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
                            sTemp = "";
                            ProjectStats.NumValid = ProcessHDR(ref ProjectStats.RawPage, ref sTemp);
                            tbHdrInfo.Text += sTemp;
                            if (ProjectStats.NumValid == 0)
                            {
                                tbInfo.Text = "No valid records found for " + MyComputerID + " and project " +
                                   ProjectStats.ShortName(TagOfProject);
                                return;
                            }
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
                            sTemp = "";
                            ProjectStats.NumValid = ProcessHDR(ref ProjectStats.RawPage, ref sTemp);
                            tbHdrInfo.Text += sTemp;
                            if (ProjectStats.NumValid > 0)
                            {
                                RecordsPerPage = ProcessBody();
                                ProjectStats.sTaskType = "SEQBODY";
                                Sequencer((RecordsPerPage == 0) ? "VOID" : "NEXT");
                                return;
                            }
                        }
                        Sequencer("VOID");
                        break;
                }
            }            
        }


        private void btnSaveAS_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save List As";
                saveFileDialog.InitialDirectory = WhereEXE;

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

        private void btnLoadFrom_Click(object sender, EventArgs e)
        {
            List<string> MyList = new List<string>();
            ReadStringsIntoList(ref MyList);
            lbSelectDemo.Items.Clear();
            foreach (string s in MyList)
            {
                string sOut = "";
                string sHost = "";
                int ProjectID = -1;
                string sName = "";
                string sPage = "";
                if (ParseUrl(ref ProjectStats, s.Trim(), ref sOut, ref ProjectID, ref sHost, ref sName, ref sPage))
                    lbSelectDemo.Items.Add(sOut);
                else break;                
            }   
        }



        private bool TryGetHostSets()
        {
            bool b = GetHostsSet(ref ProjectStats);
            if(b)
            {
                cbComputerList.DataSource = ProjectStats.ComputerList.ToArray();
                gbAllowSeq.Enabled = true;
                btnSaveIDs.Enabled = true;
                btnSetAll.Enabled = true;
                btnClearAll.Enabled = true;
            }
            return b;
        }



        /*
         * used to read a temp file that might still be useful
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
        */

        private void SaveDemoList()
        {
            string r = Environment.NewLine;
            foreach (cPSlist ps in ProjectStats.ProjectList)
            {

            }
        }

        private bool GetDemoList()
        {

            int iLoc = ReadHostsSets(ref ProjectStats);
            if (iLoc >= 0)
            {
                gbAllowSeq.Enabled = true;
                btnSaveIDs.Enabled = true;
                btnSetAll.Enabled = true;
                btnClearAll.Enabled = true;
                FillInDGV(iLoc);
            }
            else
            {
                MessageBox.Show("No valid project found");
            }
            return iLoc >= 0;
        }

        private void btnSaveIDs_Click(object sender, EventArgs e)
        {            
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save List As";
                saveFileDialog.InitialDirectory = WhereEXE;
                
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
            string sOut = "";
            foreach(string s in defaultNameHost) { sOut += s; }
            if (sOut.Length == 0) return;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save default IDs s";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, sOut);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


 
        private void FillInDGV(int iLoc)
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
            cbKnownIDs.Items.Clear();
            foreach(string s in cp.AppID)
                cbKnownIDs.Items.Add(s);
            SetKnownID(0);
        }

        private void EnableStudySelect(bool b)
        {
            gbXX.Enabled = b;
        }
        private void SetKnownID(int iD)
        {
            if(cbKnownIDs.Items.Count == 0)
            {
                EnableStudySelect(false);
                return;
            }
            EnableStudySelect(true);
            cbKnownIDs.SelectedIndex = iD;
            string s = cbKnownIDs.Text;
            tbXX.Text = s;
        }
        private void btnCreateIDs_Click(object sender, EventArgs e)
        {
        }

        private void cbSelProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillInDGV(cbSelProj.SelectedIndex);
        }

        private void ApplyOneProject(string HostID, string ProjName, string PCName)
        {
            ProjectStats.LocalHosts.Clear();
            int i = ProjectStats.GetNameIndex(ProjName);
            if (!ProjectStats.DoesPCexist(PCName))
            {
                cLHe c = new cLHe();
                c.ProjectsHostID = HostID;
                c.ProjectName = ProjName;
                c.ComputerID = PCName;
                c.IndexToProjectList = i;
                ProjectStats.LocalHosts.Add(c);
            }
            if (ProjectStats.LocalHosts.Count > 0)
            {
                // jys use to show contents of ProjectStates.LocalHosts here
                //foreach (cLHe c in ProjectStats.LocalHosts)
                MyComputerID = PCName;
                UpdateRB();
                ProjUrl.Clear();
                TabS.Show();
                tcProj.SelectTab("TabP");
            }
        }

        private void RestoreLocalList(string sPC)
        {
            ProjectStats.LocalHosts.Clear();
            foreach (cPSlist cP in ProjectStats.ProjectList)
            {
                int i = cP.HostNames.IndexOf(sPC);
                if (i >= 0)
                {
                    cLHe c = new cLHe();
                    c.ProjectsHostID = cP.Hosts[i];     //id used by project for PC
                    c.ProjectName = cP.name;            //name of project
                    c.ComputerID = sPC;                 //name of the pc (not known unless local
                    int nID = ProjectStats.GetNameIndex(cP.name);
                    c.IndexToProjectList = nID;
                    ProjectStats.LocalHosts.Add(c);
                }
            }
            if (ProjectStats.LocalHosts.Count > 0)
            {
                MyComputerID = sPC;
                UpdateRB();
                ProjUrl.Clear();
                TabS.Show();
                tcProj.SelectTab("TabP");
                int nID = ProjectStats.LocalHosts[0].IndexToProjectList;
                SetRB(nID, true);
                lbPCname.Text = "PC name: " + sPC;
            }
            else
            {
                MessageBox.Show("No valid project found");
            }
        }
        private void btnApplyNewPC_Click(object sender, EventArgs e)
        {
            string sPC = cbComputerList.SelectedItem.ToString();
            RestoreLocalList(sPC);
        }

        private List<string>SelectedIDs = new List<string>();
        private void btnRunCmp_Click(object sender, EventArgs e)
        {
            lbURLtoSequence.Items.Clear();
            lbViewRawH.Items.Clear();
            SelectedProject = cbSelProj.Text;
            int n = 0;
            SelectedIDs.Clear();
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
                SelectedIDs.Add(tbHOSTID.Text);
                n++;
            }
            if (n > 0)
            {
                tcProj.SelectedTab = TabS;
                lbViewRawH.Items.Add("Totals only");
                lbXX.Text = ProjectStats.GetStudy(ProjUrl.Text);
                bool b = lbXX.Text.ToString().Count() > 0;
                btnApplyAPxx.Enabled = b;
                gbXX.Enabled = b;
                if(!tcProj.TabPages.Contains(TabS))
                    tcProj.TabPages.Add(tTabS);                       
            }
            else tcProj.TabPages.Remove(tTabS);
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
        public class cEachPc
        {
            public int nValidWUs;
            public int nRemainWUs;
            public bool OutOfData;
        }
        private class cSequencer
        {
            public List<string> SeqResults;
            public List<string> SeqTotals;
            public List<cEachPc> EachPCsHDR = new List<cEachPc>();
            public int NumPagesToRead;
            public int CurrentPage;
            public int UrlIndex;
            public int NumUrls;
            public string sProject;
            public string sHostName;
            public int nLongestName;
            public void Init()
            {
                EachPCsHDR.Clear();
                nLongestName = 0;
                for(int i = 0; i < NumUrls; i++)
                {
                    cEachPc cEP = new cEachPc();
                    cEP.nValidWUs = 0;
                    cEP.OutOfData = false;
                    EachPCsHDR.Add(cEP);
                }
            }
            public bool OutOfData()
            {
                return EachPCsHDR[UrlIndex].OutOfData;
            }
            public void SetHDR(int NumValid)
            {
                EachPCsHDR[UrlIndex].nValidWUs = NumValid;
                EachPCsHDR[UrlIndex].nRemainWUs = NumValid;
            }
            public void SetBODY(int NumFetched)
            {
                EachPCsHDR[UrlIndex].nRemainWUs-= NumFetched;
                EachPCsHDR[UrlIndex].OutOfData = (EachPCsHDR[UrlIndex].nRemainWUs <= 0);
            }
        }
        private int mHover=0;
        private void StartSEQ()
        {
            if(lbURLtoSequence.Items.Count == 0) return;    
            string sUrl = lbURLtoSequence.Items[ts.UrlIndex].ToString(); 
            ts.sHostName = lbViewRawH.Items[ts.UrlIndex].ToString();
            ts.nLongestName = Math.Max(ts.nLongestName, ts.sHostName.Length);
            SequenceChanged(ts.UrlIndex);
            MyComputerID = ts.sHostName;
            tbHOSTID.Text = SelectedIDs[ts.UrlIndex].ToString();
            InstallURL(sUrl,"");
            StartRun("SEQ");
        }

        private string FormValidTotals()
        {
            string sOut = Environment.NewLine + Rp("Computer", ts.nLongestName) + "Valid WUs" + Environment.NewLine;
            for (int i = 0; i < ts.NumUrls; i++)
            {
                string sTemp = Rp(lbViewRawH.Items[i].ToString(),ts.nLongestName + 4);
                sOut += sTemp + ts.EachPCsHDR[i].nValidWUs.ToString() + Environment.NewLine;                
            }
            return sOut;
        }

        private void SequenceChanged(int index)
        {
            lbURLtoSequence.SelectedIndex = index;
            lbNameHost.Text = lbViewRawH.Items[index].ToString();
        }

        private void btnRunSeq_Click(object sender, EventArgs e)
        {
            ts = new cSequencer()
            {
                SeqResults = new List<string>(),
                SeqTotals = new List<string>(),
                NumPagesToRead = (int)nudPages.Value,
                CurrentPage = 0,
                UrlIndex = 0,
                NumUrls = lbURLtoSequence.Items.Count
            };
            ts.sProject = SelectedProject;
            bInSequencer = true;
            ts.Init();
            tbInfo.Clear();
            StartSEQ();
        }

        private void btnViewUrl_Click(object sender, EventArgs e)
        {
            Process.Start(ProjUrl.Text.ToString());
        }

        private void SeqFinished()
        {
            tbInfo.Text = string.Join(Environment.NewLine, ts.SeqTotals) + FormValidTotals();
        }

        private void Sequencer(string sCmd)
        {
            switch (sCmd)
            {
                case "NEXT":
                    if (ts.CurrentPage == 0)
                    {
                        ts.SetHDR(ProjectStats.NumValid);
                        tbInfo.Text += ts.sHostName + " number valid WUs: " + ProjectStats.NumValid.ToString() +
                            Environment.NewLine;
                    }
                    ts.SetBODY(RecordsPerPage);
                    ts.CurrentPage++;
                    if (ts.CurrentPage >= ts.NumPagesToRead || ts.OutOfData())
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
                        SequenceChanged(ts.UrlIndex);
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
                SequenceChanged(index);
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
                tbInfo.Text += FormValidTotals();
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
                    }
                }
                string u = s.Replace(sOld, sNew);
                lbURLtoSequence.Items[j] = u;
            }
        }

        private void lbURLtoSequence_MouseEnter(object sender, EventArgs e)
        {
            if (mHover >= lbViewRawH.Items.Count) return;
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



        // the following scheduler is for get getting project IDs 


        private void btnFetchID_Click(object sender, EventArgs e)
        {
            GetDemoList();
        }

        private void btnViewTop_Click(object sender, EventArgs e)
        {
            Process.Start(SelectedDemo);
        }

        private void btnRunTop_Click(object sender, EventArgs e)
        {
            string sProjectID = "";
            InstallKnownPCurl(SelectedDemo, ref sProjectID);
            StartRun("HDR");
            tcProj.SelectTab("TabP");
        }

        private void btnRestoreID_Click(object sender, EventArgs e)
        {
            string sPCID = (string) btnRestoreID.Tag;
            RestoreLocalList(sPCID);
        }

        private void descriptionOfProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://stateson.net/bthistory/CreditStats.html");
        }

        private void GetHelp(object sender, EventArgs e)
        {
            string sHelp = "";
            if (sender is ToolStripMenuItem item)
            {
                string name = item.Text;
                switch(name)
                {
                    case "Get anyones credit":
                        sHelp = "AnyoneCR.rtf";
                        break;
                    case "Get all your credits":
                        sHelp = "AllCR.rtf";
                        break;
                    case "Compare Credits":
                        sHelp = "CompareCR.rtf";
                        break;
                    case "Quick Demo":
                        sHelp = "QuickDemo.rtf";
                        break;
                }
                string FilePath = WhereEXE + "/" + sHelp;
                if(File.Exists(FilePath))
                    Process.Start(FilePath);
            }
        }

        private void cbKnownIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = cbKnownIDs.Text;
            tbXX.Text = s;
        }

        private void tsmConfigure_Click(object sender, EventArgs e)
        {
            config DoConfig = new config(ref ProjectStats);
            DoConfig.ShowDialog();
            DoConfig.Dispose();
        }
    }
}
