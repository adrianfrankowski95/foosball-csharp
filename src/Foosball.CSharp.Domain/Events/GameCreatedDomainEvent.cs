using Foosball.CSharp.Domain.AggregateModel;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.Events;

public record GameCreatedDomainEvent(GameId GameId, TeamId TeamAId, TeamId TeamBId, DateTime StartedAt) : DomainEvent;
