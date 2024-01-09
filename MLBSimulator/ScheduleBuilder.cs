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
                    bool longLogic = false; // incomplete - need to refactor idea here
                    if (TeamArray[i].GamesRemaining[j] == 0 && i != j)
                    {
                        if (TeamArray[i].Division == TeamArray[j].Division)
                        {
                            TeamArray[i].GamesRemaining[j] = 13;
                            TeamArray[j].GamesRemaining[i] = 13;
                        }
                        else if (TeamArray[i].Division[0] == TeamArray[j].Division[0])
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
                                int seriesLength = rand.Next(1, TeamArray[i].TotalSameLeagueSeries + 5);
                                if (seriesLength <= TeamArray[i].FourGameSeries || longLogic)
                                {
                                    TeamArray[i].GamesRemaining[j] = 7;
                                    TeamArray[j].GamesRemaining[i] = 7;
                                    TeamArray[i].TotalSameLeagueSeries -= 1;
                                    TeamArray[j].TotalSameLeagueSeries -= 1;
                                    TeamArray[i].FourGameSeries -= 1;
                                    TeamArray[j].FourGameSeries -= 1;
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

        public string[,] BuildSchedule()
        {
            string[,] schedule = new string[TeamArray.Length, 200];
            return schedule;
        }
    }
}
