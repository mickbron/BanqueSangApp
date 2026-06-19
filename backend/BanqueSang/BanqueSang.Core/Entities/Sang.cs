namespace BanqueSang.Core.Entities;

public class Sang
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