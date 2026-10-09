using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Api.Controllers;

[ApiController]
[Route("api/tariffs")]
public class TariffsController : ControllerBase
{
    private readonly ITariffService _tariffService;

    public TariffsController(ITariffService tariffService)
    {
        _tariffService = tariffService;
    }

    /// <summary>
    /// Get all service tariffs
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TariffDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _tariffService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<TariffDto>>.SuccessResult(result, "Tariffs retrieved successfully."));
    }

    /// <summary>
    /// Get tariffs for a specific parking lot
    /// </summary>
    [HttpGet("parking-lot/{parkingLotId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TariffDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByParkingLot(Guid parkingLotId)
    {
        var result = await _tariffService.GetByParkingLotIdAsync(parkingLotId);
        return Ok(ApiResponse<IEnumerable<TariffDto>>.SuccessResult(result, "Tariffs for parking lot retrieved successfully."));
    }

    /// <summary>
    /// Create a new tariff for a vehicle type at a parking lot
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TariffDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTariffDto dto)
    {
        var result = await _tariffService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, ApiResponse<TariffDto>.SuccessResult(result, "Tariff created successfully."));
    }

    /// <summary>
    /// Update an existing tariff
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TariffDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTariffDto dto)
    {
        var result = await _tariffService.UpdateAsync(id, dto);
        return Ok(ApiResponse<TariffDto>.SuccessResult(result, "Tariff updated successfully."));
    }

    /// <summary>
    /// Delete a tariff
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _tariffService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessMessage("Tariff deleted successfully."));
    }
}
