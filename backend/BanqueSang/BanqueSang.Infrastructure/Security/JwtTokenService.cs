using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BanqueSang.Core.Entities;
using BanqueSang.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BanqueSang.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Génère un token JWT contenant les informations principales du personnel :
    /// identifiant, login et rôle.
    /// Ce token sera utilisé par Angular pour accéder aux routes protégées.
    /// </summary>
    public string GenerateToken(Personnel personnel, DateTime expiration)
    {
        var jwtKey = _configuration["Jwt:Key"]
                     ?? throw new InvalidOperationException("La clé JWT est introuvable.");

        var issuer = _configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("L'issuer JWT est introuvable.");

        var audience = _configuration["Jwt:Audience"]
                       ?? throw new InvalidOperationException("L'audience JWT est introuvable.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, personnel.IdPersonnel.ToString()),
            new Claim(ClaimTypes.Name, personnel.PersonnelLogin),
            new Claim(ClaimTypes.Role, personnel.PersonnelFonction)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}