namespace Foosball.CSharp.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}
