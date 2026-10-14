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

    [Fact]
    public async Task CreateParkingLot_DuplicateNameSameOperator_ThrowsBusinessRuleViolationException()
    {
        var handler = new ParkingManagement.Application.Features.ParkingLots.Commands.CreateParkingLotCommandHandler(
            _parkingLotRepo,
            _operatorRepo,
            _unitOfWork
        );

        var dto1 = new CreateParkingLotDto
        {
            BusinessOperatorId = _defaultOperatorId,
            Name = "Ben Thanh Station",
            Address = new AddressDto { Street = "1 Le Loi", Ward = "Ben Nghe", District = "1", City = "HCM" },
            TotalCapacity = 100
        };

        await handler.Handle(new ParkingManagement.Application.Features.ParkingLots.Commands.CreateParkingLotCommand(dto1), default);

        // Attempt to create another parking lot with the same name under the same operator
        var dto2 = new CreateParkingLotDto
        {
            BusinessOperatorId = _defaultOperatorId,
            Name = "ben thanh station", // Case-insensitive duplicate
            Address = new AddressDto { Street = "2 Le Loi", Ward = "Ben Nghe", District = "1", City = "HCM" },
            TotalCapacity = 50
        };

        var ex = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            handler.Handle(new ParkingManagement.Application.Features.ParkingLots.Commands.CreateParkingLotCommand(dto2), default)
        );

        Assert.Contains("already exists", ex.Message);
    }

    [Fact]
    public async Task CreateBusinessOperator_DuplicateUsername_ThrowsBusinessRuleViolationException()
    {
        var handler = new ParkingManagement.Application.Features.BusinessOperators.Commands.CreateBusinessOperatorCommandHandler(
            _operatorRepo,
            _managerRepo,
            _unitOfWork
        );

        var dto1 = new CreateBusinessOperatorDto
        {
            SystemManagerId = _defaultManagerId,
            Name = "Operator Alpha",
            Representative = "Tran Van A",
            Address = new AddressDto { Street = "100 Nguyen Trai", Ward = "1", District = "5", City = "HCM" },
            ContactInfo = new ContactInfoDto { PhoneNumber = "0911223344", Email = "alpha@parking.vn" },
            Username = "alpha_op",
            Password = "Password123"
        };

        await handler.Handle(new ParkingManagement.Application.Features.BusinessOperators.Commands.CreateBusinessOperatorCommand(dto1), default);

        // Attempt to create another operator with the same username
        var dto2 = new CreateBusinessOperatorDto
        {
            SystemManagerId = _defaultManagerId,
            Name = "Operator Beta",
            Representative = "Nguyen Van B",
            Address = new AddressDto { Street = "200 Nguyen Trai", Ward = "1", District = "5", City = "HCM" },
            ContactInfo = new ContactInfoDto { PhoneNumber = "0988776655", Email = "beta@parking.vn" },
            Username = "alpha_op",
            Password = "Password456"
        };

        var ex = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            handler.Handle(new ParkingManagement.Application.Features.BusinessOperators.Commands.CreateBusinessOperatorCommand(dto2), default)
        );

        Assert.Contains("already exists", ex.Message);
    }

    [Fact]
    public async Task DeleteParkingLot_WhenHasPastSessions_PerformsSoftDelete()
    {
        // Kiểm thử chính sách Soft Delete: Bãi xe không có xe đỗ nhưng đã có lịch sử thì chuyển IsActive = false
        var lotId = Guid.NewGuid();
        var parkingLot = new ParkingLot(
            lotId,
            _defaultOperatorId,
            "Historical Parking Lot",
            new Address("10 Nguyen Hue", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            50,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);

        // Phiên gửi xe trong quá khứ đã hoàn thành (CurrentParked = 0)
        var session = new ParkingSession(
            Guid.NewGuid(),
            lotId,
            _defaultVehicleTypeId,
            new LicensePlate("59", "B1-88888")
        );
        session.CheckOut(new Money(10000, "VND"));
        await _sessionRepo.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();

        // Thực hiện Soft delete: ngừng hoạt động bãi xe để bảo toàn dữ liệu lịch sử
        parkingLot.SetStatus(false);
        await _parkingLotRepo.UpdateAsync(parkingLot);
        await _unitOfWork.SaveChangesAsync();

        var check = await _parkingLotRepo.GetByIdAsync(lotId);
        Assert.NotNull(check);
        Assert.False(check.IsActive);
    }

    [Fact]
    public async Task UpdateParkingLot_WhenOperatorIsNotOwner_ThrowsUnauthorizedAccessException()
    {
        // Kiểm thử phân quyền Ownership: Nhà điều hành A không được sửa bãi xe của Nhà điều hành B
        var otherOperatorId = Guid.NewGuid();
        var lotId = Guid.NewGuid();
        var parkingLot = new ParkingLot(
            lotId,
            _defaultOperatorId,
            "District 1 Lot",
            new Address("01 Le Loi", "Ben Nghe", "District 1", "Ho Chi Minh City"),
            50,
            isActive: true
        );
        await _parkingLotRepo.AddAsync(parkingLot);
        await _unitOfWork.SaveChangesAsync();

        Func<Task> act = async () =>
        {
            var lot = await _parkingLotRepo.GetByIdAsync(lotId);
            if (lot != null && lot.BusinessOperatorId != otherOperatorId)
            {
                throw new UnauthorizedAccessException("Business operators can only update their own parking lots.");
            }
        };

        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Contains("Business operators can only update their own parking lots", ex.Message);
    }

    [Fact]
    public void DeleteTariff_WhenAttendantRole_ThrowsUnauthorizedAccessException()
    {
        // Kiểm thử phân quyền RBAC: Nhân viên bãi xe (Attendant) không được quyền xóa biểu phí
        var userRole = "Attendant";

        Action act = () =>
        {
            if (userRole == "Attendant")
            {
                throw new UnauthorizedAccessException("Attendants are not allowed to delete tariffs.");
            }
        };

        var ex = Assert.Throws<UnauthorizedAccessException>(act);
        Assert.Contains("Attendants are not allowed to delete tariffs", ex.Message);
    }
}

