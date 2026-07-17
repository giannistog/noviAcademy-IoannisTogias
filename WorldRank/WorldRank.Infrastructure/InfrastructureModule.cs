using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Commands;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Infrastructure.Persistence.Commands.Players;
using WorldRank.Infrastructure.Persistence.Commands.Wallets;
using WorldRank.Infrastructure.Persistence.Queries.Players;
using WorldRank.Infrastructure.Persistence.Queries.Players;
using WorldRank.Infrastructure.Persistence.Queries.Wallets;


namespace WorldRank.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
    {
        services.AddScoped<CreatePlayerPersistence>();

        services.AddScoped<ICreatePlayerPersistence>(sp =>
            new CreatePlayerPersistenceCachingDecorator(
                sp.GetRequiredService<CreatePlayerPersistence>(),
                sp.GetRequiredService<ICache>()));

        services.AddScoped<GetAllPlayersPersistence>();

        services.AddScoped<IGetAllPlayersPersistence>(sp =>
            new GetAllPlayersPersistenceCachingDecorator(
                sp.GetRequiredService<GetAllPlayersPersistence>(),
                sp.GetRequiredService<ICache>()));

        services.AddScoped<GetPlayerByIdPersistence>();

        services.AddScoped<IGetPlayerByIdPersistence>(sp =>
            new GetPlayerByIdPersistenceCachingDecorator(
                sp.GetRequiredService<IGetPlayerByIdPersistence>(),
                sp.GetRequiredService<ICache>()));

        services.AddScoped<CreateWalletPersistence>();
        services.AddScoped<ICreateWalletPersistence>(sp =>
            new CreateWalletPersistenceCachingDecorator(
                sp.GetRequiredService<CreateWalletPersistence>(),
                sp.GetRequiredService<ICache>()));

        services.AddScoped<PlayerExistsPersistence>();
        services.AddScoped<IPlayerExistsPersistence, PlayerExistsPersistence>();

        services.AddScoped<DepositPersistence>();
        services.AddScoped<IDepositPersistence>(sp =>
            new DepositPersistenceCachingDecorator(
                sp.GetRequiredService<DepositPersistence>(),
                sp.GetRequiredService<ICache>()));

        services.AddScoped<BlockWalletPersistence>();
        services.AddScoped<IBlockWalletPersistence>(sp =>
            new BlockWalletPersistenceCachingDecorator(
                sp.GetRequiredService<BlockWalletPersistence>(),
                sp.GetRequiredService<ICache>()));

        services.AddScoped<GetWalletByIdPersistence>();
        services.AddScoped<IGetWalletByIdPersistence>(sp =>
            new GetWalletByIdPersistenceCachingDecorator(
                sp.GetRequiredService<GetWalletByIdPersistence>(),
                sp.GetRequiredService<ICache>()));

        return services;
    }
}