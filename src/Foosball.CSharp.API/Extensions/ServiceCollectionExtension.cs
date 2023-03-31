
using Foosball.CSharp.API.Application.Commands;
using Foosball.CSharp.API.Application.Queries;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Foosball.CSharp.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddQueriesAndCommandHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<IGameQueries, DapperGameQueries>();
        services.TryAddScoped<ICreateGameCommandHandler, CreateGameCommandHandler>();
        services.TryAddScoped<IUpdateGameCommandHandler, UpdateGameCommandHandler>();

        return services;
    }
}
