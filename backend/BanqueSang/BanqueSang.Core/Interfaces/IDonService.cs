using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IDonService
{
    /// <summary>
    /// Enregistre un don et crée automatiquement une poche de sang associée.
    /// </summary>
    Task<CreateDonResult> CreateAsync(Don don, string typeComposant);

    /// <summary>
    /// Récupère tous les dons.
    /// </summary>
    Task<IEnumerable<Don>> GetAllAsync();

    /// <summary>
    /// Récupère un don par son identifiant.
    /// </summary>
    Task<Don?> GetByIdAsync(int id);
}