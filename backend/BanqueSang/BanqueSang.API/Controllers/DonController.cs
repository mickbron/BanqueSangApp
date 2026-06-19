using BanqueSang.API.DTOs.Dons;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/dons")]
[Authorize]
public class DonController : ControllerBase
{
    private readonly IDonService _donService;

    public DonController(IDonService donService)
    {
        _donService = donService;
    }

    /// <summary>
    /// Récupère tous les dons enregistrés.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dons = await _donService.GetAllAsync();

        var response = dons.Select(don => new DonResponseDto
        {
            IdDon = don.IdDon,
            IdDonneur = don.IdDonneur,
            IdCentre = don.IdCentre,
            DonDate = don.DonDate,
            DonStatut = don.DonStatut,
            DonVolumeMl = don.DonVolumeMl
        });

        return Ok(response);
    }

    /// <summary>
    /// Récupère un don par son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var don = await _donService.GetByIdAsync(id);

        if (don is null)
        {
            return NotFound(new
            {
                message = "Don introuvable."
            });
        }

        return Ok(new DonResponseDto
        {
            IdDon = don.IdDon,
            IdDonneur = don.IdDonneur,
            IdCentre = don.IdCentre,
            DonDate = don.DonDate,
            DonStatut = don.DonStatut,
            DonVolumeMl = don.DonVolumeMl
        });
    }

    /// <summary>
    /// Enregistre un nouveau don.
    /// Cette action crée aussi automatiquement une poche de sang associée.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "TECHNICIEN,ADMINISTRATEUR")]
    public async Task<IActionResult> Create([FromBody] CreateDonRequestDto request)
    {
        try
        {
            var don = new Don
            {
                IdDonneur = request.IdDonneur,
                IdCentre = request.IdCentre,
                DonDate = request.DonDate,
                DonVolumeMl = request.DonVolumeMl
            };

            var result = await _donService.CreateAsync(don, request.SangTypeComposant);

            var response = new CreateDonResponseDto
            {
                DonId = result.DonId,
                SangId = result.SangId,
                Statut = result.Statut,
                Message = "Don enregistré avec succès. Une poche de sang a été créée en attente de tests."
            };

            return CreatedAtAction(nameof(GetById), new { id = result.DonId }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}