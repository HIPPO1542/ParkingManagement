namespace ParkingManagement.Domain.ValueObjects;

public class LicensePlate : IEquatable<LicensePlate>
{
    public string ProvinceCode { get; private set; } = string.Empty;
    public string PlateNumber { get; private set; } = string.Empty;

    // EF Core parameterless constructor
    protected LicensePlate() { }

    public LicensePlate(string provinceCode, string plateNumber)
    {
        if (string.IsNullOrWhiteSpace(provinceCode))
            throw new ArgumentException("Province code cannot be empty.", nameof(provinceCode));
        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("Plate number cannot be empty.", nameof(plateNumber));

        ProvinceCode = provinceCode.Trim().ToUpperInvariant();
        PlateNumber = plateNumber.Trim().ToUpperInvariant();
    }

    public string FullPlateNumber => $"{ProvinceCode}-{PlateNumber}";

    public override string ToString() => FullPlateNumber;

    public bool Equals(LicensePlate? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(ProvinceCode, other.ProvinceCode, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(PlateNumber, other.PlateNumber, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as LicensePlate);

    public override int GetHashCode() =>
        HashCode.Combine(
            StringComparer.OrdinalIgnoreCase.GetHashCode(ProvinceCode),
            StringComparer.OrdinalIgnoreCase.GetHashCode(PlateNumber));

    public static bool operator ==(LicensePlate? left, LicensePlate? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(LicensePlate? left, LicensePlate? right) => !(left == right);
}
