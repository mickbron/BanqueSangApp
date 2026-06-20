using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;

namespace BanqueSang.Core.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    /// <summary>
    /// Récupère tous les patients enregistrés.
    /// </summary>
    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _patientRepository.GetAllAsync();
    }

    /// <summary>
    /// Récupère un patient par son identifiant.
    /// </summary>
    public async Task<Patient?> GetByIdAsync(int idPatient)
    {
        return await _patientRepository.GetByIdAsync(idPatient);
    }

    /// <summary>
    /// Crée un patient après vérification des champs obligatoires.
    /// </summary>
    public async Task<int> CreateAsync(Patient patient)
    {
        ValidatePatient(patient);

        return await _patientRepository.CreateAsync(patient);
    }

    /// <summary>
    /// Met à jour un patient existant.
    /// </summary>
    public async Task<bool> UpdateAsync(int idPatient, Patient patient)
    {
        ValidatePatient(patient);

        var exists = await _patientRepository.ExistsAsync(idPatient);

        if (!exists)
            throw new KeyNotFoundException("Patient introuvable.");

        patient.IdPatient = idPatient;

        return await _patientRepository.UpdateAsync(patient);
    }

    /// <summary>
    /// Vérifie que les données du patient sont valides.
    /// </summary>
    private static void ValidatePatient(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.PatientNom))
            throw new ArgumentException("Le nom du patient est obligatoire.");

        if (string.IsNullOrWhiteSpace(patient.PatientPrenom))
            throw new ArgumentException("Le prénom du patient est obligatoire.");

        if (patient.PatientDateNaissance == default)
            throw new ArgumentException("La date de naissance du patient est obligatoire.");

        if (string.IsNullOrWhiteSpace(patient.PatientGroupeSanguin))
            throw new ArgumentException("Le groupe sanguin du patient est obligatoire.");

        var groupesAutorises = new[]
        {
            "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
        };

        if (!groupesAutorises.Contains(patient.PatientGroupeSanguin))
            throw new ArgumentException("Le groupe sanguin du patient est invalide.");
    }
}