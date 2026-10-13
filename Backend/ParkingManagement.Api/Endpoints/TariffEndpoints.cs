using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.Tariffs.Commands;
using ParkingManagement.Application.Features.Tariffs.Queries;

namespace ParkingManagement.Api.Endpoints;

public class TariffEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tariffs").WithTags("Tariffs");

        group.MapGet("", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTariffsQuery());
            return Results.Ok(ApiResponse<IEnumerable<TariffDto>>.SuccessResult(result, "Tariffs retrieved successfully."));
        })
        .WithName("GetTariffs")
        .WithSummary("Get all service tariffs")
        .Produces<ApiResponse<IEnumerable<TariffDto>>>(StatusCodes.Status200OK);

        group.MapGet("parking-lot/{parkingLotId:guid}", async (Guid parkingLotId, ISender sender) =>
        {
            var result = await sender.Send(new GetTariffsByParkingLotQuery(parkingLotId));
            return Results.Ok(ApiResponse<IEnumerable<TariffDto>>.SuccessResult(result, "Tariffs for parking lot retrieved successfully."));
        })
        .WithName("GetTariffsByParkingLot")
        .WithSummary("Get tariffs for a specific parking lot")
        .Produces<ApiResponse<IEnumerable<TariffDto>>>(StatusCodes.Status200OK);

        group.MapPost("", async (CreateTariffDto dto, ISender sender) =>
        {
            var result = await sender.Send(new CreateTariffCommand(dto));
            return Results.Created($"/api/tariffs/{result.Id}", ApiResponse<TariffDto>.SuccessResult(result, "Tariff created successfully."));
        })
        .WithName("CreateTariff")
        .WithSummary("Create a new tariff")
        .Produces<ApiResponse<TariffDto>>(StatusCodes.Status201Created);

        group.MapPut("{id:guid}", async (Guid id, UpdateTariffDto dto, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTariffCommand(id, dto));
            return Results.Ok(ApiResponse<TariffDto>.SuccessResult(result, "Tariff updated successfully."));
        })
        .WithName("UpdateTariff")
        .WithSummary("Update an existing tariff")
        .Produces<ApiResponse<TariffDto>>(StatusCodes.Status200OK);

        group.MapDelete("{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteTariffCommand(id));
            return Results.Ok(ApiResponse<object>.SuccessMessage("Tariff deleted successfully."));
        })
        .WithName("DeleteTariff")
        .WithSummary("Delete tariff")
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK);
    }
}
