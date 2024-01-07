using MLBSimulator;
using System.Linq.Expressions;

string version = "0.3";
DateTime lastUpdated = new DateTime(2024, 1, 7);

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
        MLBTeams[count++] = new Team(components[0], components[1], components[3], components[2], Convert.ToInt32(components[4]));
    }
    return MLBTeams;
}

printVer(version, lastUpdated);
populateTeams(@"..\..\..\Teams.txt", 30);