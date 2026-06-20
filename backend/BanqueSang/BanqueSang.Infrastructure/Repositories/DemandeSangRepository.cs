using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;
using BanqueSang.Infrastructure.Data;
using Dapper;

namespace BanqueSang.Infrastructure.Repositories;

public class DemandeSangRepository : IDemandeSangRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DemandeSangRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Récupère toutes les demandes de sang avec les informations du patient et du personnel.
    /// </summary>
    public async Task<IEnumerable<DemandeSangDetail>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                ds.id_demande AS IdDemande,
                ds.id_patient AS IdPatient,
                p.patient_nom AS PatientNom,
                p.patient_prenom AS PatientPrenom,
                p.patient_groupe_sanguin AS PatientGroupeSanguin,
                ds.id_sang AS IdSang,
                ds.id_personnel AS IdPersonnel,
                pe.personnel_nom AS PersonnelNom,
                pe.personnel_prenom AS PersonnelPrenom,
                ds.type_composant AS TypeComposant,
                ds.quantite_poches AS QuantitePoche,
                ds.urgence AS Urgence,
                ds.date_demande AS DateDemande,
                ds.commentaire AS Commentaire,
                ds.statut AS Statut
            FROM DEMANDE_SANG ds
            INNER JOIN PATIENT p ON ds.id_patient = p.id_patient
            INNER JOIN PERSONNEL pe ON ds.id_personnel = pe.id_personnel
            ORDER BY ds.date_demande DESC;
        """;

        return await connection.QueryAsync<DemandeSangDetail>(sql);
    }

    /// <summary>
    /// Récupère une demande de sang par son identifiant.
    /// </summary>
    public async Task<DemandeSangDetail?> GetByIdAsync(int idDemande)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                ds.id_demande AS IdDemande,
                ds.id_patient AS IdPatient,
                p.patient_nom AS PatientNom,
                p.patient_prenom AS PatientPrenom,
                p.patient_groupe_sanguin AS PatientGroupeSanguin,
                ds.id_sang AS IdSang,
                ds.id_personnel AS IdPersonnel,
                pe.personnel_nom AS PersonnelNom,
                pe.personnel_prenom AS PersonnelPrenom,
                ds.type_composant AS TypeComposant,
                ds.quantite_poches AS QuantitePoche,
                ds.urgence AS Urgence,
                ds.date_demande AS DateDemande,
                ds.commentaire AS Commentaire,
                ds.statut AS Statut
            FROM DEMANDE_SANG ds
            INNER JOIN PATIENT p ON ds.id_patient = p.id_patient
            INNER JOIN PERSONNEL pe ON ds.id_personnel = pe.id_personnel
            WHERE ds.id_demande = @IdDemande
            LIMIT 1;
        """;

        return await connection.QueryFirstOrDefaultAsync<DemandeSangDetail>(sql, new
        {
            IdDemande = idDemande
        });
    }

    /// <summary>
    /// Crée une nouvelle demande de sang.
    /// Le statut est défini par le service, généralement EN_ATTENTE.
    /// </summary>
    public async Task<int> CreateAsync(DemandeSang demandeSang)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO DEMANDE_SANG (
                id_patient,
                id_sang,
                id_personnel,
                type_composant,
                quantite_poches,
                urgence,
                date_demande,
                commentaire,
                statut
            )
            VALUES (
                @IdPatient,
                @IdSang,
                @IdPersonnel,
                @TypeComposant,
                @QuantitePoche,
                @Urgence,
                @DateDemande,
                @Commentaire,
                @Statut
            );

            SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, demandeSang);
    }

    /// <summary>
    /// Met à jour uniquement le statut d'une demande de sang.
    /// </summary>
    public async Task<bool> UpdateStatutAsync(int idDemande, string statut)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE DEMANDE_SANG
            SET statut = @Statut
            WHERE id_demande = @IdDemande;
        """;

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            IdDemande = idDemande,
            Statut = statut
        });

        return rowsAffected > 0;
    }
}