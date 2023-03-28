namespace Foosball.CSharp.Domain.AggregateModel;

using System.Collections.Generic;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

public class Goals : ValueObject, IComparable<Goals>
{
    public static readonly Goals ToWin = 10.Goals();
    public int Value { get; }

    public Goals(int value)
    {
        if (value < 0)
        {
            throw new FoosballDomainException("Goals cannot have a negative value.");
        }

        if (value > ToWin.Value)
        {
            throw new FoosballDomainException($"Scored goals cannot exceed ${ToWin.Value}.");
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