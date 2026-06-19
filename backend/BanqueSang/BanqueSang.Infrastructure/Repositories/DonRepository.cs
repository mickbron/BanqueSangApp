using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class DonRepository : IDonRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DonRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Crée un don dans la table DON et retourne l'identifiant généré par MySQL.
    /// </summary>
    public async Task<int> CreateAsync(Don don)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO DON (
                id_donneur,
                id_centre,
                don_date,
                don_statut,
                don_volume_ml
            )
            VALUES (
                @IdDonneur,
                @IdCentre,
                @DonDate,
                @DonStatut,
                @DonVolumeMl
            );

            SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, don);
    }

    /// <summary>
    /// Récupère tous les dons depuis la base de données.
    /// </summary>
    public async Task<IEnumerable<Don>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_don AS IdDon,
                id_donneur AS IdDonneur,
                id_centre AS IdCentre,
                don_date AS DonDate,
                don_statut AS DonStatut,
                don_volume_ml AS DonVolumeMl
            FROM DON
            ORDER BY don_date DESC;
        """;

        return await connection.QueryAsync<Don>(sql);
    }

    /// <summary>
    /// Récupère un don par son identifiant.
    /// </summary>
    public async Task<Don?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_don AS IdDon,
                id_donneur AS IdDonneur,
                id_centre AS IdCentre,
                don_date AS DonDate,
                don_statut AS DonStatut,
                don_volume_ml AS DonVolumeMl
            FROM DON
            WHERE id_don = @Id
            LIMIT 1;
        """;

        return await connection.QueryFirstOrDefaultAsync<Don>(sql, new
        {
            Id = id
        });
    }
    
    
    /// <summary>
    /// Met à jour la date du dernier don d'un donneur après l'enregistrement d'un don.
    /// </summary>
    public async Task<bool> UpdateDernierDonAsync(int idDonneur, DateTime dateDon)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               UPDATE DONNEUR
                               SET donneur_dernier_don = @DateDon
                               WHERE id_donneur = @IdDonneur;
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            IdDonneur = idDonneur,
            DateDon = dateDon
        });

        return rowsAffected > 0;
    }
}