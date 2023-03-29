using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Domain.Events;

public record GameUpdatedDomainEvent(GameId GameId, TeamId TeamAId, TeamId TeamBId, IReadOnlyList<Set> Sets) : DomainEvent;
