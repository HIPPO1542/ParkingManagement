using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.ParkingSessions.Queries;

public record GetParkingSessionByIdQuery(Guid Id) : IRequest<ParkingSessionDto>;

public class GetParkingSessionByIdQueryHandler : IRequestHandler<GetParkingSessionByIdQuery, ParkingSessionDto>
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public GetParkingSessionByIdQueryHandler(
        IParkingSessionRepository sessionRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository)
    {
        _sessionRepository = sessionRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task<ParkingSessionDto> Handle(GetParkingSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (session == null)
            throw new EntityNotFoundException("Parking session", request.Id);

        var lot = await _parkingLotRepository.GetByIdAsync(session.ParkingLotId, cancellationToken);
        var vType = await _vehicleTypeRepository.GetByIdAsync(session.VehicleTypeId, cancellationToken);

        return new ParkingSessionDto
        {
            Id = session.Id,
            ParkingLotId = session.ParkingLotId,
            ParkingLotName = lot?.Name ?? "",
            VehicleTypeId = session.VehicleTypeId,
            VehicleTypeName = vType?.Name ?? "",
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
