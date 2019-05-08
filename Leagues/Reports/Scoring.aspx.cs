﻿using Leagues.Code;
using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Leagues.Reports
{
    public partial class Scoring : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var weekid = int.Parse(Request.QueryString["weekid"]);
                var league = Request.QueryString["league"];
                rv1.Height = Unit.Parse("100%");
                rv1.Width = Unit.Parse("100%");
                rv1.CssClass = "table";

                // Set report mode for local processing.
                rv1.ProcessingMode = ProcessingMode.Local;

                rv1.LocalReport.DataSources.Clear();

                var ds = new LeaguesDS();
                var WeekDate = "";
                using (LeagueEntities db = new LeagueEntities())
                {
                    foreach (var item in db.TuesdayMatches.Where(x=>x.GameDate==weekid).OrderBy(x => x.Rink))
                    {
                        ds.Game.AddGameRow(item.Team1, item.TuesdayTeam.Player.FullName,
                            item.Team2, item.TuesdayTeam1.Player.FullName, item.Team1Score,
                            item.Team2Score, item.Rink);
                    }
                    WeekDate = db.TuesdaySchedules.Find(weekid).GameDateFormatted;
                }

                rv1.LocalReport.DataSources.Add(new ReportDataSource("Game", ds.Game.Rows));
                rv1.LocalReport.DataSources.Add(new ReportDataSource("Stand", 
                    league=="Tuesday"?CalculateStandings.Tuesday(weekid).Rows: CalculateStandings.Wednesday(weekid).Rows));

                var p1 = new ReportParameter("WeekDate", WeekDate);
                var p2 = new ReportParameter("League", league);
                rv1.LocalReport.SetParameters(new ReportParameter[] { p1, p2});

                //parameters
                rv1.ShowToolBar = true;

                // Refresh the ReportViewer.
                rv1.LocalReport.Refresh();
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }
}