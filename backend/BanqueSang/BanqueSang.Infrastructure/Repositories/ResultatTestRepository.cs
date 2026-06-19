using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class ResultatTestRepository : IResultatTestRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ResultatTestRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère tous les résultats de tests avec les informations du test et du personnel.
    /// </summary>
    public async Task<IEnumerable<ResultatTestDetail>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                rt.id_resultat_test AS IdResultatTest,
                rt.id_personnel AS IdPersonnel,
                p.personnel_nom AS PersonnelNom,
                p.personnel_prenom AS PersonnelPrenom,
                rt.id_sang AS IdSang,
                rt.id_test AS IdTest,
                t.test_nom AS TestNom,
                rt.date_test AS DateTest,
                rt.resultat AS Resultat,
                rt.commentaire AS Commentaire,
                rt.statut_test AS StatutTest
            FROM RESULTAT_TEST rt
            INNER JOIN PERSONNEL p ON rt.id_personnel = p.id_personnel
            INNER JOIN TEST t ON rt.id_test = t.id_test
            ORDER BY rt.date_test DESC;
        """;

        return await connection.QueryAsync<ResultatTestDetail>(sql);
    }

    /// <summary>
    /// Récupère les résultats d'une poche de sang donnée.
    /// </summary>
    public async Task<IEnumerable<ResultatTest>> GetBySangIdAsync(int idSang)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_resultat_test AS IdResultatTest,
                id_personnel AS IdPersonnel,
                id_sang AS IdSang,
                id_test AS IdTest,
                date_test AS DateTest,
                resultat AS Resultat,
                commentaire AS Commentaire,
                statut_test AS StatutTest
            FROM RESULTAT_TEST
            WHERE id_sang = @IdSang;
        """;

        return await connection.QueryAsync<ResultatTest>(sql, new
        {
            IdSang = idSang
        });
    }

    /// <summary>
    /// Vérifie si un résultat existe déjà pour une poche et un test.
    /// </summary>
    public async Task<bool> ExistsForSangAndTestAsync(int idSang, int idTest)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT COUNT(1)
            FROM RESULTAT_TEST
            WHERE id_sang = @IdSang
              AND id_test = @IdTest;
        """;

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            IdSang = idSang,
            IdTest = idTest
        });

        return count > 0;
    }

    /// <summary>
    /// Insère un nouveau résultat de test biologique.
    /// </summary>
    public async Task<int> CreateAsync(ResultatTest resultatTest)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO RESULTAT_TEST (
                id_personnel,
                id_sang,
                id_test,
                date_test,
                resultat,
                commentaire,
                statut_test
            )
            VALUES (
                @IdPersonnel,
                @IdSang,
                @IdTest,
                @DateTest,
                @Resultat,
                @Commentaire,
                @StatutTest
            );

            SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, resultatTest);
    }
    
    /// <summary>
    /// Récupère un résultat de test par son identifiant.
    /// </summary>
    public async Task<ResultatTest?> GetByIdAsync(int idResultatTest)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT
                                   id_resultat_test AS IdResultatTest,
                                   id_personnel AS IdPersonnel,
                                   id_sang AS IdSang,
                                   id_test AS IdTest,
                                   date_test AS DateTest,
                                   resultat AS Resultat,
                                   commentaire AS Commentaire,
                                   statut_test AS StatutTest
                               FROM RESULTAT_TEST
                               WHERE id_resultat_test = @IdResultatTest
                               LIMIT 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<ResultatTest>(sql, new
        {
            IdResultatTest = idResultatTest
        });
    }

    /// <summary>
    /// Met à jour le résultat, le commentaire et le statut d'un test biologique.
    /// </summary>
    public async Task<bool> UpdateAsync(ResultatTest resultatTest)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               UPDATE RESULTAT_TEST
                               SET
                                   resultat = @Resultat,
                                   commentaire = @Commentaire,
                                   statut_test = @StatutTest
                               WHERE id_resultat_test = @IdResultatTest;
                           """;

        var rowsAffected = await connection.ExecuteAsync(sql, resultatTest);

        return rowsAffected > 0;
    }
}