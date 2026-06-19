namespace BanqueSang.Core.Models;

public class StockGroupeResult
{
    public string GroupeSanguin { get; set; } = string.Empty;

    public int Total { get; set; }

    public int Disponibles { get; set; }

    public int Indisponibles { get; set; }

    public int Perimees { get; set; }

    public int ProchesPeremption { get; set; }
}