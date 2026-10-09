using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;
using Xunit;

namespace ParkingManagement.Tests;

public class BusinessUseCasesTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ParkingDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParkingLotRepository _parkingLotRepo;
    private readonly IBusinessOperatorRepository _operatorRepo;
    private readonly IParkingSessionRepository _sessionRepo;
    private readonly ITariffRepository _tariffRepo;
    private readonly IVehicleTypeRepository _vehicleTypeRepo;
    private readonly ISystemManagerRepository _managerRepo;

    private readonly Guid _defaultManagerId = Guid.NewGuid();
    private readonly Guid _defaultOperatorId = Guid.NewGuid();
    private readonly Guid _defaultVehicleTypeId = Guid.NewGuid();

    public BusinessUseCasesTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ParkingDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ParkingDbContext(options);
        _context.Database.EnsureCreated();

        _unitOfWork = new UnitOfWork(_context);
        _parkingLotRepo = new ParkingLotRepository(_context);
        _operatorRepo = new BusinessOperatorRepository(_context);
        _sessionRepo = new ParkingSessionRepository(_context);
        _tariffRepo = new TariffRepository(_context);
        _vehicleTypeRepo = new VehicleTypeRepository(_context);
        _managerRepo = new SystemManagerRepository(_context);

        // Seed baseline FK parents
        InitBaseParents();
    }

    private void InitBaseParents()
    {
        var manager = new SystemManager(
            _defaultManagerId,
            "Test Manager",
            "General Manager",
            new ContactInfo("0901111111", "ql@test.vn"),
            "admin_test",
            "hash"
        );
        _context.SystemManagers.Add(manager);

        var op = new BusinessOperator(
            _defaultOperatorId,
            _defaultManagerId,
            "Default Operator",
            "Representative Person",
            new Address("01 Le Loi", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            new ContactInfo("0902222222", "dv@test.vn")
        );
        _context.BusinessOperators.Add(op);

        var vehicleType = new VehicleType(_defaultVehicleTypeId, "Standard Motorbike");
        _context.VehicleTypes.Add(vehicleType);

        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task DeleteParkingLot_WhenVehiclesCurrentlyParked_ThrowsBusinessRuleViolationException()
    {
        // Arrange
        var service = new ParkingLotService(_parkingLotRepo, _operatorRepo, _unitOfWork);
        var lotId = Guid.NewGuid();

        var parkingLot = new ParkingLot(
            lotId,
            _defaultOperatorId,
            "District 1 Parking Lot",
            new Address("01 Le Loi", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            100,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);

        // Simulate active parked vehicle
        var session = new ParkingSession(
            Guid.NewGuid(),
            lotId,
            _defaultVehicleTypeId,
            new LicensePlate("59", "T1-99999")
        );
        await _sessionRepo.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BusinessRuleViolationException>(() => service.DeleteAsync(lotId));
        Assert.Contains("Cannot delete parking lot", ex.Message);
    }

    [Fact]
    public async Task DeleteParkingLot_WhenNoVehiclesParked_DeletesSuccessfully()
    {
        // Arrange
        var service = new ParkingLotService(_parkingLotRepo, _operatorRepo, _unitOfWork);
        var lotId = Guid.NewGuid();

        var parkingLot = new ParkingLot(
            lotId,
            _defaultOperatorId,
            "Empty Parking Lot",
            new Address("02 Le Loi", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            50,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);
        await _unitOfWork.SaveChangesAsync();

        // Act
        await service.DeleteAsync(lotId);

        // Assert
        var check = await _parkingLotRepo.GetByIdAsync(lotId);
        Assert.Null(check);
    }

    [Fact]
    public async Task DeleteBusinessOperator_WhenActiveParkingLotsExist_ThrowsBusinessRuleViolationException()
    {
        // Arrange
        var service = new BusinessOperatorService(_operatorRepo, _managerRepo, _unitOfWork);
        var opId = Guid.NewGuid();

        var op = new BusinessOperator(
            opId,
            _defaultManagerId,
            "Operator A",
            "Nguyen Van A",
            new Address("100 Pasteur", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            new ContactInfo("0909999999", "a@operator.vn")
        );
        await _operatorRepo.AddAsync(op);

        // Active parking lot exists
        var parkingLot = new ParkingLot(
            Guid.NewGuid(),
            opId,
            "Running Parking Lot",
            new Address("100 Pasteur", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            100,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);
        await _unitOfWork.SaveChangesAsync();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BusinessRuleViolationException>(() => service.DeleteAsync(opId));
        Assert.Contains("Cannot delete business operator", ex.Message);
    }

    [Fact]
    public async Task DeleteBusinessOperator_WhenNoActiveParkingLots_DeletesSuccessfully()
    {
        // Arrange
        var service = new BusinessOperatorService(_operatorRepo, _managerRepo, _unitOfWork);
        var opId = Guid.NewGuid();

        var op = new BusinessOperator(
            opId,
            _defaultManagerId,
            "Operator B",
            "Nguyen Van B",
            new Address("200 Pasteur", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            new ContactInfo("0908888888", "b@operator.vn")
        );
        await _operatorRepo.AddAsync(op);

        // Inactive parking lot
        var parkingLot = new ParkingLot(
            Guid.NewGuid(),
            opId,
            "Paused Parking Lot",
            new Address("200 Pasteur", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            100,
            isActive: false
        );
        await _parkingLotRepo.AddAsync(parkingLot);
        await _unitOfWork.SaveChangesAsync();

        // Act
        await service.DeleteAsync(opId);

        // Assert
        var check = await _operatorRepo.GetByIdAsync(opId);
        Assert.Null(check);
    }

    [Fact]
    public async Task Report_InvalidDateRange_ThrowsBusinessRuleViolationException()
    {
        // Arrange
        var reportService = new ReportService(_sessionRepo, _parkingLotRepo);
        var lotId = Guid.NewGuid();

        // FromDate > ToDate
        var ex1 = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            reportService.GetActivityReportAsync(lotId, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
        Assert.Equal("Invalid date range!", ex1.Message);

        // Future date
        var ex2 = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            reportService.GetActivityReportAsync(lotId, DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(10)));
        Assert.Equal("Invalid date range!", ex2.Message);
    }

    [Fact]
    public async Task Report_NoDataFound_ReturnsNull()
    {
        // Arrange
        var reportService = new ReportService(_sessionRepo, _parkingLotRepo);
        var lotId = Guid.NewGuid();

        var parkingLot = new ParkingLot(
            lotId,
            _defaultOperatorId,
            "New Parking Lot",
            new Address("01 Hoa Sen", "Ward 7", "District 3", "Ho Chi Minh City"),
            50,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var result = await reportService.GetActivityReportAsync(lotId, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-5));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CheckIn_And_CheckOut_CalculatesFeeAndUpdatesSpotsCorrectly()
    {
        // Arrange
        var lotId = Guid.NewGuid();

        var parkingLot = new ParkingLot(
            lotId,
            _defaultOperatorId,
            "CheckIn Test Lot",
            new Address("123 CMT8", "Ward 5", "District 3", "Ho Chi Minh City"),
            totalCapacity: 10,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);

        var tariff = new Tariff(
            Guid.NewGuid(),
            lotId,
            _defaultVehicleTypeId,
            new Money(5000, "VND"),
            PricingType.PerTurn
        );
        await _tariffRepo.AddAsync(tariff);
        await _unitOfWork.SaveChangesAsync();

        var sessionService = new ParkingSessionService(_sessionRepo, _parkingLotRepo, _vehicleTypeRepo, _tariffRepo, _unitOfWork);

        // 1. Check-In
        var checkInDto = new CheckInDto
        {
            ParkingLotId = lotId,
            VehicleTypeId = _defaultVehicleTypeId,
            ProvinceCode = "59",
            PlateNumber = "P1-12345"
        };
        var checkInResult = await sessionService.CheckInAsync(checkInDto);

        Assert.Equal(ParkingStatus.Active, checkInResult.Status);
        var lotAfterCheckIn = await _parkingLotRepo.GetByIdAsync(lotId);
        Assert.Equal(9, lotAfterCheckIn!.AvailableSpots);

        // 2. Check-Out
        var checkOutDto = new CheckOutDto
        {
            ParkingLotId = lotId,
            ProvinceCode = "59",
            PlateNumber = "P1-12345"
        };
        var checkOutResult = await sessionService.CheckOutAsync(checkOutDto);

        Assert.Equal(ParkingStatus.Completed, checkOutResult.Status);
        Assert.Equal(5000, checkOutResult.Fee.Amount);

        var lotAfterCheckOut = await _parkingLotRepo.GetByIdAsync(lotId);
        Assert.Equal(10, lotAfterCheckOut!.AvailableSpots);
    }
}
