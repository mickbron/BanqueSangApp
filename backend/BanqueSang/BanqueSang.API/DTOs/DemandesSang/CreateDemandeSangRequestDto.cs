namespace BanqueSang.API.DTOs.DemandesSang;

public class CreateDemandeSangRequestDto
{
    public int IdPatient { get; set; }

    public int? IdSang { get; set; }

    public int IdPersonnel { get; set; }

    public string TypeComposant { get; set; } = string.Empty;

    public int QuantitePoche { get; set; }

    public string Urgence { get; set; } = "NORMAL";

    public DateTime DateDemande { get; set; }

    public string? Commentaire { get; set; }
}