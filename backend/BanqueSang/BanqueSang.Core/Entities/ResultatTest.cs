namespace BanqueSang.Core.Entities;

public class ResultatTest
{
    public int IdResultatTest { get; set; }

    public int IdPersonnel { get; set; }

    public int IdSang { get; set; }

    public int IdTest { get; set; }

    public DateTime DateTest { get; set; }

    public string Resultat { get; set; } = "EN_ATTENTE";

    public string? Commentaire { get; set; }

    public string StatutTest { get; set; } = "EN_ATTENTE";
}