using ParkingManagement.Domain.Common;

namespace ParkingManagement.Domain.Entities;

public class VehicleType : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    // EF Core constructor
    protected VehicleType() { }

    public VehicleType(Guid id, string name, string description = "")
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Vehicle type name cannot be empty.", nameof(name));

        Id = id;
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Vehicle type name cannot be empty.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }
}
