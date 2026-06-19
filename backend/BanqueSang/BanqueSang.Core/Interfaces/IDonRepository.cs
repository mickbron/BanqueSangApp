using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface IDonRepository
{
    /// <summary>
    /// Crée un nouveau don et retourne son identifiant généré.
    /// </summary>
    Task<int> CreateAsync(Don don);

    /// <summary>
    /// Récupère tous les dons enregistrés.
    /// </summary>
    Task<IEnumerable<Don>> GetAllAsync();

    /// <summary>
    /// Récupère un don par son identifiant.
    /// </summary>
    Task<Don?> GetByIdAsync(int id);
    
    /// <summary>
    /// Met à jour la date du dernier don d'un donneur.
    /// </summary>
    Task<bool> UpdateDernierDonAsync(int idDonneur, DateTime dateDon);
}