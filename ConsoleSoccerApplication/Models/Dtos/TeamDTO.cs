using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamShortName { get; set; }
        public string TeamTLA { get; set; }
    }
}