using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBSimulator
{
    public class Team
    {
        public string CityName { get; set; }

        public string TeamName { get; set; }

        public string Abbreviation { get; set; }

        public string Division { get; set; }

        public int ELO { get; set; }

        public string Rival { get; set; }

        public Team(string cityName, string teamName, string division, string abr, int elo, string rival)
        {
            CityName = cityName;    
            TeamName = teamName;
            Division = division;
            Abbreviation = abr;
            ELO = elo;
            Rival = rival;
        }
    }
}
