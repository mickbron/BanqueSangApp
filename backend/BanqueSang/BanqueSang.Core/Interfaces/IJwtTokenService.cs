using BanqueSang.Core.Entities;

namespace BanqueSang.Core.Interfaces;

public interface IJwtTokenService
{
    
    /// Génère un token JWT pour un utilisateur authentifié.
    /// Le token contient l'identifiant, le login et le rôle du personnel.
    
    string GenerateToken(Personnel personnel, DateTime expiration);
}