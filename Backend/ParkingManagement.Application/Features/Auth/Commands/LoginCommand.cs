using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequestDto Dto) : IRequest<LoginResponseDto>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly ISystemManagerRepository _systemManagerRepository;
    private readonly IBusinessOperatorRepository _businessOperatorRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        ISystemManagerRepository systemManagerRepository,
        IBusinessOperatorRepository businessOperatorRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _systemManagerRepository = systemManagerRepository;
        _businessOperatorRepository = businessOperatorRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        string username = dto.Username.Trim();
        string password = dto.Password.Trim();

        // 1. Check System Manager
        var manager = await _systemManagerRepository.GetByUsernameAsync(username, cancellationToken);
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
        var op = await _businessOperatorRepository.GetByUsernameAsync(username, cancellationToken);
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
}
