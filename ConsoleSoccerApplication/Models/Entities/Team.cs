﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoccerApplication.Models.Entities
{
    public class Team
    {
        public Area Area { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string TLA { get; set; }
        public string Crest { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public int? Founded { get; set; }
        public string ClubColors { get; set; }
        public string Venue { get; set; }
        public IEnumerable<RunningCompetition> RunningCompetitions { get; set; }
        public Coach Coach { get; set; }
        public IEnumerable<Player> Squad { get; set; }
        public IEnumerable<Coach> Staff { get; set; }
        public DateTime LastUpdate { get; set; }
        public Contract Contract { get; set; }
    }

    public class TeamsResponse
    {
        public int Count { get; set; }

        public IEnumerable<Team> Teams { get; set; }
    }
}
