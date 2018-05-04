using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GRCearned
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

     
        protected void btnGetProj_Click(object sender, EventArgs e)
        {
            Response.Redirect("/HostProjectStats");
        }

        protected void btnCPID_Click(object sender, EventArgs e)
        {
            Response.Redirect("/GRCearnings");
        }
    }
}