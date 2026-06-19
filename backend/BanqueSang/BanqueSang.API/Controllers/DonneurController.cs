using BanqueSang.API.DTOs.Donneurs;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/donneurs")]
[Authorize]
public class DonneurController : ControllerBase
{
    private readonly IDonneurService _donneurService;

    public DonneurController(IDonneurService donneurService)
    {
        _donneurService = donneurService;
    }

    /// <summary>
    /// Récupère tous les donneurs enregistrés.
    /// Cette route est protégée par JWT.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var donneurs = await _donneurService.GetAllAsync();

        var response = donneurs.Select(ToResponseDto);

        return Ok(response);
    }

    /// <summary>
    /// Récupère un donneur par son identifiant.
    /// Retourne 404 si le donneur n'existe pas.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var donneur = await _donneurService.GetByIdAsync(id);

        if (donneur is null)
        {
            return NotFound(new
            {
                message = "Donneur introuvable."
            });
        }

        return Ok(ToResponseDto(donneur));
    }

    /// <summary>
    /// Crée un nouveau donneur.
    /// Les données sont validées dans le service métier.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "TECHNICIEN,ADMINISTRATEUR")]
    public async Task<IActionResult> Create([FromBody] CreateDonneurRequestDto request)
    {
        try
        {
            var donneur = new Donneur
            {
                DonneurNom = request.DonneurNom,
                DonneurPrenom = request.DonneurPrenom,
                DonneurAdresse = request.DonneurAdresse,
                DonneurTelephone = request.DonneurTelephone,
                DonneurDateNaissance = request.DonneurDateNaissance,
                DonneurGroupeSanguin = request.DonneurGroupeSanguin,
                DonneurStatutEligibilite = request.DonneurStatutEligibilite,
                DonneurDernierDon = request.DonneurDernierDon
            };

            var id = await _donneurService.CreateAsync(donneur);

            return CreatedAtAction(nameof(GetById), new { id }, new
            {
                idDonneur = id,
                message = "Donneur créé avec succès."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Met à jour un donneur existant.
    /// Retourne 404 si le donneur n'existe pas.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "TECHNICIEN,ADMINISTRATEUR")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDonneurRequestDto request)
    {
        try
        {
            var donneur = new Donneur
            {
                DonneurNom = request.DonneurNom,
                DonneurPrenom = request.DonneurPrenom,
                DonneurAdresse = request.DonneurAdresse,
                DonneurTelephone = request.DonneurTelephone,
                DonneurDateNaissance = request.DonneurDateNaissance,
                DonneurGroupeSanguin = request.DonneurGroupeSanguin,
                DonneurStatutEligibilite = request.DonneurStatutEligibilite,
                DonneurDernierDon = request.DonneurDernierDon
            };

            var updated = await _donneurService.UpdateAsync(id, donneur);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Donneur introuvable."
                });
            }

            return Ok(new
            {
                message = "Donneur modifié avec succès."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Vérifie l'éligibilité d'un donneur.
    /// Le système retourne le résultat ainsi que les raisons de non-éligibilité.
    /// </summary>
    [HttpGet("{id:int}/eligibilite")]
    [Authorize(Roles = "TECHNICIEN,ADMINISTRATEUR")]
    public async Task<IActionResult> VerifierEligibilite(int id)
    {
        var result = await _donneurService.VerifierEligibiliteAsync(id);

        if (!result.Eligible && result.Raisons.Contains("Donneur introuvable."))
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Convertit une entité Donneur en DTO de réponse.
    /// Cela évite d'exposer directement l'entité métier dans l'API.
    /// </summary>
    private static DonneurResponseDto ToResponseDto(Donneur donneur)
    {
        return new DonneurResponseDto
        {
            IdDonneur = donneur.IdDonneur,
            DonneurNom = donneur.DonneurNom,
            DonneurPrenom = donneur.DonneurPrenom,
            DonneurAdresse = donneur.DonneurAdresse,
            DonneurTelephone = donneur.DonneurTelephone,
            DonneurDateNaissance = donneur.DonneurDateNaissance,
            DonneurGroupeSanguin = donneur.DonneurGroupeSanguin,
            DonneurStatutEligibilite = donneur.DonneurStatutEligibilite,
            DonneurDernierDon = donneur.DonneurDernierDon
        };
    }
}