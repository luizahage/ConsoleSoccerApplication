using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class TopScorers
    {
        public Competition Competition { get; set; }
        public CurrentSeason Season { get; set; }
        public IEnumerable<Scorer> Scorers { get; set; }
    }
}
