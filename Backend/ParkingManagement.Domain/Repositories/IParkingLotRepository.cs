using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Repositories;

public interface IParkingLotRepository
{
    Task<ParkingLot?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ParkingLot>> GetAllAsync(Guid? operatorId = null, bool? isActive = null, int page = 1, int pageSize = 20, CancellationToken ct = default);
    Task<int> CountAsync(Guid? operatorId = null, bool? isActive = null, CancellationToken ct = default);
    Task<int> CheckActiveParkingVehiclesAsync(Guid parkingLotId, CancellationToken ct = default);
    Task AddAsync(ParkingLot parkingLot, CancellationToken ct = default);
    Task UpdateAsync(ParkingLot parkingLot, CancellationToken ct = default);
    Task DeleteAsync(ParkingLot parkingLot, CancellationToken ct = default);
}
