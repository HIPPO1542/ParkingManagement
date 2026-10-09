using ParkingManagement.Domain.Common;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Domain.Entities;

public class BusinessOperator : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid SystemManagerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Representative { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public ContactInfo ContactInfo { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    // Navigation property
    public List<ParkingLot> ParkingLots { get; private set; } = new();

    // EF Core constructor
    protected BusinessOperator() { }

    public BusinessOperator(
        Guid id,
        Guid systemManagerId,
        string name,
        string representative,
        Address address,
        ContactInfo contactInfo,
        bool isActive = true,
        string username = "",
        string passwordHash = "")
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        if (systemManagerId == Guid.Empty)
            throw new ArgumentException("System manager ID cannot be empty.", nameof(systemManagerId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Business operator name cannot be empty.", nameof(name));

        Id = id;
        SystemManagerId = systemManagerId;
        Name = name.Trim();
        Representative = representative?.Trim() ?? string.Empty;
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        IsActive = isActive;
        Username = username?.Trim() ?? string.Empty;
        PasswordHash = passwordHash ?? string.Empty;
    }

    public void SetupAccount(string username, string passwordHash)
    {
        Username = username?.Trim() ?? string.Empty;
        PasswordHash = passwordHash ?? string.Empty;
    }

    public void AssignManager(Guid newManagerId)
    {
        if (newManagerId == Guid.Empty)
            throw new ArgumentException("New manager ID is invalid.", nameof(newManagerId));

        SystemManagerId = newManagerId;
    }

    public void UpdateInfo(
        string name,
        string representative,
        Address address,
        ContactInfo contactInfo,
        bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Business operator name cannot be empty.", nameof(name));

        Name = name.Trim();
        Representative = representative?.Trim() ?? string.Empty;
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        IsActive = isActive;
    }
}
