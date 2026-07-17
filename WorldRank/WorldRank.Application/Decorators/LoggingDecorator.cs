using Microsoft.Extensions.Logging;
using WorldRank.Application.Abstractions;

namespace WorldRank.Application.Decorators;

public class LoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;
    private readonly ILogger<LoggingDecorator<TCommand, TResult>> _logger;

    public LoggingDecorator(ICommandHandler<TCommand, TResult> inner, ILogger<LoggingDecorator<TCommand, TResult>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResult> Handle(TCommand command, CancellationToken ct = default)
    {
        _logger.LogInformation("Handling {Command}", typeof(TCommand).Name);
        var result = await _inner.Handle(command, ct);
        _logger.LogInformation("Handled {Command} successfully", typeof(TCommand).Name);

        return result;
    }
}