using MLBSimulator;
using System.Linq.Expressions;

string version = "0.6.2";
DateTime lastUpdated = new DateTime(2025, 9, 1);

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
int sum = 0;
int day; 
string[,] schedule;
int longestSchedule = 0;
for(int zz = 0; zz < 5000; zz++)
{
    ScheduleBuilder builder = new ScheduleBuilder(@"..\..\..\Teams.txt");
    (schedule, day) = builder.BuildSchedule();
    sum += day;
    if (day > longestSchedule)
    {
        longestSchedule = day;
    }
}
System.Console.WriteLine(sum / 5000);
System.Console.WriteLine(longestSchedule);