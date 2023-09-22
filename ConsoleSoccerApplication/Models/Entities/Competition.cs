using ConsoleSoccerApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Competition
    {
        public int Id { get; set; }
        public Area Area { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Emblem { get; set; }
        public string Plan { get; set; }
        public CurrentSeason CurrentSeason { get; set; }
        public int NumberOfAvailableSeasons { get; set; }
        public DateTime LastUpdated { get; set; }

    }

    public class CompetitionsResponse
    {
        public int Count { get; set; }
        public IEnumerable<Competition> Competitions { get; set; }
    }
}
