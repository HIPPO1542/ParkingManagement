using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.ParkingSessions.Queries;

public record GetParkingSessionsQuery(
    Guid? ParkingLotId = null,
    ParkingStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<ParkingSessionDto>>;

public class GetParkingSessionsQueryHandler : IRequestHandler<GetParkingSessionsQuery, PagedResult<ParkingSessionDto>>
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public GetParkingSessionsQueryHandler(
        IParkingSessionRepository sessionRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository)
    {
        _sessionRepository = sessionRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task<PagedResult<ParkingSessionDto>> Handle(GetParkingSessionsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

        var items = await _sessionRepository.GetAllAsync(
            request.ParkingLotId,
            request.Status,
            page,
            pageSize,
            cancellationToken
        );

        var total = await _sessionRepository.CountAsync(request.ParkingLotId, request.Status, cancellationToken);

        var dtos = new List<ParkingSessionDto>();
        foreach (var session in items)
        {
            var lot = await _parkingLotRepository.GetByIdAsync(session.ParkingLotId, cancellationToken);
            var vType = await _vehicleTypeRepository.GetByIdAsync(session.VehicleTypeId, cancellationToken);

            dtos.Add(new ParkingSessionDto
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
            });
        }

        return new PagedResult<ParkingSessionDto>(dtos, total, page, pageSize);
    }
}
