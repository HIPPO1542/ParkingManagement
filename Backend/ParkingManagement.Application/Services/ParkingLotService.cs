using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Services;

public class ParkingLotService : IParkingLotService
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IBusinessOperatorRepository _operatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ParkingLotService(
        IParkingLotRepository parkingLotRepository,
        IBusinessOperatorRepository operatorRepository,
        IUnitOfWork unitOfWork)
    {
        _parkingLotRepository = parkingLotRepository;
        _operatorRepository = operatorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<ParkingLotDto>> GetPagedAsync(Guid? operatorId = null, bool? isActive = null, int page = 1, int pageSize = 20)
    {
        var items = await _parkingLotRepository.GetAllAsync(operatorId, isActive, page, pageSize);
        var totalCount = await _parkingLotRepository.CountAsync(operatorId, isActive);

        var dtos = items.Select(MapToDto).ToList();
        return new PagedResult<ParkingLotDto>(dtos, totalCount, page, pageSize);
    }

    public async Task<ParkingLotDto> GetByIdAsync(Guid id)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(id);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", id);

        return MapToDto(lot);
    }

    public async Task<ParkingLotDto> CreateAsync(CreateParkingLotDto dto)
    {
        var op = await _operatorRepository.GetByIdAsync(dto.BusinessOperatorId);
        if (op == null)
            throw new EntityNotFoundException("Business operator", dto.BusinessOperatorId);

        var address = new Address(
            dto.Address.Street,
            dto.Address.Ward,
            dto.Address.District,
            dto.Address.City
        );

        var lot = new ParkingLot(
            Guid.NewGuid(),
            dto.BusinessOperatorId,
            dto.Name,
            address,
            dto.TotalCapacity,
            dto.OpenTime,
            dto.CloseTime,
            dto.IsActive
        );

        await _parkingLotRepository.AddAsync(lot);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(lot);
    }

    public async Task<ParkingLotDto> UpdateAsync(Guid id, UpdateParkingLotDto dto)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(id);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", id);

        var address = new Address(
            dto.Address.Street,
            dto.Address.Ward,
            dto.Address.District,
            dto.Address.City
        );

        lot.UpdateInfo(
            dto.Name,
            address,
            dto.TotalCapacity,
            dto.OpenTime,
            dto.CloseTime,
            dto.IsActive
        );

        await _parkingLotRepository.UpdateAsync(lot);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(lot);
    }

    public async Task DeleteAsync(Guid id)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(id);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", id);

        int activeCars = await _parkingLotRepository.CheckActiveParkingVehiclesAsync(id);
        if (activeCars > 0)
            throw new BusinessRuleViolationException($"Cannot delete parking lot while {activeCars} vehicle(s) are still parked inside!");

        await _parkingLotRepository.DeleteAsync(lot);
        await _unitOfWork.SaveChangesAsync();
    }

    private static ParkingLotDto MapToDto(ParkingLot lot)
    {
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
