using BanqueSang.Core.Interfaces;
using BanqueSang.Core.Services;
using BanqueSang.Infrastructure.Data;
using BanqueSang.Infrastructure.Repositories;
using BanqueSang.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace BanqueSang.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Enregistre les services de la couche Infrastructure dans le conteneur d'injection de dépendances.
    /// Cela permet à l'API d'utiliser les repositories Dapper, BCrypt et JWT.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DbConnectionFactory>();

        services.AddScoped<IDatabaseTestRepository, DatabaseTestRepository>();
        services.AddScoped<IPersonnelRepository, PersonnelRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IAuthService, AuthService>();
        
        services.AddScoped<IDonneurRepository, DonneurRepository>();
        services.AddScoped<IDonneurService, DonneurService>();
        
        services.AddScoped<IDonRepository, DonRepository>();
        services.AddScoped<ISangRepository, SangRepository>();
        services.AddScoped<IDonService, DonService>();
        
        services.AddScoped<IStockService, StockService>();

        return services;
    }
}