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
namespace BTHistoryReader
{
    public partial class CompareHistories : Form
    {
        public BTHistory btf;
        public List<string> Projects;
        public List<string> Systems;
        public List<cKPAlocs> KPAlocs;
        public List<string> SystemsCompared;    // this and following are paired
        public List<string> SystemsCompared_nConcurrent;
        private string strAverageAll = "Average All";
        public List<bool> bItemsToColor = new List<bool>(); // use color to show which apps have values
        public List<cSeriesData> MySeriesData;


        public class cKPAapps
        {
            public string sAppName;
            public int iSystem;
            public List<double> dLelapsedTime = new List<double>();
            public void AddValue(double d)
            {
                dLelapsedTime.Add(d);
            }
            public int nConcurrent = 1;    // save info to allow restoreing when switching across apps and systems
        }

        public string Last_sProj="";
        public string Last_sApp="";

        public class cKPAproj
        {
            public string sProjName;
            public List<cKPAapps> KPAapps = new List<cKPAapps>();
            public cKPAapps AddApp(string sAppName)
            {
                cKPAapps ckpaa = new cKPAapps();
                ckpaa.sAppName = sAppName;
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

        // since system name might be identical in different files then we must change the system name to
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

        // when comparing projects, say "seti" app can be nvidia, amd or cpu
        // cannot test for full app name else no match unless both systems had same devices
        public CompareHistories(Form refForm, bool bIgnoreLong)
        {
            InitializeComponent();
            string ThisSystem = "";
            int DuplicateNameCnt = 0;
            Projects = new List<string>();
            KPAlocs = new List<cKPAlocs>();
            Systems = new List<string>();;
            SystemsCompared = new List<string>();
            SystemsCompared_nConcurrent = new List<string>();
            int iSystem = -1;
            int NumberProjects = 0;

            btf = (BTHistory)refForm;
            LBoxApps.Items.Clear();
            LBoxProjects.Items.Clear();
            btf.CurrentSystem = "";
            foreach(string sProj in btf.AllHistories)
            {
                int RtnCod;
                if (bIgnoreLong && sProj.Contains("_long_"))
                    continue;
                RtnCod = btf.ValidateHistory(sProj, ref ThisSystem);
                if (RtnCod < 0) continue;
                DuplicateNameCnt = 0;
                ThisSystem = MustBeUniqueName(ThisSystem,ref DuplicateNameCnt);
                Systems.Add(ThisSystem);
                iSystem = Systems.Count - 1;    // index into name of system
                btf.ClearPreviousHistory();
                btf.ProcessHistoryFile();
                foreach (cKnownProjApps kpa in btf.KnownProjApps)
                {
                    if (Projects.Contains(kpa.ProjName)) continue;
                    if (kpa.NumDValues() == 0) continue;
                    Projects.Add(kpa.ProjName);
                }
                cKPAlocs ckpal = new cKPAlocs();
                KPAlocs.Add(ckpal);
                ckpal.AddNewPath(sProj);    // sProj does not contain the exact name of system that "history" has
                foreach (cKnownProjApps kpa in btf.KnownProjApps)
                {
                    if (kpa.nAppsUsed == 0) continue;
                    cKPAproj ckpap = ckpal.AddProj(kpa.ProjName);
                    foreach (cAppName AppName in kpa.KnownApps)
                    {
                        int nEntries = AppName.dElapsedTime.Count;  // nEntries from AppName is not valid here
                        if (nEntries == 0) continue;
                        NumberProjects++;
                        cKPAapps ckpaa = ckpap.AddApp(AppName.Name);
                        btf.ThisProjectInfo = new List<cProjectInfo>(nEntries);
                        cProjectInfo cpi = new cProjectInfo();
                        cpi.iSystem = iSystem;
                        ckpaa.iSystem = iSystem;
                        foreach (double d in AppName.dElapsedTime)
                        {
                            ckpaa.AddValue(d / 60.0); 
                        }
                        btf.ThisProjectInfo.Add(cpi);
                    }
                }
            }
            btf.ClearPreviousHistory(); // not sure why this was in the above loop ???
            foreach (string s in Projects)
            {
                LBoxProjects.Items.Add(s);
            }
        }

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
                            //bItemsToColor.Add(ckpaa.dLelapsedTime.Count > 0);
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
            LBoxApps.SetSelected(0, true);
        }

        private bool bUseThisSystem(string strProjName, string strListOfUsed)
        {
            string strQualifier;
            if(strListOfUsed == strAverageAll)return true;
            strQualifier = "-" + strProjName + "-";
            return strListOfUsed.Contains(strQualifier);
        }

        private string strDivide(int n)
        {
            if (n == 1) return "";
            return "/" + n.ToString();
        }


        // if bMakeTable is not true then we are recalculating using number of devices from table
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

            Last_sProj = sProj;
            Last_sApp = sApp;
            btnApply.Visible = true;

            if(bMakeTable)
            {
                SystemsCompared.Clear();
                SystemsCompared_nConcurrent.Clear();
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
                            if (ckpaa.sAppName == sApp)                                     // if it is the app we want then do analysis
                            {
                                if (ckpaa.dLelapsedTime.Count == 0)                         // must have data
                                    continue;
                                strLoc = Systems[ckpaa.iSystem];
                                if (!bUseThisSystem(strLoc, strSystems)) continue;          // not sure why I put system name into strloc
                                if (bMakeTable)
                                {
                                    SystemsCompared.Add(strLoc);
                                    SystemsCompared_nConcurrent.Add(ckpaa.nConcurrent.ToString());
                                }

                                else
                                {
                                    ckpaa.nConcurrent = GetConcurrency(strLoc);
                                }
                                ncnt = 0;
                                foreach (double d in ckpaa.dLelapsedTime)
                                {
                                    double dd = d / ckpaa.nConcurrent;
                                    dTemp.Add(dd);
                                    dAvg += dd;
                                    string strValue = dd.ToString("###,##0.00").PadLeft(12);
                                    sTemp += (strValue +"   " + strLoc +  "\r\n");
                                    ncnt++;
                                }
                                strSPAstats += strLoc + ": " + sProj + "-" + sApp + "(" + ncnt.ToString()  + ")" + strDivide(ckpaa.nConcurrent) +"\r\n";
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
            TBoxStats.Text = strSPAstats +"\r\n" +  sTemp;
            toolTip1.SetToolTip(BtnCmpSave, strSystems);
        }



        private void UpdateAppInfo()
        {
            int i = 0;
            string strAppSelected = LBoxApps.Text;
            string[] strSysConc;
            ListViewItem itm;
            LViewConc.Items.Clear();
            // probably need a better way to keep track of both items
            int iString = strAppSelected.IndexOf(")");
            if (iString <= 0) return;   // probably should assert this
            ShowPBar(true);
            CalcAllValues(LBoxProjects.Text, strAppSelected.Substring(iString + 2), true, strAverageAll);
            foreach (string s in SystemsCompared)
            {
                strSysConc = new string[3];
                strSysConc[0] = SystemsCompared_nConcurrent[i];
                strSysConc[1] = s;
                itm = new ListViewItem(strSysConc);
                itm.Checked = true;
                LViewConc.Items.Add(itm);
                i++;
            }
            ShowPBar(false);
        }

        private void LBoxApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAppInfo();
            btnShowScatter.Visible = LBoxApps.Items.Count > 0;
        }

        public int GetConcurrency(string strSystem)
        {
            foreach (ListViewItem itm in LViewConc.Items)
            {
                if(itm.SubItems[1].Text == strSystem)
                {
                    int n = Convert.ToInt32(itm.SubItems[0].Text);
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

        // the following was not used after all
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

        private void BackfitConcurrency(string sysname, string projname, string appname, int value)
        {
            foreach (cKPAlocs ckpal in KPAlocs)                          // for each system
            {
                foreach (cKPAproj ckpap in ckpal.KPAproj)               // for each project
                {
                    if (ckpap.sProjName == projname)               // for the project selected
                    {
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)       // for each app in the project
                        {
                            if (ckpaa.sAppName == appname)         // is it one of the apps we want?
                            {
                                if (Systems[ckpaa.iSystem] == sysname)
                                {
                                    ckpaa.nConcurrent = value;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strWhatToAverage = "";
            bool bAny = false;
            string strSysName, strProjName, strAppName;
            int nConcurrent = 1;
            foreach(ListViewItem itm in LViewConc.Items)
            {
                strSysName = itm.SubItems[1].Text;
                nConcurrent = Convert.ToInt32(itm.SubItems[0].Text);
                strProjName = LBoxProjects.Text;
                strAppName = LBoxApps.Text;
                BackfitConcurrency(strSysName, strProjName, strAppName, nConcurrent);
                if (itm.Checked)
                {
                    strWhatToAverage += "-" + strSysName ;
                    bAny = true;
                }
                strWhatToAverage += "-";
            }
            if (bAny)
                CalcAllValues(Last_sProj, Last_sApp, false, strWhatToAverage);
            else
                UpdateAppInfo();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            CompareHelp MyCompareHelp = new CompareHelp();
            MyCompareHelp.ShowDialog();
            MyCompareHelp.Dispose();
        }

        // form statistics for the selected event
        private void LViewConc_SelectedIndexChanged(object sender, EventArgs e)
        {


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
            CalcAllValues(Last_sProj, Last_sApp, false, strAverageAll);
        }

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
                                if (Systems[ckpaa.iSystem] != sa.strSystemName)
                                    continue;
                                if (ckpaa.dLelapsedTime.Count == 0)                         // must have data
                                    continue;
                                foreach (double d in ckpaa.dLelapsedTime)
                                {
                                    sa.dValues.Add(d);
                                }
                            }
                        }
                    }
                }
            }
            return (sa.dValues.Count > 0);
        }

        private bool GetAllAppData(ref cSeriesData sa)
        {
            foreach(cKPAlocs ckpal in KPAlocs)                          // for each system
            {
                foreach(cKPAproj ckpap in ckpal.KPAproj )               // for each project
                {
                    if(ckpap.sProjName == sa.strProjName)               // for the project selected
                    {
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)       // for each app in the project
                        {
                            if(ckpaa.sAppName == sa.strAppName)         // is it one of the apps we want?
                            {
                                if (ckpaa.dLelapsedTime.Count == 0)
                                    continue;
                                //sa.nConcurrent = GetConcurrency(Systems[ckpaa.iSystem]);
                                //this does not work as it uses the box only that one project is in there
                                foreach(double d in ckpaa.dLelapsedTime)
                                {
                                    sa.dValues.Add(d/ ckpaa.nConcurrent);
                                }
                            }
                        }
                    }
                }
            }
            return (sa.dValues.Count > 0) ;
        }

        private List<string> strAppsForSeries;
        private bool FormSeriesFromApps(int n)
        {
            strAppsForSeries = new List<string>(n);
            foreach(string strInfo in LBoxApps.Items)
            {
                int i = strInfo.IndexOf(") ");  // really need the name of the app
                if (i < 2) return false;        // cant be
                string strApp = strInfo.Substring(i+2);
                strAppsForSeries.Add(strApp);
            }
            MySeriesData = new List<cSeriesData>(n);
            foreach(string strName in strAppsForSeries)
            {
                cSeriesData sa = new cSeriesData();
                sa.strAppName = strName;
                sa.strProjName = LBoxProjects.Text;
                sa.dValues = new List<double>();
                sa.bIsShowingApp = true;
                sa.nConcurrent = 1; // this may be overwritten when data is obtain as we dont know the system yet
                if (GetAllAppData(ref sa))
                {
                    MySeriesData.Add(sa);
                }
                else sa = null;
            }
            return MySeriesData.Count > 0;
        }

       

        private bool FormSeriesFromProjects(int n)
        {
            int NumChecked = 0;
            MySeriesData = new List<cSeriesData>();                 // must be only those checked, not all
            foreach (ListViewItem itm in LViewConc.Items)
            {
                if(itm.Checked)
                {
                    cSeriesData sa = new cSeriesData();
                    NumChecked++;
                    sa.strSystemName = itm.SubItems[1].Text;
                    int i = LBoxApps.Text.IndexOf(") ");  // really need the name of the app
                    if (i < 3) return false;        // cant be
                    sa.strAppName = LBoxApps.Text.Substring(i + 2);
                    sa.strProjName = LBoxProjects.Text;
                    sa.dValues = new List<double>();
                    sa.bIsShowingApp = false;
                    sa.nConcurrent = Convert.ToInt32(itm.SubItems[0].Text);
                    if (GetSysProjData(ref sa))
                    {
                        MySeriesData.Add(sa);
                    }
                    else sa = null;
                }
            }  
            return NumChecked > 0;
        }

        private bool GetScatterData()
        {
            int n;
            if (rbScatApps.Checked)
            {
                n = LBoxApps.Items.Count;
                if (n == 0) return false;
                return FormSeriesFromApps(n);
            }
            else
            {
                n = LViewConc.Items.Count;
                return FormSeriesFromProjects(n);
            }
        }

        private void ShowScatter()
        {
            ScatterForm PlotScatter = new ScatterForm(ref MySeriesData);
            PlotScatter.ShowDialog();
            PlotScatter.Dispose();
        }

        private void btnShowScatter_Click(object sender, EventArgs e)
        {
            if(GetScatterData())
            {
                ShowScatter();
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

