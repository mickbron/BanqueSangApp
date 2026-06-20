namespace BanqueSang.Core.Models;

public class DashboardStats
{
    public int TotalDonneurs { get; set; }

    public int TotalDons { get; set; }

    public int TotalPoches { get; set; }

    public int PochesDisponibles { get; set; }

    public int PochesIndisponibles { get; set; }

    public int TotalPatients { get; set; }

    public int TotalDemandes { get; set; }

    public int DemandesEnAttente { get; set; }

    public int TestsPositifs { get; set; }

    public int TestsNegatifs { get; set; }
}