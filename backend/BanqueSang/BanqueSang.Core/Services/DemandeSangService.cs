using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class DemandeSangService : IDemandeSangService
{
    private readonly IDemandeSangRepository _demandeSangRepository;
    private readonly IPatientRepository _patientRepository;

    public DemandeSangService(
        IDemandeSangRepository demandeSangRepository,
        IPatientRepository patientRepository)
    {
        _demandeSangRepository = demandeSangRepository;
        _patientRepository = patientRepository;
    }

    /// <summary>
    /// Récupère toutes les demandes de sang.
    /// </summary>
    public async Task<IEnumerable<DemandeSangDetail>> GetAllAsync()
    {
        return await _demandeSangRepository.GetAllAsync();
    }

    /// <summary>
    /// Récupère une demande de sang par son identifiant.
    /// </summary>
    public async Task<DemandeSangDetail?> GetByIdAsync(int idDemande)
    {
        return await _demandeSangRepository.GetByIdAsync(idDemande);
    }

    /// <summary>
    /// Crée une demande de sang après validation.
    /// Le statut est automatiquement EN_ATTENTE à la création.
    /// </summary>
    public async Task<int> CreateAsync(DemandeSang demandeSang)
    {
        await ValidateDemandeAsync(demandeSang);

        demandeSang.DateDemande = demandeSang.DateDemande == default
            ? DateTime.Now
            : demandeSang.DateDemande;

        demandeSang.Statut = "EN_ATTENTE";

        return await _demandeSangRepository.CreateAsync(demandeSang);
    }

    /// <summary>
    /// Modifie uniquement le statut d'une demande.
    /// </summary>
    public async Task<bool> UpdateStatutAsync(int idDemande, string statut)
    {
        var statutsAutorises = new[]
        {
            "EN_ATTENTE", "RESERVEE", "TRAITEE", "ANNULEE"
        };

        if (!statutsAutorises.Contains(statut))
            throw new ArgumentException("Le statut doit être EN_ATTENTE, RESERVEE, TRAITEE ou ANNULEE.");

        var demande = await _demandeSangRepository.GetByIdAsync(idDemande);

        if (demande is null)
            throw new KeyNotFoundException("Demande de sang introuvable.");

        return await _demandeSangRepository.UpdateStatutAsync(idDemande, statut);
    }

    /// <summary>
    /// Vérifie les champs obligatoires d'une demande de sang.
    /// </summary>
    private async Task ValidateDemandeAsync(DemandeSang demandeSang)
    {
        if (demandeSang.IdPatient <= 0)
            throw new ArgumentException("Le patient est obligatoire.");

        var patientExists = await _patientRepository.ExistsAsync(demandeSang.IdPatient);

        if (!patientExists)
            throw new KeyNotFoundException("Patient introuvable.");

        if (demandeSang.IdPersonnel <= 0)
            throw new ArgumentException("Le personnel est obligatoire.");

        if (string.IsNullOrWhiteSpace(demandeSang.TypeComposant))
            throw new ArgumentException("Le type de composant est obligatoire.");

        var composantsAutorises = new[]
        {
            "SANG_TOTAL", "GLOBULES_ROUGES", "PLASMA", "PLAQUETTES"
        };

        if (!composantsAutorises.Contains(demandeSang.TypeComposant))
            throw new ArgumentException("Le type de composant est invalide.");

        if (demandeSang.QuantitePoche <= 0)
            throw new ArgumentException("La quantité de poches doit être supérieure à 0.");

        var urgencesAutorisees = new[]
        {
            "NORMAL", "URGENT", "CRITIQUE"
        };

        if (!urgencesAutorisees.Contains(demandeSang.Urgence))
            throw new ArgumentException("L'urgence doit être NORMAL, URGENT ou CRITIQUE.");
    }
}