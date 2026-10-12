using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.VehicleTypes.Commands;
using ParkingManagement.Application.Features.VehicleTypes.Queries;

namespace ParkingManagement.Api.Endpoints;

public class VehicleTypeEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/vehicle-types")
            .WithTags("VehicleTypes");

        group.MapGet("", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllVehicleTypesQuery());
            return Results.Ok(ApiResponse<IEnumerable<VehicleTypeDto>>.SuccessResult(result, "Vehicle types retrieved successfully."));
        })
        .WithName("GetAllVehicleTypes")
        .Produces<ApiResponse<IEnumerable<VehicleTypeDto>>>(StatusCodes.Status200OK);

        group.MapPost("", async (CreateVehicleTypeDto dto, ISender sender) =>
        {
            var result = await sender.Send(new CreateVehicleTypeCommand(dto));
            return Results.Created($"/api/vehicle-types/{result.Id}", ApiResponse<VehicleTypeDto>.SuccessResult(result, "Vehicle type created successfully."));
        })
        .WithName("CreateVehicleType")
        .Produces<ApiResponse<VehicleTypeDto>>(StatusCodes.Status201Created);
    }
}
