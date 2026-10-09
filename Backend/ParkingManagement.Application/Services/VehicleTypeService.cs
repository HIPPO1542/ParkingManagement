using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Services;

public interface IVehicleTypeService
{
    Task<IEnumerable<VehicleTypeDto>> GetAllAsync();
    Task<VehicleTypeDto> GetByIdAsync(Guid id);
    Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeDto dto);
}

public class VehicleTypeService : IVehicleTypeService
{
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleTypeService(IVehicleTypeRepository vehicleTypeRepository, IUnitOfWork unitOfWork)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehicleTypeDto>> GetAllAsync()
    {
        var list = await _vehicleTypeRepository.GetAllAsync();
        return list.Select(x => new VehicleTypeDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        });
    }

    public async Task<VehicleTypeDto> GetByIdAsync(Guid id)
    {
        var entity = await _vehicleTypeRepository.GetByIdAsync(id);
        if (entity == null)
            throw new EntityNotFoundException("Vehicle type", id);

        return new VehicleTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public async Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeDto dto)
    {
        var vehicleType = new VehicleType(Guid.NewGuid(), dto.Name, dto.Description);
        await _vehicleTypeRepository.AddAsync(vehicleType);
        await _unitOfWork.SaveChangesAsync();

        return new VehicleTypeDto
        {
            Id = vehicleType.Id,
            Name = vehicleType.Name,
            Description = vehicleType.Description
        };
    }
}
