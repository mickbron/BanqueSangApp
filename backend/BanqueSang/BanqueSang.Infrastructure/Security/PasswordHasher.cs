using BanqueSang.Core.Interfaces;

namespace BanqueSang.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Vérifie si le mot de passe en clair correspond au hash BCrypt.
    /// BCrypt compare automatiquement le mot de passe saisi avec le hash stocké.
    /// </summary>
    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }

    /// <summary>
    /// Génère un hash BCrypt sécurisé à partir d'un mot de passe en clair.
    /// Cette méthode sera utilisée plus tard pour créer des comptes utilisateurs.
    /// </summary>
    public string HashPassword(string plainPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainPassword);
    }
}