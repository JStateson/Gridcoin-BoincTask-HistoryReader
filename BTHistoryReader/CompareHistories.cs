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
    public partial class CompareHistories : Form
    {
        public BTHistory btf;
        public List<string> Projects;
        public List<string> Systems;
        public List<cKPAlocs> KPAlocs;

        public List<bool> bItemsToColor = new List<bool>(); // use color to show which apps have values

        public class cKPAapps
        {
            public string sAppName;
            public int iSystem;
            public List<double> dLelapsedTime = new List<double>();
            public void AddValue(double d)
            {
                dLelapsedTime.Add(d);
            }
        }

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

        public CompareHistories(Form refForm)
        {
            InitializeComponent();
            Projects = new List<string>();
            KPAlocs = new List<cKPAlocs>();
            Systems = new List<string>();;
            int iSystem = -1;

            btf = (BTHistory)refForm;
            LBoxApps.Items.Clear();
            LBoxProjects.Items.Clear();
            btf.CurrentSystem = "";
            foreach(string sProj in btf.AllHistories)
            {
                int RtnCod = btf.ValidateHistory(sProj);
                Systems.Add(btf.ThisSystem);
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
                ckpal.AddNewPath(sProj);
                foreach (cKnownProjApps kpa in btf.KnownProjApps)
                {
                    cKPAproj ckpap = ckpal.AddProj(kpa.ProjName);
                    foreach (cAppName AppName in kpa.KnownApps)
                    {
                        int nEntries = AppName.dElapsedTime.Count;  // nEntries from AppName is not valid here
                        if (nEntries == 0) continue;
                        cKPAapps ckpaa = ckpap.AddApp(AppName.Name);
                        btf.ThisProjectInfo = new List<cProjectInfo>(nEntries);
                        cProjectInfo cpi = new cProjectInfo();
                        cpi.iSystem = iSystem;
                        ckpaa.iSystem = iSystem;
                        foreach (double d in AppName.dElapsedTime)
                                ckpaa.AddValue(d / 60.0);
                        btf.ThisProjectInfo.Add(cpi);
                    }
                    btf.ClearPreviousHistory();
                }
            }
            foreach (string s in Projects)
                LBoxProjects.Items.Add(s);
        }

        private void ShowAppsThisProj(string sProj)
        {
            List<string> ProjApps = new List<string>();
            List<int> NumApps = new List<int>();
            int i = 0;
            foreach(cKPAlocs ckpal in KPAlocs)
            {
                foreach(cKPAproj ckpap in ckpal.KPAproj)
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
        }



        private void CalcAllValues(string sProj, string sApp)
        {
            TBoxResults.Text = "";
            TBoxStats.Text = "";
            string sTemp = "";
            string strLoc = "";
            double dAvg=0.0;
            double dRms=0.0;
            double dStd;

            List<double> dTemp = new List<double>();
            foreach (cKPAlocs ckpal in KPAlocs)
            {
                
                foreach (cKPAproj ckpap in ckpal.KPAproj)
                {
                    if (sProj == ckpap.sProjName)
                    {
                        foreach (cKPAapps ckpaa in ckpap.KPAapps)
                        {
                            if (ckpaa.sAppName == sApp)
                            {
                                strLoc = Systems[ckpaa.iSystem];
                                foreach (double d in ckpaa.dLelapsedTime)
                                {
                                    dTemp.Add(d);
                                    dAvg += d;
                                    string strValue = d.ToString("###,##0.00").PadLeft(12);
                                    sTemp += (strValue +"   " + strLoc +  "\r\n");
                                }
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
            TBoxResults.Text = sTemp;
            sTemp = "Num: " + dTemp.Count.ToString("0") + "\r\n";
            sTemp += "AVG: " + dAvg.ToString("#,##0.00").PadLeft(12) + "\r\n";
            sTemp += "STD: " + dStd.ToString("#,##0.00").PadLeft(12) + "\r\n";
            TBoxStats.Text = sTemp;
        }



        private void LBoxApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strAppSelected = LBoxApps.Text;
            // probably need a better way to keep track of both items
            int iString = strAppSelected.IndexOf(")");
            if (iString <= 0) return;   // probably should assert this
            CalcAllValues(LBoxProjects.Text, strAppSelected.Substring(iString+2));
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

    }
}
