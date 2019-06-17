using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvalidAnalysis
{
    public partial class InfoForm : Form
    {

        private List<cProjPage> ProjectPages;
        private ErrorAnalysis ea;
        public enum eShowType
        {
            eShowAllcol = 0,    // only one shown as collapsed (default)
            eShowAllexp = 1,    // these must match the "tag" in radio button
            eShowFail = 2,
            eShowSucc = 3,
            eShowInc = 4
        }

        // following need to come from default settings
        /*
        private bool bShowWIN = true;
        private bool bShowAPP = true;
        private bool bShowLIN = true;
        private bool bShowAND = true;
        private bool bShowATI = true;
        private bool bShowNVI = true;
        private bool bShowINT = true;
        private bool bShowCPU = true;
        */

        private bool bShowWIN = Properties.Settings.Default.bShowWIN ;
        private bool bShowAPP = Properties.Settings.Default.bShowAPP ;
        private bool bShowLIN = Properties.Settings.Default.bShowLIN ;
        private bool bShowAND = Properties.Settings.Default.bShowAND ;
        private bool bShowATI = Properties.Settings.Default.bShowATI ;
        private bool bShowNVI = Properties.Settings.Default.bShowNVI ;
        private bool bShowINT = Properties.Settings.Default.bShowINT ;
        private bool bShowCPU = Properties.Settings.Default.bShowCPU ;

        private bool bShowSuccess = true;
        private bool bShowAll = true;
        private bool bShowFail = false;
        private bool bExpandAll = false;
        private bool bShowInconclusive = false;

        private int[] PFcnt = new int[4];   // this must correspond to the 4 platforms in order
        private int[] DVcnt = new int[4];   // this must correspond to the 4 devices in order

        private void ZeroCounts()
        {
            for(int i = 0; i < 4; i ++)
            {
                PFcnt[i] = 0;
                DVcnt[i] = 0;
            }
        }

        private void GetFilters()
        {
            bShowWIN = cbWIN.Checked;
            bShowAPP = cbAPPLE.Checked;
            bShowLIN = cbLINUX.Checked;
            bShowAND = cbAND.Checked;
            bShowATI = cbATI.Checked;
            bShowNVI = cbNVIDIA.Checked;
            bShowINT = cbINTEL.Checked;
            bShowCPU = cbCPU.Checked;
        }

        public TreeNode mainNode;

        public static eShowType ShowType = eShowType.eShowAllcol;

        public InfoForm(Form refForm, string WhatToDo)
        {
            InitializeComponent();
            ea = (ErrorAnalysis) refForm;
            ProjectPages = ea.ProjectPages;
            mainNode = new TreeNode();
            mainNode.Text = WhatToDo;
            tv_projapps.Nodes.Add(mainNode);
            GetTreeLayout();
            ShowTree();
        }

        private bool bWantThisDevice(string strIn, ref int iLoc)
        {
            if (strIn.Contains("_ati_"))
            {
                if (bShowATI) return DeviceCount(iLoc=0);
                return false;
            }
            if (strIn.Contains("_nvidia_"))
            {
                if (bShowNVI) return DeviceCount(iLoc = 1);
                return false;
            }
            if (strIn.Contains("_intel_"))  // this is a guess as I have never seen one
            {
                if (bShowINT) return DeviceCount(iLoc = 2);
                return false;
            }
            //if (strIn.Contains("_cpu_"))  // this is OBVIOUSLY WRONG NEED TO FIND OUT WHAT IT IS:  there is no id, just has x6_64 or whatever
            //{
             //   bIsCpu = false;
                if (bShowCPU) return DeviceCount(iLoc = 3);
            //}
            return false; 
        }

        private bool PlatformCount(int iLoc)
        {
            PFcnt[iLoc]++;
            return true;
        }

        private bool DeviceCount(int iLoc)
        {
            DVcnt[iLoc]++;
            return true;
        }

        private bool bWantThisPlatform(string strIn)
        {
            if(strIn.Contains("windows_"))
            {
                if (bShowWIN) return PlatformCount(0);
                return false;
            }
            if (strIn.Contains("-linux-"))
            {
                if (bShowLIN) return PlatformCount(2);
                return false;
            }
            if (strIn.Contains("-apple-"))
            {
                if (bShowAPP) return PlatformCount(1);
                return false;
            }
            if (strIn.Contains("-android-"))  // this is a guess as I have never seen one
            {
                if (bShowAND) return PlatformCount(3);
                return false;
            }
            return false;  
        }

        public void ShowTree()
        {
            int i = 1;
            TreeNode n, c;
            bool bShowedSomething;
            ZeroCounts();
            foreach (cProjPage pp in ProjectPages)
            {

                n = new TreeNode();
                n.Name = i.ToString();
                n.Text = pp.strWorkUnit + " - " + pp.Wingmen[0].name;
                foreach (cWorkUnit wu in pp.Wingmen)
                {
                    int iUsed = -1;
                    bShowedSomething = false;

                    if (bShowAll)
                    {
                        c = new TreeNode();
                        c.Text = wu.strStatus + ": " + wu.strComputer;
                        if (wu.strStatus.Contains("error")) c.ForeColor = Color.Red;
                        if (wu.strStatus.Contains("Error")) c.ForeColor = Color.Red;
                        if (wu.strStatus.Contains("Aborted")) c.ForeColor = Color.Red;
                        if (wu.strStatus.Contains("inconclusive")) c.ForeColor = Color.Red;
                        n.Nodes.Add(c);
                        bShowedSomething = true;
                        if (!bWantThisDevice(wu.Application, ref iUsed)) continue;
                        // problem:  the above counted a value towared the device but
                        // it may be excluded if the platform is not wanted
                        if (!bWantThisPlatform(wu.Application))
                        {
                            Debug.Assert(iUsed >= 0);
                            DVcnt[iUsed]--; // fixup code
                            continue;
                        }
                    }
                    else
                    {
                        if (bShowSuccess)
                        {
                            if (wu.strStatus.Contains("Completed and validated"))
                            {
                                if (!bWantThisDevice(wu.Application, ref iUsed)) continue;
                                // problem:  the above counted a value towared the device but
                                // it may be excluded if the platform is not wanted
                                if (!bWantThisPlatform(wu.Application))
                                {
                                    Debug.Assert(iUsed >= 0);
                                    DVcnt[iUsed]--; // fixup code
                                    continue;
                                }
                                c = new TreeNode();
                                c.Text = wu.strStatus + ": " + wu.strComputer;
                                n.Nodes.Add(c);
                                bShowedSomething = true;
                                continue;
                            }
    
                        }
                        if (bShowInconclusive)
                        {
                            if (wu.strStatus.Contains("inconclusive"))
                            {
                                if (!bWantThisDevice(wu.Application, ref iUsed)) continue;
                                // problem:  the above counted a value towared the device but
                                // it may be excluded if the platform is not wanted
                                if (!bWantThisPlatform(wu.Application))
                                {
                                    Debug.Assert(iUsed >= 0);
                                    DVcnt[iUsed]--; // fixup code
                                    continue;
                                }
                                c = new TreeNode();
                                c.Text = wu.strStatus + ": " + wu.strComputer;
                                n.Nodes.Add(c);
                                bShowedSomething = true;
                                continue;
                            }
         
                        }
                        if (bShowFail)
                        {
                            if (wu.strStatus.Contains("error") ||
                                wu.strStatus.Contains("Error") ||
                                wu.strStatus.Contains("no response") ||
                                wu.strStatus.Contains("Aborted"))
                            {
                                bShowedSomething = true;
                                if (!bWantThisDevice(wu.Application, ref iUsed)) continue;
                                // problem:  the above counted a value towared the device but
                                // it may be excluded if the platform is not wanted
                                if (!bWantThisPlatform(wu.Application))
                                {
                                    Debug.Assert(iUsed >= 0);
                                    DVcnt[iUsed]--; // fixup code
                                    continue;
                                }
                                c = new TreeNode();
                                c.Text = wu.strStatus + ": " + wu.strComputer;
                                c.ForeColor = Color.Red;
                                n.Nodes.Add(c);
                            }
                        }
                    }

                    if(bShowedSomething)
                    {
                        c = new TreeNode();
                        c.Text = wu.Application;
                        n.Nodes.Add(c);
                    }
                }
                if(n.Nodes.Count > 0)
                    tv_projapps.Nodes.Add(n);
                if (bExpandAll)
                    n.ExpandAll();
            }
            SetFilterCount();
        }

        public void RevealApps()
        {
            GetFilters();
            GetTreeLayout();
            tv_projapps.Nodes.Clear();
            ShowTree();
        }

        public void GetTreeLayout()
        {
            foreach (RadioButton rb in gb_Reveal.Controls)
            {
                if (rb.Checked)
                {
                    ShowType = (eShowType)Convert.ToInt32(rb.Tag.ToString());
                    break;
                }
            }
            bShowInconclusive = (ShowType == eShowType.eShowInc);
            bShowFail = (ShowType == eShowType.eShowFail);
            bShowSuccess = (ShowType == eShowType.eShowSucc);
            bShowAll = (ShowType == eShowType.eShowAllcol) || (ShowType == eShowType.eShowAllexp);
            bExpandAll = (ShowType != eShowType.eShowAllcol);

            tv_projapps.Nodes.Clear();

        }

        private void rbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }

        private void rbShowHis_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }

        private void rbShowUnk_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }

        private void rbShowStats_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }

        private void rbExpandAll_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            GetFilters();
            GetTreeLayout();
            ShowTree();
        }

        private void ChangeFilters(bool b)
        {
            foreach (Control cc in gBoxPlatforms.Controls)
            {
                if(cc is CheckBox)
                {
                    CheckBox c = (CheckBox)cc;
                    c.Checked = b;
                }
            }
            foreach (Control cc in gBoxDevices.Controls)
            {
                if (cc is CheckBox)
                {
                    CheckBox c = (CheckBox)cc;
                    c.Checked = b;
                }
            }
        }

        private string SetValue(string s, int v)
        {
            int i = s.IndexOf(' ');
            if (i > 0) s = s.Substring(0, i);
            if (v == 0) return s;
            return s + " (" + v.ToString() + ")";
        }

        // seems forach starts at bottom and goes up
        private void SetFilterCount()
        {
            int iWhich, iValue;
            string s;
            iWhich = 3;
            foreach (Control cc in gBoxPlatforms.Controls)
            {
                if (cc is CheckBox)
                {
                    iValue = PFcnt[iWhich];
                    CheckBox c = (CheckBox)cc;
                    s = c.Text;
                    c.Text = SetValue(s, iValue);
                    iWhich--;
                }
            }
            iWhich = 3;
            foreach (Control cc in gBoxDevices.Controls)
            {
                if (cc is CheckBox)
                {
                    iValue = DVcnt[iWhich];
                    CheckBox c = (CheckBox)cc;
                    s = c.Text;
                    c.Text = SetValue(s, iValue);
                    iWhich--;
                }
            }
        }


        private void btnSetFilters_Click(object sender, EventArgs e)
        {
            ChangeFilters(true);
        }

        private void btnClrFilter_Click(object sender, EventArgs e)
        {
            ChangeFilters(false);
        }

        private void btnSaveFilters_Click(object sender, EventArgs e)
        {
            GetFilters();
            Properties.Settings.Default.bShowWIN = bShowWIN;
            Properties.Settings.Default.bShowAPP = bShowAPP;
            Properties.Settings.Default.bShowLIN = bShowLIN;
            Properties.Settings.Default.bShowAND = bShowAND;
            Properties.Settings.Default.bShowATI = bShowATI;
            Properties.Settings.Default.bShowNVI = bShowNVI;
            Properties.Settings.Default.bShowINT = bShowINT;
            Properties.Settings.Default.bShowCPU = bShowCPU;
            Properties.Settings.Default.Save();
        }

        private void rbIncon_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }
    }
}
