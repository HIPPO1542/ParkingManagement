using System.Security.Claims;
using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.ParkingLots.Commands;
using ParkingManagement.Application.Features.ParkingLots.Queries;

namespace ParkingManagement.Api.Endpoints;

public class ParkingLotEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/parking-lots").WithTags("Parking Lots");

        group.MapGet("", async (
            Guid? operatorId,
            bool? isActive,
            int page = 1,
            int pageSize = 20,
            ClaimsPrincipal user = null!,
            ISender sender = null!) =>
        {
            var targetOperatorId = operatorId;
            var operatorClaim = user?.FindFirst("BusinessOperatorId")?.Value ?? user?.FindFirst("OperatorId")?.Value;
            if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
            {
                targetOperatorId = userOperatorId;
            }

            var query = new GetParkingLotsQuery(targetOperatorId, isActive, page, pageSize);
            var result = await sender.Send(query);
            return Results.Ok(ApiResponse<PagedResult<ParkingLotDto>>.SuccessResult(result, "Parking lots retrieved successfully."));
        })
        .WithName("GetParkingLots")
        .WithSummary("Get paged list of parking lots with filtering")
        .Produces<ApiResponse<PagedResult<ParkingLotDto>>>(StatusCodes.Status200OK);

        group.MapGet("{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            var result = await sender.Send(new GetParkingLotByIdQuery(id));

            var operatorClaim = user?.FindFirst("BusinessOperatorId")?.Value ?? user?.FindFirst("OperatorId")?.Value;
            if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
            {
                if (result.BusinessOperatorId != userOperatorId)
                {
                    return Results.Forbid();
                }
            }

            return Results.Ok(ApiResponse<ParkingLotDto>.SuccessResult(result, "Parking lot details retrieved successfully."));
        })
        .WithName("GetParkingLotById")
        .WithSummary("Get parking lot details by ID")
        .Produces<ApiResponse<ParkingLotDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", async (CreateParkingLotDto dto, ClaimsPrincipal user, ISender sender) =>
        {
            if (user.IsInRole("BusinessOperator"))
            {
                var operatorClaim = user.FindFirst("BusinessOperatorId")?.Value ?? user.FindFirst("OperatorId")?.Value;
                if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
                {
                    dto.BusinessOperatorId = userOperatorId;
                }
            }

            var result = await sender.Send(new CreateParkingLotCommand(dto));
            return Results.Created($"/api/parking-lots/{result.Id}", ApiResponse<ParkingLotDto>.SuccessResult(result, "Parking lot created successfully."));
        })
        .WithName("CreateParkingLot")
        .WithSummary("Create a new parking lot")
        .Produces<ApiResponse<ParkingLotDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("{id:guid}", async (Guid id, UpdateParkingLotDto dto, ClaimsPrincipal user, ISender sender) =>
        {
            var operatorClaim = user?.FindFirst("BusinessOperatorId")?.Value ?? user?.FindFirst("OperatorId")?.Value;
            if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
            {
                var currentLot = await sender.Send(new GetParkingLotByIdQuery(id));
                if (currentLot.BusinessOperatorId != userOperatorId)
                {
                    return Results.Json(ApiResponse<object>.Fail("Business operators can only update their own parking lots.", StatusCodes.Status403Forbidden), statusCode: StatusCodes.Status403Forbidden);
                }
            }

            var result = await sender.Send(new UpdateParkingLotCommand(id, dto));
            return Results.Ok(ApiResponse<ParkingLotDto>.SuccessResult(result, "Parking lot updated successfully."));
        })
        .WithName("UpdateParkingLot")
        .WithSummary("Update an existing parking lot")
        .Produces<ApiResponse<ParkingLotDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            if (user.IsInRole("BusinessOperator"))
            {
                return Results.Json(ApiResponse<object>.Fail("Business operators are not allowed to delete parking lots.", StatusCodes.Status403Forbidden), statusCode: StatusCodes.Status403Forbidden);
            }

            await sender.Send(new DeleteParkingLotCommand(id));
            return Results.Ok(ApiResponse<object>.SuccessMessage("Parking lot deleted successfully."));
        })
        .WithName("DeleteParkingLot")
        .WithSummary("Delete parking lot (Admin only)")
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
