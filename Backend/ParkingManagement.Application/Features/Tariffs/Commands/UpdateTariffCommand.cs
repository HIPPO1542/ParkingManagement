using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.Tariffs.Commands;

public record UpdateTariffCommand(Guid Id, UpdateTariffDto Dto) : IRequest<TariffDto>;

public class UpdateTariffCommandHandler : IRequestHandler<UpdateTariffCommand, TariffDto>
{
    private readonly ITariffRepository _tariffRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTariffCommandHandler(
        ITariffRepository tariffRepository,
        IParkingLotRepository parkingLotRepository,
        IVehicleTypeRepository vehicleTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _tariffRepository = tariffRepository;
        _parkingLotRepository = parkingLotRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TariffDto> Handle(UpdateTariffCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var dto = request.Dto;

        var tariff = await _tariffRepository.GetByIdAsync(id, cancellationToken);
        if (tariff == null)
            throw new EntityNotFoundException("Tariff", id);

        var lot = await _parkingLotRepository.GetByIdAsync(dto.ParkingLotId, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", dto.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(dto.VehicleTypeId, cancellationToken);
        if (vType == null)
            throw new EntityNotFoundException("Vehicle type", dto.VehicleTypeId);

        var price = new Money(dto.Price.Amount, dto.Price.Currency);

        tariff.UpdatePricing(price, dto.PricingType, dto.EffectiveDate ?? DateTime.UtcNow);

        await _tariffRepository.UpdateAsync(tariff, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TariffDto
        {
            Id = tariff.Id,
            ParkingLotId = tariff.ParkingLotId,
            ParkingLotName = lot.Name,
            VehicleTypeId = tariff.VehicleTypeId,
            VehicleTypeName = vType.Name,
            Price = new MoneyDto
            {
                Amount = tariff.Price.Amount,
                Currency = tariff.Price.Currency
            },
            PricingType = tariff.PricingType,
            EffectiveDate = tariff.EffectiveDate
        };
    }
}
