using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Models;

namespace BanqueSang.Core.Services;

public class AuthService : IAuthService
{
    private readonly IPersonnelRepository _personnelRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IPersonnelRepository personnelRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _personnelRepository = personnelRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Authentifie un membre du personnel.
    /// Étapes :
    /// 1. Vérifier que les champs ne sont pas vides.
    /// 2. Rechercher le personnel par login.
    /// 3. Vérifier que le compte existe.
    /// 4. Vérifier que le compte est actif.
    /// 5. Vérifier le mot de passe avec BCrypt.
    /// 6. Générer un token JWT.
    /// </summary>
    public async Task<AuthResult> LoginAsync(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new UnauthorizedAccessException("Le login est obligatoire.");

        if (string.IsNullOrWhiteSpace(password))
            throw new UnauthorizedAccessException("Le mot de passe est obligatoire.");

        var personnel = await _personnelRepository.GetByLoginAsync(login);

        if (personnel is null)
            throw new UnauthorizedAccessException("Login ou mot de passe incorrect.");

        if (!personnel.PersonnelActif)
            throw new InvalidOperationException("Ce compte est inactif.");

        var passwordIsValid = _passwordHasher.VerifyPassword(password, personnel.PersonnelPassword);

        if (!passwordIsValid)
            throw new UnauthorizedAccessException("Login ou mot de passe incorrect.");

        var expiration = DateTime.UtcNow.AddHours(24);

        var token = _jwtTokenService.GenerateToken(personnel, expiration);

        return new AuthResult
        {
            Token = token,
            Role = personnel.PersonnelFonction,
            Expiration = expiration
        };
    }
}