using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.Auth.Commands;
using ParkingManagement.Application.Features.Auth.Queries;

namespace ParkingManagement.Api.Endpoints;

public class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Authentication");

        group.MapPost("login", async (LoginRequestDto dto, ISender sender) =>
        {
            var result = await sender.Send(new LoginCommand(dto));
            return Results.Ok(result);
        })
        .WithName("Login")
        .WithSummary("User login for Managers and Business Operators")
        .Produces<LoginResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("me", [Authorize] async (ClaimsPrincipal user, ISender sender) =>
        {
            var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = user.FindFirstValue(ClaimTypes.Role) ?? "";

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Results.Unauthorized();
            }

            var profile = await sender.Send(new GetCurrentUserProfileQuery(userId, role));
            return Results.Ok(profile);
        })
        .WithName("GetCurrentUser")
        .WithSummary("Get currently authenticated user profile")
        .Produces<UserProfileDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
