namespace BanqueSang.API.DTOs.Donneurs;

public class DonneurResponseDto
{
    public int IdDonneur { get; set; }

    public string DonneurNom { get; set; } = string.Empty;

    public string DonneurPrenom { get; set; } = string.Empty;

    public string DonneurAdresse { get; set; } = string.Empty;

    public string DonneurTelephone { get; set; } = string.Empty;

    public DateTime DonneurDateNaissance { get; set; }

    public string DonneurGroupeSanguin { get; set; } = string.Empty;

    public string DonneurStatutEligibilite { get; set; } = string.Empty;

    public DateTime? DonneurDernierDon { get; set; }
}