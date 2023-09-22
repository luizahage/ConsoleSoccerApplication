using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Referee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Nationality { get; set; }
    }
}
