using System.ComponentModel;

namespace ConsoleSoccerApplication.Enums
{
    public enum EnumTypeCompetition
    {
        [Description("Liga")]
        LEAGUE,
        [Description("Liga-Copa")]
        LEAGUE_CUP,
        [Description("Copa")]
        CUP,
        [Description("Play-offs")]
        PLAYOFFS,
        [Description("Total")]
        TOTAL
    }
}
