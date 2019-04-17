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
        public InfoForm(Form refForm)
        {
            InitializeComponent();
            btf = (BTHistory) refForm;
            KnownProjApps = btf.KnownProjApps;
            TreeNode mainNode = new TreeNode();
            mainNode.Name = "History";
            mainNode.Text = "Projects";
            int i = 1;
            tv_projapps.Nodes.Add(mainNode);

            foreach (cKnownProjApps kpa in KnownProjApps)
            {
                TreeNode n = new TreeNode();
                n.Name = i.ToString(); ;
                n.Text = kpa.ProjName;
                foreach(cAppName appName in kpa.KnownApps)
                {
                    TreeNode c = new TreeNode();
                    c.Text = appName.Name;
                    n.Nodes.Add(c);
                }
                tv_projapps.Nodes.Add(n);
            }
        }
    }
}
