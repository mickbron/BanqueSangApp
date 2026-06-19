using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class SangRepository : ISangRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public SangRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Crée une poche de sang dans la table SANG.
    /// La poche est indisponible au départ, car elle doit passer les tests biologiques.
    /// </summary>
    public async Task<int> CreateAsync(Sang sang)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO SANG (
                id_don,
                id_centre,
                id_hopital,
                sang_type_composant,
                sang_date_creation,
                sang_date_peremption,
                sang_disponible
            )
            VALUES (
                @IdDon,
                @IdCentre,
                @IdHopital,
                @SangTypeComposant,
                @SangDateCreation,
                @SangDatePeremption,
                @SangDisponible
            );

            SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, sang);
    }

    /// <summary>
    /// Récupère toutes les poches de sang.
    /// </summary>
    public async Task<IEnumerable<Sang>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_sang AS IdSang,
                id_don AS IdDon,
                id_centre AS IdCentre,
                id_hopital AS IdHopital,
                sang_type_composant AS SangTypeComposant,
                sang_date_creation AS SangDateCreation,
                sang_date_peremption AS SangDatePeremption,
                sang_disponible AS SangDisponible
            FROM SANG
            ORDER BY sang_date_peremption ASC;
        """;

        return await connection.QueryAsync<Sang>(sql);
    }

    /// <summary>
    /// Calcule le stock pour tous les groupes sanguins.
    /// Même si un groupe n'a aucune poche, il est affiché avec des valeurs à zéro.
    /// </summary>
    public async Task<IEnumerable<StockGroupeResult>> GetStockParGroupeAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                groupes.groupe_sanguin AS GroupeSanguin,

                COUNT(s.id_sang) AS Total,

                COALESCE(SUM(CASE 
                    WHEN s.sang_disponible = TRUE 
                         AND s.sang_date_peremption >= CURDATE()
                    THEN 1 ELSE 0 END), 0) AS Disponibles,

                COALESCE(SUM(CASE 
                    WHEN s.id_sang IS NOT NULL 
                         AND s.sang_disponible = FALSE
                    THEN 1 ELSE 0 END), 0) AS Indisponibles,

                COALESCE(SUM(CASE 
                    WHEN s.sang_date_peremption < CURDATE()
                    THEN 1 ELSE 0 END), 0) AS Perimees,

                COALESCE(SUM(CASE 
                    WHEN s.sang_date_peremption >= CURDATE()
                         AND s.sang_date_peremption <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                    THEN 1 ELSE 0 END), 0) AS ProchesPeremption

            FROM (
                SELECT 'A+' AS groupe_sanguin
                UNION ALL SELECT 'A-'
                UNION ALL SELECT 'B+'
                UNION ALL SELECT 'B-'
                UNION ALL SELECT 'O+'
                UNION ALL SELECT 'O-'
                UNION ALL SELECT 'AB+'
                UNION ALL SELECT 'AB-'
            ) groupes

            LEFT JOIN DONNEUR dnr
                ON dnr.donneur_groupe_sanguin = groupes.groupe_sanguin

            LEFT JOIN DON d
                ON d.id_donneur = dnr.id_donneur

            LEFT JOIN SANG s
                ON s.id_don = d.id_don

            GROUP BY groupes.groupe_sanguin

            ORDER BY
                CASE groupes.groupe_sanguin
                    WHEN 'A+' THEN 1
                    WHEN 'A-' THEN 2
                    WHEN 'B+' THEN 3
                    WHEN 'B-' THEN 4
                    WHEN 'O+' THEN 5
                    WHEN 'O-' THEN 6
                    WHEN 'AB+' THEN 7
                    WHEN 'AB-' THEN 8
                END;
        """;

        return await connection.QueryAsync<StockGroupeResult>(sql);
    }

    /// <summary>
    /// Récupère les poches périmées ou proches de la péremption dans les 7 prochains jours.
    /// </summary>
    public async Task<IEnumerable<Sang>> GetAlertesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_sang AS IdSang,
                id_don AS IdDon,
                id_centre AS IdCentre,
                id_hopital AS IdHopital,
                sang_type_composant AS SangTypeComposant,
                sang_date_creation AS SangDateCreation,
                sang_date_peremption AS SangDatePeremption,
                sang_disponible AS SangDisponible
            FROM SANG
            WHERE sang_date_peremption <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
            ORDER BY sang_date_peremption ASC;
        """;

        return await connection.QueryAsync<Sang>(sql);
    }
    
    /// <summary>
    /// Vérifie si une poche de sang existe.
    /// </summary>
    public async Task<bool> ExistsAsync(int idSang)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT COUNT(1)
                               FROM SANG
                               WHERE id_sang = @IdSang;
                           """;

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            IdSang = idSang
        });

        return count > 0;
    }

    /// <summary>
    /// Met à jour la disponibilité d'une poche de sang.
    /// </summary>
    public async Task<bool> UpdateDisponibiliteAsync(int idSang, bool disponible)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               UPDATE SANG
                               SET sang_disponible = @Disponible
                               WHERE id_sang = @IdSang;
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            IdSang = idSang,
            Disponible = disponible
        });

        return rowsAffected > 0;
    }

    /// <summary>
    /// Récupère l'identifiant du don associé à une poche de sang.
    /// </summary>
    public async Task<int?> GetDonIdBySangIdAsync(int idSang)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT id_don
                               FROM SANG
                               WHERE id_sang = @IdSang
                               LIMIT 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<int?>(sql, new
        {
            IdSang = idSang
        });
    }
}