using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Infrastructure;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly FoosballDbContext _ctx;
    private bool isDisposed;

    public EfUnitOfWork(FoosballDbContext ctx)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken)
        => _ctx.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        if (!isDisposed)
        {
            isDisposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            _ctx.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!isDisposed)
        {
            isDisposed = true;
            await DisposeAsync(true).ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
    }

    protected async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing)
            await _ctx.DisposeAsync().ConfigureAwait(false);
    }
}
