using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface ITestRepository
{
    /// <summary>
    /// Récupère tous les types de tests biologiques configurés.
    /// </summary>
    Task<IEnumerable<Test>> GetAllAsync();

    /// <summary>
    /// Vérifie si un test existe à partir de son identifiant.
    /// </summary>
    Task<bool> ExistsAsync(int idTest);
}