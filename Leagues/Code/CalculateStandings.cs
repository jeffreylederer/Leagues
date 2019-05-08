﻿using System;
using System.Collections.Generic;
using System.Linq;
using Leagues.Models;
using Leagues.Reports;

namespace Leagues.Code
{
    public static class CalculateStandings
    {

        public static LeaguesDS.StandingDataTable Tuesday(int weekid)
        {
            var ds = new LeaguesDS();
            using (var db = new LeagueEntities())
            {
                var list = new List<Standing>();
                foreach (var team in db.TuesdayTeams)
                {
                    list.Add(new Standing()
                    {
                        TeamNumber = team.id,
                        Wins = 0,
                        Loses = 0,
                        TotalScore = 0,
                        Skip = team.Player.FullName
                    });
                }
                foreach (var week in db.TuesdaySchedules.Where(x => x.id <= weekid))
                {
                    foreach (var match in db.TuesdayMatches.Where(x => x.GameDate == week.id))
                    {
                        if (match.Team1Score > match.Team2Score)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            var loser = list.Find(x => x.TeamNumber == match.Team2);
                            winner.Wins++;
                            loser.Loses++;
                            winner.TotalScore += match.Team1Score;
                            loser.TotalScore += match.Team2Score;
                        }
                        else
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team2);
                            var loser = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            loser.Loses++;
                            winner.TotalScore += match.Team2Score;
                            loser.TotalScore += match.Team1Score;

                        }
                    }
                    list.Sort((a, b) => (b.Wins * 1000 + b.TotalScore).CompareTo(a.Wins * 1000 + a.TotalScore));

                    int place = 1;
                    foreach (var item in list)
                    {
                        ds.Standing.AddStandingRow(item.TeamNumber, item.Skip, item.TotalScore, place++,item.Wins, item.Loses
                            );
                    }
                }
            }
            return ds.Standing;
        }

        public static LeaguesDS.StandingDataTable Wednesday(int weekid)
        {
            var ds = new LeaguesDS();
            using (var db = new LeagueEntities())
            {
                var list = new List<Standing>();
                foreach (var team in db.WednesdayTeams)
                {
                    list.Add(new Standing()
                    {
                        TeamNumber = team.id,
                        Wins = 0,
                        Loses = 0,
                        TotalScore = 0,
                        Skip = team.Player.FullName
                    });
                }
                foreach (var week in db.WednesdaySchedules.Where(x => x.id < weekid))
                {
                    foreach (var match in db.WednesdayMatches.Where(x => x.GameDate == week.id))
                    {
                        if (match.Team1Score > match.Team2Score)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            var loser = list.Find(x => x.TeamNumber == match.Team2);
                            winner.Wins++;
                            winner.TeamNumber += winner.TeamNumber;
                            winner.TeamNumber += loser.TeamNumber;
                        }
                        else
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team2);
                            var loser = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            winner.TeamNumber += winner.TeamNumber;
                            winner.TeamNumber += loser.TeamNumber;

                        }
                    }
                    list.Sort((a, b) => (a.Wins * 1000 + a.TotalScore).CompareTo(b.Wins * 1000 + b.TotalScore));

                    int place = 1;
                    foreach (var item in list)
                    {
                        ds.Standing.AddStandingRow(item.TeamNumber, item.Skip, item.TotalScore, item.Wins, item.Loses,
                            place++);
                    }
                }
            }
            return ds.Standing;
        }
    }


    internal class Standing
    {
        public int TeamNumber { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int TotalScore { get; set; }
        public string Skip { get; set; }
        
    }
}