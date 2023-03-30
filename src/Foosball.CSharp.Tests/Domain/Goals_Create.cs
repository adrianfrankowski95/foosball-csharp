using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.Tests.Domain;

public class Goals_Create
{
    [Theory]
    [InlineData(Goals.ToWin + 1)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_CreatedWithInvalidValue_ThrowsDomainException(int goals)
    {
        Assert.Throws<FoosballDomainException>(() => goals.Goals());
    }

    [Theory]
    [InlineData(Goals.ToWin)]
    [InlineData(0)]
    public void Create_CreatedWithValidValue_HaveValidValue(int goals)
    {
        Assert.True(goals.Goals().Value == goals);
    }
}
