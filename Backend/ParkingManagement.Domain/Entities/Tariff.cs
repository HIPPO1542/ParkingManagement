using ParkingManagement.Domain.Common;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Domain.Entities;

public class Tariff : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid ParkingLotId { get; private set; }
    public Guid VehicleTypeId { get; private set; }
    public Money Price { get; private set; } = null!;
    public PricingType PricingType { get; private set; } = PricingType.PerTurn;
    public DateTime EffectiveDate { get; private set; }

    // Navigation properties
    public ParkingLot ParkingLot { get; private set; } = null!;
    public VehicleType VehicleType { get; private set; } = null!;

    // EF Core constructor
    protected Tariff() { }

    public Tariff(
        Guid id,
        Guid parkingLotId,
        Guid vehicleTypeId,
        Money price,
        PricingType pricingType = PricingType.PerTurn,
        DateTime? effectiveDate = null)
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        if (parkingLotId == Guid.Empty)
            throw new ArgumentException("Parking lot ID cannot be empty.", nameof(parkingLotId));
        if (vehicleTypeId == Guid.Empty)
            throw new ArgumentException("Vehicle type ID cannot be empty.", nameof(vehicleTypeId));

        Id = id;
        ParkingLotId = parkingLotId;
        VehicleTypeId = vehicleTypeId;
        Price = price ?? throw new ArgumentNullException(nameof(price));
        PricingType = pricingType;
        EffectiveDate = effectiveDate ?? DateTime.UtcNow;
    }

    public Money CalculateFee(TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
            duration = TimeSpan.Zero;

        switch (PricingType)
        {
            case PricingType.PerHour:
                int hours = (int)Math.Ceiling(duration.TotalHours);
                if (hours <= 0) hours = 1;
                return new Money(Price.Amount * hours, Price.Currency);

            case PricingType.PerDay:
                int days = (int)Math.Ceiling(duration.TotalDays);
                if (days <= 0) days = 1;
                return new Money(Price.Amount * days, Price.Currency);

            case PricingType.PerTurn:
            default:
                return new Money(Price.Amount, Price.Currency);
        }
    }

    public void Update(Money newPrice, PricingType newPricingType, DateTime? newEffectiveDate = null)
    {
        Price = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
        PricingType = newPricingType;
        EffectiveDate = newEffectiveDate ?? DateTime.UtcNow;
    }

    public void UpdatePricing(Money newPrice, PricingType newPricingType, DateTime? newEffectiveDate = null)
    {
        Update(newPrice, newPricingType, newEffectiveDate);
    }

    public void UpdateAll(Guid parkingLotId, Guid vehicleTypeId, Money newPrice, PricingType newPricingType, DateTime? newEffectiveDate = null)
    {
        if (parkingLotId != Guid.Empty) ParkingLotId = parkingLotId;
        if (vehicleTypeId != Guid.Empty) VehicleTypeId = vehicleTypeId;
        Update(newPrice, newPricingType, newEffectiveDate);
    }
}
