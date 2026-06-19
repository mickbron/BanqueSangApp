namespace BanqueSang.API.DTOs.Tests;

public class CreateResultatTestRequestDto
{
    public int IdPersonnel { get; set; }

    public int IdSang { get; set; }

    public int IdTest { get; set; }

    public DateTime DateTest { get; set; }

    public string Resultat { get; set; } = string.Empty;

    public string? Commentaire { get; set; }
}