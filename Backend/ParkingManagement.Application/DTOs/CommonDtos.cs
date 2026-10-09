using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Application.DTOs;

public class AddressDto
{
    [Required(ErrorMessage = "Street cannot be empty")]
    public string Street { get; set; } = string.Empty;

    public string Ward { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    [Required(ErrorMessage = "City cannot be empty")]
    public string City { get; set; } = string.Empty;

    public string FullAddress => $"{Street}, {Ward}, {District}, {City}".Replace(", ,", ",").Trim(',', ' ');
}

public class LicensePlateDto
{
    [Required(ErrorMessage = "Province code cannot be empty")]
    public string ProvinceCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plate number cannot be empty")]
    public string PlateNumber { get; set; } = string.Empty;

    public string FullPlateNumber => $"{ProvinceCode}-{PlateNumber}";
    public string FullPlate => FullPlateNumber;
}

public class MoneyDto
{
    [Range(0, double.MaxValue, ErrorMessage = "Amount cannot be negative")]
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "VND";
}

public class ContactInfoDto
{
    [Required(ErrorMessage = "Phone number cannot be empty")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
}
