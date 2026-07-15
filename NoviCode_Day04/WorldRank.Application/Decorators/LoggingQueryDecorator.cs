using Microsoft.Extensions.Logging;
using WorldRank.Application.Abstractions;

namespace WorldRank.Application.Decorators;

public class LoggingQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _inner;
    private readonly ILogger<LoggingQueryDecorator<TQuery, TResult>> _logger;

    public LoggingQueryDecorator(IQueryHandler<TQuery, TResult> inner, ILogger<LoggingQueryDecorator<TQuery, TResult>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResult> Handle(TQuery query, CancellationToken ct = default)
    {
        _logger.LogInformation("Handling {Query}", typeof(TQuery).Name);
        var result = await _inner.Handle(query, ct);
        _logger.LogInformation("Handled {Query} successfully", typeof(TQuery).Name);

        return result;
    }
}