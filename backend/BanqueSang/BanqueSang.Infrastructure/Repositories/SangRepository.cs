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
    /// Calcule le stock par groupe sanguin en passant par la relation SANG -> DON -> DONNEUR.
    /// </summary>
    public async Task<IEnumerable<StockGroupeResult>> GetStockParGroupeAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                dnr.donneur_groupe_sanguin AS GroupeSanguin,
                COUNT(s.id_sang) AS Total,
                SUM(CASE 
                    WHEN s.sang_disponible = TRUE 
                         AND s.sang_date_peremption >= CURDATE()
                    THEN 1 ELSE 0 END) AS Disponibles,
                SUM(CASE 
                    WHEN s.sang_disponible = FALSE 
                    THEN 1 ELSE 0 END) AS Indisponibles,
                SUM(CASE 
                    WHEN s.sang_date_peremption < CURDATE()
                    THEN 1 ELSE 0 END) AS Perimees,
                SUM(CASE 
                    WHEN s.sang_date_peremption >= CURDATE()
                         AND s.sang_date_peremption <= DATE_ADD(CURDATE(), INTERVAL 7 DAY)
                    THEN 1 ELSE 0 END) AS ProchesPeremption
            FROM SANG s
            INNER JOIN DON d ON s.id_don = d.id_don
            INNER JOIN DONNEUR dnr ON d.id_donneur = dnr.id_donneur
            GROUP BY dnr.donneur_groupe_sanguin
            ORDER BY dnr.donneur_groupe_sanguin;
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
}