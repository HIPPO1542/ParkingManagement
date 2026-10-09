using System.ComponentModel.DataAnnotations;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.DTOs;

public class CheckInDto
{
    [Required(ErrorMessage = "Parking lot ID is required")]
    public Guid ParkingLotId { get; set; }

    [Required(ErrorMessage = "Vehicle type ID is required")]
    public Guid VehicleTypeId { get; set; }

    [Required(ErrorMessage = "Province code is required")]
    [MaxLength(20, ErrorMessage = "Province code cannot exceed 20 characters")]
    public string ProvinceCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plate number is required")]
    [MaxLength(50, ErrorMessage = "Plate number cannot exceed 50 characters")]
    public string PlateNumber { get; set; } = string.Empty;
}

public class CheckOutDto
{
    [Required(ErrorMessage = "Parking lot ID is required")]
    public Guid ParkingLotId { get; set; }

    [Required(ErrorMessage = "Province code is required")]
    public string ProvinceCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plate number is required")]
    public string PlateNumber { get; set; } = string.Empty;
}

public class ParkingSessionDto
{
    public Guid Id { get; set; }
    public Guid ParkingLotId { get; set; }
    public string ParkingLotName { get; set; } = string.Empty;
    public Guid VehicleTypeId { get; set; }
    public string VehicleTypeName { get; set; } = string.Empty;
    public LicensePlateDto LicensePlate { get; set; } = null!;
    public string FullPlateNumber { get; set; } = string.Empty;
    public string FullLicensePlate => FullPlateNumber;
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public MoneyDto Fee { get; set; } = null!;
    public ParkingStatus Status { get; set; } = ParkingStatus.Active;
    public double? DurationHours { get; set; }
}
