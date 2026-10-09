using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class SystemManagerRepository : ISystemManagerRepository
{
    private readonly ParkingDbContext _context;

    public SystemManagerRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public async Task<SystemManager?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.SystemManagers.FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<SystemManager?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.SystemManagers.FirstOrDefaultAsync(m => m.Username == username, ct);
    }

    public async Task<IEnumerable<SystemManager>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.SystemManagers
            .AsNoTracking()
            .OrderBy(m => m.FullName)
            .ToListAsync(ct);
    }

    public async Task AddAsync(SystemManager manager, CancellationToken ct = default)
    {
        await _context.SystemManagers.AddAsync(manager, ct);
    }

    public Task UpdateAsync(SystemManager manager, CancellationToken ct = default)
    {
        _context.SystemManagers.Update(manager);
        return Task.CompletedTask;
    }
}
