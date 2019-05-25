using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;



namespace Leagues.Reports
{
    public partial class TuesdayScoreCards : System.Web.UI.Page
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
                string date;
                using (LeaguesEntities db = new LeaguesEntities())
                {
                    foreach (var item in db.Tuesday_GetMatchAll(weekid))
                    {
                        ds.TuesdayScoreCards.AddTuesdayScoreCardsRow(
                            item.Rink.ToString(),
                            item.Skip1,
                            item.Date,
                            item.Lead1,
                            item.Skip2,
                            item.Lead2,
                            item.Team1.ToString(),
                            item.Team2.ToString());
                    }
                }
                rv1.LocalReport.ReportPath = "./Reports/ReportFiles/TuesdayScoreCard.rdlc";
                rv1.LocalReport.DataSources.Add(new ReportDataSource("Match", ds.TuesdayScoreCards.Rows));

                
               // Refresh the ReportViewer.
               rv1.LocalReport.Refresh();
            }
        }



        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var weekid = int.Parse(Request.QueryString["weekid"]);
                hyReturn.NavigateUrl = $"/TuesdayMatches/Index/?ScheduleID={weekid}";
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }
}