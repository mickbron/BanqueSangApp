using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IResultatTestRepository
{
    /// <summary>
    /// Récupère tous les résultats de tests biologiques avec les informations du test et du personnel.
    /// </summary>
    Task<IEnumerable<ResultatTestDetail>> GetAllAsync();

    /// <summary>
    /// Récupère tous les résultats associés à une poche de sang.
    /// </summary>
    Task<IEnumerable<ResultatTest>> GetBySangIdAsync(int idSang);

    /// <summary>
    /// Vérifie si un résultat existe déjà pour une poche et un type de test.
    /// </summary>
    Task<bool> ExistsForSangAndTestAsync(int idSang, int idTest);

    /// <summary>
    /// Crée un nouveau résultat de test biologique.
    /// </summary>
    Task<int> CreateAsync(ResultatTest resultatTest);
    
    /// <summary>
    /// Récupère un résultat de test par son identifiant.
    /// </summary>
    Task<ResultatTest?> GetByIdAsync(int idResultatTest);

    /// <summary>
    /// Met à jour un résultat de test existant.
    /// </summary>
    Task<bool> UpdateAsync(ResultatTest resultatTest);
}