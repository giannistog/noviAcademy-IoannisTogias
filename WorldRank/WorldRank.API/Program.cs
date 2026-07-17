using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System.Data.Common;
using System.Text.Json.Serialization;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank.Application.Strategies;
using WorldRank.Infrastructure.Caching;
using WorldRank.Infrastructure.Persistence.Context;
using WorldRank.Infrastructure.Repositories;
using WorldRank.Application;    
using WorldRank.Infrastructure;
//using WorldRank.API;


var builder = WebApplication.CreateBuilder(args);

// Logging via NLog (same nlog.config layout as the Console app).
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

builder.Services.AddDbContext<WorldRankDbContext>(options =>
{
    options.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true");
});

// Repositories
builder.Services.AddScoped<IPlayerRepository, DBPlayerRepository>();
builder.Services.AddScoped<IWalletRepository, DBWalletRepository>();

// Application services
builder.Services.AddScoped<WorldRank.Application.Services.PlayerService>();
builder.Services.AddScoped<WorldRank.Application.Services.WalletService>();

// Funds strategies
builder.Services.AddSingleton<IFundsStrategy, AddFundsStrategy>();
builder.Services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
builder.Services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

// Single-instance in-memory cache (Day 6). Redis would replace this behind a load balancer.
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICache, MemoryCacheStore>();

// CQRS + Decorator wiring (Day 7 exercise)
builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule();
// Accept/emit enums (e.g. Currency) as their string names, not numbers.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Swagger / OpenAPI — interactive API docs at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")); // root → Swagger UI
}

app.MapControllers();

app.Run();