using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class MatchDTO
    {
        public CompetitionDTO Competition { get; set; }
        public string AreaName { get; set; }
        public int CurrentMatchday { get; set; }
        public DateTime MatchDate { get; set; }
        public string Status { get; set; }
        public int? Matchday { get; set; }
        public string Stage { get; set; }
        public string Group { get; set; }
        public TeamDTO HomeTeam { get; set; }
        public TeamDTO AwayTeam { get; set; }
        public ScoreDTO Score { get; set; }
        public IEnumerable<RefereeDTO> Referees { get; set; }


    }
}
