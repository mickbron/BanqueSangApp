namespace BanqueSang.Core.Entities;

public class Personnel
{
    public int IdPersonnel { get; set; }
    public string PersonnelNom { get; set; } = string.Empty;
    public string PersonnelPrenom { get; set; } = string.Empty;
    public string PersonnelAdresse { get; set; } = string.Empty;
    public string PersonnelTelephone { get; set; } = string.Empty;
    public DateTime PersonnelDateNaissance { get; set; }
    public string PersonnelFonction { get; set; } = string.Empty;
    public string PersonnelLogin { get; set; } = string.Empty;
    public string PersonnelPassword { get; set; } = string.Empty;
    public bool PersonnelActif { get; set; }
}