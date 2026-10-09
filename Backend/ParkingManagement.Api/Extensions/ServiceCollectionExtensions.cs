using Microsoft.Extensions.DependencyInjection;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Api.Extensions;

/// <summary>
/// Registers application-layer services at the API layer (Composition Root).
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IParkingLotService, ParkingLotService>();
        services.AddScoped<IBusinessOperatorService, BusinessOperatorService>();
        services.AddScoped<IParkingSessionService, ParkingSessionService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IVehicleTypeService, VehicleTypeService>();
        services.AddScoped<ITariffService, TariffService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
