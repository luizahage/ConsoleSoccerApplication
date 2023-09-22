using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class PlayerMatches
    {
        public ResultSet ResultSet { get; set; }
        public string Aggregations { get; set; }
        public Player Person { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
