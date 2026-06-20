namespace BanqueSang.Core.Entities;

public class DemandeSang
{
    public int IdDemande { get; set; }

    public int IdPatient { get; set; }

    public int? IdSang { get; set; }

    public int IdPersonnel { get; set; }

    public string TypeComposant { get; set; } = string.Empty;

    public int QuantitePoche { get; set; }

    public string Urgence { get; set; } = "NORMAL";

    public DateTime DateDemande { get; set; }

    public string? Commentaire { get; set; }

    public string Statut { get; set; } = "EN_ATTENTE";
}