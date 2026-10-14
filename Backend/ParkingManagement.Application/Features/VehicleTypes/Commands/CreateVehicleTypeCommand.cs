using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.VehicleTypes.Commands;

public record CreateVehicleTypeCommand(CreateVehicleTypeDto Dto) : IRequest<VehicleTypeDto>;

public class CreateVehicleTypeCommandHandler : IRequestHandler<CreateVehicleTypeCommand, VehicleTypeDto>
{
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleTypeCommandHandler(
        IVehicleTypeRepository vehicleTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleTypeDto> Handle(CreateVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new VehicleType(
            Guid.NewGuid(),
            request.Dto.Name,
            request.Dto.Description
        );

        await _vehicleTypeRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VehicleTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}
