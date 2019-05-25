using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;



namespace Leagues.Reports
{
    public partial class WednesdayScoreCards : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                // Set report mode for local processing.
                rv1.ProcessingMode = ProcessingMode.Local;
                rv1.LocalReport.DataSources.Clear();
                var weekid = int.Parse(Request.QueryString["weekid"]);
               
                var ds = new LeaguesDS();
                using (LeaguesEntities db = new LeaguesEntities())
                {
                    foreach (var item in db.Wednesday_GetMatchAll(weekid))
                        ds.WednesdayScoreCards.AddWednesdayScoreCardsRow(
                            item.Rink.ToString(),
                            item.Skip1,
                            item.Date,
                            item.Lead1,
                            item.Skip2,
                            item.Lead2,
                            item.Team1.ToString(),
                            item.Team2.ToString(),
                            item.Vice1,
                            item.Vice2);
                }
                rv1.LocalReport.ReportPath = "./Reports/ReportFiles/WednesdayScoreCard.rdlc";
                rv1.LocalReport.DataSources.Add(new ReportDataSource("Match", ds.WednesdayScoreCards.Rows));
                
               // Refresh the ReportViewer.
               rv1.LocalReport.Refresh();
            }
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var weekid = int.Parse(Request.QueryString["weekid"]);
                hyReturn.NavigateUrl = $"/WednesdayMatches/Index/?ScheduleID={weekid}";
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }
}