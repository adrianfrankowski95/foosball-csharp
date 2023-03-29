using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Domain.Events;

public record GameCreatedDomainEvent(GameId GameId, TeamId TeamAId, TeamId TeamBId, DateTime StartedAt) : DomainEvent;
