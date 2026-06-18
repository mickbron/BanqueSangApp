using BanqueSang.Core.Models;

namespace BanqueSang.Core.Interfaces;

public interface IAuthService
{
   
    /// Authentifie un utilisateur avec son login et son mot de passe.
    /// Retourne un résultat contenant le token JWT si l'authentification réussit.
   
    Task<AuthResult> LoginAsync(string login, string password);
}