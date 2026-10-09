using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Api.Controllers;

[ApiController]
[Route("api/vehicle-types")]
public class VehicleTypesController : ControllerBase
{
    private readonly IVehicleTypeService _vehicleTypeService;

    public VehicleTypesController(IVehicleTypeService vehicleTypeService)
    {
        _vehicleTypeService = vehicleTypeService;
    }

    /// <summary>
    /// Get all vehicle types
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VehicleTypeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _vehicleTypeService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<VehicleTypeDto>>.SuccessResult(result, "Vehicle types retrieved successfully."));
    }

    /// <summary>
    /// Create a new vehicle type
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VehicleTypeDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateVehicleTypeDto dto)
    {
        var result = await _vehicleTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, ApiResponse<VehicleTypeDto>.SuccessResult(result, "Vehicle type created successfully."));
    }
}
