using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Leagues.Reports
{
    public partial class Site1 : MasterPage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (!HttpContext.Current.User.IsInRole("Admin"))
            {
                hyError.Visible = false;
                hyTCreate.Visible = false;
                hyTClear.Visible = false;
                hyWCreate.Visible = false;
                hyWClear.Visible = false;
                lblDashT.Visible = false;
                lblDashW.Visible = false;
            }
        }
    }
}