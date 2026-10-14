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
    public void Money_Zero_HasZeroAmount()
    {
        var zero = Money.Zero();

        Assert.Equal(0, zero.Amount);
        Assert.Equal("VND", zero.Currency);
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

    [Fact]
    public void Tariff_CalculateFee_PerDay_CalculatesCorrectly()
    {
        // Kiểm thử tính phí theo ngày: 1.5 ngày làm tròn thành 2 ngày
        var tariff = new Tariff(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(100000, "VND"),
            pricingType: PricingType.PerDay
        );

        var fee = tariff.CalculateFee(TimeSpan.FromHours(36));

        Assert.Equal(200000, fee.Amount);
        Assert.Equal("VND", fee.Currency);
    }

    [Fact]
    public void Tariff_CalculateFee_PerHour_ShortDuration_MinimumOneHour()
    {
        // Kiểm thử đỗ thời gian cực ngắn (12 phút) -> Không ân hạn, tính đủ 1 block giờ
        var tariff = new Tariff(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(20000, "VND"),
            pricingType: PricingType.PerHour
        );

        var fee = tariff.CalculateFee(TimeSpan.FromMinutes(12));

        Assert.Equal(20000, fee.Amount);
        Assert.Equal("VND", fee.Currency);
    }
}
