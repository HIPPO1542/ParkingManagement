using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Api.Controllers;

[ApiController]
[Route("api/parking-lots")]
public class ParkingLotsController : ControllerBase
{
    private readonly IParkingLotService _parkingLotService;

    public ParkingLotsController(IParkingLotService parkingLotService)
    {
        _parkingLotService = parkingLotService;
    }

    /// <summary>
    /// Get paged list of parking lots with filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ParkingLotDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? operatorId,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var targetOperatorId = operatorId;

        // Role-based filtering: Business operators can only view their own parking lots
        var operatorClaim = User.FindFirst("BusinessOperatorId")?.Value ?? User.FindFirst("OperatorId")?.Value;
        if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
        {
            targetOperatorId = userOperatorId;
        }

        var result = await _parkingLotService.GetPagedAsync(targetOperatorId, isActive, page, pageSize);
        return Ok(ApiResponse<PagedResult<ParkingLotDto>>.SuccessResult(result, "Parking lots retrieved successfully."));
    }

    /// <summary>
    /// Get parking lot details by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ParkingLotDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _parkingLotService.GetByIdAsync(id);

        var operatorClaim = User.FindFirst("BusinessOperatorId")?.Value ?? User.FindFirst("OperatorId")?.Value;
        if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
        {
            if (result.BusinessOperatorId != userOperatorId)
            {
                return Forbid();
            }
        }

        return Ok(ApiResponse<ParkingLotDto>.SuccessResult(result, "Parking lot details retrieved successfully."));
    }

    /// <summary>
    /// Create a new parking lot (Admin only)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ParkingLotDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateParkingLotDto dto)
    {
        if (User.IsInRole("BusinessOperator"))
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Fail("Business operators are not allowed to create parking lots."));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid data provided.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var result = await _parkingLotService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ParkingLotDto>.SuccessResult(result, "Parking lot created successfully."));
    }

    /// <summary>
    /// Update parking lot information
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ParkingLotDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateParkingLotDto dto)
    {
        var operatorClaim = User.FindFirst("BusinessOperatorId")?.Value ?? User.FindFirst("OperatorId")?.Value;
        if (!string.IsNullOrEmpty(operatorClaim) && Guid.TryParse(operatorClaim, out var userOperatorId))
        {
            var currentLot = await _parkingLotService.GetByIdAsync(id);
            if (currentLot.BusinessOperatorId != userOperatorId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Fail("Business operators can only update their own parking lots."));
            }
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid data provided.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var result = await _parkingLotService.UpdateAsync(id, dto);
        return Ok(ApiResponse<ParkingLotDto>.SuccessResult(result, "Parking lot updated successfully."));
    }

    /// <summary>
    /// Delete parking lot (Admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (User.IsInRole("BusinessOperator"))
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Fail("Business operators are not allowed to delete parking lots."));
        }

        await _parkingLotService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessMessage("Parking lot deleted successfully."));
    }
}
