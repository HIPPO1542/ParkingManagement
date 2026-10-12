using System.Security.Claims;
using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.BusinessOperators.Commands;
using ParkingManagement.Application.Features.BusinessOperators.Queries;

namespace ParkingManagement.Api.Endpoints;

public class BusinessOperatorEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/business-operators")
            .WithTags("BusinessOperators");

        // GET /api/business-operators
        group.MapGet("", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllBusinessOperatorsQuery());
            return Results.Ok(ApiResponse<IEnumerable<BusinessOperatorDto>>.SuccessResult(result, "Business operators retrieved successfully."));
        })
        .WithName("GetAllBusinessOperators")
        .Produces<ApiResponse<IEnumerable<BusinessOperatorDto>>>(StatusCodes.Status200OK);

        // GET /api/business-operators/{id}
        group.MapGet("{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetBusinessOperatorByIdQuery(id));
            return Results.Ok(ApiResponse<BusinessOperatorDto>.SuccessResult(result, "Business operator details retrieved successfully."));
        })
        .WithName("GetBusinessOperatorById")
        .Produces<ApiResponse<BusinessOperatorDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);

        // POST /api/business-operators
        group.MapPost("", async (CreateBusinessOperatorDto dto, ClaimsPrincipal user, ISender sender) =>
        {
            if (user.IsInRole("BusinessOperator"))
            {
                return Results.Json(
                    ApiResponse<object>.Fail("Business operators are not allowed to manage business operators."),
                    statusCode: StatusCodes.Status403Forbidden);
            }

            var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (dto.SystemManagerId == Guid.Empty && !string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userId))
            {
                dto.SystemManagerId = userId;
            }

            var result = await sender.Send(new CreateBusinessOperatorCommand(dto));
            return Results.Created($"/api/business-operators/{result.Id}", ApiResponse<BusinessOperatorDto>.SuccessResult(result, "Business operator created successfully."));
        })
        .WithName("CreateBusinessOperator")
        .Produces<ApiResponse<BusinessOperatorDto>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status403Forbidden);

        // PUT /api/business-operators/{id}
        group.MapPut("{id:guid}", async (Guid id, UpdateBusinessOperatorDto dto, ClaimsPrincipal user, ISender sender) =>
        {
            if (user.IsInRole("BusinessOperator"))
            {
                var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdStr, out var operatorId) || operatorId != id)
                {
                    return Results.Json(
                        ApiResponse<object>.Fail("Business operators can only update their own profile."),
                        statusCode: StatusCodes.Status403Forbidden);
                }
            }

            var result = await sender.Send(new UpdateBusinessOperatorCommand(id, dto));
            return Results.Ok(ApiResponse<BusinessOperatorDto>.SuccessResult(result, "Business operator updated successfully."));
        })
        .WithName("UpdateBusinessOperator")
        .Produces<ApiResponse<BusinessOperatorDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status403Forbidden)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);

        // DELETE /api/business-operators/{id}
        group.MapDelete("{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            if (user.IsInRole("BusinessOperator"))
            {
                return Results.Json(
                    ApiResponse<object>.Fail("Business operators are not allowed to manage business operators."),
                    statusCode: StatusCodes.Status403Forbidden);
            }

            await sender.Send(new DeleteBusinessOperatorCommand(id));
            return Results.Ok(ApiResponse<object>.SuccessMessage("Business operator deleted successfully."));
        })
        .WithName("DeleteBusinessOperator")
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status403Forbidden)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);
    }
}
