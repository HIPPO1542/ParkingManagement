using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Repositories;

public interface ISystemManagerRepository
{
    Task<SystemManager?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SystemManager?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<IEnumerable<SystemManager>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SystemManager manager, CancellationToken ct = default);
    Task UpdateAsync(SystemManager manager, CancellationToken ct = default);
}
