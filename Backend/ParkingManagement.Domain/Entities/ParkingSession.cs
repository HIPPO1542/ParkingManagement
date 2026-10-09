using ParkingManagement.Domain.Common;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Domain.Entities;

public class ParkingSession : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid ParkingLotId { get; private set; }
    public Guid VehicleTypeId { get; private set; }
    public LicensePlate LicensePlate { get; private set; } = null!;
    public DateTime CheckInTime { get; private set; }
    public DateTime? CheckOutTime { get; private set; }
    public Money Fee { get; private set; } = null!;
    public ParkingStatus Status { get; private set; } = ParkingStatus.Active;

    // Navigation properties
    public ParkingLot ParkingLot { get; private set; } = null!;
    public VehicleType VehicleType { get; private set; } = null!;

    // EF Core constructor
    protected ParkingSession() { }

    public ParkingSession(
        Guid id,
        Guid parkingLotId,
        Guid vehicleTypeId,
        LicensePlate licensePlate,
        DateTime? checkInTime = null)
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        if (parkingLotId == Guid.Empty)
            throw new ArgumentException("Parking lot ID cannot be empty.", nameof(parkingLotId));
        if (vehicleTypeId == Guid.Empty)
            throw new ArgumentException("Vehicle type ID cannot be empty.", nameof(vehicleTypeId));

        Id = id;
        ParkingLotId = parkingLotId;
        VehicleTypeId = vehicleTypeId;
        LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
        CheckInTime = checkInTime ?? DateTime.UtcNow;
        Status = ParkingStatus.Active;
        Fee = Money.Zero();
    }

    public void CheckIn()
    {
        Status = ParkingStatus.Active;
        CheckInTime = DateTime.UtcNow;
        Fee = Money.Zero();
    }

    public void CheckOut(Money fee)
    {
        if (Status == ParkingStatus.Completed)
            throw new BusinessRuleViolationException("This parking session has already been checked out!");

        Fee = fee ?? throw new ArgumentNullException(nameof(fee));
        CheckOutTime = DateTime.UtcNow;
        Status = ParkingStatus.Completed;
    }
}
