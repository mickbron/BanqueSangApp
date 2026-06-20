using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    /// <summary>
    /// Retourne les statistiques principales utilisées par le tableau de bord.
    /// </summary>
    public async Task<DashboardStats> GetStatsAsync()
    {
        return await _dashboardRepository.GetStatsAsync();
    }
}