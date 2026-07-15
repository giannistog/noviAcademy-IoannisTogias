using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Infrastructure.Persistence.Commands.Players;
using WorldRank.Infrastructure.Persistence.Queries.Players;
using WorldRank.Infrastructure.Persistence.Queries;

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

        services.AddScoped<IGetPlayerByIdPersistence>();

        services.AddScoped<IGetPlayerByIdPersistence>(sp =>
            new GetPlayerByIdPersistenceCachingDecorator(
                sp.GetRequiredService<IGetPlayerByIdPersistence>(),
                sp.GetRequiredService<ICache>()));
        return services;
    }
}