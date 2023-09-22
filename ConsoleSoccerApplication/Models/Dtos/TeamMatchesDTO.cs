using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class TeamMatchesDTO
    {
        public string Competitions { get; set; }
        public DateTime FirstMatch { get; set; }
        public DateTime LastMatch { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public IEnumerable<MatchDTO> Matches { get; set; }
    }
}
