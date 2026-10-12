using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.ParkingSessions.Commands;
using ParkingManagement.Application.Features.ParkingSessions.Queries;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Api.Endpoints;

public class ParkingSessionEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/parking-sessions").WithTags("Parking Sessions");

        group.MapPost("check-in", async (CheckInDto dto, ISender sender) =>
        {
            var result = await sender.Send(new CheckInCommand(dto));
            return Results.Created($"/api/parking-sessions/{result.Id}", ApiResponse<ParkingSessionDto>.SuccessResult(result, "Vehicle checked in successfully."));
        })
        .WithName("CheckIn")
        .WithSummary("Check-in: Record a vehicle entering the parking lot")
        .Produces<ApiResponse<ParkingSessionDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("check-out", async (CheckOutDto dto, ISender sender) =>
        {
            var result = await sender.Send(new CheckOutByPlateCommand(dto));
            return Results.Ok(ApiResponse<ParkingSessionDto>.SuccessResult(result, "Vehicle checked out successfully. Fee calculated."));
        })
        .WithName("CheckOutByPlate")
        .WithSummary("Check-out: Record a vehicle leaving the parking lot by plate number")
        .Produces<ApiResponse<ParkingSessionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("{id:guid}/check-out", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new CheckOutByIdCommand(id));
            return Results.Ok(ApiResponse<ParkingSessionDto>.SuccessResult(result, "Parking session checked out successfully."));
        })
        .WithName("CheckOutById")
        .WithSummary("Check-out by parking session ID")
        .Produces<ApiResponse<ParkingSessionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetParkingSessionByIdQuery(id));
            return Results.Ok(ApiResponse<ParkingSessionDto>.SuccessResult(result, "Parking session details retrieved successfully."));
        })
        .WithName("GetParkingSessionById")
        .WithSummary("Get parking session details by ID")
        .Produces<ApiResponse<ParkingSessionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("", async (Guid? parkingLotId, ParkingStatus? status, int page = 1, int pageSize = 20, ISender sender = null!) =>
        {
            var result = await sender.Send(new GetParkingSessionsQuery(parkingLotId, status, page, pageSize));
            return Results.Ok(ApiResponse<PagedResult<ParkingSessionDto>>.SuccessResult(result, "Parking sessions retrieved successfully."));
        })
        .WithName("GetParkingSessions")
        .WithSummary("Query parking sessions with pagination and filtering")
        .Produces<ApiResponse<PagedResult<ParkingSessionDto>>>(StatusCodes.Status200OK);
    }
}
