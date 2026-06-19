using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IDonneurService
{
    /// <summary>
    /// Retourne la liste complète des donneurs.
    /// </summary>
    Task<IEnumerable<Donneur>> GetAllAsync();

    /// <summary>
    /// Retourne un donneur par son identifiant.
    /// </summary>
    Task<Donneur?> GetByIdAsync(int id);

    /// <summary>
    /// Crée un nouveau donneur après validation des données métier.
    /// </summary>
    Task<int> CreateAsync(Donneur donneur);

    /// <summary>
    /// Modifie un donneur existant après validation des données métier.
    /// </summary>
    Task<bool> UpdateAsync(int id, Donneur donneur);

    /// <summary>
    /// Vérifie l'éligibilité d'un donneur selon son âge, son dernier don
    /// et son statut actuel.
    /// </summary>
    Task<EligibiliteResult> VerifierEligibiliteAsync(int id);
}