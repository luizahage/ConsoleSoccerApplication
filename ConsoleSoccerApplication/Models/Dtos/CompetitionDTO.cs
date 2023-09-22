using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class CompetitionDTO
    {
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TeamDTO Winner { get; set; }
        public string AreaName { get; set; }
    }
}
