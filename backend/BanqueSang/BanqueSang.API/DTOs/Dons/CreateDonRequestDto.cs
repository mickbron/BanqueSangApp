namespace BanqueSang.API.DTOs.Dons;

public class CreateDonRequestDto
{
    public int IdDonneur { get; set; }

    public int? IdCentre { get; set; }

    public DateTime DonDate { get; set; }

    public int DonVolumeMl { get; set; }

    public string SangTypeComposant { get; set; } = "SANG_TOTAL";
}