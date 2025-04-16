using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals.Utils;

namespace CreditStatistics
{
    public partial class config : Form
    {
        private cProjectStats ProjectStats;
        string crlf = Environment.NewLine;
        public config(ref cProjectStats rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            HaveStringsData();
            ShowStudy();
            ShowClients(4);
            WorkingFolderLoc.Text = ProjectStats.WhereEXE;
            BoincTaskFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\eFMer\\BoincTasks"; ;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Space Sciences Laboratory, U.C. Berkeley\BOINC Setup");
            List<string> StudyList = new List<string>();
            if (key != null)
            {
                tbWhereBoinc.Text = key.GetValue("DATADIR")?.ToString();
                key.Close();
            }
            rtbLocalHostsBT.Clear();
            btnRunScheduler.Enabled = (ProjectStats.LocalHostList != null);
            if (ProjectStats.LocalHostList != null)
            {
                AppendColoredText(rtbLocalHostsBT, "Local PCs accessed using 31416" + crlf, Color.Blue);
                foreach (string s in ProjectStats.LocalHostList)
                {
                    AppendColoredText(rtbLocalHostsBT, s + crlf, Color.Blue);
                }
            }
        }


        public bool WantsMoreIDs { get; set; }
        public bool WantsReadBTxml { get; set; }
        public void SetTab(int iWhichTab)
        {
            TabCA.SelectTab(iWhichTab);
        }

        private bool bShowHostAvailable;
        private bool bShowAppsAvailable;

        private void HaveStringsData()
        {
            if (Properties.Settings.Default.HostList != null)
                bShowHostAvailable = Properties.Settings.Default.HostList.Count() > 0;
            if(Properties.Settings.Default.AppList != null)
                bShowAppsAvailable = Properties.Settings.Default.AppList.Count() > 0;
        }


        private void AppendColoredText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
        private void ShowClients(int nCols)
        {
            int iPJ = 0;
            int iPID = 0;
            string t = "";
            string r = Environment.NewLine;
            int iLoc = -1;
            rtfClient.Clear();
            string[] ShortNames = new string[ProjectStats.ProjectList.Count];
            foreach (cPSlist ps in ProjectStats.ProjectList)
            {
                iLoc++;
                string[] sS = ps.name.Split(' ');
                string sProjName = sS[0];
                iPJ = Math.Max(iPJ, sProjName.Length);
                ShortNames[iLoc] = sProjName;
                foreach (string sID in ps.HostNames)
                {
                    iPID = Math.Max(iPID, sID.Length);
                }
            }
            iLoc = -1;
            foreach (cPSlist ps in ProjectStats.ProjectList)
            {
                iLoc++;
                if (iLoc == 0) t = "";
                else t = Environment.NewLine + Environment.NewLine;
                AppendColoredText(rtfClient, t + Rp(ShortNames[iLoc], iPJ) + ": ", Color.Red);
                int iCR = nCols;
                t = "";
                foreach (string sID in ps.HostNames)
                {
                    iCR--;
                    string sR = (iCR == 0) ? r : "";
                    AppendColoredText(rtfClient, t+Rp(sID,iPID) + "  " + sR, Color.Blue);
                    if (iCR == 0)
                    {
                        iCR = nCols;
                        t = Lp(" ", iPJ + 2);
                    }
                    else t = "";
                }
            }
        }
        private void ShowStudy()
        {
            if (bShowAppsAvailable)
            {
                string sOut = "";
                string sOne = "";
                int i = 0;
                int nWidth = 0;
                foreach (cPSlist p in ProjectStats.ProjectList)
                {
                    if (p.AppID.Count > 0)
                    {
                        nWidth = Math.Max(nWidth, ProjectStats.ShortName(i).Length);
                    }
                    i++;
                }
                i = 0;
                nWidth += 7;
                foreach (cPSlist p in ProjectStats.ProjectList)
                {
                    if (p.AppID.Count > 0)
                    {
                        sOne = Rp(ProjectStats.ShortName(i) + " apps: ", nWidth);
                        foreach (string s in p.AppID)
                        {
                            sOne += s + " ";
                        }
                        sOne += Environment.NewLine;
                        sOut += sOne;
                    }
                    i++;
                }
                tbApp.Text = "\r\nApplication codes for various projects\r\n\r\n" + sOut;

            }
        }

        private void btnGetMoreIDs_Click(object sender, EventArgs e)
        {
            if (ProjectStats.GetProjectAPPIDs() > 0)
            {
                bShowAppsAvailable = true;
                ShowStudy();
                HaveStringsData();
            }
        }

        private void btnSaveAppIDs_Click(object sender, EventArgs e)
        {
            HaveStringsData();
            if(bShowAppsAvailable)
            {
                string[] stemp = Properties.Settings.Default.AppList;
                WriteStrings(ref stemp, "save your ApplicationIDs");
            }
        }

        private void btnLoadAppIDs_Click(object sender, EventArgs e)
        {
            string[] stemp = null;
            if(ReadStrings(ref stemp, "read your ApplicationsIDs text file"))
            {
                if (stemp.Length > 0)
                {
                    ProjectStats.ParseAppsStrings(ref stemp);
                    Properties.Settings.Default.AppList = stemp;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void btnLoadClient_Click(object sender, EventArgs e)
        {
            string[] stemp = null;
            if (ReadStrings(ref stemp, "read your project names and IDs text file"))
            {
                if (stemp.Length > 0)
                {
                    List<string> Proj_PC_ID = new List<string>();
                    Proj_PC_ID.AddRange(stemp);
                    FormHostList(ref Proj_PC_ID, ref ProjectStats);
                    rtfClient.Text = GetKnownSystems(ref ProjectStats, 2);
                    ShowClients(4);
                    Properties.Settings.Default.HostList = stemp;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void btnSaveClient_Click(object sender, EventArgs e)
        {
            HaveStringsData();
            if (bShowHostAvailable)
            {
                string[] stemp = Properties.Settings.Default.HostList;
                WriteStrings(ref stemp, "save your project names and IDs");
            }
        }


        private bool bInScheduler = false;
        private int iCurrentHOST = 0;
        private int iHostCount = 0;
        private HostRPC MYrpc = new HostRPC();
        private void btnScanClients()
        {
            bInScheduler = true;
            if (ProjectStats.LocalHostList == null)
            {
                return;
            }
            iHostCount = ProjectStats.LocalHostList.Count();
            bool bWorked = false;
            if (iHostCount > 0)
            {
                iCurrentHOST = 0;
                MYrpc.InitScheduler();
                SchTimer.Start();
                bWorked = StartScheduler();
            }
        }

        private bool StartScheduler()
        {
            int n = MYrpc.ReadStream(ProjectStats.LocalHostList[iCurrentHOST].ToString());
            return n > 0;
        }
        private bool ScheduleNext()
        {
            iCurrentHOST++;
            bool bWorked = false;
            if (iCurrentHOST == iHostCount)
            {
                SchTimer.Stop();
                pbTask.Value = 0;
                if (ProjectStats.GetHosts(MYrpc.GetSchedulerResults().ToLower()))
                {
                    if(GetHostsSet(ref ProjectStats))
                    {
                        ShowClients(4);
                    }
                    else
                    {
                        MessageBox.Show("Error reading host list");
                    }

                }
                else
                {
                    int n = Properties.Settings.Default.HostList.Length;
                    if (n == 0)
                        MessageBox.Show("No hosts found or all hosts required passwords" + Environment.NewLine +
                        "You must open a default list");
                    else
                    {
                        MessageBox.Show("Using previous host list");
                    }
                }

            }
            else
            {
                bWorked = StartScheduler();
            }
            return bWorked;
        }



        private void btnRunScheduler_Click(object sender, EventArgs e)
        {
            btnScanClients();
        }

        private void btnFindBTPCs_Click(object sender, EventArgs e)
        {

        }

        private void btnSavePClist_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.RemoteHosts = ProjectStats.LocalHostList;
            Properties.Settings.Default.Save();
        }


        private void btnReadBoinc_Click(object sender, EventArgs e)
        {
            string[] TempHostList = ReadXmlList(BoincTaskFolder.Text);
            if (TempHostList == null) return;
            ProjectStats.LocalHostList = TempHostList;
            btnRunScheduler.Enabled = ProjectStats.LocalHostList.Count() > 0;
        }

        private string[] ReadXmlList(string FolderPath)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                openFileDialog.Title = "Open computers.xml";
                openFileDialog.InitialDirectory = FolderPath;


                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = openFileDialog.FileName;
                    return GetComputerXML(FilePath);
                }
            }
            return null;
        }

        private string[] GetComputerXML(string FilePath)
        {
            List<string> PCnames = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string line;
                    string r = Environment.NewLine;
                    string s = "List of PCs handled by BoincTasks" + r;
                    AppendColoredText(rtbLocalHostsBT, s, Color.Blue);
                    while ((line = reader.ReadLine()) != null)
                    {
                        int i = line.IndexOf("<ip>");
                        if (i < 0) continue;
                        int j = line.IndexOf("</ip>", i);
                        Debug.Assert(j > 0);
                        s = line.Substring(i + 4, j - i - 4);
                        if (s == "localhost")
                            s = Dns.GetHostName();
                        if (IsPortOpen(s))
                        {
                            PCnames.Add(s);
                            AppendColoredText(rtbLocalHostsBT, s+r, Color.Blue);
                        }
                        else
                        {
                            AppendColoredText(rtbLocalHostsBT, s+r, Color.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading file: " + ex.Message);
            }
            return PCnames.ToArray();
        }

        private void btnUseDemo_Click(object sender, EventArgs e)
        {
            UseDemoData();
        }

        private void SchTimer_Tick(object sender, EventArgs e)
        {
            pbTask.Value++;
            if (MYrpc.InScheduler())
            {
                if (pbTask.Value >= pbTask.Maximum)
                {
                    SchTimer.Stop();
                    MYrpc.StopScheduler();
                    pbTask.Value = 0;
                    return;
                }
                if (MYrpc.SchedulerDone())
                {
                    ScheduleNext();
                }
                return;
            }
        }
    }
}
