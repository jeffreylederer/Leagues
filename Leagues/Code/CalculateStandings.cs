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
            var list = new List<Standing>();
            using (var db = new LeaguesEntities())
            {
                var list1 = db.TuesdayTeams.ToList();
                foreach (var team in list1)
                {
                    list.Add(new Standing()
                    {
                        TeamNumber = team.id,
                        Wins = 0,
                        Loses = 0,
                        TotalScore = 0,
                        Players = team.Player.NickName + ", " + team.Player1.NickName
                    });
                }
                var list4 =  db.TuesdaySchedules.Where(x => x.id <= weekid).ToList();
                foreach (var week in list4)
                {
                    if (week.IsCancelled)
                        continue;
                    var total = 0;
                    var numMatches = 0;
                    var bye = false;
                    int forfeit = 0;

                    var list2 = db.TuesdayMatches.Where(x => x.GameDate == week.id).ToList();
                    foreach (var match in list2)
                    {
                        if (match.Team1Score > match.Team2Score && match.Rink != -1 && match.Forfeit == 1)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            var loser = list.Find(x => x.TeamNumber == match.Team2);
                            winner.Wins++;
                            loser.Loses++;
                            winner.TotalScore += Math.Min(20, match.Team1Score);
                            loser.TotalScore += Math.Min(20, match.Team2Score);
                            total += Math.Min(20, match.Team1Score);
                            numMatches++;
                        }
                        else if (match.Rink != -1 && match.Forfeit == 1)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team2);
                            var loser = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            loser.Loses++;
                            winner.TotalScore += Math.Min(20, match.Team2Score);
                            loser.TotalScore += Math.Min(20, match.Team1Score);
                            total += Math.Min(20, match.Team2Score);
                            numMatches++;
                        }
                        else if (match.Rink != -1 && match.Forfeit == 3)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            var loser = list.Find(x => x.TeamNumber == match.Team2);
                            forfeit++;
                            winner.Wins++;
                            loser.Loses++;
                        }
                        else if (match.Rink != -1 && match.Forfeit == 2)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team2);
                            var loser = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            loser.Loses++;
                            forfeit++;
                        }
                        else
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            bye = true;
                        }

                    }
                    if (bye || forfeit > 0)
                    {
                        var list3 = db.TuesdayMatches.Where(x => x.GameDate == week.id).ToList();
                        foreach (var match in list3)
                        {
                            if (match.Rink != -1 && match.Forfeit == 3)
                            {
                                var winner = list.Find(x => x.TeamNumber == match.Team1);
                                winner.TotalScore += total / numMatches;
                            }
                            else if (match.Rink != -1 && match.Forfeit == 2)
                            {
                                var winner = list.Find(x => x.TeamNumber == match.Team2);
                                winner.TotalScore += total / numMatches;
                            }
                            else if (match.Rink == -1)
                            {
                                var winner = list.Find(x => x.TeamNumber == match.Team1);
                                winner.TotalScore += total / numMatches;
                            }
                        }
                    }
                    

                    
                }
            }
            int place = 1;
            list.Sort((a, b) => (b.Wins * 1000 + b.TotalScore).CompareTo(a.Wins * 1000 + a.TotalScore));
            foreach (var item in list)
            {
                ds.Standing.AddStandingRow(item.TeamNumber, item.Players, item.TotalScore, place++, item.Wins, item.Loses);
            }
            return ds.Standing;
        }

        public static LeaguesDS.StandingDataTable Wednesday(int weekid)
        {
            var ds = new LeaguesDS();
            var list = new List<Standing>();
            using (var db = new LeaguesEntities())
            {
                var list1 = db.WednesdayTeams.ToList();
                foreach (var team in list1)
                {
                    list.Add(new Standing()
                    {
                        TeamNumber = team.id,
                        Wins = 0,
                        Loses = 0,
                        TotalScore = 0,
                        Players = team.Player.NickName + ", " + team.Player1.NickName + ", " + team.Player2.NickName
                    });
                }
                var list2 = db.WednesdaySchedules.Where(x => x.id <= weekid).ToList();
                foreach (var week in list2)
                {
                    if (week.IsCancelled)
                        continue;
                    var total = 0;
                    var numMatches = 0;
                    var bye = false;
                    int forfeit = 0;
                    var list4 = db.WednesdayMatches.Where(x => x.GameDate == week.id).ToList();
                    foreach (var match in list4)
                    {
                        
                        if (match.Team1Score > match.Team2Score && match.Rink != -1 && match.Forfeit == 1)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            var loser = list.Find(x => x.TeamNumber == match.Team2);
                            winner.Wins++;
                            loser.Loses++;
                            winner.TotalScore += Math.Min(20, match.Team1Score);
                            loser.TotalScore += Math.Min(20, match.Team2Score);
                            total += Math.Min(20, match.Team1Score);
                            numMatches++;
                        }
                        else if (match.Rink != -1 && match.Forfeit == 1)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team2);
                            var loser = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            loser.Loses++;
                            winner.TotalScore += Math.Min(20, match.Team2Score);
                            loser.TotalScore += Math.Min(20, match.Team1Score);
                            total += Math.Min(20, match.Team2Score);
                            numMatches++;
                        }
                        else if (match.Rink != -1 && match.Forfeit == 3)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            var loser = list.Find(x => x.TeamNumber == match.Team2);
                            forfeit++;
                            winner.Wins++;
                            loser.Loses++;
                        }
                        else if (match.Rink != -1 && match.Forfeit == 2)
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team2);
                            var loser = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            loser.Loses++;
                            forfeit++;
                        }
                        else 
                        {
                            var winner = list.Find(x => x.TeamNumber == match.Team1);
                            winner.Wins++;
                            bye = true;
                        }

                    }
                    if (bye || forfeit > 0)
                    {
                        var list3 = db.WednesdayMatches.Where(x => x.GameDate == week.id).ToList();
                        foreach (var match in list3)
                        {
                             if (match.Rink != -1 && match.Forfeit == 3)
                            {
                                var winner = list.Find(x => x.TeamNumber == match.Team1);
                                winner.TotalScore += total / numMatches;
                            }
                            else if (match.Rink != -1 && match.Forfeit == 2)
                            {
                                var winner = list.Find(x => x.TeamNumber == match.Team2);
                                winner.TotalScore += total / numMatches;
                            }
                            else if (match.Rink == -1)
                            {
                                var winner = list.Find(x => x.TeamNumber == match.Team1);
                                winner.TotalScore += total / numMatches;
                            }
                        }
                    }
                    
                }
            }
            list.Sort((a, b) => (b.Wins * 1000 + b.TotalScore).CompareTo(a.Wins * 1000 + a.TotalScore));

            int place = 1;
            foreach (var item in list)
            {
                ds.Standing.AddStandingRow(item.TeamNumber, item.Players, item.TotalScore, place++, item.Wins, item.Loses);
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
        public string Players { get; set; }
        
    }
}