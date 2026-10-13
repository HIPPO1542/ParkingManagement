using MediatR;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.ParkingLots.Commands;

public record DeleteParkingLotCommand(Guid Id) : IRequest<bool>;

public class DeleteParkingLotCommandHandler : IRequestHandler<DeleteParkingLotCommand, bool>
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteParkingLotCommandHandler(
        IParkingLotRepository parkingLotRepository,
        IUnitOfWork unitOfWork)
    {
        _parkingLotRepository = parkingLotRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteParkingLotCommand request, CancellationToken cancellationToken)
    {
        var lot = await _parkingLotRepository.GetByIdAsync(request.Id, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", request.Id);

        int activeCars = await _parkingLotRepository.CheckActiveParkingVehiclesAsync(request.Id, cancellationToken);
        if (activeCars > 0)
            throw new BusinessRuleViolationException($"Cannot delete parking lot while {activeCars} vehicle(s) are still parked inside!");

        await _parkingLotRepository.DeleteAsync(lot, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
