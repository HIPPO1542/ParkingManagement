using System.ComponentModel.DataAnnotations;

namespace ParkingManagement.Application.DTOs;

public class BusinessOperatorDto
{
    public Guid Id { get; set; }
    public Guid SystemManagerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Representative { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = null!;
    public string FullAddress { get; set; } = string.Empty;
    public ContactInfoDto ContactInfo { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<ParkingLotDto> ParkingLots { get; set; } = new();
}

public class CreateBusinessOperatorDto
{
    [Required(ErrorMessage = "System manager ID is required")]
    public Guid SystemManagerId { get; set; }

    [Required(ErrorMessage = "Business operator name is required")]
    [MaxLength(200, ErrorMessage = "Business operator name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150, ErrorMessage = "Representative name cannot exceed 150 characters")]
    public string Representative { get; set; } = string.Empty;

    [Required(ErrorMessage = "Business address is required")]
    public AddressDto Address { get; set; } = null!;

    [Required(ErrorMessage = "Contact information is required")]
    public ContactInfoDto ContactInfo { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class UpdateBusinessOperatorDto : CreateBusinessOperatorDto { }
