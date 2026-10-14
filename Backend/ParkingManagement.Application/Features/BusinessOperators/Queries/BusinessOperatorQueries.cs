using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.BusinessOperators.Queries;

public record GetAllBusinessOperatorsQuery : IRequest<IEnumerable<BusinessOperatorDto>>;
public record GetBusinessOperatorByIdQuery(Guid Id) : IRequest<BusinessOperatorDto>;

public class GetBusinessOperatorsQueryHandler :
    IRequestHandler<GetAllBusinessOperatorsQuery, IEnumerable<BusinessOperatorDto>>,
    IRequestHandler<GetBusinessOperatorByIdQuery, BusinessOperatorDto>
{
    private readonly IBusinessOperatorRepository _operatorRepository;

    public GetBusinessOperatorsQueryHandler(IBusinessOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<IEnumerable<BusinessOperatorDto>> Handle(GetAllBusinessOperatorsQuery request, CancellationToken cancellationToken)
    {
        var items = await _operatorRepository.GetAllAsync(cancellationToken);
        return items.Select(MapToDto);
    }

    public async Task<BusinessOperatorDto> Handle(GetBusinessOperatorByIdQuery request, CancellationToken cancellationToken)
    {
        var op = await _operatorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (op == null)
            throw new EntityNotFoundException("Business operator", request.Id);

        return MapToDto(op);
    }

    private static BusinessOperatorDto MapToDto(BusinessOperator op) => new()
    {
        Id = op.Id,
        SystemManagerId = op.SystemManagerId,
        Name = op.Name,
        Representative = op.Representative,
        Address = new AddressDto
        {
            Street = op.Address.Street,
            Ward = op.Address.Ward,
            District = op.Address.District,
            City = op.Address.City
        },
        FullAddress = op.Address.ToString(),
        ContactInfo = new ContactInfoDto
        {
            PhoneNumber = op.ContactInfo.PhoneNumber,
            Email = op.ContactInfo.Email
        },
        IsActive = op.IsActive,
        Username = op.Username,
        ParkingLots = op.ParkingLots?.Select(p => new ParkingLotDto
        {
            Id = p.Id,
            BusinessOperatorId = p.BusinessOperatorId,
            Name = p.Name,
            FullAddress = p.Address?.ToString() ?? "",
            TotalCapacity = p.TotalCapacity,
            AvailableSpots = p.AvailableSpots,
            OpenTime = p.OpenTime,
            CloseTime = p.CloseTime,
            IsActive = p.IsActive
        }).ToList() ?? new()
    };
}
