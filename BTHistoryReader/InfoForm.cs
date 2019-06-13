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
    public partial class InfoForm : Form
    {

        public List<cKnownProjApps> KnownProjApps;
        public BTHistory btf;
        public enum eShowType
        {
            eShowAllcol = 0,    // only one shown as collapsed (default)
            eShowAllexp = 1,    // these must match the "tag" in radio button
            eShowHis = 2,
            eShowUnk = 3,
            eShowStats = 4
        }
        public TreeNode mainNode;

        public static eShowType ShowType = eShowType.eShowAllcol;

        public InfoForm(Form refForm)
        {
            InitializeComponent();
            btf = (BTHistory) refForm;
            KnownProjApps = btf.KnownProjApps;
            mainNode = new TreeNode();
            mainNode.Text = btf.CurrentSystem;
            tv_projapps.Nodes.Add(mainNode);
            // GetTreeLayout(); // not used as better to show last tree layout and not start over
            ShowTree();
        }

        public void ShowTree()
        {
            int i = 1;
            TreeNode n, c, dn;
            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                if (kpa.nAppsUsed == 0 && ShowType==eShowType.eShowHis)  // do not show unused projects
                    continue;
                if (ShowType == eShowType.eShowUnk)
                {
                    if(!kpa.bIsUnknown)
                    {
                        if (!kpa.bContainsUnknownApps)
                            continue;
                    }
                }
                n = new TreeNode();
                n.Name = i.ToString(); ;
                n.Text = kpa.ProjName;
                if (kpa.bIsUnknown)
                {
                    n.ForeColor = System.Drawing.Color.Red;
                }
                else n.ForeColor = System.Drawing.Color.DarkBlue;
                foreach (cAppName appName in kpa.KnownApps)
                {
                    if (appName.nAppEntries == 0 &&
                        ((ShowType == eShowType.eShowHis) || (ShowType == eShowType.eShowStats))) // do not show unused apps
                        continue;
                    c = new TreeNode();
                    c.Text = appName.Name;
                    if (appName.nAppEntries > 0)     // http://www.99colors.net/dot-net-colors
                    {
                        if (appName.bIsUnknown)
                        {
                            c.ForeColor = System.Drawing.Color.Red;
                        }
                        else c.ForeColor = System.Drawing.Color.DarkBlue;
                        if(ShowType == eShowType.eShowStats)
                        {
                            if (appName.bNoResults)
                                continue;
                            c.Text += " [" + appName.strAvgStd + "]";
                        }
                        else
                        {
                            c.Text += " (" + appName.nAppEntries.ToString() + ")";
                        }
                    }
                    foreach (cNameValue nv in appName.DataName.DataNameInfo)
                    {
                        dn = new TreeNode();
                        dn.Text = nv.DataName + " (" + nv.ElapsedTime.Count.ToString() + ")" ;
                        dn.ForeColor = Color.ForestGreen;
                        c.Nodes.Add(dn);
                    }
                    n.Nodes.Add(c);
                }
                tv_projapps.Nodes.Add(n);
            }
            if (ShowType == eShowType.eShowAllcol)
                mainNode.Collapse(true);
            else
            {
                foreach (TreeNode node in tv_projapps.Nodes)
                {
                    node.Expand();
                    foreach (TreeNode subnode in node.Nodes)
                        subnode.Expand();
                }
            }
        }

        public void RevealApps()
        {
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

        private void InfoForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {

        }

        private void rbShowStats_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }

        private void rbExpandAll_CheckedChanged(object sender, EventArgs e)
        {
            RevealApps();
        }
    }
}
