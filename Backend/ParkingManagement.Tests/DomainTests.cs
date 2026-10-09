using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.ValueObjects;
using Xunit;

namespace ParkingManagement.Tests;

public class DomainTests
{
    [Fact]
    public void Money_Add_SameCurrency_ReturnsCorrectSum()
    {
        var money1 = new Money(20000, "VND");
        var money2 = new Money(30000, "VND");

        var sum = money1.Add(money2);

        Assert.Equal(50000, sum.Amount);
        Assert.Equal("VND", sum.Currency);
    }

    [Fact]
    public void Money_Add_DifferentCurrency_ThrowsInvalidOperationException()
    {
        var vnd = new Money(20000, "VND");
        var usd = new Money(5, "USD");

        Assert.Throws<InvalidOperationException>(() => vnd.Add(usd));
    }

    [Fact]
    public void Money_Zero_HasZeroAmount()
    {
        var zero = Money.Zero();

        Assert.Equal(0, zero.Amount);
        Assert.Equal("VND", zero.Currency);
    }

    [Fact]
    public void ParkingLot_RecordVehicleEntry_DecreasesAvailableSpots()
    {
        var parkingLot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test Lot",
            new Address("123 Do Xuan Hop", "Phuoc Long B", "Thu Duc City", "Ho Chi Minh City"),
            totalCapacity: 10,
            isActive: true
        );

        Assert.Equal(10, parkingLot.AvailableSpots);

        parkingLot.RecordVehicleEntry();

        Assert.Equal(9, parkingLot.AvailableSpots);
    }

    [Fact]
    public void ParkingLot_RecordVehicleEntry_WhenFull_ThrowsBusinessRuleViolationException()
    {
        var parkingLot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Full Lot",
            new Address("123 Do Xuan Hop", "Phuoc Long B", "Thu Duc City", "Ho Chi Minh City"),
            totalCapacity: 1,
            isActive: true
        );

        parkingLot.RecordVehicleEntry();
        Assert.Equal(0, parkingLot.AvailableSpots);

        var ex = Assert.Throws<BusinessRuleViolationException>(() => parkingLot.RecordVehicleEntry());
        Assert.Equal("Parking lot is full!", ex.Message);
    }

    [Fact]
    public void ParkingLot_RecordVehicleEntry_WhenInactive_ThrowsBusinessRuleViolationException()
    {
        var parkingLot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Inactive Lot",
            new Address("123 Do Xuan Hop", "Phuoc Long B", "Thu Duc City", "Ho Chi Minh City"),
            totalCapacity: 10,
            isActive: false
        );

        var ex = Assert.Throws<BusinessRuleViolationException>(() => parkingLot.RecordVehicleEntry());
        Assert.Equal("Parking lot is currently inactive!", ex.Message);
    }

    [Fact]
    public void Tariff_CalculateFee_PerHour_CalculatesCorrectly()
    {
        var tariff = new Tariff(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(15000, "VND"),
            pricingType: PricingType.PerHour
        );

        // 2.5 hours rounded up to 3 hours -> 45,000 VND
        var fee = tariff.CalculateFee(TimeSpan.FromHours(2.5));

        Assert.Equal(45000, fee.Amount);
        Assert.Equal("VND", fee.Currency);
    }

    [Fact]
    public void Tariff_CalculateFee_PerTurn_ReturnsFixedRate()
    {
        var tariff = new Tariff(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(5000, "VND"),
            pricingType: PricingType.PerTurn
        );

        var fee = tariff.CalculateFee(TimeSpan.FromHours(8));

        Assert.Equal(5000, fee.Amount);
    }
}
