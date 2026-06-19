using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class DonneurService : IDonneurService
{
    private readonly IDonneurRepository _donneurRepository;

    public DonneurService(IDonneurRepository donneurRepository)
    {
        _donneurRepository = donneurRepository;
    }

    /// <summary>
    /// Récupère tous les donneurs.
    /// Cette méthode appelle uniquement le repository, car il n'y a pas de règle métier particulière.
    /// </summary>
    public async Task<IEnumerable<Donneur>> GetAllAsync()
    {
        return await _donneurRepository.GetAllAsync();
    }

    /// <summary>
    /// Récupère un donneur par son identifiant.
    /// Retourne null si le donneur n'existe pas.
    /// </summary>
    public async Task<Donneur?> GetByIdAsync(int id)
    {
        return await _donneurRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Crée un nouveau donneur.
    /// Avant l'insertion, on vérifie que les champs obligatoires sont bien remplis.
    /// </summary>
    public async Task<int> CreateAsync(Donneur donneur)
    {
        ValiderDonneur(donneur);

        donneur.DonneurStatutEligibilite = string.IsNullOrWhiteSpace(donneur.DonneurStatutEligibilite)
            ? "ELIGIBLE"
            : donneur.DonneurStatutEligibilite;

        return await _donneurRepository.CreateAsync(donneur);
    }

    /// <summary>
    /// Met à jour un donneur existant.
    /// L'identifiant reçu dans l'URL est appliqué à l'objet avant la mise à jour.
    /// </summary>
    public async Task<bool> UpdateAsync(int id, Donneur donneur)
    {
        ValiderDonneur(donneur);

        var existingDonneur = await _donneurRepository.GetByIdAsync(id);

        if (existingDonneur is null)
        {
            return false;
        }

        donneur.IdDonneur = id;

        return await _donneurRepository.UpdateAsync(donneur);
    }

    /// <summary>
    /// Vérifie si un donneur est éligible au don de sang.
    /// Règles simples :
    /// - le donneur doit exister ;
    /// - le donneur doit avoir au moins 18 ans ;
    /// - le donneur doit avoir moins de 65 ans ;
    /// - si le donneur a déjà donné, le dernier don doit dater d'au moins 90 jours ;
    /// - le statut en base ne doit pas être NON_ELIGIBLE.
    /// </summary>
    public async Task<EligibiliteResult> VerifierEligibiliteAsync(int id)
    {
        var donneur = await _donneurRepository.GetByIdAsync(id);

        if (donneur is null)
        {
            return new EligibiliteResult
            {
                DonneurId = id,
                Eligible = false,
                Raisons = new List<string> { "Donneur introuvable." }
            };
        }

        var raisons = new List<string>();

        var age = CalculerAge(donneur.DonneurDateNaissance);

        if (age < 18)
        {
            raisons.Add("Le donneur doit avoir au moins 18 ans.");
        }

        if (age > 65)
        {
            raisons.Add("Le donneur ne doit pas avoir plus de 65 ans.");
        }

        if (donneur.DonneurDernierDon.HasValue)
        {
            var joursDepuisDernierDon = (DateTime.Today - donneur.DonneurDernierDon.Value.Date).TotalDays;

            if (joursDepuisDernierDon < 90)
            {
                raisons.Add("Le délai minimum de 90 jours depuis le dernier don n'est pas respecté.");
            }
        }

        if (donneur.DonneurStatutEligibilite == "NON_ELIGIBLE")
        {
            raisons.Add("Le donneur est marqué comme non éligible dans le système.");
        }

        var eligible = raisons.Count == 0;

        await _donneurRepository.UpdateEligibiliteAsync(
            donneur.IdDonneur,
            eligible ? "ELIGIBLE" : "NON_ELIGIBLE"
        );

        return new EligibiliteResult
        {
            DonneurId = donneur.IdDonneur,
            Eligible = eligible,
            Raisons = raisons
        };
    }

    /// <summary>
    /// Valide les champs obligatoires d'un donneur.
    /// Une exception est levée si une donnée obligatoire est absente.
    /// </summary>
    private static void ValiderDonneur(Donneur donneur)
    {
        if (string.IsNullOrWhiteSpace(donneur.DonneurNom))
            throw new ArgumentException("Le nom du donneur est obligatoire.");

        if (string.IsNullOrWhiteSpace(donneur.DonneurPrenom))
            throw new ArgumentException("Le prénom du donneur est obligatoire.");

        if (string.IsNullOrWhiteSpace(donneur.DonneurAdresse))
            throw new ArgumentException("L'adresse du donneur est obligatoire.");

        if (string.IsNullOrWhiteSpace(donneur.DonneurTelephone))
            throw new ArgumentException("Le téléphone du donneur est obligatoire.");

        if (donneur.DonneurDateNaissance == default)
            throw new ArgumentException("La date de naissance du donneur est obligatoire.");

        if (string.IsNullOrWhiteSpace(donneur.DonneurGroupeSanguin))
            throw new ArgumentException("Le groupe sanguin du donneur est obligatoire.");
    }

    /// <summary>
    /// Calcule l'âge à partir de la date de naissance.
    /// Le calcul tient compte du fait que l'anniversaire soit déjà passé ou non cette année.
    /// </summary>
    private static int CalculerAge(DateTime dateNaissance)
    {
        var today = DateTime.Today;
        var age = today.Year - dateNaissance.Year;

        if (dateNaissance.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}