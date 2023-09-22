using ConsoleSoccerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class CoachDTO
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public Contract Contract { get; set; }

    }
}
