using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Repositories;

public interface IVehicleTypeRepository
{
    Task<VehicleType?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<VehicleType>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(VehicleType vehicleType, CancellationToken ct = default);
    Task UpdateAsync(VehicleType vehicleType, CancellationToken ct = default);
    Task DeleteAsync(VehicleType vehicleType, CancellationToken ct = default);
}
