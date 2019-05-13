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
                string date;
                using (LeaguesEntities db = new LeaguesEntities())
                {
                    foreach (var item in db.WednesdayMatches.Where(x => x.GameDate == weekid && x.Rink !=-1).SortBy("Rink")) 
                    {
                        ds.ScoreWeek.AddScoreWeekRow(item.id);
                    }
                    date = db.WednesdaySchedules.Find(weekid).GameDateFormatted;
                }
                rv1.LocalReport.ReportPath = "./Reports/ReportFiles/WednesdayWeekScoreCards.rdlc";
                rv1.LocalReport.DataSources.Add(new ReportDataSource("Week", ds.ScoreWeek.Rows));

                var p1 = new ReportParameter("WeekDate", date);
                rv1.LocalReport.SetParameters(new ReportParameter[] {p1});
                
               // Refresh the ReportViewer.
               rv1.LocalReport.Refresh();
            }
        }



        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            using (LeaguesEntities db = new LeaguesEntities())
            { 
                using (var ds = new LeaguesDS())
                {
                    switch (e.ReportPath)
                    {
                        case "WednesdayScoreCard":
                            var id = int.Parse(e.Parameters["matchid"].Values[0]);
                            var match = db.WednesdayMatches.Find(id);
                            ds.WednesdayScoreCards.AddWednesdayScoreCardsRow(
                                match.Rink.ToString(),
                                match.WednesdayTeam.Player.NickName,
                                match.WednesdaySchedule.GameDateFormatted,
                                match.WednesdayTeam.Player2.NickName,
                                match.WednesdayTeam1.Player.NickName,
                                match.WednesdayTeam1.Player2.NickName,
                                match.Team1.ToString(),
                                match.Team2.ToString(),
                                match.WednesdayTeam.Player1.NickName,
                                match.WednesdayTeam1.Player1.NickName);
                            e.DataSources.Add(new ReportDataSource("Match", ds.WednesdayScoreCards.Rows));
                            break;
                    }
                }
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