using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Score
    {
        public string Winner { get; set; }
        public string Duration { get; set; }
        public Time FullTime { get; set; }
        public Time HalfTime { get; set; }
    }
}
