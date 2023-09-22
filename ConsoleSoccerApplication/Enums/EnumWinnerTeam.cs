using System.ComponentModel;

namespace ConsoleSoccerApplication.Enums
{
    public enum EnumWinnerTeam
    {
        [Description("Time da casa")]
        HOME_TEAM,
        [Description("Time visitante")]
        AWAY_TEAM,
        [Description("Empate")]
        DRAW
    }
}
