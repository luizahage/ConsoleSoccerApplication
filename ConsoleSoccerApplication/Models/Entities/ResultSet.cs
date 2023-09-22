using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class ResultSet
    {
        public int Count { get; set; }
        public string Competitions { get; set; }
        public DateTime First { get; set; }
        public DateTime Last { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
    }
}
