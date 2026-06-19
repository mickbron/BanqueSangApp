namespace BanqueSang.Core.Models;

public class EligibiliteResult
{
    public int DonneurId { get; set; }

    public bool Eligible { get; set; }

    public List<string> Raisons { get; set; } = new();
}