using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface IPersonnelRepository
{
    /// Recherche un membre du personnel à partir de son login.
    /// Retourne null si aucun personnel ne correspond au login donné.
    
    Task<Personnel?> GetByLoginAsync(string login);
}