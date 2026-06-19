using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class DonneurRepository : IDonneurRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DonneurRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère tous les donneurs depuis MySQL.
    /// Les alias SQL permettent à Dapper de remplir correctement les propriétés C#.
    /// </summary>
    public async Task<IEnumerable<Donneur>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_donneur AS IdDonneur,
                donneur_nom AS DonneurNom,
                donneur_prenom AS DonneurPrenom,
                donneur_adresse AS DonneurAdresse,
                donneur_telephone AS DonneurTelephone,
                donneur_date_naissance AS DonneurDateNaissance,
                donneur_groupe_sanguin AS DonneurGroupeSanguin,
                donneur_statut_eligibilite AS DonneurStatutEligibilite,
                donneur_dernier_don AS DonneurDernierDon
            FROM DONNEUR
            ORDER BY donneur_nom, donneur_prenom;
        """;

        return await connection.QueryAsync<Donneur>(sql);
    }

    /// <summary>
    /// Récupère un donneur à partir de son identifiant.
    /// Retourne null si aucun enregistrement n'est trouvé.
    /// </summary>
    public async Task<Donneur?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_donneur AS IdDonneur,
                donneur_nom AS DonneurNom,
                donneur_prenom AS DonneurPrenom,
                donneur_adresse AS DonneurAdresse,
                donneur_telephone AS DonneurTelephone,
                donneur_date_naissance AS DonneurDateNaissance,
                donneur_groupe_sanguin AS DonneurGroupeSanguin,
                donneur_statut_eligibilite AS DonneurStatutEligibilite,
                donneur_dernier_don AS DonneurDernierDon
            FROM DONNEUR
            WHERE id_donneur = @Id
            LIMIT 1;
        """;

        return await connection.QueryFirstOrDefaultAsync<Donneur>(sql, new
        {
            Id = id
        });
    }

    /// <summary>
    /// Insère un nouveau donneur dans la base de données.
    /// LAST_INSERT_ID() permet de récupérer l'identifiant généré par MySQL.
    /// </summary>
    public async Task<int> CreateAsync(Donneur donneur)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO DONNEUR (
                donneur_nom,
                donneur_prenom,
                donneur_adresse,
                donneur_telephone,
                donneur_date_naissance,
                donneur_groupe_sanguin,
                donneur_statut_eligibilite,
                donneur_dernier_don
            )
            VALUES (
                @DonneurNom,
                @DonneurPrenom,
                @DonneurAdresse,
                @DonneurTelephone,
                @DonneurDateNaissance,
                @DonneurGroupeSanguin,
                @DonneurStatutEligibilite,
                @DonneurDernierDon
            );

            SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, donneur);
    }

    /// <summary>
    /// Met à jour les informations principales d'un donneur.
    /// ExecuteAsync retourne le nombre de lignes modifiées.
    /// </summary>
    public async Task<bool> UpdateAsync(Donneur donneur)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE DONNEUR
            SET
                donneur_nom = @DonneurNom,
                donneur_prenom = @DonneurPrenom,
                donneur_adresse = @DonneurAdresse,
                donneur_telephone = @DonneurTelephone,
                donneur_date_naissance = @DonneurDateNaissance,
                donneur_groupe_sanguin = @DonneurGroupeSanguin,
                donneur_statut_eligibilite = @DonneurStatutEligibilite,
                donneur_dernier_don = @DonneurDernierDon
            WHERE id_donneur = @IdDonneur;
        """;

        var rowsAffected = await connection.ExecuteAsync(sql, donneur);

        return rowsAffected > 0;
    }

    /// <summary>
    /// Met à jour uniquement le statut d'éligibilité.
    /// Cette méthode est utilisée après la vérification des règles métier.
    /// </summary>
    public async Task<bool> UpdateEligibiliteAsync(int idDonneur, string statutEligibilite)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE DONNEUR
            SET donneur_statut_eligibilite = @StatutEligibilite
            WHERE id_donneur = @IdDonneur;
        """;

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            IdDonneur = idDonneur,
            StatutEligibilite = statutEligibilite
        });

        return rowsAffected > 0;
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