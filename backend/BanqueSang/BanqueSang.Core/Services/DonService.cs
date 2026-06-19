using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class DonService : IDonService
{
    private readonly IDonRepository _donRepository;
    private readonly IDonneurRepository _donneurRepository;
    private readonly ISangRepository _sangRepository;

    public DonService(
        IDonRepository donRepository,
        IDonneurRepository donneurRepository,
        ISangRepository sangRepository)
    {
        _donRepository = donRepository;
        _donneurRepository = donneurRepository;
        _sangRepository = sangRepository;
    }

    /// <summary>
    /// Récupère tous les dons enregistrés.
    /// </summary>
    public async Task<IEnumerable<Don>> GetAllAsync()
    {
        return await _donRepository.GetAllAsync();
    }

    /// <summary>
    /// Récupère un don par son identifiant.
    /// </summary>
    public async Task<Don?> GetByIdAsync(int id)
    {
        return await _donRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Enregistre un don de sang.
    /// Règles appliquées :
    /// - le donneur doit exister ;
    /// - le donneur doit être éligible ;
    /// - le volume doit être supérieur à zéro ;
    /// - le don est créé avec le statut EN_ATTENTE ;
    /// - une poche de sang est créée avec disponible = false ;
    /// - la date du dernier don du donneur est mise à jour.
    /// </summary>
    public async Task<CreateDonResult> CreateAsync(Don don, string typeComposant)
    {
        if (don.IdDonneur <= 0)
            throw new ArgumentException("Le donneur est obligatoire.");

        if (don.DonVolumeMl <= 0)
            throw new ArgumentException("Le volume du don doit être supérieur à zéro.");

        if (don.DonDate == default)
            throw new ArgumentException("La date du don est obligatoire.");

        if (string.IsNullOrWhiteSpace(typeComposant))
            throw new ArgumentException("Le type de composant est obligatoire.");

        var donneur = await _donneurRepository.GetByIdAsync(don.IdDonneur);

        if (donneur is null)
            throw new KeyNotFoundException("Donneur introuvable.");

        if (donneur.DonneurStatutEligibilite != "ELIGIBLE")
            throw new InvalidOperationException("Le donneur n'est pas éligible au don.");

        don.DonStatut = "EN_ATTENTE";

        var donId = await _donRepository.CreateAsync(don);

        var sang = new Sang
        {
            IdDon = donId,
            IdCentre = don.IdCentre,
            IdHopital = null,
            SangTypeComposant = typeComposant,
            SangDateCreation = don.DonDate,
            SangDatePeremption = CalculerDatePeremption(don.DonDate, typeComposant),
            SangDisponible = false
        };

        var sangId = await _sangRepository.CreateAsync(sang);

        await _donneurRepository.UpdateDernierDonAsync(don.IdDonneur, don.DonDate);

        return new CreateDonResult
        {
            DonId = donId,
            SangId = sangId,
            Statut = don.DonStatut
        };
    }

    /// <summary>
    /// Calcule automatiquement la date de péremption selon le type de composant.
    /// Règles simples :
    /// - sang total et globules rouges : 42 jours ;
    /// - plasma : 365 jours ;
    /// - plaquettes : 5 jours.
    /// </summary>
    private static DateTime CalculerDatePeremption(DateTime dateCreation, string typeComposant)
    {
        return typeComposant switch
        {
            "PLASMA" => dateCreation.AddDays(365),
            "PLAQUETTES" => dateCreation.AddDays(5),
            "SANG_TOTAL" => dateCreation.AddDays(42),
            "GLOBULES_ROUGES" => dateCreation.AddDays(42),
            _ => dateCreation.AddDays(42)
        };
    }
}