using BanqueSang.API.DTOs.Dashboard;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Récupère les statistiques principales du tableau de bord.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _dashboardService.GetStatsAsync();

        var response = new DashboardStatsResponseDto
        {
            TotalDonneurs = stats.TotalDonneurs,
            TotalDons = stats.TotalDons,
            TotalPoches = stats.TotalPoches,
            PochesDisponibles = stats.PochesDisponibles,
            PochesIndisponibles = stats.PochesIndisponibles,
            TotalPatients = stats.TotalPatients,
            TotalDemandes = stats.TotalDemandes,
            DemandesEnAttente = stats.DemandesEnAttente,
            TestsPositifs = stats.TestsPositifs,
            TestsNegatifs = stats.TestsNegatifs
        };

        return Ok(response);
    }
}