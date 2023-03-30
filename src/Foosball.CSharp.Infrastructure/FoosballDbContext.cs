
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Foosball.CSharp.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Foosball.CSharp.Infrastructure;

public class FoosballDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Team> Teams { get; set; }

    public FoosballDbContext(DbContextOptions<FoosballDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameInProgressEntityConfiguration());
        modelBuilder.ApplyConfiguration(new FinishedGameEntityConfiguration());

        modelBuilder.ApplyConfiguration(new SetEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SetInProgressEntityConfiguration());
        modelBuilder.ApplyConfiguration(new FinishedSetEntityConfiguration());

        modelBuilder.ApplyConfiguration(new PlayerEntityConfiguration());

        modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OnePlayerTeamEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TwoPlayersTeamEntityConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.IgnoreAny<IReadOnlyList<DomainEvent>>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DispatchDomainEvents(ChangeTracker);
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        DispatchDomainEvents(ChangeTracker);
        return base.SaveChanges();
    }

    private static void DispatchDomainEvents(ChangeTracker changeTracker)
    {
        var domainEntities = changeTracker
            .Entries<Entity<object>>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents is not null && e.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEvents.ForEach(e => Console.WriteLine($"Dispatching domain event: {e.GetType().Name}, data: {e}"));
        domainEntities.ForEach(e => e.ClearDomainEvents());
    }
}
