namespace BanqueSang.API.DTOs.Stock;

public class SangResponseDto
{
    public int IdSang { get; set; }

    public int IdDon { get; set; }

    public int? IdCentre { get; set; }

    public int? IdHopital { get; set; }

    public string SangTypeComposant { get; set; } = string.Empty;

    public DateTime SangDateCreation { get; set; }

    public DateTime SangDatePeremption { get; set; }

    public bool SangDisponible { get; set; }
}