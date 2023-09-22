using System.ComponentModel;

namespace ConsoleSoccerApplication.Enums
{
    public enum EnumCompetitionStage
    {
        [Description("Final")]
        FINAL,
        [Description("Terceiro lugar")]
        THIRD_PLACE,
        [Description("Semi finais")]
        SEMI_FINALS,
        [Description("Quartas de final")]
        QUARTER_FINALS,
        [Description("Oitavas de final")]
        LAST_16,
        [Description("Últimos 32")]
        LAST_32,
        [Description("Últimos 64")]
        LAST_64,
        [Description("Quarta rodada")]
        ROUND_4,
        [Description("Teceira rodada")]
        ROUND_3,
        [Description("Segunda rodada")]
        ROUND_2,
        [Description("Primeira rodada")]
        ROUND_1,
        [Description("Fase de grupos")]
        GROUP_STAGE,
        [Description("Rodada preliminar")]
        PRELIMINARY_ROUND,
        [Description("Qualificatório")]
        QUALIFICATION,
        [Description("Primeira rodada do qualificatório")]
        QUALIFICATION_ROUND_1,
        [Description("Segunda rodada do qualificatório")]
        QUALIFICATION_ROUND_2,
        [Description("Teceira rodada do qualificatório")]
        QUALIFICATION_ROUND_3,
        [Description("Primeira rodada dos play-offs")]
        PLAYOFF_ROUND_1,
        [Description("Segunda rodada dos play-offs")]
        PLAYOFF_ROUND_2,
        [Description("Play-offs")]
        PLAYOFFS,
        [Description("Temporada regular")]
        REGULAR_SEASON,
        [Description("Fechamento (returno)")]
        CLAUSURA,
        [Description("Abertura (turno)")]
        APERTURA,
        [Description("Campeonato")]
        CHAMPIONSHIP,
        [Description("Rebaixamento")]
        RELEGATION,
        [Description("Rodada de rebaixamento")]
        RELEGATION_ROUND
    }
}
