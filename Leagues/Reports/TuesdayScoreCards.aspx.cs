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
                using (LeagueEntities db = new LeagueEntities())
                {
                    foreach (var item in db.TuesdayMatches.Where(x => x.GameDate == weekid).SortBy("Rink")) 
                    {
                        ds.ScoreWeek.AddScoreWeekRow(item.id);
                    }
                    date = db.TuesdaySchedules.Find(weekid).GameDateFormatted;
                }
                rv1.LocalReport.ReportPath = "./Reports/ReportFiles/TuesdayWeekScoreCards.rdlc";
                rv1.LocalReport.DataSources.Add(new ReportDataSource("Week", ds.ScoreWeek.Rows));

                var p1 = new ReportParameter("WeekDate", date);
                rv1.LocalReport.SetParameters(new ReportParameter[] {p1});
                
               // Refresh the ReportViewer.
               rv1.LocalReport.Refresh();
            }
        }



        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            using (LeagueEntities db = new LeagueEntities())
            {
                using (var ds = new LeaguesDS())
                {
                    switch (e.ReportPath)
                    {
                        case "TuesdayScoreCard":
                            var id = int.Parse(e.Parameters["matchid"].Values[0]);
                            var match = db.TuesdayMatches.Find(id);
                            ds.TuesdayScoreCards.AddTuesdayScoreCardsRow(
                                match.Rink.ToString(),
                                match.TuesdayTeam.Player.NickName,
                                match.TuesdaySchedule.GameDateFormatted,
                                match.TuesdayTeam.Player1.NickName,
                                match.TuesdayTeam1.Player.NickName,
                                match.TuesdayTeam1.Player1.NickName,
                                match.Team1.ToString(),
                                match.Team2.ToString());
                            e.DataSources.Add(new ReportDataSource("Match", ds.TuesdayScoreCards.Rows));
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