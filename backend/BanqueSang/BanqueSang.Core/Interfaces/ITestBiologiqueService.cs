using BanqueSang.Core.Entities;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface ITestBiologiqueService
{
    /// <summary>
    /// Récupère tous les types de tests biologiques.
    /// </summary>
    Task<IEnumerable<Test>> GetTestsAsync();

    /// <summary>
    /// Récupère tous les résultats de tests déjà soumis.
    /// </summary>
    Task<IEnumerable<ResultatTestDetail>> GetResultatsAsync();

    /// <summary>
    /// Soumet un résultat de test biologique pour une poche de sang.
    /// </summary>
    Task<int> SoumettreResultatAsync(ResultatTest resultatTest);
}