
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Foosball.CSharp.Infrastructure;

public class FoosballDbMigrator : IHostedService
{
    private readonly IServiceProvider _services;

    public FoosballDbMigrator(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<FoosballDbContext>();
            ctx.Database.Migrate();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
