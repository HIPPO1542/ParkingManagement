using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class ParkingSessionRepository : IParkingSessionRepository
{
    private readonly ParkingDbContext _context;

    public ParkingSessionRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public async Task<ParkingSession?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.ParkingSessions
            .Include(s => s.ParkingLot)
            .Include(s => s.VehicleType)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<ParkingSession?> GetActiveSessionByPlateAsync(Guid parkingLotId, string provinceCode, string plateNumber, CancellationToken ct = default)
    {
        return await _context.ParkingSessions
            .Include(s => s.ParkingLot)
            .Include(s => s.VehicleType)
            .FirstOrDefaultAsync(s =>
                s.ParkingLotId == parkingLotId &&
                s.Status == ParkingStatus.Active &&
                s.LicensePlate.ProvinceCode == provinceCode &&
                s.LicensePlate.PlateNumber == plateNumber, ct);
    }

    public async Task<List<ParkingSession>> GetSessionsInTimeRangeAsync(Guid parkingLotId, DateTime fromDate, DateTime toDate, CancellationToken ct = default)
    {
        return await _context.ParkingSessions
            .AsNoTracking()
            .Include(s => s.VehicleType)
            .Where(s => s.ParkingLotId == parkingLotId &&
                        ((s.CheckInTime >= fromDate && s.CheckInTime <= toDate) ||
                         (s.CheckOutTime.HasValue && s.CheckOutTime.Value >= fromDate && s.CheckOutTime.Value <= toDate)))
            .ToListAsync(ct);
    }

    public async Task<int> CountActiveSessionsInLotAsync(Guid parkingLotId, CancellationToken ct = default)
    {
        return await _context.ParkingSessions
            .AsNoTracking()
            .CountAsync(s => s.ParkingLotId == parkingLotId && s.Status == ParkingStatus.Active, ct);
    }

    public async Task<IEnumerable<ParkingSession>> GetAllAsync(Guid? parkingLotId = null, ParkingStatus? status = null, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var query = _context.ParkingSessions
            .AsNoTracking()
            .Include(s => s.ParkingLot)
            .Include(s => s.VehicleType)
            .AsQueryable();

        if (parkingLotId.HasValue)
            query = query.Where(s => s.ParkingLotId == parkingLotId.Value);

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        return await query
            .OrderByDescending(s => s.CheckInTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(Guid? parkingLotId = null, ParkingStatus? status = null, CancellationToken ct = default)
    {
        var query = _context.ParkingSessions.AsNoTracking();

        if (parkingLotId.HasValue)
            query = query.Where(s => s.ParkingLotId == parkingLotId.Value);

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(ParkingSession session, CancellationToken ct = default)
    {
        await _context.ParkingSessions.AddAsync(session, ct);
    }

    public Task UpdateAsync(ParkingSession session, CancellationToken ct = default)
    {
        _context.ParkingSessions.Update(session);
        return Task.CompletedTask;
    }
}
