using BanqueSang.API.DTOs.Stock;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/stock")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    /// <summary>
    /// Récupère toutes les poches de sang.
    /// Accessible à tout utilisateur connecté.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var poches = await _stockService.GetAllAsync();

        var response = poches.Select(ToSangResponseDto);

        return Ok(response);
    }

    /// <summary>
    /// Récupère le stock groupé par groupe sanguin.
    /// Sert à afficher les cartes de stock dans le frontend.
    /// </summary>
    [HttpGet("groupes")]
    public async Task<IActionResult> GetStockParGroupe()
    {
        var stock = await _stockService.GetStockParGroupeAsync();

        var response = stock.Select(ToStockGroupeResponseDto);

        return Ok(response);
    }

    /// <summary>
    /// Récupère les poches périmées ou proches de leur date de péremption.
    /// </summary>
    [HttpGet("alertes")]
    public async Task<IActionResult> GetAlertes()
    {
        var alertes = await _stockService.GetAlertesAsync();

        var response = alertes.Select(ToSangResponseDto);

        return Ok(response);
    }

    /// <summary>
    /// Convertit une entité Sang en DTO de réponse.
    /// </summary>
    private static SangResponseDto ToSangResponseDto(Sang sang)
    {
        return new SangResponseDto
        {
            IdSang = sang.IdSang,
            IdDon = sang.IdDon,
            IdCentre = sang.IdCentre,
            IdHopital = sang.IdHopital,
            SangTypeComposant = sang.SangTypeComposant,
            SangDateCreation = sang.SangDateCreation,
            SangDatePeremption = sang.SangDatePeremption,
            SangDisponible = sang.SangDisponible
        };
    }

    /// <summary>
    /// Convertit le résultat de stock groupé en DTO de réponse.
    /// </summary>
    private static StockGroupeResponseDto ToStockGroupeResponseDto(StockGroupeResult stock)
    {
        return new StockGroupeResponseDto
        {
            GroupeSanguin = stock.GroupeSanguin,
            Total = stock.Total,
            Disponibles = stock.Disponibles,
            Indisponibles = stock.Indisponibles,
            Perimees = stock.Perimees,
            ProchesPeremption = stock.ProchesPeremption
        };
    }
}