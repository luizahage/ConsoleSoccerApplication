using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Standing
    {
        public string Stage { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public IEnumerable<PositionTable> Table { get; set; }
    }
}
