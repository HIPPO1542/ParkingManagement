using System.ComponentModel.DataAnnotations;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.DTOs;

public class TariffDto
{
    public Guid Id { get; set; }
    public Guid ParkingLotId { get; set; }
    public string ParkingLotName { get; set; } = string.Empty;
    public Guid VehicleTypeId { get; set; }
    public string VehicleTypeName { get; set; } = string.Empty;
    public MoneyDto Price { get; set; } = null!;
    public PricingType PricingType { get; set; } = PricingType.PerTurn;
    public DateTime EffectiveDate { get; set; }
}

public class CreateTariffDto
{
    [Required(ErrorMessage = "Parking lot ID is required")]
    public Guid ParkingLotId { get; set; }

    [Required(ErrorMessage = "Vehicle type ID is required")]
    public Guid VehicleTypeId { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public MoneyDto Price { get; set; } = null!;

    public PricingType PricingType { get; set; } = PricingType.PerTurn;

    public DateTime? EffectiveDate { get; set; }
}

public class UpdateTariffDto : CreateTariffDto { }
