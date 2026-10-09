namespace ParkingManagement.Application.DTOs;

public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Manager" or "BusinessOperator"
    public string DisplayRole { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? BusinessOperatorId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UserProfileDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string DisplayRole { get; set; } = string.Empty;
    public Guid? BusinessOperatorId { get; set; }
    public string? OperatorName { get; set; }
}
