using BanqueSang.API.DTOs.Patients;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/patients")]
[Authorize(Roles = "MEDECIN,ADMINISTRATEUR")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Récupère tous les patients.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _patientService.GetAllAsync();

        var response = patients.Select(patient => new PatientResponseDto
        {
            IdPatient = patient.IdPatient,
            PatientNom = patient.PatientNom,
            PatientPrenom = patient.PatientPrenom,
            PatientDateNaissance = patient.PatientDateNaissance,
            PatientGroupeSanguin = patient.PatientGroupeSanguin,
            PatientTelephone = patient.PatientTelephone,
            PatientAdresse = patient.PatientAdresse,
            IdService = patient.IdService
        });

        return Ok(response);
    }

    /// <summary>
    /// Récupère un patient par son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient is null)
        {
            return NotFound(new
            {
                message = "Patient introuvable."
            });
        }

        var response = new PatientResponseDto
        {
            IdPatient = patient.IdPatient,
            PatientNom = patient.PatientNom,
            PatientPrenom = patient.PatientPrenom,
            PatientDateNaissance = patient.PatientDateNaissance,
            PatientGroupeSanguin = patient.PatientGroupeSanguin,
            PatientTelephone = patient.PatientTelephone,
            PatientAdresse = patient.PatientAdresse,
            IdService = patient.IdService
        };

        return Ok(response);
    }

    /// <summary>
    /// Crée un nouveau patient.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientRequestDto request)
    {
        try
        {
            var patient = new Patient
            {
                PatientNom = request.PatientNom,
                PatientPrenom = request.PatientPrenom,
                PatientDateNaissance = request.PatientDateNaissance,
                PatientGroupeSanguin = request.PatientGroupeSanguin,
                PatientTelephone = request.PatientTelephone,
                PatientAdresse = request.PatientAdresse,
                IdService = request.IdService
            };

            var id = await _patientService.CreateAsync(patient);

            return CreatedAtAction(nameof(GetById), new { id }, new
            {
                idPatient = id,
                message = "Patient créé avec succès."
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
    /// Met à jour un patient existant.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientRequestDto request)
    {
        try
        {
            var patient = new Patient
            {
                PatientNom = request.PatientNom,
                PatientPrenom = request.PatientPrenom,
                PatientDateNaissance = request.PatientDateNaissance,
                PatientGroupeSanguin = request.PatientGroupeSanguin,
                PatientTelephone = request.PatientTelephone,
                PatientAdresse = request.PatientAdresse,
                IdService = request.IdService
            };

            var updated = await _patientService.UpdateAsync(id, patient);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Patient introuvable."
                });
            }

            return Ok(new
            {
                message = "Patient modifié avec succès."
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