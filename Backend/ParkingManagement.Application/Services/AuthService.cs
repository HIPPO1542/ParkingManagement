using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly ISystemManagerRepository _systemManagerRepository;
    private readonly IBusinessOperatorRepository _businessOperatorRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        ISystemManagerRepository systemManagerRepository,
        IBusinessOperatorRepository businessOperatorRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _systemManagerRepository = systemManagerRepository;
        _businessOperatorRepository = businessOperatorRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        string username = dto.Username.Trim();
        string password = dto.Password.Trim();

        // 1. Check System Manager
        var manager = await _systemManagerRepository.GetByUsernameAsync(username);
        if (manager != null && manager.PasswordHash == password)
        {
            if (!manager.IsActive)
                throw new BusinessRuleViolationException("This manager account is currently locked!");

            string token = _jwtTokenGenerator.GenerateToken(
                manager.Id,
                manager.Username,
                "Manager",
                null,
                manager.FullName
            );

            return new LoginResponseDto
            {
                Token = token,
                Role = "Manager",
                DisplayRole = "System Manager",
                UserId = manager.Id,
                BusinessOperatorId = null,
                Username = manager.Username,
                FullName = manager.FullName,
                Email = manager.ContactInfo.Email
            };
        }

        // 2. Check Business Operator
        var op = await _businessOperatorRepository.GetByUsernameAsync(username);
        if (op != null && op.PasswordHash == password)
        {
            if (!op.IsActive)
                throw new BusinessRuleViolationException("This business operator account is currently inactive!");

            string token = _jwtTokenGenerator.GenerateToken(
                op.Id,
                op.Username,
                "BusinessOperator",
                op.Id,
                op.Name
            );

            return new LoginResponseDto
            {
                Token = token,
                Role = "BusinessOperator",
                DisplayRole = "Business Operator",
                UserId = op.Id,
                BusinessOperatorId = op.Id,
                Username = op.Username,
                FullName = op.Name,
                Email = op.ContactInfo.Email
            };
        }

        throw new BusinessRuleViolationException("Invalid username or password!");
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId, string role)
    {
        if (role == "Manager")
        {
            var m = await _systemManagerRepository.GetByIdAsync(userId);
            if (m == null) throw new EntityNotFoundException("System manager", userId);

            return new UserProfileDto
            {
                UserId = m.Id,
                Username = m.Username,
                FullName = m.FullName,
                Role = "Manager",
                DisplayRole = "System Manager",
                BusinessOperatorId = null,
                OperatorName = null
            };
        }
        else
        {
            var op = await _businessOperatorRepository.GetByIdAsync(userId);
            if (op == null) throw new EntityNotFoundException("Business operator", userId);

            return new UserProfileDto
            {
                UserId = op.Id,
                Username = op.Username,
                FullName = op.Representative,
                Role = "BusinessOperator",
                DisplayRole = "Business Operator",
                BusinessOperatorId = op.Id,
                OperatorName = op.Name
            };
        }
    }
}
