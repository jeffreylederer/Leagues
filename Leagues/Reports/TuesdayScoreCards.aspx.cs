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

                
                var matches = new List<Tuesday_GetMatchAll_Result>();
                var ds = new LeaguesDS();
                using (LeagueEntities db = new LeagueEntities())
                {
                    foreach (var item in db.TuesdaySchedules)
                    {
                        ds.Matches.AddMatchesRow(item.id, item.GameDateFormatted);
                    }
                }

                rv1.LocalReport.DataSources.Add(new ReportDataSource("Matches", ds.Matches.Rows));
                rv1.LocalReport.ReportPath = "./Reports/ReportFiles/AllScoreCards.rdlc";

                    

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
                                match.TuesdayTeam.Player.FullName,
                                match.TuesdaySchedule.GameDateFormatted,
                                match.TuesdayTeam.Player1.FullName,
                                match.TuesdayTeam1.Player.FullName,
                                match.TuesdayTeam1.Player1.FullName,
                                match.Team1.ToString(),
                                match.Team2.ToString());
                            e.DataSources.Add(new ReportDataSource("Match", ds.TuesdayScoreCards.Rows));
                            break;

                        case "WeekScoreCards":
                            var weekid = int.Parse(e.Parameters["weekid"].Values[0]);
                            var matches = new List<Tuesday_GetMatchAll_Result>();
                            using (var en = db.Tuesday_GetMatchAll(weekid).GetEnumerator())
                            {
                                en.MoveNext();
                                while (en.Current != null)
                                {
                                    matches.Add(en.Current);
                                    en.MoveNext();
                                }
                            }
                           
                            foreach (var item in matches)
                            {
                                ds.ScoreWeek.AddScoreWeekRow(item.ID);
                            }
                            e.DataSources.Add(new ReportDataSource("Week", ds.ScoreWeek.Rows));
                            break;
                    }
                }
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }
}