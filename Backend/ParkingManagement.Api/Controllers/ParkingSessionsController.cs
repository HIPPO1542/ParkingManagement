using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Api.Controllers;

[ApiController]
[Route("api/parking-sessions")]
public class ParkingSessionsController : ControllerBase
{
    private readonly IParkingSessionService _sessionService;

    public ParkingSessionsController(IParkingSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    /// <summary>
    /// Check-in: Record a vehicle entering the parking lot, decreases available spots
    /// </summary>
    [HttpPost("check-in")]
    [ProducesResponseType(typeof(ApiResponse<ParkingSessionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid check-in data.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var result = await _sessionService.CheckInAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ParkingSessionDto>.SuccessResult(result, "Vehicle checked in successfully."));
    }

    /// <summary>
    /// Check-out: Record a vehicle leaving the parking lot by plate number, calculates fee and releases spot
    /// </summary>
    [HttpPost("check-out")]
    [ProducesResponseType(typeof(ApiResponse<ParkingSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid check-out data.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var result = await _sessionService.CheckOutAsync(dto);
        return Ok(ApiResponse<ParkingSessionDto>.SuccessResult(result, "Vehicle checked out successfully. Fee calculated."));
    }

    /// <summary>
    /// Check-out by parking session ID
    /// </summary>
    [HttpPost("{id:guid}/check-out")]
    [ProducesResponseType(typeof(ApiResponse<ParkingSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckOutById(Guid id)
    {
        var result = await _sessionService.CheckOutByIdAsync(id);
        return Ok(ApiResponse<ParkingSessionDto>.SuccessResult(result, "Parking session checked out successfully."));
    }

    /// <summary>
    /// Get parking session details by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ParkingSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sessionService.GetByIdAsync(id);
        return Ok(ApiResponse<ParkingSessionDto>.SuccessResult(result, "Parking session details retrieved successfully."));
    }

    /// <summary>
    /// Query parking sessions with pagination and filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ParkingSessionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? parkingLotId,
        [FromQuery] ParkingStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _sessionService.GetPagedAsync(parkingLotId, status, page, pageSize);
        return Ok(ApiResponse<PagedResult<ParkingSessionDto>>.SuccessResult(result, "Parking sessions retrieved successfully."));
    }
}
