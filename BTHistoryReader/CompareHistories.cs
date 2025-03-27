//#define FormProjectNames

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Notepad;
using System.IO;
using System.Text.RegularExpressions;
using System.Management.Instrumentation;



namespace BTHistoryReader
{
    public partial class CompareHistories : Form
    {
        public BTHistory btf;
        public List<string> Projects;
        public List<string> Systems;
        public List<cKPAlocs> KPAlocs;
        public List<string> SystemsCompared;    // this and following 3 are paired
        public List<string> SystemsCompared_nConcurrent; // these pairing need to be better designed: go to the data not make a copy??
        public List<string> SystemsUniqueDev;
        public List<bool> SystemsCompared_bWantStats;
        public List<int> SystemsComparedCount;
        private string strAverageAll = "Average All";
        public List<bool> bItemsToColor = new List<bool>(); // use color to show which apps have values
        private List<cSeriesData> MySeriesData;
        public class cKPAapps
        {
            public string sAppName;
            public int iSystem;
            public int nDev;    // number of unique devices d0, d1 would be 2 devices
            public double cpd;  // credits per day
            public List<double> dLelapsedTime = new List<double>();
            public void AddValue(double d)
            {
                dLelapsedTime.Add(d);
            }
            public int nConcurrent = 1;    // save info to allow restoreing when switching across apps and systems
            public bool bUseThisAppInStatsListBox = true;
        }


        public string Last_sProj="";
        public string Last_sApp="";

        // only visible when not scattering all
        private void ShowAppSelects(bool bVisible)
        {
            btnSelAllApp.Visible = bVisible;
            BtnClrAllApp.Visible = bVisible;
            BtnInvSelApp.Visible = bVisible;
            lblWarnApps.Visible = bVisible;
            LBoxApps.SelectionMode = (bVisible ? SelectionMode.MultiExtended : SelectionMode.One);
            ClearBoxSelections();
        }

        public class cKPAproj
        {
            public string sProjName;
            public List<cKPAapps> KPAapps = new List<cKPAapps>();
            public cKPAapps AddApp(string sAppName, int iSystem, double cpd, int iDev)
            {
                cKPAapps ckpaa = new cKPAapps();
                ckpaa.sAppName = sAppName;
                ckpaa.iSystem = iSystem;
                ckpaa.nDev = iDev;
                ckpaa.cpd = cpd; // credits per day normalize to 1 
                KPAapps.Add(ckpaa);
                return ckpaa;
            }
            
        }

        // at this location, add all projects, and return pointer to project so as to fill in apps
        public class cKPAlocs
        {
            public string sPathToHistory;
            public string strSystem;
            public List<cKPAproj> KPAproj;
            public void AddNewPath(string sPath)
            {
                sPathToHistory = sPath;
                KPAproj = new List<cKPAproj>();
                
            }
            public cKPAproj AddProj(string sProjName)
            {
                cKPAproj ckpap = new cKPAproj();
                ckpap.sProjName = sProjName;
                KPAproj.Add(ckpap);
                return ckpap;
            }
        }

        // since system name might be identical in different files then we must change the system name 
        // as the name is used for the series in the plots and the series names must be unique.
        private string MustBeUniqueName(string strName, ref int n)
        {
            n++;
            foreach(string strTemp in Systems)
            {
                if(strTemp == strName)
                {
                    return MustBeUniqueName(strName + "-" + n.ToString(), ref n);
                }
            }
            return strName;
        }

        //inform the calling program when to incrment the progress bar
        private void EstimateLineCount()
        {
            double TotalBytes = 0;
            double d;
            int n, NumLines= 0;
            foreach(string strHisFile in btf.AllHistories)
            {
                d = new FileInfo(strHisFile).Length;
                TotalBytes += d;
            }
            TotalBytes = 2000.0 * TotalBytes / 500000.0;
            NumLines = Convert.ToInt32(TotalBytes);
            n = NumLines / btf.GetPBARmax();
            btf.SetPBARcnt(n);
        }

        // when comparing projects, say "seti" app can be nvidia, amd or cpu
        // cannot test for full app name else no match unless both systems had same devices
        // we need to access the callers state (the refForm) and we need whether to read in or to ignore files with "_long_" in name
        public CompareHistories(Form refForm)
        {
            InitializeComponent();
            string ThisSystem = "";
            int DuplicateNameCnt = 0;
            int iLoc;
            Projects = new List<string>();
            KPAlocs = new List<cKPAlocs>();
            Systems = new List<string>();;
            SystemsCompared = new List<string>();
            SystemsCompared_nConcurrent = new List<string>();
            SystemsUniqueDev = new List<string>();
            SystemsCompared_bWantStats = new List<bool>();
            SystemsComparedCount = new List<int>();
            int iSystem = -1;
            int NumberProjects = 0;
            int nSystems = -1;// number of computers being compared
            btf = (BTHistory)refForm;
            LBoxApps.Items.Clear();
            LBoxProjects.Items.Clear();
            btf.CurrentSystem = "";
            //ShowAppSelects(false);              // todo need implement multi-select better as systems are list for just one item
                                                // not all those selected in the listbox
            EstimateLineCount();
            foreach (string strHisFile in btf.AllHistories)
            {
                int RtnCod;
                RtnCod = btf.ValidateHistory(strHisFile, ref ThisSystem);
                if (RtnCod < 0) continue;
                DuplicateNameCnt = 0;
                ThisSystem = MustBeUniqueName(ThisSystem, ref DuplicateNameCnt);
                Systems.Add(ThisSystem);
                iSystem = Systems.Count - 1;    // index into name of system
                btf.ClearPreviousHistory();
                nSystems++;
                btf.ProcessHistoryFile(nSystems);
                btf.IncrementPBAR();
                foreach (cKnownProjApps kpa in btf.KnownProjApps)
                {
                    if (Projects.Contains(kpa.ProjName)) continue;
                    if (kpa.NumDValues() == 0) continue;
                    Projects.Add(kpa.ProjName);
                }
                cKPAlocs ckpal = new cKPAlocs();
                KPAlocs.Add(ckpal);
                ckpal.AddNewPath(strHisFile);    // strHisFile does not contain the exact name of system that "history" has
                ckpal.strSystem = ThisSystem;    // this has the system name
                foreach (cKnownProjApps kpa in btf.KnownProjApps)
                {
                    if (kpa.nAppsUsed == 0) continue;
                    cKPAproj ckpap = ckpal.AddProj(kpa.ProjName);
                    foreach (cAppName AppName in kpa.KnownApps)
                    {
                        int nEntries = AppName.dElapsedTime.Count;  // nEntries from AppName is not valid here
                        if (nEntries == 0) continue;
                        NumberProjects++;  
                        AppName.nDevices = AppName.nEntriesThisComputer[nSystems]; 
                        cKPAapps ckpaa = ckpap.AddApp(AppName.Name, iSystem, AppName.CreditPerDay,AppName.nDevices);
                        btf.ThisProjectInfo = new List<cProjectInfo>(nEntries);
                        cProjectInfo cpi = new cProjectInfo();
                        cpi.iSystem = iSystem;
                        //ckpaa.iSystem = iSystem; // duplicated in above call to AddApp
                        iLoc = 0;
                        foreach (double d in AppName.dElapsedTime)
                        {
                            if (AppName.bIsValid[iLoc])
                                ckpaa.AddValue(d / 60.0);
                            iLoc++;
                        }
                        btf.ThisProjectInfo.Add(cpi); // not sure of the purpose of this 2-8-2020 is cpi used??
                    }
                }
            }
            btf.ClearPreviousHistory();
#if FormProjectNames
            string strDebugTemp = "{";
#endif
            foreach (string s in Projects)
            {
                LBoxProjects.Items.Add(s);
#if FormProjectNames
                strDebugTemp += "\"" + s + "\","; 
#endif
            }
#if FormProjectNames
            strDebugTemp += "}";
#endif
        }

        //{"Milkyway@Home","World Community Grid","SETI@home","Rosetta@home","GPUGRID","Einstein@Home","LHC@home","Asteroids@home","NumberFields@home","latinsquares","TN-Grid Platform","collatz","Collatz Conjecture","PrimeGrid","Universe@Home","Bitcoin Utopia"}

        // for the requested project, in all history files, sum up the number of results of any project with that name
        // and put each app name into the ProjApp table along with the sum of all results of that app
        private void ShowAppsThisProj(string sProj)
        {
            List<string> ProjApps = new List<string>();
            List<int> NumApps = new List<int>();
            int i = 0;
            foreach(cKPAlocs ckpal in KPAlocs)                                  // for each history
            {
                foreach(cKPAproj ckpap in ckpal.KPAproj)                        // for each project
                {
                    if(sProj == ckpap.sProjName)
                    {
                        foreach(cKPAapps ckpaa in ckpap.KPAapps)
                        {
                            if (ProjApps.Contains(ckpaa.sAppName))
                            {
                                int iIndex = ProjApps.IndexOf(ckpaa.sAppName);
                                NumApps[iIndex] += ckpaa.dLelapsedTime.Count;
                                continue;
                            }
                            ProjApps.Add(ckpaa.sAppName);
                            NumApps.Add(ckpaa.dLelapsedTime.Count);
                        }
                    }
                }
            }
            LBoxApps.Items.Clear();
            foreach (string s in ProjApps)
            {
                string strCnt = "(" + NumApps[i].ToString() + ") ";
                i++;
                LBoxApps.Items.Add(strCnt + s);
            }
        }

        private void LBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strProjSelected = LBoxProjects.Text;
            TBoxResults.Text = "";
            TBoxStats.Text = "";
            ShowAppsThisProj(strProjSelected);
            LViewConc.Items.Clear();
            if(LBoxApps.Items.Count != 0)
                LBoxApps.SetSelected(0, true);  // FIXME  not sure why or how this fired with 0 items
        }

        // systems have unique names but when pasted together they might lose uniquness so we are surrounding them
        // with a dash.  The checkboxes for which systems to ignore are not always accssible so it is convenient to concatonate. 
        // file requested systems together and then see if a particular project in the the string
        // not used anymore
        private bool bUseThisSystem(string strProjName, string strListOfUsed)
        {
            string strQualifier;
            if(strListOfUsed == strAverageAll)return true;
            strQualifier = "-" + strProjName + "-";
            return strListOfUsed.Contains(strQualifier);
        }

        //if more than on concurrenet task on gpu then the elspsed time was divided by that number.  Remind user of this
        // using the division sign 
        private string strDivide(int n)
        {
            if (n == 1) return "";
            return "\u00F7" + n.ToString();
        }

        // test to see if app name is one of the ones selected
        private bool bIsAppSelected(string sAppName)
        {
            string strName = "";
            bool bGood=false;
            foreach (int i in LBoxApps.SelectedIndices)
            {
                bGood = GetNameFromBox(LBoxApps.Items[i].ToString(), ref strName);
                if (!bGood) return false;
                if (strName == sAppName) return true;
            }
            return false;
        }

        // if bMakeTable is not true then we are recalculating using number of devices from table
        // 7-1-2019  ignore sApp, use either the ones selected or all if none selected  
        private void CalcAllValues(string sProj, string sApp, bool bMakeTable, string strSystems)
        {
            TBoxResults.Text = "";
            TBoxStats.Text = "";
            string sTemp = "";
            string strLoc = "";
            double dAvg=0.0;
            double dRms=0.0;
            double dStd;
            string strSPAstats = "";
            int ncnt;
            List<double> dTemp = new List<double>();
            bool bShowFL = cboxFL.Checked;
            int MaxAllowed = Convert.ToInt32(tbSumCnt.Text);
            bool bShowSummary = false;

            bool bUseAll = LBoxApps.SelectedIndices.Count == 0;
            bool bUseThis;
            bool bCandidate;
            double Totalcpd=0; // credit per day

            Last_sProj = sProj;
            Last_sApp = sApp;
            btnApply.Visible = true;

            if(bMakeTable)
            {
                SystemsCompared.Clear();
                SystemsCompared_nConcurrent.Clear();
                SystemsComparedCount.Clear();
                SystemsUniqueDev.Clear();
                SystemsCompared_bWantStats.Clear();
            }

            foreach (cKPAlocs ckpal in KPAlocs)                                             // for every system
            {
                    // system name is not at strSystem !!!
                foreach (cKPAproj ckpap in ckpal.KPAproj)                                   // for each project in system
                {
                    if (sProj == ckpap.sProjName)                                           // if it is the project we want
                    {                                             // we may want this in results if has any data
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)                           // for each app in the project
                        {
                            bCandidate = bUseAll | bIsAppSelected(ckpaa.sAppName);
                            bUseThis = (sApp == "all") ? bCandidate : ckpaa.sAppName == sApp;
                            if (bUseThis)  
                            {
                                if (ckpaa.dLelapsedTime.Count == 0)                         // must have data
                                    continue;
                                strLoc = Systems[ckpaa.iSystem];
                                if (bMakeTable)
                                {
                                    SystemsCompared.Add(strLoc);
                                    SystemsCompared_nConcurrent.Add(ckpaa.nConcurrent.ToString());
                                    SystemsComparedCount.Add(ckpaa.dLelapsedTime.Count);
                                    SystemsUniqueDev.Add(ckpaa.nDev.ToString());
                                    SystemsCompared_bWantStats.Add(ckpaa.bUseThisAppInStatsListBox);
                                }

                                else
                                {
                                }
                                if (!ckpaa.bUseThisAppInStatsListBox) continue;
                                ncnt = 0;
                                bShowSummary = bShowFL & (ckpaa.dLelapsedTime.Count > MaxAllowed);
                                foreach (double d in ckpaa.dLelapsedTime)
                                {
                                    double dd = d / ckpaa.nConcurrent;
                                    dTemp.Add(dd);
                                    dAvg += dd;
                                    string strValue = dd.ToString("###,##0.00").PadLeft(12);
                                    if(!bShowSummary)
                                        sTemp += (strValue +"   " + strLoc +  "\r\n");
                                    ncnt++;
                                }
                                if(bShowSummary)
                                    sTemp += "Summary: " + strLoc + " " + ncnt.ToString() + "\r\n";
                                strSPAstats += strLoc + ":\t" + sProj + "-" + sApp + "(" + ncnt.ToString()  + ")" + strDivide(ckpaa.nConcurrent) +
                                    "\tCPD:" +ckpaa.cpd.ToString("#####.##") + "\r\n";
                                Totalcpd += ckpaa.cpd;
                            }
                        }
                    }
                }
            }
            dAvg /= dTemp.Count;
            foreach(double d in dTemp)
            {
                Double dd = d - dAvg;
                dd = dd * dd;
                dRms += dd;
            }
            dRms /= dTemp.Count;   
            dStd = Math.Sqrt(dRms);
            TBoxResults.Text  = sTemp;
            sTemp = "Num: " + dTemp.Count.ToString("0") + "\r\n";
            sTemp += "AVG: " + dAvg.ToString("#,##0.00").PadLeft(12) + "\r\n";
            sTemp += "STD: " + dStd.ToString("#,##0.00").PadLeft(12) + "\r\n";
            sTemp += "\r\nTotal  workunits per day " + Totalcpd.ToString("###,###.##");
            sTemp += "\r\nMultiply by average credit per work unit\r\nto get your average daily credit";
            TBoxStats.Text = strSPAstats + "\r\n" + sTemp;
            toolTip1.SetToolTip(BtnCmpSave, strSystems);
        }


        // uses a listview with checkbox to decide which system go into the calculation and which gpu's have multiple task
        // running on them and a count of how many results
        // if name is empty then calculate all
        private void UpdateAppInfo(string strAll)
        {
            int i = 0;
            bool bGood;
            string strAppSelected = "";
            string[] strSysConc;
            ListViewItem itm;
            LViewConc.Items.Clear();
            if (strAll == "all")
                strAppSelected = "all";
            else
                bGood = GetNameFromBox(LBoxApps.Text, ref strAppSelected);
            ShowPBar(true);
            CalcAllValues(LBoxProjects.Text, strAppSelected, true, strAverageAll);
            foreach (string s in SystemsCompared)
            {
                strSysConc = new string[4];
                strSysConc[0] = SystemsCompared_nConcurrent[i];
                strSysConc[1] = s;
                strSysConc[2] = SystemsComparedCount[i].ToString();
                strSysConc[3] = SystemsUniqueDev[i].ToString();
                itm = new ListViewItem(strSysConc);
                itm.Checked = SystemsCompared_bWantStats[i];
                LViewConc.Items.Add(itm);
                i++;
            }
            ShowPBar(false);
        }

        private void LBoxApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strApp;
            btnShowScatter.Visible = LBoxApps.Items.Count > 0;
            LViewConc.Enabled = LBoxApps.SelectedIndices.Count == 1;        // needed as apply deos not work in multiselect
            strApp = (LBoxApps.SelectedIndices.Count == 1) ? "" : "all";
            UpdateAppInfo(strApp);
        }

        // look in listview for any concurrent gpu tasks
        // this info needs to be backfit into the actual data tables to be usefull.
        public int GetConcurrency(string strSystem)
        {
            foreach (ListViewItem itm in LViewConc.Items)
            {
                if(itm.SubItems[1].Text == strSystem)
                {
                    int n = Convert.ToInt32(itm.SubItems[0].Text);
                    if (n < 1) n = 1;
                    return n;
                }
            }
            return 1;
        }

        // seems I do not need a progress bar after all
        public void ShowPBar(bool bShow)
        {
            return;
            lbEditTab.Text = bShow ? "...working on it..." : "ASSUMES ONE WU PER GPU UNLESS YOU EDIT VALUE IN FIRST COLUMN";
            lbEditTab.Update();
        }

        // the following was not used after all.   keeping for a note to myself in case I to show different colors.
        private void LBoxProjects_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lst = (ListBox)sender;
            Color col;
            bool bAny = bItemsToColor.Count > 0;
            e.DrawBackground();
            e.DrawFocusRectangle();
            //DrawItemState st = DrawItemState.Selected ^ DrawItemState.Focus;
            //Color col = ((e.State & st) == st) ? Color.Yellow : lst.BackColor;

            //if (bAny)
            //    col = bItemsToColor[e.Index] ? Color.Yellow : lst.BackColor;
            //else
                col = lst.BackColor;

            e.Graphics.DrawRectangle(new Pen(col), e.Bounds);
            e.Graphics.FillRectangle(new SolidBrush(col), e.Bounds);
            if (e.Index >= 0)
            {
                e.Graphics.DrawString(lst.Items[e.Index].ToString(), e.Font, new SolidBrush(lst.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        // put the number of concurrent items processed by the GPU into the table that has the corresponding data.
        private void BackfitConcurrency(string sysname, string projname, string appname, int value,bool bLVchecked)
        {
            foreach (cKPAlocs ckpal in KPAlocs)                          // for each system
            {
                foreach (cKPAproj ckpap in ckpal.KPAproj)               // for each project
                {
                    if (ckpap.sProjName == projname)               // for the project selected
                    if (ckpap.sProjName == projname)               // for the project selected
                    {
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)       // for each app in the project
                        {
                            if (ckpaa.sAppName == appname)         // is it one of the apps we want?
                            {
                                if (Systems[ckpaa.iSystem] == sysname)
                                {
                                    ckpaa.nConcurrent = value;
                                    ckpaa.bUseThisAppInStatsListBox = bLVchecked;
                                }
                            }
                        }
                    }
                }
            }
        }

        // same as above but used to set all of them to 1 ie: a reset.
        private void BackfitAllConcurrency(int value, bool bChecked)
        {
            foreach (cKPAlocs ckpal in KPAlocs)                          // for each system
            {
                foreach (cKPAproj ckpap in ckpal.KPAproj)               // for each project
                {
                    foreach (cKPAapps ckpaa in ckpap.KPAapps)       // for each app in the project
                    {
                        ckpaa.nConcurrent = value;
                        ckpaa.bUseThisAppInStatsListBox = bChecked;
                    }
                }
            }
        }

        // this is the "Apply" button, not sure how to rename it w/o design form going crazy on me
        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strWhatToAverage = "";
            bool bAny = false;
            string strSysName, strProjName, strAppName;
            int nConcurrent = 1;
            lblWarnApply.Visible = false;
            foreach(ListViewItem itm in LViewConc.Items)
            {
                strSysName = itm.SubItems[1].Text;
                nConcurrent = Convert.ToInt32(itm.SubItems[0].Text);
                strProjName = LBoxProjects.Text;
                strAppName = "";
                bool bBad = GetNameFromBox(LBoxApps.Text, ref strAppName);
                BackfitConcurrency(strSysName, strProjName, strAppName, nConcurrent, itm.Checked);
                if (itm.Checked)
                    bAny = true;
                // below not needed as we are saving the checkmarks during backfit
                //if (itm.Checked)
                //{
                //    strWhatToAverage += "-" + strSysName ;
                //    bAny = true;
                //}
                //strWhatToAverage += "-";
            }
           if (bAny)
                CalcAllValues(Last_sProj, Last_sApp, false, strWhatToAverage);
           else
                UpdateAppInfo("");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            CompareHelp MyCompareHelp = new CompareHelp();
            MyCompareHelp.ShowDialog();
            MyCompareHelp.Dispose();
        }

        private void WarnedOnce()
        {
            
        }

        // form statistics for the selected event
        private void LViewConc_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblWarnApply.Visible = true;
        }

        private void BtnCmpSave_Click(object sender, EventArgs e)
        {
            NotepadHelper.ShowMessage(TBoxStats.Text, toolTip1.GetToolTip(BtnCmpSave));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem itm in LViewConc.Items)
            {
                itm.Checked = true;
                itm.Text = "1";
            }
            BackfitAllConcurrency( 1,true);
            CalcAllValues(Last_sProj, "", false, strAverageAll);
        }

        // for each specified app, in each specified system, accumulate elapsed time and find min amd max values.
        private bool GetSysProjData(ref cSeriesData sa)
        {
            foreach (cKPAlocs ckpal in KPAlocs)                          // for each system
            {
                foreach (cKPAproj ckpap in ckpal.KPAproj)               // for each project
                {
                    if (ckpap.sProjName == sa.strProjName)               // for the project selected
                    {
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)       // for each app in the project
                        {
                            if (ckpaa.sAppName == sa.strAppName)         // is it one of the apps we want?
                            {
                                if (Systems[ckpaa.iSystem] != sa.strSysName)
                                    continue;
                                if (ckpaa.dLelapsedTime.Count == 0)                         // must have data
                                    continue;
                                foreach (double d in ckpaa.dLelapsedTime)
                                {
                                    if (d == 0.0)
                                    {
                                        Debug.Assert(false);                                       
                                    }
                                    sa.dValues.Add(d/ckpaa.nConcurrent);
                                    sa.bIsValid.Add(true);
                                }
                            }
                        }
                    }
                }
            }
            return (sa.dValues.Count > 0);
        }

        // same as above but we want all systems
        private bool GetAllAppData(ref cSeriesData sa)
        {
            foreach (cKPAlocs ckpal in KPAlocs)                          // for each system
            {
                foreach(cKPAproj ckpap in ckpal.KPAproj )               // for each project
                {
                    if(ckpap.sProjName == sa.strProjName)               // for the project selected
                    {
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)       // for each app in the project
                        {
                            if (!ckpaa.bUseThisAppInStatsListBox) continue;
                            if(ckpaa.sAppName == sa.strAppName)         // is it one of the apps we want?
                            {
                                if (ckpaa.dLelapsedTime.Count == 0)
                                    continue;
                                foreach(double d in ckpaa.dLelapsedTime)
                                {
                                    if (d == 0.0)
                                    {
                                        Debug.Assert(false);
                                    }
                                    sa.dValues.Add(d/ckpaa.nConcurrent);
                                    sa.iSystem.Add(ckpaa.iSystem);
                                    sa.bIsValid.Add(true);
                                }
                                sa.iTheseSystem.Add(ckpaa.iSystem);
                                sa.TheseSystems.Add(ckpal.strSystem);
                            }
                        }
                    }
                }
            }
            return (sa.dValues.Count > 0) ;
        }

        private bool GetNameFromBox(string strIn, ref string strOut)
        {
            int i = strIn.IndexOf(") ");  // really need the name of the app
            if (i < 2) return false;        // cant be
            strOut = strIn.Substring(i + 2);
            return true;
        }


        private bool MergeAppInfo(int nSelected)
        {
            bool bGood;
            MySeriesData = new List<cSeriesData>();
            string strApp = "";
            for (int i = 0; i < LBoxApps.SelectedIndices.Count; i++)
            {
                int j = LBoxApps.SelectedIndices[i];
                bGood = GetNameFromBox(LBoxApps.Items[j].ToString(), ref strApp);
                if (!bGood) return false;
                cSeriesData sa = new cSeriesData();
                sa.strAppName = strApp;
                sa.strProjName = LBoxProjects.Text;
                sa.strSysName = ""; // not applicable 
                sa.dValues = new List<double>();
                sa.iSystem = new List<int>();
                sa.TheseSystems = new List<string>();
                sa.iTheseSystem = new List<int>();
                sa.iGpuDevice = new List<int>();
                sa.ShowType = eShowType.DoingApps;
                sa.bIsValid = new List<bool>();
                sa.nConcurrent = 1; // this may be revised when data is obtain as we dont know the system yet
                if (GetAllAppData(ref sa))
                {
                    MySeriesData.Add(sa);
                }
                else sa = null;
            }
            return MySeriesData.Count > 0; ;
        }
        // using the listbox of apps, extract just the name of the app and save the name locally just below
        // then use what is in the List to add the apps data to the series for graphics
        // could be rewritten to avoid the little list.
        // this routine implements the scatter apps

 

        // simular to above but scattering systems
        private bool FormSeriesFromSystems(int n)
        {
            int NumChecked = 0;
            MySeriesData = new List<cSeriesData>();                 // must be only those checked, not all
            foreach (ListViewItem itm in LViewConc.Items)
            {
                if (itm.Checked)
                {
                    cSeriesData sa = new cSeriesData();
                    NumChecked++;
                    sa.strSysName = itm.SubItems[1].Text;   // this will be the first item to be seen in the list view
                    sa.strAppName = "";
                    bool bGood = GetNameFromBox(LBoxApps.Text, ref sa.strAppName);
                    if (!bGood) return false;

                    sa.strProjName = LBoxProjects.Text;
                    sa.dValues = new List<double>();
                    sa.ShowType = eShowType.DoingSystems;
                    sa.nConcurrent = Convert.ToInt32(itm.SubItems[0].Text);
                    sa.bIsValid = new List<bool>();
                    if (GetSysProjData(ref sa))
                    {
                        MySeriesData.Add(sa);
                    }
                    else sa = null;
                }
            }
            return (NumChecked > 0 && MySeriesData.Count > 0);
        }

        // forms scatter dagta depending on the radio button selected
        private bool GetScatterData()
        {
            int n = LViewConc.Items.Count;
            if (rbScatApps.Checked)
            {
                n = LBoxApps.SelectedItems.Count; //todo need to think about the multiselect better
                if (n == 0) return false;
                return MergeAppInfo(n);
            }
            else
            {
                return FormSeriesFromSystems(n);
            }
        }

        private void ShowScatter(string strFilter)
        {                                                   // radio button was misnamed, should be ScatterSystems
            lbAdvFilter.Text = strFilter;
            ScatterForm PlotScatter = new ScatterForm(ref MySeriesData, rbScatProj.Checked ? "Systems" : "Apps", false, strFilter, 0.0);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }

        private void btnShowScatter_Click(object sender, EventArgs e)
        {
            if(GetScatterData())
            {
                ShowScatter(lbAdvFilter.Text);
            }
        }

        private void rbScatApps_CheckedChanged(object sender, EventArgs e)
        {
            ShowAppSelects(true);
        }

        private void ClearBoxSelections()
        {
            LBoxApps.SelectedIndices.Clear();
            LViewConc.Items.Clear();
        }

        private void rbScatProj_CheckedChanged(object sender, EventArgs e)
        {
            ShowAppSelects(false);
        }

        private void btnSelAllApp_Click(object sender, EventArgs e)
        {
            for(int i=0; i < LBoxApps.Items.Count;i++)
            {
                LBoxApps.SetSelected(i, true);
            }
        }

        private void BtnClrAllApp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LBoxApps.Items.Count; i++)
            {
                LBoxApps.SetSelected(i, false);
            }
            LViewConc.Items.Clear();
        }

        private void BtnInvSelApp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LBoxApps.Items.Count; i++)
            {
                bool bSelected = LBoxApps.GetSelected(i);
                LBoxApps.SetSelected(i, !bSelected);
            }
        }
    }
}


namespace Notepad
{
    public static class NotepadHelper
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        private static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        public static void ShowMessage(string message = null, string title = null)
        {
            Process notepad = Process.Start(new ProcessStartInfo("notepad.exe"));
            if (notepad != null)
            {
                notepad.WaitForInputIdle();

                if (!string.IsNullOrEmpty(title))
                    SetWindowText(notepad.MainWindowHandle, title);

                if (!string.IsNullOrEmpty(message))
                {
                    IntPtr child = FindWindowEx(notepad.MainWindowHandle, new IntPtr(0), "Edit", null);
                    SendMessage(child, 0x000C, 0, message);
                }
            }
        }
    }
}

