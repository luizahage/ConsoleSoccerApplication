using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class CompetitionTable
    {
        public Area Area { get; set; }
        public Competition Competition { get; set; }
        public CurrentSeason Season { get; set; }
        public IEnumerable<Standing> Standings { get; set; }
    }
}
