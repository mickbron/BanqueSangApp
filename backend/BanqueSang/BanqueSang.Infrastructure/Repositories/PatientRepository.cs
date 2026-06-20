using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public PatientRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère tous les patients.
    /// </summary>
    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_patient AS IdPatient,
                id_service AS IdService,
                patient_nom AS PatientNom,
                patient_prenom AS PatientPrenom,
                patient_adresse AS PatientAdresse,
                patient_telephone AS PatientTelephone,
                patient_date_naissance AS PatientDateNaissance,
                patient_groupe_sanguin AS PatientGroupeSanguin
            FROM PATIENT
            ORDER BY patient_nom, patient_prenom;
        """;

        return await connection.QueryAsync<Patient>(sql);
    }

    /// <summary>
    /// Récupère un patient par son identifiant.
    /// </summary>
    public async Task<Patient?> GetByIdAsync(int idPatient)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id_patient AS IdPatient,
                id_service AS IdService,
                patient_nom AS PatientNom,
                patient_prenom AS PatientPrenom,
                patient_adresse AS PatientAdresse,
                patient_telephone AS PatientTelephone,
                patient_date_naissance AS PatientDateNaissance,
                patient_groupe_sanguin AS PatientGroupeSanguin
            FROM PATIENT
            WHERE id_patient = @IdPatient
            LIMIT 1;
        """;

        return await connection.QueryFirstOrDefaultAsync<Patient>(sql, new
        {
            IdPatient = idPatient
        });
    }

    /// <summary>
    /// Crée un nouveau patient.
    /// </summary>
    public async Task<int> CreateAsync(Patient patient)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO PATIENT (
                id_service,
                patient_nom,
                patient_prenom,
                patient_adresse,
                patient_telephone,
                patient_date_naissance,
                patient_groupe_sanguin
            )
            VALUES (
                @IdService,
                @PatientNom,
                @PatientPrenom,
                @PatientAdresse,
                @PatientTelephone,
                @PatientDateNaissance,
                @PatientGroupeSanguin
            );

            SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, patient);
    }

    /// <summary>
    /// Met à jour un patient existant.
    /// </summary>
    public async Task<bool> UpdateAsync(Patient patient)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE PATIENT
            SET
                id_service = @IdService,
                patient_nom = @PatientNom,
                patient_prenom = @PatientPrenom,
                patient_adresse = @PatientAdresse,
                patient_telephone = @PatientTelephone,
                patient_date_naissance = @PatientDateNaissance,
                patient_groupe_sanguin = @PatientGroupeSanguin
            WHERE id_patient = @IdPatient;
        """;

        var rowsAffected = await connection.ExecuteAsync(sql, patient);

        return rowsAffected > 0;
    }

    /// <summary>
    /// Vérifie si un patient existe.
    /// </summary>
    public async Task<bool> ExistsAsync(int idPatient)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT COUNT(1)
            FROM PATIENT
            WHERE id_patient = @IdPatient;
        """;

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            IdPatient = idPatient
        });

        return count > 0;
    }
}