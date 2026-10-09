using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.Data;

public class ParkingDbContext : DbContext
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
    {
    }

    public DbSet<SystemManager> SystemManagers => Set<SystemManager>();
    public DbSet<BusinessOperator> BusinessOperators => Set<BusinessOperator>();
    public DbSet<ParkingLot> ParkingLots => Set<ParkingLot>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<Tariff> Tariffs => Set<Tariff>();
    public DbSet<ParkingSession> ParkingSessions => Set<ParkingSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Case-insensitive comparison for SQLite text columns
        modelBuilder.UseCollation("NOCASE");

        // Automatically scan and apply all IEntityTypeConfiguration classes in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParkingDbContext).Assembly);
    }
}
