namespace ParkingManagement.Domain.ValueObjects;

public class ContactInfo : IEquatable<ContactInfo>
{
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    // EF Core parameterless constructor
    protected ContactInfo() { }

    public ContactInfo(string phoneNumber, string email)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));

        PhoneNumber = phoneNumber.Trim();
        Email = email?.Trim() ?? string.Empty;
    }

    public override string ToString() => $"{PhoneNumber} ({Email})";

    public bool Equals(ContactInfo? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return PhoneNumber == other.PhoneNumber &&
               string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as ContactInfo);

    public override int GetHashCode() =>
        HashCode.Combine(PhoneNumber, StringComparer.OrdinalIgnoreCase.GetHashCode(Email));

    public static bool operator ==(ContactInfo? left, ContactInfo? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ContactInfo? left, ContactInfo? right) => !(left == right);
}
