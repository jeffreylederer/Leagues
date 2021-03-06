﻿using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Linq;
using System.Web.UI.WebControls;


namespace Leagues.Reports
{
    public partial class WednesdayTeamsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
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
                using (LeaguesEntities db = new LeaguesEntities())
                {
                    var list = db.WednesdayTeams.OrderBy(x => x.id).ToList();
                    foreach (var item in list)
                    {
                        ds.WednesdayTeam.AddWednesdayTeamRow(item.id, item.Player.FullName, item.ViceSkip.HasValue ? item.Player1.FullName : "",
                            item.Lead.HasValue ? item.Player2.FullName : "");
                    }
                }

                rv1.LocalReport.DataSources.Add(new ReportDataSource("Team", ds.WednesdayTeam.Rows));

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