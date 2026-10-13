using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.Tariffs.Queries;

public record GetTariffsByParkingLotQuery(Guid ParkingLotId) : IRequest<IEnumerable<TariffDto>>;

public class GetTariffsByParkingLotQueryHandler : IRequestHandler<GetTariffsByParkingLotQuery, IEnumerable<TariffDto>>
{
    private readonly ITariffRepository _tariffRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public GetTariffsByParkingLotQueryHandler(
        ITariffRepository tariffRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository)
    {
        _tariffRepository = tariffRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task<IEnumerable<TariffDto>> Handle(GetTariffsByParkingLotQuery request, CancellationToken cancellationToken)
    {
        var list = await _tariffRepository.GetByParkingLotIdAsync(request.ParkingLotId, cancellationToken);
        var lot = await _parkingLotRepository.GetByIdAsync(request.ParkingLotId, cancellationToken);
        var dtos = new List<TariffDto>();
        foreach (var item in list)
        {
            var vType = await _vehicleTypeRepository.GetByIdAsync(item.VehicleTypeId, cancellationToken);
            dtos.Add(new TariffDto
            {
                Id = item.Id,
                ParkingLotId = item.ParkingLotId,
                ParkingLotName = lot?.Name ?? "",
                VehicleTypeId = item.VehicleTypeId,
                VehicleTypeName = vType?.Name ?? "",
                Price = new MoneyDto
                {
                    Amount = item.Price.Amount,
                    Currency = item.Price.Currency
                },
                PricingType = item.PricingType,
                EffectiveDate = item.EffectiveDate
            });
        }
        return dtos;
    }
}
