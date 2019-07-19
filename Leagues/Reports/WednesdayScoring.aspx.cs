using Leagues.Code;
using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Linq;

namespace Leagues.Reports
{
    public partial class WednesdayScoring : System.Web.UI.Page
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
                bool isCancelled = false;

                using (LeaguesEntities db = new LeaguesEntities())
                {
                    var week = db.WednesdaySchedules.Find(weekid);
                    WeekDate = week.GameDateFormatted;
                    if (!week.IsCancelled)
                    {
                        var list = db.WednesdayMatches.Where(x => x.GameDate == weekid && x.Rink != -1)
                            .OrderBy(x => x.Rink).ToList();
                        foreach (var item in list)
                        {
                            var forfeit = "";
                            switch (item.Forfeit)
                            {
                                case 2:
                                    forfeit = item.Team1.ToString();
                                    break;
                                case 3:
                                    forfeit = item.Team2.ToString();
                                    break;
                            }
                            ds.Game.AddGameRow(item.Team1,
                                item.WednesdayTeam.Player.NickName + ", " + item.WednesdayTeam.Player1.NickName + ", " +
                                item.WednesdayTeam.Player2.NickName,
                                item.Team2.Value,
                                item.WednesdayTeam1.Player.NickName + ", " + item.WednesdayTeam1.Player1.NickName +
                                ", " + item.WednesdayTeam1.Player2.NickName,
                                item.Team1Score,
                                item.Team2Score, item.Rink, forfeit);
                        }

                        var matches = db.WednesdayMatches.Where(x => x.Rink == -1 && x.GameDate == weekid);
                        if (matches.Any())
                        {
                            var match = matches.First();

                            ds.Byes.AddByesRow(match.WednesdaySchedule.GameDateFormatted, match.Team1,
                                match.WednesdayTeam.Player.NickName + ", " + match.WednesdayTeam.Player1.NickName +
                                ", " + match.WednesdayTeam.Player2.NickName);
                            rv1.LocalReport.DataSources.Add(new ReportDataSource("Bye", ds.Byes.Rows));
                            IsBye = true;
                        }
                        else
                        {
                            rv1.LocalReport.DataSources.Add(new ReportDataSource("Bye", new System.Data.DataTable()));
                        }
                        rv1.LocalReport.DataSources.Add(new ReportDataSource("Game", ds.Game.Rows));
                    }
                    else
                    {
                        isCancelled = true;
                        rv1.LocalReport.DataSources.Add(new ReportDataSource("Game", new System.Data.DataTable()));
                        rv1.LocalReport.DataSources.Add(new ReportDataSource("Bye", new System.Data.DataTable()));
                    }

                }

                
                rv1.LocalReport.DataSources.Add(new ReportDataSource("Stand", CalculateStandings.Wednesday(weekid).Rows));

                var p1 = new ReportParameter("WeekDate", WeekDate);
                var p2 = new ReportParameter("League", "Wednesday");
                var p3 = new ReportParameter("IsBye", IsBye?"1":"0");
                var p4 = new ReportParameter("Cancelled", isCancelled ? "1" : "0");
                rv1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

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
                
                hyReturn.NavigateUrl = $"/WednesdayMatches/Index?ScheduleID={weekid}";
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }
}