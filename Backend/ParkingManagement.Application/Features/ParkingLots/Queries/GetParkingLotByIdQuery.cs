using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.ParkingLots.Queries;

public record GetParkingLotByIdQuery(Guid Id) : IRequest<ParkingLotDto>;

public class GetParkingLotByIdQueryHandler : IRequestHandler<GetParkingLotByIdQuery, ParkingLotDto>
{
    private readonly IParkingLotRepository _parkingLotRepository;

    public GetParkingLotByIdQueryHandler(IParkingLotRepository parkingLotRepository)
    {
        _parkingLotRepository = parkingLotRepository;
    }

    public async Task<ParkingLotDto> Handle(GetParkingLotByIdQuery request, CancellationToken cancellationToken)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(request.Id, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", request.Id);

        return new ParkingLotDto
        {
            Id = lot.Id,
            BusinessOperatorId = lot.BusinessOperatorId,
            Name = lot.Name,
            Address = new AddressDto
            {
                Street = lot.Address.Street,
                Ward = lot.Address.Ward,
                District = lot.Address.District,
                City = lot.Address.City
            },
            FullAddress = lot.Address.ToString(),
            TotalCapacity = lot.TotalCapacity,
            AvailableSpots = lot.AvailableSpots,
            OpenTime = lot.OpenTime,
            CloseTime = lot.CloseTime,
            IsActive = lot.IsActive
        };
    }
}
