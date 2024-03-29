﻿using System.Diagnostics.CodeAnalysis;
using Business.Services;
using Lib.Repository;
using Lib.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        });
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBattleOfMonstersRepository, BattleOfMonstersRepository>();
        services.AddScoped<IBattleService, BattleService>();
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("cnSqlite");
        services.AddDbContext<BattleOfMonstersContext>(options =>
            options.UseSqlite(connectionString, b => b.MigrationsAssembly("API")));
    }

}
