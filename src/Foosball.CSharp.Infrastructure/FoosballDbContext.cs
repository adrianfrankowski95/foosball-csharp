
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Foosball.CSharp.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

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
}
