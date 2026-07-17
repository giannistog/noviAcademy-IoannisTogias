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

        services.AddScoped<CreateWalletCommandHandler>();

        services.AddScoped<ICommandHandler<CreateWalletCommand, Wallet>>(sp =>
            new LoggingDecorator<CreateWalletCommand, Wallet>(
                sp.GetRequiredService<CreateWalletCommandHandler>(),
                sp.GetRequiredService<ILogger<LoggingDecorator<CreateWalletCommand, Wallet>>>()));

        services.AddScoped<DepositCommandHandler>();

        services.AddScoped<ICommandHandler<DepositCommand, Wallet>>(sp =>
            new LoggingDecorator<DepositCommand, Wallet>(
                sp.GetRequiredService<DepositCommandHandler>(),
                sp.GetRequiredService<ILogger<LoggingDecorator<DepositCommand, Wallet>>>()));

        services.AddScoped<BlockWalletCommandHandler>();

        services.AddScoped<ICommandHandler<BlockWalletCommand, Wallet>>(sp =>
            new LoggingDecorator<BlockWalletCommand, Wallet>(
                sp.GetRequiredService<BlockWalletCommandHandler>(),
                sp.GetRequiredService<ILogger<LoggingDecorator<BlockWalletCommand, Wallet>>>()));

        services.AddScoped<GetWalletByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetWalletByIdQuery, Wallet?>>(sp =>
            new LoggingQueryDecorator<GetWalletByIdQuery, Wallet?>(
                sp.GetRequiredService<GetWalletByIdQueryHandler>(),
                sp.GetRequiredService<ILogger<LoggingQueryDecorator<GetWalletByIdQuery, Wallet?>>>()));

        return services;
    }
}