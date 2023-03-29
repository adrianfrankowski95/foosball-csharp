
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.TeamAggregateModel;

public interface ITeamRepository : IRepository<Team>
{
    public Task<Team?> GetTeamAsync(TeamId teamId, CancellationToken cancellationToken = default);
}
