using Leagues.Code;
using Leagues.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Linq;

namespace Leagues.Reports
{
    public partial class Byes : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               var league = Request.QueryString["league"];

                // Set report mode for local processing.
                rv1.ProcessingMode = ProcessingMode.Local;

                rv1.LocalReport.DataSources.Clear();

                var ds = new LeaguesDS();
                using (LeaguesEntities db = new LeaguesEntities())
                {
                    if (league == "Tuesday")
                    {
                        foreach (var item in db.TuesdayMatches.Where(x=>x.Rink == -1))
                        {
                            ds.Byes.AddByesRow(item.TuesdaySchedule.GameDate.ToShortDateString(), item.Team1,
                                item.TuesdayTeam.Player.NickName + ", " + item.TuesdayTeam.Player1.NickName);
                        }
                    }
                    else
                    {
                        foreach (var item in db.WednesdayMatches.Where(x=> x.Rink == -1))
                        {
                            ds.Byes.AddByesRow(item.WednesdaySchedule.GameDate.ToShortDateString(), item.Team1,
                                item.WednesdayTeam.Player.NickName + ", " + item.WednesdayTeam.Player1.NickName);
                        }
                    }
                }

                rv1.LocalReport.DataSources.Add(new ReportDataSource("Byes", ds.Byes.Rows));

                var p2 = new ReportParameter("League", league);
                rv1.LocalReport.SetParameters(new ReportParameter[] { p2 });

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
                var league = Request.QueryString["league"];

                hyReturn.NavigateUrl = $"/{league}Matches/Index?ScheduleID=1";
            }
        }

        protected void Rv1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblError.Text = e.Exception.Message;
            e.Handled = true;
        }
    }

}