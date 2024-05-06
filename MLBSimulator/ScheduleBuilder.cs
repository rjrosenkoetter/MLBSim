using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MLBSimulator
{
    // This class will include the necessary tools to build the schedule of an MLB season.
    // Currently will only build a schedule based on the real 2024 schedule building process
    // and only accept 30 teams in the team list.
    public class ScheduleBuilder
    {
        public Team[] TeamArray { get; set; } = new Team[30];
        public ScheduleBuilder(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            int count = 0;
            Random rand = new Random();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] components = line.Split(',');
                TeamArray[count++] = new Team(components[0], components[1], components[3], components[2], Convert.ToInt32(components[4]), components[5], components[6]);
            }
            for (int i = 0; i < 29; i++) // 29 because once the 29th team's array is completed, all arrays will be zeroed out
            {
                int div1 = 0;
                int div2 = 0;
                bool div1Active = true;
                for(int j = 0; j < 30; j++)
                {
                    if (TeamArray[i].GamesRemaining[j] == 0 && i != j)
                    {
                        if (TeamArray[i].Division == TeamArray[j].Division) // if they are in the same division..
                        {
                            TeamArray[i].GamesRemaining[j] = 13;
                            TeamArray[j].GamesRemaining[i] = 13;
                        }
                        else if (TeamArray[i].Division[0] == TeamArray[j].Division[0]) // if they are in the same league (American League/National League)
                        {
                            if (TeamArray[i].FourGameSeries == 0 || TeamArray[j].FourGameSeries == 0)
                            {
                                TeamArray[i].GamesRemaining[j] = 6;
                                TeamArray[j].GamesRemaining[i] = 6;
                                TeamArray[i].TotalSameLeagueSeries -= 1;
                                TeamArray[j].TotalSameLeagueSeries -= 1;
                            }
                            else
                            {
                                int start = 0;
                                if (TeamArray[i].Division[0] == 'N') start = 15;

                                int firstDivisionStop = ReturnFirstDivisionStop(TeamArray[i], start);
                                int secondDivisionStop = ReturnSecondDivisionStop(TeamArray[i], start);
                                bool inFirstDivision = j <= firstDivisionStop;
                                int end = 0;
                                if(inFirstDivision)
                                {
                                    end = firstDivisionStop + 1;
                                }
                                else
                                {
                                    end = secondDivisionStop + 1;
                                }
                                int numOfTeamsLeft = 0;
                                int gamesForThisDivision = 0;
                                if(inFirstDivision)
                                {
                                    gamesForThisDivision = TeamArray[i].FourGameSeries - 2;
                                }
                                else
                                {
                                    gamesForThisDivision = TeamArray[i].FourGameSeries;
                                }
                                bool otherTeamInFirstDivision = false;
                                if (((TeamArray[j].Division[2] == 'C' || TeamArray[j].Division[2] == 'W') && TeamArray[i].Division[2] == 'E') || TeamArray[j].Division[2] == 'E' && TeamArray[i].Division[2] == 'C')
                                {
                                    otherTeamInFirstDivision = true;
                                }
                                bool isSecondToLastTeamInDiv = i % 5 == 3;
                                int forcedTeam = -1;
                                for (int k = j; k < end; k++)
                                {
                                    if (TeamArray[k].GamesLeftWithFirstDivision != 0 && otherTeamInFirstDivision) numOfTeamsLeft++;
                                    else if (TeamArray[k].FourGameSeries != 0 && !otherTeamInFirstDivision) numOfTeamsLeft++;

                                    if(isSecondToLastTeamInDiv && ((TeamArray[k].GamesLeftWithFirstDivision == 2 && otherTeamInFirstDivision) || TeamArray[k].FourGameSeries == 2 && !otherTeamInFirstDivision))
                                    {
                                        forcedTeam = k;
                                    }
                                }
                                int seriesLength = rand.Next(1, TeamArray[i].TotalSameLeagueSeries);
                                if (((seriesLength <= TeamArray[i].FourGameSeries || numOfTeamsLeft == gamesForThisDivision || forcedTeam == j) && !(gamesForThisDivision == 1 && forcedTeam != -1 && j != forcedTeam)) && !DoneWithFirstDivision(TeamArray[i], j) && (TeamArray[j].GamesLeftWithFirstDivision != 0 || !otherTeamInFirstDivision))
                                {
                                    TeamArray[i].GamesRemaining[j] = 7;
                                    TeamArray[j].GamesRemaining[i] = 7;
                                    TeamArray[i].TotalSameLeagueSeries -= 1;
                                    TeamArray[j].TotalSameLeagueSeries -= 1;
                                    TeamArray[i].FourGameSeries -= 1;
                                    TeamArray[j].FourGameSeries -= 1;
                                    if(inFirstDivision)
                                    {
                                        TeamArray[i].GamesLeftWithFirstDivision -= 1;
                                    }
                                    if(otherTeamInFirstDivision)
                                    {
                                        TeamArray[j].GamesLeftWithFirstDivision -= 1;
                                    }
                                }
                                else
                                {
                                    TeamArray[i].GamesRemaining[j] = 6;
                                    TeamArray[j].GamesRemaining[i] = 6;
                                    TeamArray[i].TotalSameLeagueSeries -= 1;
                                    TeamArray[j].TotalSameLeagueSeries -= 1;
                                }
                            }
                        }
                        else if (TeamArray[i].Rival == TeamArray[j].Abbreviation)
                        {
                            TeamArray[i].GamesRemaining[j] = 4;
                            TeamArray[j].GamesRemaining[i] = 4;
                        }
                        else
                        {
                            TeamArray[i].GamesRemaining[j] = 3;
                            TeamArray[j].GamesRemaining[i] = 3;
                        }
                    }
                }
            }
        }

        public bool DoneWithFirstDivision(Team team, int currentNum)
        {
            int boost = 0;
            if(team.Division[0] == 'N')
            {
                boost += 15;
            }
            int firstDivisionStop = ReturnFirstDivisionStop(team, boost);

            if(currentNum <= firstDivisionStop && team.FourGameSeries == 2)
            {
                return true;
            }
            return false;
        }
        public int ReturnFirstDivisionStop(Team team, int boost)
        {
            int firstDivisionStop = boost;
            if (team.Division[2] == 'E')
            {
                firstDivisionStop += 9;
            }
            else
            {
                firstDivisionStop += 4;
            }
            return firstDivisionStop;
        }

        public int ReturnSecondDivisionStop(Team team, int boost)
        {
            int secondDivisionStop = boost;
            if (team.Division[2] == 'W')
            {
                secondDivisionStop += 9;
            }
            else
            {
                secondDivisionStop += 14;
            }
            return secondDivisionStop;
        }
        public string[,] BuildSchedule()
        {
            string[,] schedule = new string[TeamArray.Length, 200];
            return schedule;
        }
    }
}
