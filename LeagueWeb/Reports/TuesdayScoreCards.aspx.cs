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
        private List<Tuesday_GetMatchAll_Result> matches;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rv1.Height = Unit.Parse("100%");
                rv1.Width = Unit.Parse("100%");
                rv1.CssClass = "table";

                // Set report mode for local processing.
                rv1.ProcessingMode = ProcessingMode.Local;
                rv1.LocalReport.DataSources.Clear();

                var ds = new LeaguesDS();
                using (LeaguesEntities db = new LeaguesEntities())
                {
                    foreach (var item in db.TuesdaySchedules)
                    {
                        ds.Matches.AddMatchesRow(item.id, item.GameDateFormatted);
                    }
                    matches = new List<Tuesday_GetMatchAll_Result>();
                    using (var en = db.Tuesday_GetMatchAll().GetEnumerator())
                    {
                        en.MoveNext();
                        while (en.Current != null)
                        {
                            matches.Add(en.Current);
                            en.MoveNext();
                        }
                    }
                }

                rv1.LocalReport.DataSources.Add(new ReportDataSource("Matches", ds.Matches.Rows));

                
                rv1.ShowToolBar = true;

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
                        case "TuesdayScoreCard":
                            var id = int.Parse(e.Parameters["matchid"].Values[0]);
                            var match = matches.Find(x => x.ID == id);

                            ds.TuesdayScoreCards.AddTuesdayScoreCardsRow(
                                match.Rink.ToString(),
                                match.Team1Skip,
                                match.GameDateFormatted,
                                match.Team1Lead,
                                match.Team2Skip,
                                match.Team2Lead,
                                match.Team1.ToString(),
                                match.Team2.ToString());
                            e.DataSources.Add(new ReportDataSource("Match", ds.TuesdayScoreCards.Rows));
                            break;

                        case "WeekScoreCard":
                            var week = int.Parse(e.Parameters["weekid"].Values[0]);

                            var matchweek = matches.FindAll(x => x.WeekId == week);
                            foreach (var item in matchweek)
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