using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Api.Controllers;

[ApiController]
[Route("api/business-operators")]
public class BusinessOperatorsController : ControllerBase
{
    private readonly IBusinessOperatorService _operatorService;

    public BusinessOperatorsController(IBusinessOperatorService operatorService)
    {
        _operatorService = operatorService;
    }

    /// <summary>
    /// Get all business operators including their parking lots
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BusinessOperatorDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _operatorService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<BusinessOperatorDto>>.SuccessResult(result, "Business operators retrieved successfully."));
    }

    /// <summary>
    /// Get business operator details by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BusinessOperatorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _operatorService.GetByIdAsync(id);
        return Ok(ApiResponse<BusinessOperatorDto>.SuccessResult(result, "Business operator details retrieved successfully."));
    }

    /// <summary>
    /// Create a new business operator (Admin only)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BusinessOperatorDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateBusinessOperatorDto dto)
    {
        if (User.IsInRole("BusinessOperator"))
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Fail("Business operators are not allowed to manage business operators."));
        }

        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (dto.SystemManagerId == Guid.Empty && !string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userId))
        {
            dto.SystemManagerId = userId;
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid data provided.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var result = await _operatorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<BusinessOperatorDto>.SuccessResult(result, "Business operator created successfully."));
    }

    /// <summary>
    /// Update business operator information
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BusinessOperatorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBusinessOperatorDto dto)
    {
        if (User.IsInRole("BusinessOperator"))
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdStr, out var operatorId) || operatorId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Fail("Business operators can only update their own profile."));
            }
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid data provided.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }

        var result = await _operatorService.UpdateAsync(id, dto);
        return Ok(ApiResponse<BusinessOperatorDto>.SuccessResult(result, "Business operator updated successfully."));
    }

    /// <summary>
    /// Delete business operator (Admin only)
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
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Fail("Business operators are not allowed to manage business operators."));
        }

        await _operatorService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessMessage("Business operator deleted successfully."));
    }
}
