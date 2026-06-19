using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class StockService : IStockService
{
    private readonly ISangRepository _sangRepository;

    public StockService(ISangRepository sangRepository)
    {
        _sangRepository = sangRepository;
    }

    /// <summary>
    /// Récupère toutes les poches de sang.
    /// </summary>
    public async Task<IEnumerable<Sang>> GetAllAsync()
    {
        return await _sangRepository.GetAllAsync();
    }

    /// <summary>
    /// Récupère les statistiques de stock par groupe sanguin.
    /// </summary>
    public async Task<IEnumerable<StockGroupeResult>> GetStockParGroupeAsync()
    {
        return await _sangRepository.GetStockParGroupeAsync();
    }

    /// <summary>
    /// Récupère les poches périmées ou proches de leur date de péremption.
    /// </summary>
    public async Task<IEnumerable<Sang>> GetAlertesAsync()
    {
        return await _sangRepository.GetAlertesAsync();
    }
}