using ParkingManagement.Domain.Common;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Domain.Entities;

public class SystemManager : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public ContactInfo ContactInfo { get; private set; } = null!;
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; } = UserRole.Manager;
    public bool IsActive { get; private set; }

    // EF Core constructor
    protected SystemManager() { }

    public SystemManager(
        Guid id,
        string fullName,
        string position,
        ContactInfo contactInfo,
        string username,
        string passwordHash,
        UserRole role = UserRole.Manager,
        bool isActive = true)
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.", nameof(username));

        Id = id;
        FullName = fullName.Trim();
        Position = position?.Trim() ?? string.Empty;
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        Username = username.Trim();
        PasswordHash = passwordHash ?? string.Empty;
        Role = role;
        IsActive = isActive;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;

    public void UpdateProfile(string fullName, string position, ContactInfo contactInfo)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

        FullName = fullName.Trim();
        Position = position?.Trim() ?? string.Empty;
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
    }
}
