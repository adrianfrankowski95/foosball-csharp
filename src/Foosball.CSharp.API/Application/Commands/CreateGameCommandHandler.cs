using Foosball.CSharp.API.Exceptions;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Foosball.CSharp.API.Application.Commands;

public class CreateGameCommandHandler : ICreateGameCommandHandler
{
    private readonly ITeamRepository _teamRepository;
    private readonly IGameRepository _gameRepository;

    public CreateGameCommandHandler(ITeamRepository teamRepository, IGameRepository gameRepository)
    {
        _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async Task<GameId> HandleAsync(CreateGameCommand command, CancellationToken cancellationToken)
    {
        var teamA = await _teamRepository.GetTeamAsync(TeamId.FromExisting(command.TeamAId), cancellationToken).ConfigureAwait(false);
        var teamB = await _teamRepository.GetTeamAsync(TeamId.FromExisting(command.TeamBId), cancellationToken).ConfigureAwait(false);

        if (teamA is null || teamB is null)
        {
            throw new ObjectNotFoundException("teams", command.TeamAId, command.TeamBId);
        }

        GameInProgress game = teamA switch
        {
            OnePlayerTeam onePlayerTeamA when teamB is OnePlayerTeam onePlayerTeamB
                => GameInProgress.Create(onePlayerTeamA, onePlayerTeamB, DateTime.UtcNow),

            TwoPlayerTeam TwoPlayerTeamA when teamB is TwoPlayerTeam TwoPlayerTeamB
                => GameInProgress.Create(TwoPlayerTeamA, TwoPlayerTeamB, DateTime.UtcNow),

            _ => throw new FoosballDomainException("Both teams must contain the same number of players."),
        };

        return await _gameRepository.AddGameAsync(game, cancellationToken).ConfigureAwait(false)
            ? game.Id
            : throw new DbUpdateException("Error saving a new game.");
    }
}
