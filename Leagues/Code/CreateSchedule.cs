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

            numberOfTeams += numberOfTeams % 2;
            var numberOfRinks = numberOfTeams / 2;

            var matches = new List<Match>();

            int[] leftside = new int[numberOfRinks];
            int[] rightside = new int[numberOfRinks];
            for (int r = 0; r < numberOfRinks; r++)
            {
                leftside[r] = r;
                rightside[r] = numberOfTeams - r - 1;
                matches.Add(new Match()
                {
                    Week = 0,
                    Rink = r,
                    Team1 = r,
                    Team2 = numberOfTeams - r - 1
                });
            }

            for (int w = 1; w < numberofWeeks; w++)
            {
                int remainder = ShiftRight(leftside);
                int other = ShiftLeft(rightside, remainder);
                leftside[1] = other;
                for (int r = 0; r < numberOfRinks; r++)
                {
                    matches.Add(new Match()
                    {
                        Week = w,
                        Rink = r,
                        Team1 = leftside[r]< rightside[r]? leftside[r]: rightside[r],
                        Team2 = leftside[r] < rightside[r] ? rightside[r] : leftside[r]
                    });
                }
            }
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