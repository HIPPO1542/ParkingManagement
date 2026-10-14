using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.ParkingSessions.Commands;

public record CheckOutByPlateCommand(CheckOutDto Dto) : IRequest<ParkingSessionDto>;

public class CheckOutByPlateCommandHandler : IRequestHandler<CheckOutByPlateCommand, ParkingSessionDto>
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckOutByPlateCommandHandler(
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

    public async Task<ParkingSessionDto> Handle(CheckOutByPlateCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var session = await _sessionRepository.GetActiveSessionByPlateAsync(
            dto.ParkingLotId, dto.ProvinceCode, dto.PlateNumber, cancellationToken);
        if (session == null)
            throw new BusinessRuleViolationException($"No active vehicle with plate {dto.ProvinceCode}-{dto.PlateNumber} found in this parking lot!");

        var lot = await _parkingLotRepository.GetByIdAsync(session.ParkingLotId, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", session.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(session.VehicleTypeId, cancellationToken);
        var typeName = vType?.Name ?? "Unknown";

        var duration = DateTime.UtcNow - session.CheckInTime;

        var tariff = await _tariffRepository.GetCurrentTariffAsync(session.ParkingLotId, session.VehicleTypeId, cancellationToken);
        Money fee = tariff != null ? tariff.CalculateFee(duration) : new Money(10000, "VND");

        session.CheckOut(fee);
        lot.RecordVehicleExit();

        await _sessionRepository.UpdateAsync(session, cancellationToken);
        await _parkingLotRepository.UpdateAsync(lot, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ParkingSessionDto
        {
            Id = session.Id,
            ParkingLotId = session.ParkingLotId,
            ParkingLotName = lot.Name,
            VehicleTypeId = session.VehicleTypeId,
            VehicleTypeName = typeName,
            LicensePlate = new LicensePlateDto
            {
                ProvinceCode = session.LicensePlate.ProvinceCode,
                PlateNumber = session.LicensePlate.PlateNumber
            },
            CheckInTime = session.CheckInTime,
            CheckOutTime = session.CheckOutTime,
            Fee = new MoneyDto
            {
                Amount = session.Fee.Amount,
                Currency = session.Fee.Currency
            },
            Status = session.Status
        };
    }
}
