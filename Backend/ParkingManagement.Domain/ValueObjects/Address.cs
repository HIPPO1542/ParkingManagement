namespace ParkingManagement.Domain.ValueObjects;

public class Address : IEquatable<Address>
{
    public string Street { get; private set; } = string.Empty;
    public string Ward { get; private set; } = string.Empty;
    public string District { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;

    // EF Core parameterless constructor
    protected Address() { }

    public Address(string street, string ward, string district, string city)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));

        Street = street.Trim();
        Ward = ward?.Trim() ?? string.Empty;
        District = district?.Trim() ?? string.Empty;
        City = city.Trim();
    }

    public override string ToString()
    {
        var parts = new[] { Street, Ward, District, City }
            .Where(s => !string.IsNullOrWhiteSpace(s));
        return string.Join(", ", parts);
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Street == other.Street &&
               Ward == other.Ward &&
               District == other.District &&
               City == other.City;
    }

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode() =>
        HashCode.Combine(Street, Ward, District, City);

    public static bool operator ==(Address? left, Address? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Address? left, Address? right) => !(left == right);
}
