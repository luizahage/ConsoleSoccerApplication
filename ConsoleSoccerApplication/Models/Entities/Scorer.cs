using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Scorer
    {
        public Player Player { get; set; }
        public Team Team { get; set; }
        public int? PlayedMatches { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? Penalties { get; set; }
    }
}
