using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;


namespace Leagues.Code
{
    public class CreateSchedule
    {
        private const int BYE = -1;

        public List<Match> DoIt(int numberofWeeks, int numberOfTeams)
        {

            var Teams = numberOfTeams +  numberOfTeams % 2;
            var numberOfRinks = Teams / 2;
            bool bye = Teams != numberOfTeams;

            var matches = new List<Match>();

            int[] leftside = new int[numberOfRinks];
            int[] rightside = new int[numberOfRinks];
            if (bye)
            {
                leftside[0] = 0;
                rightside[0] = Teams - 1;
                matches.Add(new Match()
                {
                    Week = 0,
                    Rink = -1,
                    Team1 = 0,
                    Team2 =  -1
                });
                for (int r = 1; r < numberOfRinks; r++)
                {
                    leftside[r] = r;
                    rightside[r] = Teams - r - 1;
                    matches.Add(new Match()
                    {
                        Week = 0,
                        Rink = r - 1,
                        Team1 = r,
                        Team2 = Teams - r - 1
                    });
                }
            }
            else
            {
                for (int r = 0; r < numberOfRinks; r++)
                {
                    leftside[r] = r;
                    rightside[r] = Teams - r - 1;
                    matches.Add(new Match()
                    {
                        Week = 0,
                        Rink = r,
                        Team1 = r,
                        Team2 = Teams - r - 1
                    });
                }

            }
            

            for (int w = 1; w < numberofWeeks; w++)
            {
                int remainder = ShiftRight(leftside);
                int other = ShiftLeft(rightside, remainder);
                leftside[1] = other;
                int rink = 0;
                for (int r = 0; r < numberOfRinks; r++)
                {
                    var left = leftside[r];
                    var right = rightside[r];

                    var match = new Match()
                    {
                        Week = w,
                        Rink = rink,
                        Team1 = left < right? left: right,
                        Team2 = left < right ? right : left
                    };
                    if (bye && match.Team2 == Teams-1)
                    {
                        match.Team2 = -1;
                        match.Rink = -1;
                    }
                    else
                    {
                        rink++;
                    }
                    matches.Add(match);
                }
            }

            // bye fixup
            if (Teams != numberOfTeams)
            {
                for (int w = 1; w < numberofWeeks; w++)
                {
                    for (int r = 0; r < numberOfRinks; r++)
                    {
                    }
                }
            }
            matches.Sort((a,b) => (a.Week*100 + a.Rink+1).CompareTo(b.Week*100+b.Rink+1));
            return matches;

        }

        private int ShiftRight(int[] leftside)
        {
            int remainder = leftside[leftside.Length - 1];
            for (int i = leftside.Length-2; i > 0; i--)
            {
                leftside[i+1] = leftside[i];
            }
            return remainder;
        }

        private int ShiftLeft(int[] rightside, int remainder)
        {
            int other = rightside[0];
            for (int i = 0; i < rightside.Length - 1; i++)
            {
                rightside[i] = rightside[i+1];
            }
            rightside[rightside.Length - 1] = remainder;
            return other;
        }
    }

    public class Match
    {
        public int Week;
        public int Rink;
        public int Team1;
        public int Team2;

    }
}