using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Business activity report for a parking lot within a date range
    /// </summary>
    [HttpGet("activity")]
    [ProducesResponseType(typeof(ApiResponse<ParkingActivityReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActivityReport(
        [FromQuery] Guid? parkingLotId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        if (!parkingLotId.HasValue || parkingLotId.Value == Guid.Empty)
        {
            return BadRequest(ApiResponse<object>.Fail("Parking lot ID is required."));
        }

        var startDate = fromDate ?? DateTime.UtcNow.Date;
        var endDate = toDate ?? DateTime.UtcNow;

        var result = await _reportService.GetActivityReportAsync(parkingLotId.Value, startDate, endDate);

        if (result == null)
        {
            return Ok(ApiResponse<ParkingActivityReportDto?>.SuccessResult(null, "No data available for the given period."));
        }

        return Ok(ApiResponse<ParkingActivityReportDto>.SuccessResult(result, "Activity report retrieved successfully."));
    }
}
