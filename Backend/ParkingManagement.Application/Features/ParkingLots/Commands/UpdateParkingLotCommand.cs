using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.ParkingLots.Commands;

public record UpdateParkingLotCommand(Guid Id, UpdateParkingLotDto Dto) : IRequest<ParkingLotDto>;

public class UpdateParkingLotCommandHandler : IRequestHandler<UpdateParkingLotCommand, ParkingLotDto>
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateParkingLotCommandHandler(
        IParkingLotRepository parkingLotRepository,
        IUnitOfWork unitOfWork)
    {
        _parkingLotRepository = parkingLotRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingLotDto> Handle(UpdateParkingLotCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var dto = request.Dto;

        var lot = await _parkingLotRepository.GetByIdAsync(id, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", id);

        var address = new Address(
            dto.Address.Street,
            dto.Address.Ward,
            dto.Address.District,
            dto.Address.City
        );

        lot.UpdateInfo(
            dto.Name,
            address,
            dto.TotalCapacity,
            dto.OpenTime,
            dto.CloseTime,
            dto.IsActive
        );

        await _parkingLotRepository.UpdateAsync(lot, cancellationToken);
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
