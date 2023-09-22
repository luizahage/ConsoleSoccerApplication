using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class RunningCompetition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Emblem { get; set; }
    }
}
