using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IStockService
{
    /// <summary>
    /// Retourne toutes les poches de sang.
    /// </summary>
    Task<IEnumerable<Sang>> GetAllAsync();

    /// <summary>
    /// Retourne le stock groupé par groupe sanguin.
    /// </summary>
    Task<IEnumerable<StockGroupeResult>> GetStockParGroupeAsync();

    /// <summary>
    /// Retourne les alertes de stock liées à la péremption.
    /// </summary>
    Task<IEnumerable<Sang>> GetAlertesAsync();
}