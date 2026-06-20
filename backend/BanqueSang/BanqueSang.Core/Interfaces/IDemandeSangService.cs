using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IDemandeSangService
{
    /// <summary>
    /// Récupère toutes les demandes de sang.
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
    /// Modifie le statut d'une demande de sang.
    /// </summary>
    Task<bool> UpdateStatutAsync(int idDemande, string statut);
}