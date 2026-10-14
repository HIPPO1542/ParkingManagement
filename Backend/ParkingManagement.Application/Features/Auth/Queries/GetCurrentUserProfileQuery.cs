using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.Auth.Queries;

public record GetCurrentUserProfileQuery(Guid UserId, string Role) : IRequest<UserProfileDto>;

public class GetCurrentUserProfileQueryHandler : IRequestHandler<GetCurrentUserProfileQuery, UserProfileDto>
{
    private readonly ISystemManagerRepository _systemManagerRepository;
    private readonly IBusinessOperatorRepository _businessOperatorRepository;

    public GetCurrentUserProfileQueryHandler(
        ISystemManagerRepository systemManagerRepository,
        IBusinessOperatorRepository businessOperatorRepository)
    {
        _systemManagerRepository = systemManagerRepository;
        _businessOperatorRepository = businessOperatorRepository;
    }

    public async Task<UserProfileDto> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (request.Role == "Manager")
        {
            var m = await _systemManagerRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (m == null) throw new EntityNotFoundException("System manager", request.UserId);

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
            var op = await _businessOperatorRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (op == null) throw new EntityNotFoundException("Business operator", request.UserId);

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
