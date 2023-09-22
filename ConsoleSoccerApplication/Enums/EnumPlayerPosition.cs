using System.ComponentModel;

namespace ConsoleSoccerApplication.Enums
{
    public enum EnumPlayerPosition
    {
        [Description("Defensor")]
        Defence,
        [Description("Meio-campista")]
        Midfield,
        [Description("Goleiro")]
        Goalkeeper,
        [Description("Atacante")]
        Offence,
        [Description("Meio-campista")]
        Midfielder,
        [Description("Goleiro")]
        Keeper,
        [Description("Centroavante")]
        CentreForward,
        [Description("Meio-campista ofensivo")]
        AttackingMidfield,
        [Description("Zagueiro")]
        CentreBack,
        [Description("Lateral direito")]
        RightBack,
        [Description("Lateral direito")]
        LeftBack,
        [Description("Meio-campista central")]
        CentralMidfield,
        [Description("Meio-campista defensivo (Volante)")]
        DefensiveMidfield,
        [Description("Atacante pela direita (Ala direita)")]
        RightWinger,
        [Description("Atacante pela esquerda (Ala esquerda)")]
        LeftWinger,
    }
}
