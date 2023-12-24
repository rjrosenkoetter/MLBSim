using System.Linq.Expressions;

string version = "0.1";
DateTime lastUpdated = new DateTime(2023, 12, 24);

static void printVer(string ver, DateTime lastUpdate)
{
    Console.WriteLine("MLB Simulator Version v" + ver);
    Console.WriteLine("Last updated: " + lastUpdate.ToString("MMMM dd, yyyy"));
    Console.WriteLine("- Created by Ryan Rosenkoetter.");
}

printVer(version, lastUpdated);