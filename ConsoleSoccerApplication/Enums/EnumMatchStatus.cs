using System.ComponentModel;

namespace ConsoleSoccerApplication.Enums
{
    public enum EnumMatchStatus
    {
        [Description("Agendado")]
        SCHEDULED,
        [Description("Programado")]
        TIMED,
        [Description("Em jogo")]
        IN_PLAY,
        [Description("Pausado")]
        PAUSED,
        [Description("Tempo extra")]
        EXTRA_TIME,
        [Description("Disputa de pênaltis")]
        PENALTY_SHOOTOUT,
        [Description("Finalizado")]
        FINISHED,
        [Description("Suspenso")]
        SUSPENDED,
        [Description("Adiado")]
        POSTPONED,
        [Description("Cancelado")]
        CANCELLED,
        [Description("Premiado")]
        AWARDED
    }
}
