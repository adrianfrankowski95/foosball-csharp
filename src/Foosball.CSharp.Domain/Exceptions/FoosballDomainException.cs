namespace Foosball.CSharp.Domain.Exceptions;

public class FoosballDomainException : Exception
{
    public FoosballDomainException(string description) : base(description)
    {

    }
}
