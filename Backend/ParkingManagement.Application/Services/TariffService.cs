using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Services;

public interface ITariffService
{
    Task<IEnumerable<TariffDto>> GetAllAsync();
    Task<IEnumerable<TariffDto>> GetByParkingLotIdAsync(Guid parkingLotId);
    Task<TariffDto> CreateAsync(CreateTariffDto dto);
    Task<TariffDto> UpdateAsync(Guid id, UpdateTariffDto dto);
    Task DeleteAsync(Guid id);
}

public class TariffService : ITariffService
{
    private readonly ITariffRepository _tariffRepository;
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TariffService(
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

    public async Task<IEnumerable<TariffDto>> GetAllAsync()
    {
        var list = await _tariffRepository.GetAllAsync();
        var dtos = new List<TariffDto>();
        foreach (var item in list)
        {
            var lot = await _parkingLotRepository.GetByIdAsync(item.ParkingLotId);
            var vType = await _vehicleTypeRepository.GetByIdAsync(item.VehicleTypeId);
            dtos.Add(MapToDto(item, lot?.Name ?? "", vType?.Name ?? ""));
        }
        return dtos;
    }

    public async Task<IEnumerable<TariffDto>> GetByParkingLotIdAsync(Guid parkingLotId)
    {
        var list = await _tariffRepository.GetByParkingLotIdAsync(parkingLotId);
        var lot = await _parkingLotRepository.GetByIdAsync(parkingLotId);
        var dtos = new List<TariffDto>();
        foreach (var item in list)
        {
            var vType = await _vehicleTypeRepository.GetByIdAsync(item.VehicleTypeId);
            dtos.Add(MapToDto(item, lot?.Name ?? "", vType?.Name ?? ""));
        }
        return dtos;
    }

    public async Task<TariffDto> CreateAsync(CreateTariffDto dto)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(dto.ParkingLotId);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", dto.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(dto.VehicleTypeId);
        if (vType == null)
            throw new EntityNotFoundException("Vehicle type", dto.VehicleTypeId);

        var price = new Money(dto.Price.Amount, dto.Price.Currency);

        var tariff = new Tariff(
            Guid.NewGuid(),
            dto.ParkingLotId,
            dto.VehicleTypeId,
            price,
            dto.PricingType,
            dto.EffectiveDate ?? DateTime.UtcNow
        );

        await _tariffRepository.AddAsync(tariff);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(tariff, lot.Name, vType.Name);
    }

    public async Task<TariffDto> UpdateAsync(Guid id, UpdateTariffDto dto)
    {
        var tariff = await _tariffRepository.GetByIdAsync(id);
        if (tariff == null)
            throw new EntityNotFoundException("Tariff", id);

        var lot = await _parkingLotRepository.GetByIdAsync(dto.ParkingLotId);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", dto.ParkingLotId);

        var vType = await _vehicleTypeRepository.GetByIdAsync(dto.VehicleTypeId);
        if (vType == null)
            throw new EntityNotFoundException("Vehicle type", dto.VehicleTypeId);

        var price = new Money(dto.Price.Amount, dto.Price.Currency);

        tariff.UpdatePricing(price, dto.PricingType, dto.EffectiveDate ?? DateTime.UtcNow);

        await _tariffRepository.UpdateAsync(tariff);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(tariff, lot.Name, vType.Name);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _tariffRepository.GetByIdAsync(id);
        if (entity == null)
            throw new EntityNotFoundException("Tariff", id);

        await _tariffRepository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private static TariffDto MapToDto(Tariff entity, string parkingLotName, string vehicleTypeName)
    {
        return new TariffDto
        {
            Id = entity.Id,
            ParkingLotId = entity.ParkingLotId,
            ParkingLotName = parkingLotName,
            VehicleTypeId = entity.VehicleTypeId,
            VehicleTypeName = vehicleTypeName,
            Price = new MoneyDto
            {
                Amount = entity.Price.Amount,
                Currency = entity.Price.Currency
            },
            PricingType = entity.PricingType,
            EffectiveDate = entity.EffectiveDate
        };
    }
}
