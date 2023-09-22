using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class TopScorersDTO
    {
        public CompetitionDTO Competition { get; set; }
        public int CurrentMatchday { get; set; }
        public string TypeCompetition { get; set; }
        public IEnumerable<ScorerDTO> Scorers { get; set; }
    }
}
