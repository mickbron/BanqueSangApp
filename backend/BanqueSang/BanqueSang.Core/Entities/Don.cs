namespace BanqueSang.Core.Entities;

public class Don
{
    public int IdDon { get; set; }

    public int IdDonneur { get; set; }

    public int? IdCentre { get; set; }

    public DateTime DonDate { get; set; }

    public string DonStatut { get; set; } = "EN_ATTENTE";

    public int DonVolumeMl { get; set; }
}