using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class DatabaseTestRepository : IDatabaseTestRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DatabaseTestRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<string> TestConnectionAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = "SELECT DATABASE();";

        var databaseName = await connection.QuerySingleAsync<string>(sql);

        return databaseName;
    }
}