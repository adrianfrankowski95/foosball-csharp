
namespace Foosball.CSharp.Domain.SeedWork;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityAttributes();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not ValueObject valueObject)
        {
            return false;
        }

        return valueObject.GetType().Equals(GetType())
            && GetEqualityAttributes().SequenceEqual(valueObject.GetEqualityAttributes());
    }

    public override int GetHashCode()
        => GetEqualityAttributes()
            .Select(attr => attr?.GetHashCode() ?? 1)
            .Aggregate((x, y) => x ^ y);
}
