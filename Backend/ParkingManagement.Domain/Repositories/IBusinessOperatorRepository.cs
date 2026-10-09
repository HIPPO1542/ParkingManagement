using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Domain.Repositories;

public interface IBusinessOperatorRepository
{
    Task<BusinessOperator?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BusinessOperator?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<IEnumerable<BusinessOperator>> GetAllAsync(CancellationToken ct = default);
    Task<int> CheckActiveParkingLotsAsync(Guid operatorId, CancellationToken ct = default);
    Task AddAsync(BusinessOperator businessOperator, CancellationToken ct = default);
    Task UpdateAsync(BusinessOperator businessOperator, CancellationToken ct = default);
    Task DeleteAsync(BusinessOperator businessOperator, CancellationToken ct = default);
}
