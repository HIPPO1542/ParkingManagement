using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.Reports.Queries;

namespace ParkingManagement.Api.Endpoints;

public class ReportEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/reports")
            .WithTags("Reports")
            .RequireAuthorization();

        // GET /api/reports/activity
        group.MapGet("activity", async (
            [FromQuery] Guid? parkingLotId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            ISender sender) =>
        {
            if (!parkingLotId.HasValue || parkingLotId.Value == Guid.Empty)
            {
                return Results.BadRequest(ApiResponse<object>.Fail("Parking lot ID is required."));
            }

            var startDate = fromDate ?? DateTime.UtcNow.Date;
            var endDate = toDate ?? DateTime.UtcNow;

            var result = await sender.Send(new GetActivityReportQuery(parkingLotId.Value, startDate, endDate));

            if (result == null)
            {
                return Results.Ok(ApiResponse<ParkingActivityReportDto?>.SuccessResult(null, "No data available for the given period."));
            }

            return Results.Ok(ApiResponse<ParkingActivityReportDto>.SuccessResult(result, "Activity report retrieved successfully."));
        })
        .WithName("GetActivityReport")
        .Produces<ApiResponse<ParkingActivityReportDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);
    }
}
