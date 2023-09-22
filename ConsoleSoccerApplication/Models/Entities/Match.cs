using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Match
    {
        public Area Area { get; set; }
        public Competition Competition { get; set; }
        public CurrentSeason Season { get; set; }
        public int Id { get; set; }
        public DateTime UtcDate { get; set; }
        public string Status { get; set; }
        public int? Matchday { get; set; }
        public string Stage { get; set; }
        public string Group { get; set; }
        public DateTime LastUpdate { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Score Score { get; set; }
        public Odds Odds { get; set; }
        public IEnumerable<Referee> Referees { get; set; }
    }
}
