namespace BanqueSang.API.DTOs.DemandesSang;

public class DemandeSangResponseDto
{
    public int IdDemande { get; set; }

    public int IdPatient { get; set; }

    public string PatientNom { get; set; } = string.Empty;

    public string PatientPrenom { get; set; } = string.Empty;

    public string PatientGroupeSanguin { get; set; } = string.Empty;

    public int? IdSang { get; set; }

    public int IdPersonnel { get; set; }

    public string PersonnelNom { get; set; } = string.Empty;

    public string PersonnelPrenom { get; set; } = string.Empty;

    public string TypeComposant { get; set; } = string.Empty;

    public int QuantitePoche { get; set; }

    public string Urgence { get; set; } = string.Empty;

    public DateTime DateDemande { get; set; }

    public string? Commentaire { get; set; }

    public string Statut { get; set; } = string.Empty;
}