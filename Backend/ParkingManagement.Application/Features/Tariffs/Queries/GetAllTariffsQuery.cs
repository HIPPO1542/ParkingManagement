using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.Tariffs.Queries;

public record GetAllTariffsQuery : IRequest<IEnumerable<TariffDto>>;

public class GetAllTariffsQueryHandler : IRequestHandler<GetAllTariffsQuery, IEnumerable<TariffDto>>
{
    private readonly ITariffRepository _tariffRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public GetAllTariffsQueryHandler(
        ITariffRepository tariffRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository)
    {
        _tariffRepository = tariffRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task<IEnumerable<TariffDto>> Handle(GetAllTariffsQuery request, CancellationToken cancellationToken)
    {
        var list = await _tariffRepository.GetAllAsync(cancellationToken);
        var dtos = new List<TariffDto>();
        foreach (var item in list)
        {
            var lot = await _parkingLotRepository.GetByIdAsync(item.ParkingLotId, cancellationToken);
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
