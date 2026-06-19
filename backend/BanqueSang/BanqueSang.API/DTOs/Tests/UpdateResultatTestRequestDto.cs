namespace BanqueSang.API.DTOs.Tests;

public class UpdateResultatTestRequestDto
{
    public string Resultat { get; set; } = string.Empty;

    public string? Commentaire { get; set; }

    public string StatutTest { get; set; } = "VALIDE";
}