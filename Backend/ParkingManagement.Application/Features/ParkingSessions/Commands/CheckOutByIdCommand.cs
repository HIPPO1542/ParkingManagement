using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.ParkingSessions.Commands;

public record CheckOutByIdCommand(Guid SessionId) : IRequest<ParkingSessionDto>;

public class CheckOutByIdCommandHandler : IRequestHandler<CheckOutByIdCommand, ParkingSessionDto>
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckOutByIdCommandHandler(
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

    public async Task<ParkingSessionDto> Handle(CheckOutByIdCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session == null)
            throw new EntityNotFoundException("Parking session", request.SessionId);

        if (session.Status == ParkingStatus.Completed)
            throw new BusinessRuleViolationException("This parking session has already been checked out!");

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
