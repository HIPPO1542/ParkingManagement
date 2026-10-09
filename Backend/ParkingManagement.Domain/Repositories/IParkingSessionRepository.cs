using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Domain.Repositories;

public interface IParkingSessionRepository
{
    Task<ParkingSession?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ParkingSession?> GetActiveSessionByPlateAsync(Guid parkingLotId, string provinceCode, string plateNumber, CancellationToken ct = default);
    Task<List<ParkingSession>> GetSessionsInTimeRangeAsync(Guid parkingLotId, DateTime fromDate, DateTime toDate, CancellationToken ct = default);
    Task<int> CountActiveSessionsInLotAsync(Guid parkingLotId, CancellationToken ct = default);
    Task<IEnumerable<ParkingSession>> GetAllAsync(Guid? parkingLotId = null, ParkingStatus? status = null, int page = 1, int pageSize = 20, CancellationToken ct = default);
    Task<int> CountAsync(Guid? parkingLotId = null, ParkingStatus? status = null, CancellationToken ct = default);
    Task AddAsync(ParkingSession session, CancellationToken ct = default);
    Task UpdateAsync(ParkingSession session, CancellationToken ct = default);
}
