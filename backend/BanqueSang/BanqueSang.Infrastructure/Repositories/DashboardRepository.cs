using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DashboardRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère toutes les statistiques principales du tableau de bord.
    /// </summary>
    public async Task<DashboardStats> GetStatsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT
                                   (SELECT COUNT(*) FROM DONNEUR) AS TotalDonneurs,

                                   (SELECT COUNT(*) FROM DON) AS TotalDons,

                                   (SELECT COUNT(*) FROM SANG) AS TotalPoches,

                                   (SELECT COUNT(*) FROM SANG WHERE sang_disponible = 1) AS PochesDisponibles,

                                   (SELECT COUNT(*) FROM SANG WHERE sang_disponible = 0) AS PochesIndisponibles,

                                   (SELECT COUNT(*) FROM PATIENT) AS TotalPatients,

                                   (SELECT COUNT(*) FROM DEMANDE_SANG) AS TotalDemandes,

                                   (SELECT COUNT(*) FROM DEMANDE_SANG WHERE statut = 'EN_ATTENTE') AS DemandesEnAttente,

                                   (SELECT COUNT(*) FROM RESULTAT_TEST WHERE resultat = 'POSITIF') AS TestsPositifs,

                                   (SELECT COUNT(*) FROM RESULTAT_TEST WHERE resultat = 'NEGATIF') AS TestsNegatifs;
                           """;

        return await connection.QuerySingleAsync<DashboardStats>(sql);
    }
}