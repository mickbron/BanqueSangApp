using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class TestBiologiqueService : ITestBiologiqueService
{
    private readonly ITestRepository _testRepository;
    private readonly IResultatTestRepository _resultatTestRepository;
    private readonly ISangRepository _sangRepository;
    private readonly IDonRepository _donRepository;

    public TestBiologiqueService(
        ITestRepository testRepository,
        IResultatTestRepository resultatTestRepository,
        ISangRepository sangRepository,
        IDonRepository donRepository)
    {
        _testRepository = testRepository;
        _resultatTestRepository = resultatTestRepository;
        _sangRepository = sangRepository;
        _donRepository = donRepository;
    }

    /// <summary>
    /// Retourne la liste des tests biologiques configurés dans la table TEST.
    /// </summary>
    public async Task<IEnumerable<Test>> GetTestsAsync()
    {
        return await _testRepository.GetAllAsync();
    }

    /// <summary>
    /// Retourne la liste complète des résultats biologiques déjà enregistrés.
    /// </summary>
    public async Task<IEnumerable<ResultatTestDetail>> GetResultatsAsync()
    {
        return await _resultatTestRepository.GetAllAsync();
    }

    /// <summary>
    /// Soumet un résultat de test biologique.
    /// Règles appliquées :
    /// - la poche de sang doit exister ;
    /// - le type de test doit exister ;
    /// - le même test ne peut pas être soumis deux fois pour la même poche ;
    /// - un résultat positif rejette immédiatement le don et rend la poche indisponible ;
    /// - si tous les tests obligatoires sont négatifs, le don est validé et la poche devient disponible.
    /// </summary>
    public async Task<int> SoumettreResultatAsync(ResultatTest resultatTest)
    {
        if (resultatTest.IdPersonnel <= 0)
            throw new ArgumentException("Le personnel est obligatoire.");

        if (resultatTest.IdSang <= 0)
            throw new ArgumentException("La poche de sang est obligatoire.");

        if (resultatTest.IdTest <= 0)
            throw new ArgumentException("Le type de test est obligatoire.");

        if (string.IsNullOrWhiteSpace(resultatTest.Resultat))
            throw new ArgumentException("Le résultat du test est obligatoire.");

        var resultatsAutorises = new[] { "NEGATIF", "POSITIF", "EN_ATTENTE" };

        if (!resultatsAutorises.Contains(resultatTest.Resultat))
            throw new ArgumentException("Le résultat doit être NEGATIF, POSITIF ou EN_ATTENTE.");

        var sangExists = await _sangRepository.ExistsAsync(resultatTest.IdSang);

        if (!sangExists)
            throw new KeyNotFoundException("Poche de sang introuvable.");

        var testExists = await _testRepository.ExistsAsync(resultatTest.IdTest);

        if (!testExists)
            throw new KeyNotFoundException("Type de test introuvable.");

        var alreadyExists = await _resultatTestRepository.ExistsForSangAndTestAsync(
            resultatTest.IdSang,
            resultatTest.IdTest
        );

        if (alreadyExists)
            throw new InvalidOperationException("Ce test a déjà été soumis pour cette poche de sang.");

        resultatTest.DateTest = resultatTest.DateTest == default
            ? DateTime.Now
            : resultatTest.DateTest;

        resultatTest.StatutTest = resultatTest.Resultat == "EN_ATTENTE"
            ? "EN_ATTENTE"
            : "VALIDE";

        var idResultat = await _resultatTestRepository.CreateAsync(resultatTest);

        await MettreAJourStatutPocheEtDonAsync(resultatTest.IdSang);

        return idResultat;
    }

    /// <summary>
    /// Met à jour automatiquement le statut du don et la disponibilité de la poche
    /// selon les résultats biologiques déjà enregistrés.
    /// </summary>
    private async Task MettreAJourStatutPocheEtDonAsync(int idSang)
    {
        var donId = await _sangRepository.GetDonIdBySangIdAsync(idSang);

        if (donId is null)
            return;

        var resultats = (await _resultatTestRepository.GetBySangIdAsync(idSang)).ToList();
        var tests = (await _testRepository.GetAllAsync()).ToList();

        var hasPositive = resultats.Any(r => r.Resultat == "POSITIF");

        if (hasPositive)
        {
            await _donRepository.UpdateStatutAsync(donId.Value, "REJETE");
            await _sangRepository.UpdateDisponibiliteAsync(idSang, false);
            return;
        }

        var nombreTestsObligatoires = tests.Count;
        var nombreTestsNegatifs = resultats.Count(r => r.Resultat == "NEGATIF");

        if (nombreTestsNegatifs >= nombreTestsObligatoires)
        {
            await _donRepository.UpdateStatutAsync(donId.Value, "VALIDE");
            await _sangRepository.UpdateDisponibiliteAsync(idSang, true);
            return;
        }

        await _donRepository.UpdateStatutAsync(donId.Value, "EN_ATTENTE");
        await _sangRepository.UpdateDisponibiliteAsync(idSang, false);
    }
}