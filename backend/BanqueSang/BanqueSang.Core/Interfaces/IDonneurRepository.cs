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
    /// </summary>
    Task<Donneur?> GetByIdAsync(int id);

    /// <summary>
    /// Ajoute un nouveau donneur dans la base de données.
    /// </summary>
    Task<int> CreateAsync(Donneur donneur);

    /// <summary>
    /// Met à jour les informations d'un donneur existant.
    /// </summary>
    Task<bool> UpdateAsync(Donneur donneur);

    /// <summary>
    /// Met à jour uniquement le statut d'éligibilité d'un donneur.
    /// </summary>
    Task<bool> UpdateEligibiliteAsync(int idDonneur, string statutEligibilite);

    /// <summary>
    /// Met à jour la date du dernier don d'un donneur.
    /// </summary>
    Task<bool> UpdateDernierDonAsync(int idDonneur, DateTime dateDon);
}