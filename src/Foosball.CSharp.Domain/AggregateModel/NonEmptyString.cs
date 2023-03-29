using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public class NonEmptyString : ValueObject
{
    public string Value { get; }

    public NonEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new FoosballDomainException("Value must not be null or empty.");
        }

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public static implicit operator NonEmptyString(string val) => new(val);

    public override string ToString() => Value;
}
