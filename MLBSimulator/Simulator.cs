using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MLBSimulator
{
    public class Simulator
    {
        public Simulator() 
        { 
        
        }

        /// <summary>
        /// This method simulates a game when given two teams ELO ratings. There is no assumption on the lower/higher ELO being a certain parameter.
        /// The calculated win percentage uses a power equation with curve fitting based on desired percentages based
        /// on how large the ELO gap is.
        /// </summary>
        /// <param name="elo1">The first team's ELO.</param>
        /// <param name="elo2">The second team's ELO.</param>
        /// <returns></returns>
        public bool SimulateGame(int elo1, int elo2)
        {
            bool firstEloHigher = elo1 >= elo2;
            int difference = Math.Abs(elo1 - elo2);
            double adjustment = 0.0039656 * Math.Pow(difference, 0.7706);
            double percentage = .5 + adjustment;

            if (!firstEloHigher) percentage -= 2 * adjustment; // if the second ELO is higher the benefit should go to that team, not team 1.

            Random rand = new Random();
            double simulatedNumber = rand.Next(1, 101) / 100.0;
            return simulatedNumber <= percentage;
        }
    }
}
