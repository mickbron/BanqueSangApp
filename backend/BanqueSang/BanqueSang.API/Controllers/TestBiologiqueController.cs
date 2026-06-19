using BanqueSang.API.DTOs.Tests;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/tests")]
[Authorize]
public class TestBiologiqueController : ControllerBase
{
    private readonly ITestBiologiqueService _testBiologiqueService;

    public TestBiologiqueController(ITestBiologiqueService testBiologiqueService)
    {
        _testBiologiqueService = testBiologiqueService;
    }

    /// <summary>
    /// Récupère tous les types de tests biologiques disponibles.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetTests()
    {
        var tests = await _testBiologiqueService.GetTestsAsync();

        var response = tests.Select(test => new TestResponseDto
        {
            IdTest = test.IdTest,
            TestNom = test.TestNom
        });

        return Ok(response);
    }

    /// <summary>
    /// Récupère tous les résultats de tests biologiques déjà soumis.
    /// </summary>
    [HttpGet("resultats")]
    public async Task<IActionResult> GetResultats()
    {
        var resultats = await _testBiologiqueService.GetResultatsAsync();

        var response = resultats.Select(resultat => new ResultatTestResponseDto
        {
            IdResultatTest = resultat.IdResultatTest,
            IdPersonnel = resultat.IdPersonnel,
            PersonnelNom = resultat.PersonnelNom,
            PersonnelPrenom = resultat.PersonnelPrenom,
            IdSang = resultat.IdSang,
            IdTest = resultat.IdTest,
            TestNom = resultat.TestNom,
            DateTest = resultat.DateTest,
            Resultat = resultat.Resultat,
            Commentaire = resultat.Commentaire,
            StatutTest = resultat.StatutTest
        });

        return Ok(response);
    }

    /// <summary>
    /// Soumet un résultat de test biologique pour une poche de sang.
    /// Selon le résultat, le backend peut valider ou rejeter automatiquement le don.
    /// </summary>
    [HttpPost("resultats")]
    [Authorize(Roles = "TECHNICIEN,MEDECIN,ADMINISTRATEUR")]
    public async Task<IActionResult> CreateResultat([FromBody] CreateResultatTestRequestDto request)
    {
        try
        {
            var resultatTest = new ResultatTest
            {
                IdPersonnel = request.IdPersonnel,
                IdSang = request.IdSang,
                IdTest = request.IdTest,
                DateTest = request.DateTest,
                Resultat = request.Resultat,
                Commentaire = request.Commentaire
            };

            var id = await _testBiologiqueService.SoumettreResultatAsync(resultatTest);

            return CreatedAtAction(nameof(GetResultats), new CreateResultatTestResponseDto
            {
                IdResultatTest = id,
                Message = "Résultat de test enregistré avec succès."
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
    
    /// <summary>
    /// Modifie un résultat de test biologique existant.
    /// Après modification, le système recalcule automatiquement
    /// le statut du don et la disponibilité de la poche de sang.
    /// </summary>
    [HttpPut("resultats/{id:int}")]
    [Authorize(Roles = "MEDECIN,ADMINISTRATEUR")]
    public async Task<IActionResult> UpdateResultat(int id, [FromBody] UpdateResultatTestRequestDto request)
    {
        try
        {
            var updated = await _testBiologiqueService.ModifierResultatAsync(
                id,
                request.Resultat,
                request.Commentaire,
                request.StatutTest
            );

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Résultat de test introuvable."
                });
            }

            return Ok(new
            {
                message = "Résultat de test modifié avec succès."
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