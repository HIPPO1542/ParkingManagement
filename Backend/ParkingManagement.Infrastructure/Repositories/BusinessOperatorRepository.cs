using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class BusinessOperatorRepository : IBusinessOperatorRepository
{
    private readonly ParkingDbContext _context;

    public BusinessOperatorRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessOperator?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.BusinessOperators
            .Include(o => o.ParkingLots)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task<BusinessOperator?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.BusinessOperators
            .FirstOrDefaultAsync(o => o.Username == username, ct);
    }

    public async Task<IEnumerable<BusinessOperator>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.BusinessOperators
            .Include(o => o.ParkingLots)
            .AsNoTracking()
            .OrderBy(o => o.Name)
            .ToListAsync(ct);
    }

    public async Task<int> CheckActiveParkingLotsAsync(Guid operatorId, CancellationToken ct = default)
    {
        return await _context.ParkingLots
            .AsNoTracking()
            .CountAsync(p => p.BusinessOperatorId == operatorId && p.IsActive, ct);
    }

    public async Task AddAsync(BusinessOperator businessOperator, CancellationToken ct = default)
    {
        await _context.BusinessOperators.AddAsync(businessOperator, ct);
    }

    public Task UpdateAsync(BusinessOperator businessOperator, CancellationToken ct = default)
    {
        _context.BusinessOperators.Update(businessOperator);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(BusinessOperator businessOperator, CancellationToken ct = default)
    {
        _context.BusinessOperators.Remove(businessOperator);
        return Task.CompletedTask;
    }
}
