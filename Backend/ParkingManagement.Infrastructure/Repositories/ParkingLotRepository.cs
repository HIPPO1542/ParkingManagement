using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class ParkingLotRepository : IParkingLotRepository
{
    private readonly ParkingDbContext _context;

    public ParkingLotRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public async Task<ParkingLot?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.ParkingLots
            .Include(p => p.BusinessOperator)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<ParkingLot>> GetAllAsync(Guid? operatorId = null, bool? isActive = null, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var query = _context.ParkingLots.AsNoTracking();

        if (operatorId.HasValue)
            query = query.Where(p => p.BusinessOperatorId == operatorId.Value);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        return await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(Guid? operatorId = null, bool? isActive = null, CancellationToken ct = default)
    {
        var query = _context.ParkingLots.AsNoTracking();

        if (operatorId.HasValue)
            query = query.Where(p => p.BusinessOperatorId == operatorId.Value);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        return await query.CountAsync(ct);
    }

    public async Task<int> CheckActiveParkingVehiclesAsync(Guid parkingLotId, CancellationToken ct = default)
    {
        return await _context.ParkingSessions
            .AsNoTracking()
            .CountAsync(s => s.ParkingLotId == parkingLotId && s.Status == ParkingStatus.Active, ct);
    }

    public async Task AddAsync(ParkingLot parkingLot, CancellationToken ct = default)
    {
        await _context.ParkingLots.AddAsync(parkingLot, ct);
    }

    public Task UpdateAsync(ParkingLot parkingLot, CancellationToken ct = default)
    {
        _context.ParkingLots.Update(parkingLot);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ParkingLot parkingLot, CancellationToken ct = default)
    {
        _context.ParkingLots.Remove(parkingLot);
        return Task.CompletedTask;
    }
}
