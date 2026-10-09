using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username, string role, Guid? businessOperatorId, string fullName);
}

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<UserProfileDto> GetProfileAsync(Guid userId, string role);
}
