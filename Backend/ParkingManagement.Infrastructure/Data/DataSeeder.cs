using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static readonly Guid AdminManagerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid OperatorId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid ParkingLot1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid ParkingLot2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid ParkingLotInactiveId = Guid.Parse("55555555-5555-5555-5555-555555555555");

    public static readonly Guid VehicleTypeMotorbikeId = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static readonly Guid VehicleTypeCarId = Guid.Parse("77777777-7777-7777-7777-777777777777");
    public static readonly Guid VehicleTypeTruckId = Guid.Parse("88888888-8888-8888-8888-888888888888");

    public static async Task SeedAsync(ParkingDbContext context)
    {
        if (await context.SystemManagers.AnyAsync())
        {
            return; // Data already exists
        }

        // 1. Seed System Manager
        var manager = new SystemManager(
            AdminManagerId,
            "Nguyen Van Quan Tri",
            "System Administrator",
            new ContactInfo("0901234567", "admin@parking.vn"),
            "admin",
            "admin@123",
            UserRole.Manager,
            true
        );
        await context.SystemManagers.AddAsync(manager);

        // 2. Seed Business Operator
        var op = new BusinessOperator(
            OperatorId,
            AdminManagerId,
            "Sai Gon Parking Urban Corp",
            "Tran Thi Director",
            new Address("123 Le Loi Street", "Ben Nghe Ward", "District 1", "Ho Chi Minh City"),
            new ContactInfo("02838222333", "contact@sgparking.vn"),
            true,
            "sgparking",
            "123456"
        );
        await context.BusinessOperators.AddAsync(op);

        // 3. Seed Parking Lots
        var lot1 = new ParkingLot(
            ParkingLot1Id,
            OperatorId,
            "Ben Thanh Central Parking Lot",
            new Address("01 Lam Son Square", "Ben Nghe Ward", "District 1", "Ho Chi Minh City"),
            totalCapacity: 100,
            openTime: "06:00",
            closeTime: "23:00",
            isActive: true
        );

        var lot2 = new ParkingLot(
            ParkingLot2Id,
            OperatorId,
            "Landmark Tower Parking Lot",
            new Address("208 Nguyen Huu Canh", "Ward 22", "Binh Thanh District", "Ho Chi Minh City"),
            totalCapacity: 50,
            openTime: "00:00",
            closeTime: "23:59",
            isActive: true
        );

        var lot3 = new ParkingLot(
            ParkingLotInactiveId,
            OperatorId,
            "Binh Trieu Station Parking Lot (Maintenance)",
            new Address("05 Kha Van Can", "Hiep Binh Chanh Ward", "Thu Duc City", "Ho Chi Minh City"),
            totalCapacity: 80,
            openTime: "07:00",
            closeTime: "21:00",
            isActive: false
        );

        // 4. Seed Vehicle Types
        var motorbike = new VehicleType(VehicleTypeMotorbikeId, "Motorbike", "Two-wheeled motor vehicle");
        var car = new VehicleType(VehicleTypeCarId, "Car (4-7 seats)", "Family passenger car under 9 seats");
        var truck = new VehicleType(VehicleTypeTruckId, "Light Truck", "Freight truck under 2.5 tons");

        await context.VehicleTypes.AddRangeAsync(motorbike, car, truck);

        // 5. Seed Tariffs
        var tariffMotorbikeLot1 = new Tariff(
            Guid.NewGuid(),
            ParkingLot1Id,
            VehicleTypeMotorbikeId,
            new Money(5000, "VND"),
            PricingType.PerTurn,
            DateTime.UtcNow.AddMonths(-1)
        );

        var tariffCarLot1 = new Tariff(
            Guid.NewGuid(),
            ParkingLot1Id,
            VehicleTypeCarId,
            new Money(20000, "VND"),
            PricingType.PerHour,
            DateTime.UtcNow.AddMonths(-1)
        );

        var tariffMotorbikeLot2 = new Tariff(
            Guid.NewGuid(),
            ParkingLot2Id,
            VehicleTypeMotorbikeId,
            new Money(6000, "VND"),
            PricingType.PerTurn,
            DateTime.UtcNow.AddMonths(-1)
        );

        var tariffCarLot2 = new Tariff(
            Guid.NewGuid(),
            ParkingLot2Id,
            VehicleTypeCarId,
            new Money(25000, "VND"),
            PricingType.PerHour,
            DateTime.UtcNow.AddMonths(-1)
        );

        await context.Tariffs.AddRangeAsync(tariffMotorbikeLot1, tariffCarLot1, tariffMotorbikeLot2, tariffCarLot2);
        await context.ParkingLots.AddRangeAsync(lot1, lot2, lot3);

        await context.SaveChangesAsync();
    }
}
