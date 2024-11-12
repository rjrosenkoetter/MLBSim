﻿using MLBSimulator;
using System.Linq.Expressions;

string version = "0.5.2";
DateTime lastUpdated = new DateTime(2024, 11, 11);

static void printVer(string ver, DateTime lastUpdate)
{
    Console.WriteLine("MLB Simulator Version v" + ver);
    Console.WriteLine("Last updated: " + lastUpdate.ToString("MMMM dd, yyyy"));
    Console.WriteLine("- Created by Ryan Rosenkoetter.");
}

static Team[] populateTeams(string fileName, int numOfTeams)
{
    Team[] MLBTeams = new Team[numOfTeams];
    StreamReader sr = new StreamReader(fileName);
    int count = 0;
    while(!sr.EndOfStream)
    {
        string line = sr.ReadLine();
        string[] components = line.Split(',');
        MLBTeams[count++] = new Team(components[0], components[1], components[3], components[2], Convert.ToInt32(components[4]), components[5], components[6]);
    }
    return MLBTeams;
}

printVer(version, lastUpdated);
bool error = false;
int errors = 0;
for(int z = 0; z < 500; z++)
{
    error = false;
    ScheduleBuilder builder2 = new ScheduleBuilder(@"..\..\..\Teams.txt");
    for (int i = 0; i < 30; i++)
    {
        if (builder2.TeamArray[i].GamesRemaining.Sum() != 162)
        {
            error = true;
        }
    }
    if (error) errors++;
}
Console.WriteLine(errors);
ScheduleBuilder builder = new ScheduleBuilder(@"..\..\..\Teams.txt");
builder.BuildSchedule();