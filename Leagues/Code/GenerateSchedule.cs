using Leagues.Models;
using Leagues.Reports;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Leagues.Code
{
    public static  class GenerateSchedule
    {
        public static string Tuesday()
        {
            var topLine = new StringBuilder();
            using (var db = new LeaguesEntities())
            {
                var teams = db.TuesdayTeams.Count();
                if (teams == 0)
                    return "<bold>No Teams</bold>";
                var rinks = teams / 2;
                

                for (var rink =0;rink<rinks;rink++)
                {
                    topLine.Append($"{rink},");
                }

                var tline = topLine.ToString();
                topLine.Append(tline.Substring(0, tline.Length - 1) + "<br/>");
                
                var weeks = db.TuesdaySchedules.SortBy("Id");
                foreach (var week in weeks)
                {
                    var matches = db.TuesdayMatches.Where(x => x.GameDate == week.id).OrderBy(x => x.Rink);
                    var weekLine = new StringBuilder();
                    foreach (var match in matches)
                    {
                        if(match.Rink != -1)
                            weekLine.Append($"{match.Team1}-{match.Team2},");
                    }
                    var wline = weekLine.ToString();
                    if (wline.Length == 0)
                        return "<bold>No matches</bold>";
                    topLine.Append(wline.Substring(0, wline.Length-1)+"<br>");
                }
            }
            return topLine.ToString();
        }

        public static string Wednesday()
        {
            var topLine = new StringBuilder();
            using (var db = new LeaguesEntities())
            {
                var teams = db.WednesdayTeams.Count();
                if (teams == 0)
                    return "<bold>No Teams</bold>";
                var rinks = teams / 2;


                for (var rink = 0; rink < rinks; rink++)
                {
                    topLine.Append($"{rink},");
                }

                var tline = topLine.ToString();
                topLine.Append(tline.Substring(0, tline.Length - 1) + "<br/>");

                var weeks = db.WednesdaySchedules.SortBy("Id");
                foreach (var week in weeks)
                {
                    var matches = db.WednesdayMatches.Where(x => x.GameDate == week.id).OrderBy(x => x.Rink);
                    var weekLine = new StringBuilder();
                    foreach (var match in matches)
                    {
                        if (match.Rink != -1)
                            weekLine.Append($"{match.Team1}-{match.Team2},");
                    }
                    var wline = weekLine.ToString();
                    if (wline.Length == 0)
                        return "<bold>No matches</bold>";
                    topLine.Append(wline.Substring(0, wline.Length - 1) + "<br>");
                }
            }
            return topLine.ToString();
        }

    }
}