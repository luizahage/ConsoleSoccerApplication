using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class StandingDTO
    {
        public string Stage { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public IEnumerable<PositionTableDTO> Table { get; set; }
    }
}
