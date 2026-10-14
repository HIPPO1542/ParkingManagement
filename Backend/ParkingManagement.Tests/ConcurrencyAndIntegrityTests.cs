using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Features.ParkingSessions.Commands;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.ValueObjects;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;
using Xunit;

namespace ParkingManagement.Tests;

/// <summary>
/// Kiểm thử đối kháng (Adversarial Testing), Tranh chấp đồng thời (Concurrency) & Tính toàn vẹn Transaction (Rollback)
/// </summary>
public class ConcurrencyAndIntegrityTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ParkingDbContext _context;
    private readonly UnitOfWork _unitOfWork;
    private readonly ParkingLotRepository _parkingLotRepo;
    private readonly ParkingSessionRepository _sessionRepo;
    private readonly VehicleTypeRepository _vehicleTypeRepo;
    private readonly BusinessOperatorRepository _operatorRepo;

    private readonly Guid _testLotId = Guid.NewGuid();
    private readonly Guid _testVehicleTypeId = Guid.NewGuid();
    private readonly Guid _testOperatorId = Guid.NewGuid();

    public ConcurrencyAndIntegrityTests()
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
        _sessionRepo = new ParkingSessionRepository(_context);
        _vehicleTypeRepo = new VehicleTypeRepository(_context);
        _operatorRepo = new BusinessOperatorRepository(_context);

        SeedData();
    }

    private readonly Guid _testManagerId = Guid.NewGuid();

    private void SeedData()
    {
        var manager = new SystemManager(
            _testManagerId,
            "Concurrent Manager",
            "Manager Title",
            new ContactInfo("0901234567", "manager@parking.vn"),
            "manager_concurrent",
            "password"
        );
        _context.SystemManagers.Add(manager);

        var op = new BusinessOperator(
            _testOperatorId,
            _testManagerId,
            "Concurrent Op",
            "Representative",
            new Address("123 Le Loi", "Ben Nghe", "1", "HCM"),
            new ContactInfo("0900000000", "op@parking.vn"),
            true,
            "op_concurrent",
            "password"
        );
        _context.BusinessOperators.Add(op);

        var vt = new VehicleType(_testVehicleTypeId, "Motorbike", "Two-wheel vehicle");
        _context.VehicleTypes.Add(vt);

        // Bãi xe chỉ có đúng 1 chỗ đỗ duy nhất để test tranh chấp đồng thời
        var lot = new ParkingLot(
            _testLotId,
            _testOperatorId,
            "Single Spot Lot",
            new Address("123 Le Loi", "Ben Nghe", "1", "HCM"),
            totalCapacity: 1
        );
        _context.ParkingLots.Add(lot);

        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task CheckIn_WhenOnlyOneSpotAvailable_SecondRequestThrowsBusinessRuleViolationException()
    {
        // ARRANGE: Bãi xe chỉ còn đúng 1 chỗ trống. Có 2 xe cùng muốn vào bãi
        var handler = new CheckInCommandHandler(_sessionRepo, _parkingLotRepo, _vehicleTypeRepo, _unitOfWork);

        var request1 = new CheckInCommand(new CheckInDto
        {
            ParkingLotId = _testLotId,
            VehicleTypeId = _testVehicleTypeId,
            ProvinceCode = "59",
            PlateNumber = "11111"
        });

        var request2 = new CheckInCommand(new CheckInDto
        {
            ParkingLotId = _testLotId,
            VehicleTypeId = _testVehicleTypeId,
            ProvinceCode = "59",
            PlateNumber = "22222"
        });

        // ACT: Request 1 vào trước chiếm chỗ cuối cùng
        var result1 = await handler.Handle(request1, default);

        // Request 2 vào sau khi bãi đã hết chỗ ➔ Phải bị chặn lại
        var ex = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            handler.Handle(request2, default)
        );

        // ASSERT: Request 1 thành công, Request 2 bị từ chối vì bãi đầy
        Assert.NotNull(result1);
        Assert.Equal("Parking lot is full!", ex.Message);

        // Bãi xe phải có đúng 0 chỗ trống còn lại, không bao giờ bị âm
        var lotAfter = await _parkingLotRepo.GetByIdAsync(_testLotId);
        Assert.Equal(0, lotAfter!.AvailableSpots);
        Assert.Equal(1, lotAfter.OccupiedSpots);
    }

    [Fact]
    public async Task CheckOut_RepeatedRequest_SameSession_SecondAttemptThrowsException()
    {
        // ARRANGE: Tạo sẵn 1 lượt gửi xe đang Active
        var session = new ParkingSession(
            Guid.NewGuid(),
            _testLotId,
            _testVehicleTypeId,
            new LicensePlate("59", "88888")
        );
        await _sessionRepo.AddAsync(session);
        var lot = await _parkingLotRepo.GetByIdAsync(_testLotId);
        lot!.RecordVehicleEntry();
        await _unitOfWork.SaveChangesAsync();

        var fee = new Money(10000, "VND");

        // ACT: Lần 1 Check-out thành công
        session.CheckOut(fee);
        await _unitOfWork.SaveChangesAsync();

        // ACT & ASSERT: Lần 2 cố tình Check-out lại trên cùng phiên đã hoàn thành ➔ Báo lỗi
        var ex = Assert.Throws<BusinessRuleViolationException>(() => session.CheckOut(fee));

        Assert.Equal("This parking session has already been checked out!", ex.Message);
        Assert.Equal(ParkingStatus.Completed, session.Status);
    }

    [Fact]
    public async Task CheckIn_SameLicensePlate_WhileAlreadyActive_ThrowsBusinessRuleViolationException()
    {
        // ARRANGE: Xe 59-99999 đã check-in vào bãi
        var handler = new CheckInCommandHandler(_sessionRepo, _parkingLotRepo, _vehicleTypeRepo, _unitOfWork);
        var dto = new CheckInDto
        {
            ParkingLotId = _testLotId,
            VehicleTypeId = _testVehicleTypeId,
            ProvinceCode = "59",
            PlateNumber = "99999"
        };
        await handler.Handle(new CheckInCommand(dto), default);

        // ACT & ASSERT: Xe đó cố tình check-in lần nữa khi chưa check-out ➔ Bị chặn
        var ex = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            handler.Handle(new CheckInCommand(dto), default)
        );

        Assert.Contains("currently parked in this parking lot", ex.Message);
    }

    [Fact]
    public async Task CheckIn_Rollback_WhenExceptionOccurs_NoOrphanSessionCreated()
    {
        // ARRANGE: Đăng ký xe vào nhưng truyền ID loại xe không tồn tại
        var handler = new CheckInCommandHandler(_sessionRepo, _parkingLotRepo, _vehicleTypeRepo, _unitOfWork);
        var invalidDto = new CheckInDto
        {
            ParkingLotId = _testLotId,
            VehicleTypeId = Guid.NewGuid(), // Không tồn tại
            ProvinceCode = "59",
            PlateNumber = "77777"
        };

        // ACT: Gọi handler và kỳ vọng ném lỗi EntityNotFoundException
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            handler.Handle(new CheckInCommand(invalidDto), default)
        );

        // ASSERT: Không có session rác nào được lưu vào DB, chỗ trống bãi xe vẫn giữ nguyên
        var sessions = await _sessionRepo.GetAllAsync(_testLotId);
        Assert.Empty(sessions);

        var lot = await _parkingLotRepo.GetByIdAsync(_testLotId);
        Assert.Equal(1, lot!.AvailableSpots);
        Assert.Equal(0, lot.OccupiedSpots);
    }
}
