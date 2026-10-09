using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Services;

public interface IParkingSessionService
{
    Task<ParkingSessionDto> CheckInAsync(CheckInDto dto);
    Task<ParkingSessionDto> CheckOutAsync(CheckOutDto dto);
    Task<ParkingSessionDto> CheckOutByIdAsync(Guid sessionId);
    Task<ParkingSessionDto> GetByIdAsync(Guid id);
    Task<PagedResult<ParkingSessionDto>> GetPagedAsync(Guid? parkingLotId = null, ParkingStatus? status = null, int page = 1, int pageSize = 20);
}

public class ParkingSessionService : IParkingSessionService
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ParkingSessionService(
        IParkingSessionRepository sessionRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository,
        ITariffRepository tariffRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
        _tariffRepository = tariffRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingSessionDto> CheckInAsync(CheckInDto dto)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(dto.ParkingLotId);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", dto.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(dto.VehicleTypeId);
        if (vType == null)
            throw new EntityNotFoundException("Vehicle type", dto.VehicleTypeId);

        var licensePlate = new LicensePlate(dto.ProvinceCode, dto.PlateNumber);

        // Check duplicate active vehicle plate in the same parking lot
        var existingActive = await _sessionRepository.GetActiveSessionByPlateAsync(dto.ParkingLotId, dto.ProvinceCode, dto.PlateNumber);
        if (existingActive != null)
            throw new BusinessRuleViolationException($"Vehicle with plate {licensePlate} is currently parked in this parking lot!");

        // Check capacity and record entry
        lot.RecordVehicleEntry();

        var session = new ParkingSession(
            Guid.NewGuid(),
            dto.ParkingLotId,
            dto.VehicleTypeId,
            licensePlate
        );

        await _sessionRepository.AddAsync(session);
        await _parkingLotRepository.UpdateAsync(lot);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(session, lot.Name, vType.Name);
    }

    public async Task<ParkingSessionDto> CheckOutAsync(CheckOutDto dto)
    {
        var session = await _sessionRepository.GetActiveSessionByPlateAsync(dto.ParkingLotId, dto.ProvinceCode, dto.PlateNumber);
        if (session == null)
            throw new BusinessRuleViolationException($"No active vehicle with plate {dto.ProvinceCode}-{dto.PlateNumber} found in this parking lot!");

        return await ProcessCheckOut(session);
    }

    public async Task<ParkingSessionDto> CheckOutByIdAsync(Guid sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new EntityNotFoundException("Parking session", sessionId);

        if (session.Status == ParkingStatus.Completed)
            throw new BusinessRuleViolationException("This parking session has already been checked out!");

        return await ProcessCheckOut(session);
    }

    private async Task<ParkingSessionDto> ProcessCheckOut(ParkingSession session)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(session.ParkingLotId);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", session.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(session.VehicleTypeId);
        var typeName = vType?.Name ?? "Unknown";

        var duration = DateTime.UtcNow - session.CheckInTime;

        var tariff = await _tariffRepository.GetCurrentTariffAsync(session.ParkingLotId, session.VehicleTypeId);
        Money fee = tariff != null ? tariff.CalculateFee(duration) : new Money(10000, "VND");

        session.CheckOut(fee);
        lot.RecordVehicleExit();

        await _sessionRepository.UpdateAsync(session);
        await _parkingLotRepository.UpdateAsync(lot);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(session, lot.Name, typeName);
    }

    public async Task<ParkingSessionDto> GetByIdAsync(Guid id)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
            throw new EntityNotFoundException("Parking session", id);

        var lot = await _parkingLotRepository.GetByIdAsync(session.ParkingLotId);
        var vType = await _vehicleTypeRepository.GetByIdAsync(session.VehicleTypeId);

        return MapToDto(session, lot?.Name ?? "", vType?.Name ?? "");
    }

    public async Task<PagedResult<ParkingSessionDto>> GetPagedAsync(
        Guid? parkingLotId = null,
        ParkingStatus? status = null,
        int page = 1,
        int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var total = await _sessionRepository.CountAsync(parkingLotId, status);
        var items = await _sessionRepository.GetAllAsync(parkingLotId, status, page, pageSize);

        var dtos = new List<ParkingSessionDto>();
        foreach (var item in items)
        {
            var lot = await _parkingLotRepository.GetByIdAsync(item.ParkingLotId);
            var vType = await _vehicleTypeRepository.GetByIdAsync(item.VehicleTypeId);
            dtos.Add(MapToDto(item, lot?.Name ?? "", vType?.Name ?? ""));
        }

        return new PagedResult<ParkingSessionDto>(dtos, total, page, pageSize);
    }

    private static ParkingSessionDto MapToDto(ParkingSession item, string lotName, string vehicleTypeName)
    {
        double? durationHours = null;
        if (item.CheckOutTime.HasValue)
        {
            durationHours = Math.Round((item.CheckOutTime.Value - item.CheckInTime).TotalHours, 2);
        }

        return new ParkingSessionDto
        {
            Id = item.Id,
            ParkingLotId = item.ParkingLotId,
            ParkingLotName = lotName,
            VehicleTypeId = item.VehicleTypeId,
            VehicleTypeName = vehicleTypeName,
            LicensePlate = new LicensePlateDto
            {
                ProvinceCode = item.LicensePlate.ProvinceCode,
                PlateNumber = item.LicensePlate.PlateNumber
            },
            FullPlateNumber = item.LicensePlate.ToString(),
            CheckInTime = item.CheckInTime,
            CheckOutTime = item.CheckOutTime,
            Fee = new MoneyDto
            {
                Amount = item.Fee.Amount,
                Currency = item.Fee.Currency
            },
            Status = item.Status,
            DurationHours = durationHours
        };
    }
}
