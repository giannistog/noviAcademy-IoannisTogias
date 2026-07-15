using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorldRank.Application.Abstractions;
using WorldRank.Application.Commands;
using WorldRank.Application.Decorators;
using WorldRank.Application.Queries;
using WorldRank.Domain.Entities;
using WorldRank.Application.Queries;

namespace WorldRank.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<CreatePlayerCommandHandler>();

        services.AddScoped<ICommandHandler<CreatePlayerCommand, Player>>(sp =>
            new LoggingDecorator<CreatePlayerCommand, Player>(
                sp.GetRequiredService<CreatePlayerCommandHandler>(),
                sp.GetRequiredService<ILogger<LoggingDecorator<CreatePlayerCommand, Player>>>()));

        services.AddScoped<GetPlayerByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetPlayerByIdQuery, Player?>>(sp =>
            new LoggingQueryDecorator<GetPlayerByIdQuery, Player?>(
                sp.GetRequiredService<GetPlayerByIdQueryHandler>(),
                sp.GetRequiredService<ILogger<LoggingQueryDecorator<GetPlayerByIdQuery, Player?>>>()));

        services.AddScoped<GetAllPlayersQueryHandler>();

        services.AddScoped<IQueryHandler<GetAllPlayersQuery, IEnumerable<Player>>>(sp =>
            new LoggingQueryDecorator<GetAllPlayersQuery, IEnumerable<Player>>(
                sp.GetRequiredService<GetAllPlayersQueryHandler>(),
                sp.GetRequiredService<ILogger<LoggingQueryDecorator<GetAllPlayersQuery, IEnumerable<Player>>>>()));

        return services;
    }
}