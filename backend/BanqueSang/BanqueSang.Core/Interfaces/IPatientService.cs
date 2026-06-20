using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface IPatientService
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
    /// Crée un nouveau patient après validation des données.
    /// </summary>
    Task<int> CreateAsync(Patient patient);

    /// <summary>
    /// Modifie un patient existant après validation des données.
    /// </summary>
    Task<bool> UpdateAsync(int idPatient, Patient patient);
}