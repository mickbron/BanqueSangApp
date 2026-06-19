using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface IDonneurRepository
{
    /// <summary>
    /// Récupère tous les donneurs enregistrés dans la base de données.
    /// </summary>
    Task<IEnumerable<Donneur>> GetAllAsync();

    /// <summary>
    /// Récupère un donneur à partir de son identifiant.
    /// Retourne null si aucun donneur n'existe avec cet identifiant.
    /// </summary>
    Task<Donneur?> GetByIdAsync(int id);

    /// <summary>
    /// Ajoute un nouveau donneur dans la base de données.
    /// Retourne l'identifiant généré automatiquement.
    /// </summary>
    Task<int> CreateAsync(Donneur donneur);

    /// <summary>
    /// Met à jour les informations d'un donneur existant.
    /// Retourne true si la mise à jour a réussi.
    /// </summary>
    Task<bool> UpdateAsync(Donneur donneur);

    /// <summary>
    /// Met à jour uniquement le statut d'éligibilité d'un donneur.
    /// </summary>
    Task<bool> UpdateEligibiliteAsync(int idDonneur, string statutEligibilite);
}