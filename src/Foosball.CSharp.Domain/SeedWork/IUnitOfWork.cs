namespace Foosball.CSharp.Domain.SeedWork;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
