using Foosball.CSharp.Domain.TeamAggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Foosball.CSharp.Infrastructure.Repositories;

public class EfTeamRepository : ITeamRepository
{
    private readonly FoosballDbContext _ctx;
    private readonly DbSet<Team> _teams;

    public EfTeamRepository(FoosballDbContext ctx)
    {
        _teams = ctx.Teams ?? throw new ArgumentNullException(nameof(ctx));
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
    }

    public Task<Team?> GetTeamAsync(TeamId teamId, CancellationToken cancellationToken = default)
        => _teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id.Equals(teamId), cancellationToken);

    public async Task<bool> AddTeamAsync(Team team, CancellationToken cancellationToken = default)
    {
        _teams.Add(team);
        return (await _ctx.SaveChangesAsync(cancellationToken)) > 0;
    }
}
