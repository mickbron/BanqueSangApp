using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class PersonnelRepository : IPersonnelRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public PersonnelRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère un membre du personnel à partir de son login.
    /// Les alias SQL permettent de faire correspondre les colonnes MySQL
    /// avec les propriétés C# de l'entité Personnel.
    /// </summary>
    public async Task<Personnel?> GetByLoginAsync(string login)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
                               SELECT
                                   id_personnel AS IdPersonnel,
                                   personnel_nom AS PersonnelNom,
                                   personnel_prenom AS PersonnelPrenom,
                                   personnel_adresse AS PersonnelAdresse,
                                   personnel_telephone AS PersonnelTelephone,
                                   personnel_date_naissance AS PersonnelDateNaissance,
                                   personnel_fonction AS PersonnelFonction,
                                   personnel_login AS PersonnelLogin,
                                   personnel_password AS PersonnelPassword,
                                   personnel_actif AS PersonnelActif
                               FROM PERSONNEL
                               WHERE personnel_login = @Login
                               LIMIT 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<Personnel>(sql, new
        {
            Login = login
        });
    }
}