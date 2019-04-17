﻿using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI.WebControls;

namespace Leagues.Reports
{
    public partial class TuesdayTeamsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rv1.Height = Unit.Parse("100%");
                rv1.Width = Unit.Parse("100%");
                rv1.CssClass = "table";

                // Set report mode for local processing.
                rv1.ProcessingMode = ProcessingMode.Local;

                // Validate report source.
                //var rptPath = Server.MapPath(@"./Reports/BowlsInventory.rdlc");

                //if(!File.Exists(rptPath))
                //    return;

                // Set report path.
                //this.rv1.LocalReport.ReportPath = rptPath;

                rv1.LocalReport.DataSources.Clear();

                var ds = new LeaguesDS();
                using (LeagueEntities db = new LeagueEntities())
                {
                    foreach (var item in db.TuesdayTeams)
                    {
                        ds.TuesdayTeam.AddTuesdayTeamRow(item.id, item.Player.FullName, item.Lead.HasValue?item.Player1.FullName:"");
                    }
                }

                rv1.LocalReport.DataSources.Add(new ReportDataSource("Team", ds.TuesdayTeam.Rows));

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