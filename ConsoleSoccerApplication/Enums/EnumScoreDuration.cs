using System.ComponentModel;

namespace ConsoleSoccerApplication.Enums
{
    public enum EnumScoreDuration
    {
        [Description("Regular")]
        REGULAR,
        [Description("Tempo extra")]
        EXTRA_TIME,
        [Description("Disputa de pênaltis")]
        PENALTY_SHOOTOUT
    }
}
