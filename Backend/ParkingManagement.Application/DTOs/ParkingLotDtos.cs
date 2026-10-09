using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Application.DTOs;

public class ParkingLotDto
{
    public Guid Id { get; set; }
    public Guid BusinessOperatorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = null!;
    public string FullAddress { get; set; } = string.Empty;
    public int TotalCapacity { get; set; }
    public int AvailableSpots { get; set; }
    public string OpenTime { get; set; } = string.Empty;
    public string CloseTime { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateParkingLotDto
{
    [Required(ErrorMessage = "Business operator ID is required")]
    public Guid BusinessOperatorId { get; set; }

    [Required(ErrorMessage = "Parking lot name is required")]
    [MaxLength(200, ErrorMessage = "Parking lot name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parking lot address is required")]
    public AddressDto Address { get; set; } = null!;

    [Range(1, 100000, ErrorMessage = "Total capacity must be greater than 0")]
    public int TotalCapacity { get; set; }

    public string OpenTime { get; set; } = "06:00";
    public string CloseTime { get; set; } = "22:00";
    public bool IsActive { get; set; } = true;
}

public class UpdateParkingLotDto
{
    [Required(ErrorMessage = "Parking lot name is required")]
    [MaxLength(200, ErrorMessage = "Parking lot name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parking lot address is required")]
    public AddressDto Address { get; set; } = null!;

    [Range(1, 100000, ErrorMessage = "Total capacity must be greater than 0")]
    public int TotalCapacity { get; set; }

    public string OpenTime { get; set; } = "06:00";
    public string CloseTime { get; set; } = "22:00";
    public bool IsActive { get; set; } = true;
}
