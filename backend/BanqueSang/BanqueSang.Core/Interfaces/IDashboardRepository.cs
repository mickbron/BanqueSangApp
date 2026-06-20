using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IDashboardRepository
{
    /// <summary>
    /// Récupère les statistiques principales du tableau de bord.
    /// </summary>
    Task<DashboardStats> GetStatsAsync();
}