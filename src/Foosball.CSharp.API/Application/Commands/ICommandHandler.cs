namespace Foosball.CSharp.API.Application.Commands;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
