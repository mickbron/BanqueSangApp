namespace BanqueSang.Core.Models;

public class CreateDonResult
{
    public int DonId { get; set; }

    public int SangId { get; set; }

    public string Statut { get; set; } = string.Empty;
}