using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface IPatientRepository
{
    /// <summary>
    /// Récupère tous les patients.
    /// </summary>
    Task<IEnumerable<Patient>> GetAllAsync();

    /// <summary>
    /// Récupère un patient par son identifiant.
    /// </summary>
    Task<Patient?> GetByIdAsync(int idPatient);

    /// <summary>
    /// Crée un nouveau patient.
    /// </summary>
    Task<int> CreateAsync(Patient patient);

    /// <summary>
    /// Met à jour un patient existant.
    /// </summary>
    Task<bool> UpdateAsync(Patient patient);

    /// <summary>
    /// Vérifie si un patient existe.
    /// </summary>
    Task<bool> ExistsAsync(int idPatient);
}