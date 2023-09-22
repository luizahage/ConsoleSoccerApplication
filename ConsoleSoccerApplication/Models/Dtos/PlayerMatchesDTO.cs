using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class PlayerMatchesDTO
    {
        public string Competitions { get; set; }
        public DateTime FirstMatch { get; set; }
        public DateTime LastMatch { get; set; }
        public PlayerInfoDTO Player { get; set; }
        public IEnumerable<MatchDTO> Matches { get; set; }
    }
}
