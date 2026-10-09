namespace ParkingManagement.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "VND";

    // EF Core parameterless constructor
    protected Money() { }

    public Money(decimal amount, string currency = "VND")
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

        Amount = amount;
        Currency = string.IsNullOrWhiteSpace(currency) ? "VND" : currency.Trim().ToUpperInvariant();
    }

    public static Money Zero(string currency = "VND") => new Money(0, currency);

    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Cannot add money of different currencies: '{Currency}' and '{other.Currency}'.");

        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Amount:N0} {Currency}";

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount &&
               string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as Money);

    public override int GetHashCode() =>
        HashCode.Combine(Amount, StringComparer.OrdinalIgnoreCase.GetHashCode(Currency));

    public static bool operator ==(Money? left, Money? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Money? left, Money? right) => !(left == right);
}
