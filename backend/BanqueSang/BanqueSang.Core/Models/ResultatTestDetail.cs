namespace BanqueSang.Core.Models;

public class ResultatTestDetail
{
    public int IdResultatTest { get; set; }

    public int IdPersonnel { get; set; }

    public string PersonnelNom { get; set; } = string.Empty;

    public string PersonnelPrenom { get; set; } = string.Empty;

    public int IdSang { get; set; }

    public int IdTest { get; set; }

    public string TestNom { get; set; } = string.Empty;

    public DateTime DateTest { get; set; }

    public string Resultat { get; set; } = string.Empty;

    public string? Commentaire { get; set; }

    public string StatutTest { get; set; } = string.Empty;
}