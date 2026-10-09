using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;

namespace ParkingManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var rawConn = configuration.GetConnectionString("DefaultConnection");
        
        string connectionString;
        if (!string.IsNullOrWhiteSpace(rawConn) && !rawConn.Contains("parking.db"))
        {
            connectionString = rawConn;
        }
        else
        {
            // Automatically locate the Database/parking.db folder regardless of runtime context (CLI, VS, or VS Code)
            var currentDir = Directory.GetCurrentDirectory();
            var candidatePaths = new[]
            {
                Path.Combine(currentDir, "Database", "parking.db"),
                Path.Combine(currentDir, "..", "..", "Database", "parking.db"),
                Path.Combine(currentDir, "..", "Database", "parking.db"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Database", "parking.db")
            };

            var dbFile = candidatePaths.FirstOrDefault(p =>
            {
                var dir = Path.GetDirectoryName(p);
                return !string.IsNullOrEmpty(dir) && Directory.Exists(dir);
            });

            if (dbFile == null)
            {
                dbFile = Path.Combine(currentDir, "parking.db");
            }

            var parentDir = Path.GetDirectoryName(dbFile);
            if (!string.IsNullOrEmpty(parentDir) && !Directory.Exists(parentDir))
            {
                Directory.CreateDirectory(parentDir);
            }

            connectionString = $"Data Source={Path.GetFullPath(dbFile)};Default Timeout=30;";
        }

        services.AddDbContext<ParkingDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
        services.AddScoped<IBusinessOperatorRepository, BusinessOperatorRepository>();
        services.AddScoped<IParkingSessionRepository, ParkingSessionRepository>();
        services.AddScoped<ITariffRepository, TariffRepository>();
        services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
        services.AddScoped<ISystemManagerRepository, SystemManagerRepository>();
        services.AddSingleton<ParkingManagement.Application.Services.IJwtTokenGenerator, ParkingManagement.Infrastructure.Services.JwtTokenGenerator>();

        return services;
    }
}
