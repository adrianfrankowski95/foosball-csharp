using Foosball.CSharp.Domain.AggregateModel;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.Events;

public record GameUpdatedDomainEvent(GameId GameId, TeamId TeamAId, TeamId TeamBId, SetsInProgress Sets) : DomainEvent;
