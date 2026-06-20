using BanqueSang.API.DTOs.DemandesSang;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/demandes-sang")]
[Authorize(Roles = "MEDECIN,ADMINISTRATEUR")]
public class DemandeSangController : ControllerBase
{
    private readonly IDemandeSangService _demandeSangService;

    public DemandeSangController(IDemandeSangService demandeSangService)
    {
        _demandeSangService = demandeSangService;
    }

    /// <summary>
    /// Récupère toutes les demandes de sang.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var demandes = await _demandeSangService.GetAllAsync();

        var response = demandes.Select(demande => new DemandeSangResponseDto
        {
            IdDemande = demande.IdDemande,
            IdPatient = demande.IdPatient,
            PatientNom = demande.PatientNom,
            PatientPrenom = demande.PatientPrenom,
            PatientGroupeSanguin = demande.PatientGroupeSanguin,
            IdSang = demande.IdSang,
            IdPersonnel = demande.IdPersonnel,
            PersonnelNom = demande.PersonnelNom,
            PersonnelPrenom = demande.PersonnelPrenom,
            TypeComposant = demande.TypeComposant,
            QuantitePoche = demande.QuantitePoche,
            Urgence = demande.Urgence,
            DateDemande = demande.DateDemande,
            Commentaire = demande.Commentaire,
            Statut = demande.Statut
        });

        return Ok(response);
    }

    /// <summary>
    /// Récupère une demande de sang par son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var demande = await _demandeSangService.GetByIdAsync(id);

        if (demande is null)
        {
            return NotFound(new
            {
                message = "Demande de sang introuvable."
            });
        }

        var response = new DemandeSangResponseDto
        {
            IdDemande = demande.IdDemande,
            IdPatient = demande.IdPatient,
            PatientNom = demande.PatientNom,
            PatientPrenom = demande.PatientPrenom,
            PatientGroupeSanguin = demande.PatientGroupeSanguin,
            IdSang = demande.IdSang,
            IdPersonnel = demande.IdPersonnel,
            PersonnelNom = demande.PersonnelNom,
            PersonnelPrenom = demande.PersonnelPrenom,
            TypeComposant = demande.TypeComposant,
            QuantitePoche = demande.QuantitePoche,
            Urgence = demande.Urgence,
            DateDemande = demande.DateDemande,
            Commentaire = demande.Commentaire,
            Statut = demande.Statut
        };

        return Ok(response);
    }

    /// <summary>
    /// Crée une nouvelle demande de sang.
    /// Le statut de départ est automatiquement EN_ATTENTE.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDemandeSangRequestDto request)
    {
        try
        {
            var demandeSang = new DemandeSang
            {
                IdPatient = request.IdPatient,
                IdSang = request.IdSang,
                IdPersonnel = request.IdPersonnel,
                TypeComposant = request.TypeComposant,
                QuantitePoche = request.QuantitePoche,
                Urgence = request.Urgence,
                DateDemande = request.DateDemande,
                Commentaire = request.Commentaire,
                Statut = "EN_ATTENTE"
            };

            var id = await _demandeSangService.CreateAsync(demandeSang);

            return CreatedAtAction(nameof(GetById), new { id }, new
            {
                idDemande = id,
                message = "Demande de sang créée avec succès."
            });
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
    }

    /// <summary>
    /// Modifie uniquement le statut d'une demande de sang.
    /// Statuts autorisés : EN_ATTENTE, RESERVEE, TRAITEE, ANNULEE.
    /// </summary>
    [HttpPut("{id:int}/statut")]
    public async Task<IActionResult> UpdateStatut(
        int id,
        [FromBody] UpdateDemandeSangStatutRequestDto request)
    {
        try
        {
            var updated = await _demandeSangService.UpdateStatutAsync(
                id,
                request.Statut
            );

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Demande de sang introuvable."
                });
            }

            return Ok(new
            {
                message = "Statut de la demande modifié avec succès."
            });
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
    }
}