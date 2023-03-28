using Foosball.CSharp.Domain.AggregateModel;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.Events;

public record GameFinishedDomainEvent(GameId GameId, TeamId TeamAId, TeamId TeamBId, TeamId WinnerTeamId, FinishedSets Sets) : DomainEvent;
