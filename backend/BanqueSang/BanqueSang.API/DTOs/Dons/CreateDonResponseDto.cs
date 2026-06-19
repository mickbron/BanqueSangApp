namespace BanqueSang.API.DTOs.Dons;

public class CreateDonResponseDto
{
    public int DonId { get; set; }

    public int SangId { get; set; }

    public string Statut { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}