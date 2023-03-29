using System.Data.Common;
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Foosball.CSharp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Foosball.CSharp.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        string? connectionString = config.GetConnectionString("FoosballDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Missing FoosballDb connection string.");
        }

        services.AddDbContextPool<FoosballDbContext>(opts =>
        {
            opts.UseNpgsql(connectionString, opts =>
            {
                opts.MigrationsAssembly("Foosball.CSharp.API");
            });
            opts.UseSnakeCaseNamingConvention();
        });

        services.TryAddScoped<IGameRepository, EfGameRepository>();
        services.TryAddScoped<ITeamRepository, EfTeamRepository>();

        services.AddHostedService<FoosballDbMigrator>();
        services.AddHostedService<FoosballDbSeeder>();

        services.AddTransient<DbConnection, NpgsqlConnection>(sp => new NpgsqlConnection(connectionString));

        return services;
    }
}
