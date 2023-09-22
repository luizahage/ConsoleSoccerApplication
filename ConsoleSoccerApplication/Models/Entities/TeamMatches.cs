using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class TeamMatches
    {
        public ResultSet ResultSet { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
