namespace Foosball.CSharp.Domain.AggregateModel;

using System.Collections.Generic;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

public class Goals : ValueObject, IComparable<Goals>
{
    public const int ToWin = 10;
    public int Value { get; }

    public Goals(int value)
    {
        if (value < 0)
        {
            throw new FoosballDomainException("Goals cannot have a negative value.");
        }

        if (value > ToWin)
        {
            throw new FoosballDomainException($"Scored goals cannot exceed {ToWin}.");
        }

        Value = value;
    }

    public int CompareTo(Goals? other)
    {
        return Value.CompareTo(other?.Value ?? 0);
    }

    public static bool operator >(Goals a, Goals b) => a.CompareTo(b) > 0;
    public static bool operator <(Goals a, Goals b) => a.CompareTo(b) < 0;
    public static bool operator ==(Goals a, Goals b) => a.CompareTo(b) == 0;
    public static bool operator !=(Goals a, Goals b) => a.CompareTo(b) != 0;
    public static bool operator >=(Goals a, Goals b) => a.CompareTo(b) >= 0;
    public static bool operator <=(Goals a, Goals b) => a.CompareTo(b) <= 0;

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