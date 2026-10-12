using MediatR;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.Tariffs.Commands;

public record DeleteTariffCommand(Guid Id) : IRequest<bool>;

public class DeleteTariffCommandHandler : IRequestHandler<DeleteTariffCommand, bool>
{
    private readonly ITariffRepository _tariffRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTariffCommandHandler(
        ITariffRepository tariffRepository,
        IUnitOfWork unitOfWork)
    {
        _tariffRepository = tariffRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTariffCommand request, CancellationToken cancellationToken)
    {
        var entity = await _tariffRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new EntityNotFoundException("Tariff", request.Id);

        await _tariffRepository.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
