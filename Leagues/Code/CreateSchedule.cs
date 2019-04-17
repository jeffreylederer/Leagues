using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leagues.Code
{
    public class CreateSchedule
    {
        public List<Match> DoIt(int numberofWeeks, int numberOfTeams)
        {
            var numberOfRinks = numberOfTeams / 2;
            bool byes = numberOfTeams != numberOfRinks * 2;
            var matches = new List<Match>();

            var teams = new List<TheSchedule>();
            for (int i = 0; i < numberOfTeams; i++)
            {
                teams.Add(new TheSchedule(numberofWeeks, numberOfTeams));
            }

            for (var w = 0; w < numberofWeeks; w++)
            {
                var numofMatches = 0;
                int rink = 0;
                while(numofMatches < numberOfRinks)
                {
                    for (var t1 = 0; t1 < numberOfTeams; t1++)
                    {
                        for (var t2 = 0; t2 < numberOfTeams; t2++)
                        {
                            var schedule1 = teams.Find(x => x.TeamNum == t1);
                            if (t2 != t1 && !schedule1.Opponents.Contains(t2) &&
                                matches.Any(x => x.WeekNum == w && x.Team1 != t1 && x.Team2 != t1
                                                 && x.Team1 != t2 && x.Team2 != t2))
                            {
                                schedule1.Opponents.Add(t1);
                                var schedule2 = teams.Find(x => x.TeamNum == t2);
                                matches.Add(new Match()
                                {
                                    WeekNum = w,
                                    Rink = rink+w % numberOfRinks,
                                    Team1 = t1 < t2 ? t1 : t2,
                                    Team2 = t1 < t2 ? t2 : t1
                                });
                                rink++;
                                break;
                            } // t2 loop
                        }
                        numofMatches++;
                        if (numberOfRinks == numofMatches)
                            break;
                    } // t1 loop
               } // while loop
            } //weeks loop
            return matches;
        }
    }

    internal class TheSchedule
    {
        public int TeamNum { get; set; }
        public List<int> Opponents { get; set; }

        public TheSchedule(int numOfWeeks, int teamNum)
        {
            Opponents = new List<int>();
            TeamNum = teamNum;
        }
    }


    public class Match
    {
        public int WeekNum { get; set; }
        public int Rink { get; set; }
        public int Team1 { get; set; }
        public int Team2 { get; set; }
    }
}