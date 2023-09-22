using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class ScoreDTO
    {
        public string Winner { get; set; }
        public string Duration { get; set; }
        public TimeDTO FullTime { get; set; }
        public TimeDTO HalfTime { get; set; }
    }
}
