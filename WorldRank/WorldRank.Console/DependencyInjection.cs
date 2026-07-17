using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using WorldRank.Application;
using WorldRank.Infrastructure;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Console;

public static class DependencyInjection
{
	
	public static IServiceCollection AddWorldRank(this IServiceCollection services, bool useDatabase = false)
	{
		
		services.AddLogging(builder =>
		{
			builder.ClearProviders();
			builder.SetMinimumLevel(LogLevel.Trace); 
			builder.AddNLog();
		});

		services.AddApplication();
		services.AddInfrastructure(useDatabase);
        services.AddDbContext<WorldRankDbContext>(options => { options.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true"); });

		return services;
	}
}
