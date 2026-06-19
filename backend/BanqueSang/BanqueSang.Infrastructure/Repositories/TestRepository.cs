using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class TestRepository : ITestRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public TestRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère tous les types de tests biologiques.
    /// </summary>
    public async Task<IEnumerable<Test>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT
                                   id_test AS IdTest,
                                   test_nom AS TestNom
                               FROM TEST
                               ORDER BY test_nom;
                           """;

        return await connection.QueryAsync<Test>(sql);
    }

    /// <summary>
    /// Vérifie l'existence d'un type de test biologique.
    /// </summary>
    public async Task<bool> ExistsAsync(int idTest)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT COUNT(1)
                               FROM TEST
                               WHERE id_test = @IdTest;
                           """;

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            IdTest = idTest
        });

        return count > 0;
    }
}