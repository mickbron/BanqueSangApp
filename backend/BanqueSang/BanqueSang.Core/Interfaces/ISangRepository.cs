using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface ISangRepository
{
    /// <summary>
    /// Crée une nouvelle poche de sang associée à un don.
    /// Retourne l'identifiant de la poche créée.
    /// </summary>
    Task<int> CreateAsync(Sang sang);
}