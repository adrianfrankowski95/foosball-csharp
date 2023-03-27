namespace Foosball.CSharp.Domain.AggregateModel;

using System.Collections.Generic;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

public class Goals : ValueObject, IComparable<Goals>
{
    public const int MaxValue = 10;
    public int Value { get; }

    public Goals(int value)
    {
        if (value < 0)
        {
            throw new FoosballDomainException("Goals cannot have a negative value.");
        }

        if (value > MaxValue)
        {
            throw new FoosballDomainException($"Scored goals cannot exceed ${MaxValue}.");
        }

        Value = value;
    }
    public int CompareTo(Goals? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return Value.CompareTo(other.Value);
    }

    public static bool operator >(Goals a, Goals b) => a.CompareTo(b) > 0;
    public static bool operator <(Goals a, Goals b) => a.CompareTo(b) < 0;
    public static bool operator ==(Goals a, Goals b) => a.CompareTo(b) == 0;
    public static bool operator !=(Goals a, Goals b) => a.CompareTo(b) != 0;

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override bool Equals(object? obj)
    {
        return Value.Equals(obj);
    }
}

public static class GoalsExtensions
{
    public static Goals Goals(this int value) => new(value);
}