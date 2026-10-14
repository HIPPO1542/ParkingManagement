using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.ParkingSessions.Commands;

public record CheckInCommand(CheckInDto Dto) : IRequest<ParkingSessionDto>;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, ParkingSessionDto>
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckInCommandHandler(
        IParkingSessionRepository sessionRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingSessionDto> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var lot = await _parkingLotRepository.GetByIdAsync(dto.ParkingLotId, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", dto.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(dto.VehicleTypeId, cancellationToken);
        if (vType == null)
            throw new EntityNotFoundException("Vehicle type", dto.VehicleTypeId);

        var licensePlate = new LicensePlate(dto.ProvinceCode, dto.PlateNumber);

        var existingActive = await _sessionRepository.GetActiveSessionByPlateAsync(
            dto.ParkingLotId, dto.ProvinceCode, dto.PlateNumber, cancellationToken);
        if (existingActive != null)
            throw new BusinessRuleViolationException($"Vehicle with plate {licensePlate} is currently parked in this parking lot!");

        lot.RecordVehicleEntry();

        var session = new ParkingSession(
            Guid.NewGuid(),
            dto.ParkingLotId,
            dto.VehicleTypeId,
            licensePlate
        );

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _parkingLotRepository.UpdateAsync(lot, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ParkingSessionDto
        {
            Id = session.Id,
            ParkingLotId = session.ParkingLotId,
            ParkingLotName = lot.Name,
            VehicleTypeId = session.VehicleTypeId,
            VehicleTypeName = vType.Name,
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
