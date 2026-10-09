using ParkingManagement.Domain.Common;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Domain.Entities;

public class ParkingLot : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid BusinessOperatorId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public int TotalCapacity { get; private set; }
    public int AvailableSpots { get; private set; }
    public string OpenTime { get; private set; } = "00:00";
    public string CloseTime { get; private set; } = "23:59";
    public bool IsActive { get; private set; }

    // Navigation property
    public BusinessOperator BusinessOperator { get; private set; } = null!;
    public List<Tariff> Tariffs { get; private set; } = new();

    // EF Core constructor
    protected ParkingLot() { }

    public ParkingLot(
        Guid id,
        Guid businessOperatorId,
        string name,
        Address address,
        int totalCapacity,
        string openTime = "06:00",
        string closeTime = "22:00",
        bool isActive = true)
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        if (businessOperatorId == Guid.Empty)
            throw new ArgumentException("Business operator ID cannot be empty.", nameof(businessOperatorId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Parking lot name cannot be empty.", nameof(name));
        if (totalCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalCapacity), "Total capacity must be greater than 0.");

        Id = id;
        BusinessOperatorId = businessOperatorId;
        Name = name.Trim();
        Address = address ?? throw new ArgumentNullException(nameof(address));
        TotalCapacity = totalCapacity;
        AvailableSpots = totalCapacity;
        OpenTime = string.IsNullOrWhiteSpace(openTime) ? "06:00" : openTime.Trim();
        CloseTime = string.IsNullOrWhiteSpace(closeTime) ? "22:00" : closeTime.Trim();
        IsActive = isActive;
    }

    public int OccupiedSpots => TotalCapacity - AvailableSpots;

    public void RecordVehicleEntry()
    {
        if (!IsActive)
            throw new BusinessRuleViolationException("Parking lot is currently inactive!");

        if (AvailableSpots <= 0)
            throw new BusinessRuleViolationException("Parking lot is full!");

        AvailableSpots--;
    }

    public void RecordVehicleExit()
    {
        if (AvailableSpots < TotalCapacity)
        {
            AvailableSpots++;
        }
    }

    public void UpdateInfo(
        string name,
        Address address,
        int totalCapacity,
        string openTime,
        string closeTime,
        bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Parking lot name cannot be empty.", nameof(name));
        if (totalCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalCapacity), "Total capacity must be greater than 0.");

        int delta = totalCapacity - TotalCapacity;
        TotalCapacity = totalCapacity;
        AvailableSpots = Math.Clamp(AvailableSpots + delta, 0, TotalCapacity);

        Name = name.Trim();
        Address = address ?? throw new ArgumentNullException(nameof(address));
        OpenTime = string.IsNullOrWhiteSpace(openTime) ? "06:00" : openTime.Trim();
        CloseTime = string.IsNullOrWhiteSpace(closeTime) ? "22:00" : closeTime.Trim();
        IsActive = isActive;
    }

    public void SetStatus(bool isActive)
    {
        IsActive = isActive;
    }
}
