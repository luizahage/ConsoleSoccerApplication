using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class CompetitionTableDTO
    {
        public string AreaName { get; set; }
        public string CompetitionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrentMatchday { get; set; }
        public TeamDTO Winner { get; set; }
        public IEnumerable<StandingDTO> Standings { get; set; }
    }
}
