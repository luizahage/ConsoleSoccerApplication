using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
    }
}
