using MediatR;
using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.ParkingLots.Queries;

public record GetParkingLotsQuery(
    Guid? OperatorId = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<ParkingLotDto>>;

public class GetParkingLotsQueryHandler : IRequestHandler<GetParkingLotsQuery, PagedResult<ParkingLotDto>>
{
    private readonly IParkingLotRepository _parkingLotRepository;

    public GetParkingLotsQueryHandler(IParkingLotRepository parkingLotRepository)
    {
        _parkingLotRepository = parkingLotRepository;
    }

    public async Task<PagedResult<ParkingLotDto>> Handle(GetParkingLotsQuery request, CancellationToken cancellationToken)
    {
        var items = await _parkingLotRepository.GetAllAsync(
            request.OperatorId,
            request.IsActive,
            request.Page,
            request.PageSize,
            cancellationToken
        );

        var totalCount = await _parkingLotRepository.CountAsync(
            request.OperatorId,
            request.IsActive,
            cancellationToken
        );

        var dtos = items.Select(lot => new ParkingLotDto
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
        }).ToList();

        return new PagedResult<ParkingLotDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
