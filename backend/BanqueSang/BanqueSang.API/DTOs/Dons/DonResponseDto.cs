namespace BanqueSang.API.DTOs.Dons;

public class DonResponseDto
{
    public int IdDon { get; set; }

    public int IdDonneur { get; set; }

    public int? IdCentre { get; set; }

    public DateTime DonDate { get; set; }

    public string DonStatut { get; set; } = string.Empty;

    public int DonVolumeMl { get; set; }
}