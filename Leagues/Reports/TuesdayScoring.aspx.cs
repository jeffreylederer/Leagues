using Leagues.Code;
using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Linq;

namespace Leagues.Reports
{
    public partial class TuesdayScoring : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var weekid = int.Parse(Request.QueryString["weekid"]);
                
               // Set report mode for local processing.
                rv1.ProcessingMode = ProcessingMode.Local;

                rv1.LocalReport.DataSources.Clear();
                
                var ds = new LeaguesDS();
                var WeekDate = "";
                bool IsBye = false;

                using (LeaguesEntities db = new LeaguesEntities())
                {
                    foreach (var item in db.TuesdayMatches.Where(x=>x.GameDate==weekid && x.Rink!=-1).OrderBy(x => x.Rink))
                    {
                        ds.Game.AddGameRow(item.Team1, 
                            item.TuesdayTeam.Player.NickName + ", " + item.TuesdayTeam.Player1.NickName,
                            item.Team2.Value,
                            item.TuesdayTeam1.Player.NickName + ", " + item.TuesdayTeam1.Player1.NickName,
                            item.Team1Score,
                            item.Team2Score, item.Rink);
                    }
                    WeekDate = db.TuesdaySchedules.Find(weekid).GameDateFormatted;
                    var matches = db.TuesdayMatches.Where(x => x.Rink == -1 && x.GameDate == weekid);
                    if (matches.Count() > 0)
                    {
                        var match = matches.First();

                        ds.Byes.AddByesRow(match.TuesdaySchedule.GameDateFormatted, match.Team1,
                            match.TuesdayTeam.Player.NickName + ", " + match.TuesdayTeam.Player1.NickName);
                        rv1.LocalReport.DataSources.Add(new ReportDataSource("Bye", ds.Byes.Rows));
                        IsBye = true;
                    }
                    else
                    {
                        rv1.LocalReport.DataSources.Add(new ReportDataSource("Bye", new System.Data.DataTable()));
                    }

                }


            rv1.LocalReport.DataSources.Add(new ReportDataSource("Game", ds.Game.Rows));
            rv1.LocalReport.DataSources.Add(new ReportDataSource("Stand",CalculateStandings.Tuesday(weekid).Rows));

            var p1 = new ReportParameter("WeekDate", WeekDate);
            var p2 = new ReportParameter("League", "Tuesday");
            var p3 = new ReportParameter("IsBye", IsBye ? "1" : "0");
            rv1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

            //parameters
            rv1.ShowToolBar = true;

                // Refresh the ReportViewer.
                rv1.LocalReport.Refresh();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var weekid = int.Parse(Request.QueryString["weekid"]);
                
                hyReturn.NavigateUrl = $"/TuesdayMatches/Index?ScheduleID={weekid}";
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }
}