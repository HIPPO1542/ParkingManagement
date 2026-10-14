using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.ParkingLots.Commands;

public record CreateParkingLotCommand(CreateParkingLotDto Dto) : IRequest<ParkingLotDto>;

public class CreateParkingLotCommandHandler : IRequestHandler<CreateParkingLotCommand, ParkingLotDto>
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IBusinessOperatorRepository _operatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateParkingLotCommandHandler(
        IParkingLotRepository parkingLotRepository,
        IBusinessOperatorRepository operatorRepository,
        IUnitOfWork unitOfWork)
    {
        _parkingLotRepository = parkingLotRepository;
        _operatorRepository = operatorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingLotDto> Handle(CreateParkingLotCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var op = await _operatorRepository.GetByIdAsync(dto.BusinessOperatorId, cancellationToken);
        if (op == null)
            throw new EntityNotFoundException("Business operator", dto.BusinessOperatorId);

        var existingLots = await _parkingLotRepository.GetAllAsync(operatorId: dto.BusinessOperatorId, ct: cancellationToken);
        if (existingLots.Any(l => string.Equals(l.Name, dto.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new BusinessRuleViolationException($"A parking lot with the name '{dto.Name}' already exists for this business operator.");

        var address = new Address(
            dto.Address.Street,
            dto.Address.Ward,
            dto.Address.District,
            dto.Address.City
        );

        var lot = new ParkingLot(
            Guid.NewGuid(),
            dto.BusinessOperatorId,
            dto.Name,
            address,
            dto.TotalCapacity,
            dto.OpenTime,
            dto.CloseTime,
            dto.IsActive
        );

        await _parkingLotRepository.AddAsync(lot, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ParkingLotDto
        {
            Id = lot.Id,
            BusinessOperatorId = lot.BusinessOperatorId,
            Name = lot.Name,
            Address = new AddressDto
            {
                Street = lot.Address.Street,
                Ward = lot.Address.Ward,
                District = lot.Address.District,
                City = lot.Address.City
            },
            FullAddress = lot.Address.ToString(),
            TotalCapacity = lot.TotalCapacity,
            AvailableSpots = lot.AvailableSpots,
            OpenTime = lot.OpenTime,
            CloseTime = lot.CloseTime,
            IsActive = lot.IsActive
        };
    }
}
