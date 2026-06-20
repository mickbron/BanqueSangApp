using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IDemandeSangRepository
{
    /// <summary>
    /// Récupère toutes les demandes de sang avec les informations du patient et du personnel.
    /// </summary>
    Task<IEnumerable<DemandeSangDetail>> GetAllAsync();

    /// <summary>
    /// Récupère une demande de sang par son identifiant.
    /// </summary>
    Task<DemandeSangDetail?> GetByIdAsync(int idDemande);

    /// <summary>
    /// Crée une nouvelle demande de sang.
    /// </summary>
    Task<int> CreateAsync(DemandeSang demandeSang);

    /// <summary>
    /// Met à jour le statut d'une demande de sang.
    /// </summary>
    Task<bool> UpdateStatutAsync(int idDemande, string statut);
}