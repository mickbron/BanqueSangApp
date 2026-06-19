using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
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
    /// La poche est généralement indisponible au départ, car elle doit passer les tests biologiques.
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
}