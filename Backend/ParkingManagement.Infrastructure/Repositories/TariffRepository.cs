using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class TariffRepository : ITariffRepository
{
    private readonly ParkingDbContext _context;

    public TariffRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public async Task<Tariff?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Tariffs
            .Include(t => t.ParkingLot)
            .Include(t => t.VehicleType)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<Tariff?> GetCurrentTariffAsync(Guid parkingLotId, Guid vehicleTypeId, CancellationToken ct = default)
    {
        return await _context.Tariffs
            .Include(t => t.ParkingLot)
            .Include(t => t.VehicleType)
            .Where(t => t.ParkingLotId == parkingLotId && t.VehicleTypeId == vehicleTypeId)
            .OrderByDescending(t => t.EffectiveDate)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<Tariff>> GetByParkingLotIdAsync(Guid parkingLotId, CancellationToken ct = default)
    {
        return await _context.Tariffs
            .Include(t => t.ParkingLot)
            .Include(t => t.VehicleType)
            .AsNoTracking()
            .Where(t => t.ParkingLotId == parkingLotId)
            .OrderBy(t => t.VehicleType.Name)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Tariff>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Tariffs
            .Include(t => t.ParkingLot)
            .Include(t => t.VehicleType)
            .AsNoTracking()
            .OrderBy(t => t.ParkingLot.Name)
            .ThenBy(t => t.VehicleType.Name)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Tariff tariff, CancellationToken ct = default)
    {
        await _context.Tariffs.AddAsync(tariff, ct);
    }

    public Task UpdateAsync(Tariff tariff, CancellationToken ct = default)
    {
        _context.Tariffs.Update(tariff);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Tariff tariff, CancellationToken ct = default)
    {
        _context.Tariffs.Remove(tariff);
        return Task.CompletedTask;
    }
}
