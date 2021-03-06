﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Leagues.Code;

namespace Leagues.Reports
{
	public partial class Schedule : System.Web.UI.Page
	{
		protected void Page_PreRender(object sender, EventArgs e)
		{
		    var league = Request.QueryString["League"];
		    lblLeague.Text = league;
		}

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var league = Request.QueryString["League"];
            string schedule = string.Empty;
            if (league == "Tuesday")
            {
                schedule = GenerateSchedule.Tuesday();
            }
            else
            {
                schedule = GenerateSchedule.Wednesday();
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"inline;filename={league}.csv");
            Response.Charset = "";
            Response.ContentType = "application/csv";
            Response.Output.Write(schedule);
            Response.Flush();
            Response.End();
        }
    }
}