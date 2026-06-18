namespace BanqueSang.Core.Interfaces;

public interface IPasswordHasher
{
    /// Vérifie si le mot de passe saisi correspond au mot de passe hashé stocké en base de données.
    bool VerifyPassword(string plainPassword, string hashedPassword);

    
    /// Génère un hash BCrypt à partir d'un mot de passe en clair.
    /// Cette méthode servira pour créer des comptes de test ou de nouveaux utilisateurs.
    string HashPassword(string plainPassword);
}