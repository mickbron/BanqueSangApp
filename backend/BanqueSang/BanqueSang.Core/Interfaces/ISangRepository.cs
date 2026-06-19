using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface ISangRepository
{
    /// <summary>
    /// Crée une nouvelle poche de sang associée à un don.
    /// Retourne l'identifiant de la poche créée.
    /// </summary>
    Task<int> CreateAsync(Sang sang);

    /// <summary>
    /// Récupère toutes les poches de sang enregistrées.
    /// </summary>
    Task<IEnumerable<Sang>> GetAllAsync();

    /// <summary>
    /// Récupère les statistiques de stock groupées par groupe sanguin.
    /// </summary>
    Task<IEnumerable<StockGroupeResult>> GetStockParGroupeAsync();

    /// <summary>
    /// Récupère les poches proches de la péremption ou déjà périmées.
    /// </summary>
    Task<IEnumerable<Sang>> GetAlertesAsync();
}