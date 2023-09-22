using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class TeamInfoDTO
    {
        public string Nationality { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Founded { get; set; }
        public string ClubColors { get; set; }
        public string Venue { get; set; }
        public List<string> NamesRunningCompetitions { get; set; } = new List<string>();
        public CoachDTO Coach { get; set;}
        public IEnumerable<PlayerDTO> Squad { get; set; }
        public IEnumerable<CoachDTO> Staff { get; set; }
    }
}
