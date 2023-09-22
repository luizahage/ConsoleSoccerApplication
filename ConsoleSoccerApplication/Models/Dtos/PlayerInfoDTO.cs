using System;
using System.Collections.Generic;
using ConsoleSoccerApplication.Models.Entities;
using System.Text;

namespace ConsoleSoccerApplication.Models.Dtos
{
    public class PlayerInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Nationality { get; set; }
        public string Section { get; set; }
        public string Position { get; set; }
        public int? ShirtNumber { get; set; }
        public TeamInfoDTO CurrentTeam { get; set; }
        public Contract Contract { get; set; }
    }
}
