using BanqueSang.Core.Interfaces;
using BanqueSang.Infrastructure.Data;
using BanqueSang.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BanqueSang.Infrastructure;

//Ce fichier permet d’enregistrer les classes Infrastructure dans le conteneur de dépendances de .NET. 
//injection de dépendances
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DbConnectionFactory>();

        services.AddScoped<IDatabaseTestRepository, DatabaseTestRepository>();

        return services;
    }
}