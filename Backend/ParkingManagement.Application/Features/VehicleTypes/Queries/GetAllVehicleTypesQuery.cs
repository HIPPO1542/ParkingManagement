using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.VehicleTypes.Queries;

public record GetAllVehicleTypesQuery : IRequest<IEnumerable<VehicleTypeDto>>;

public class GetAllVehicleTypesQueryHandler : IRequestHandler<GetAllVehicleTypesQuery, IEnumerable<VehicleTypeDto>>
{
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public GetAllVehicleTypesQueryHandler(IVehicleTypeRepository vehicleTypeRepository)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task<IEnumerable<VehicleTypeDto>> Handle(GetAllVehicleTypesQuery request, CancellationToken cancellationToken)
    {
        var list = await _vehicleTypeRepository.GetAllAsync(cancellationToken);
        return list.Select(e => new VehicleTypeDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description
        });
    }
}
