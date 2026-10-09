using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Application.DTOs;

public class VehicleTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateVehicleTypeDto
{
    [Required(ErrorMessage = "Vehicle type name is required")]
    [MaxLength(100, ErrorMessage = "Vehicle type name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
