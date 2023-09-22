using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Section { get; set; }
        public string Position { get; set; }
        public int? ShirtNumber { get; set; }
        public string Nationality { get; set; }
        public Team CurrentTeam { get; set; }

        public int? CalcPlayerAge()
        {
            if (DateOfBirth != null)
            {
                DateTime currentDate = DateTime.Now;
                return currentDate.Year - DateOfBirth.Value.Year;
            }
            return null;
        }
    }
}
