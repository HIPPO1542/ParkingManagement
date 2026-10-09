using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class VehicleTypeRepository : IVehicleTypeRepository
{
    private readonly ParkingDbContext _context;

    public VehicleTypeRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleType?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Id == id, ct);
    }

    public async Task<IEnumerable<VehicleType>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.VehicleTypes
            .AsNoTracking()
            .OrderBy(v => v.Name)
            .ToListAsync(ct);
    }

    public async Task AddAsync(VehicleType vehicleType, CancellationToken ct = default)
    {
        await _context.VehicleTypes.AddAsync(vehicleType, ct);
    }

    public Task UpdateAsync(VehicleType vehicleType, CancellationToken ct = default)
    {
        _context.VehicleTypes.Update(vehicleType);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(VehicleType vehicleType, CancellationToken ct = default)
    {
        _context.VehicleTypes.Remove(vehicleType);
        return Task.CompletedTask;
    }
}
