using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Repositories;

public interface ITariffRepository
{
    Task<Tariff?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Tariff?> GetCurrentTariffAsync(Guid parkingLotId, Guid vehicleTypeId, CancellationToken ct = default);
    Task<IEnumerable<Tariff>> GetByParkingLotIdAsync(Guid parkingLotId, CancellationToken ct = default);
    Task<IEnumerable<Tariff>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Tariff tariff, CancellationToken ct = default);
    Task UpdateAsync(Tariff tariff, CancellationToken ct = default);
    Task DeleteAsync(Tariff tariff, CancellationToken ct = default);
}
