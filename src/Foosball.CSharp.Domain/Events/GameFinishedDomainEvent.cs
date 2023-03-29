using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Domain.Events;

public record GameFinishedDomainEvent(GameId GameId, TeamId TeamAId, TeamId TeamBId, TeamId WinnerTeamId, IReadOnlyList<FinishedSet> Sets) : DomainEvent;
