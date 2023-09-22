namespace ConsoleSoccerApplication.Models.Dtos
{
    public class ScorerDTO
    {
        public PlayerDTO Player { get; set; }
        public TeamDTO Team { get; set; }
        public int? PlayedMatches { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? Penalties { get; set; }
    }
}