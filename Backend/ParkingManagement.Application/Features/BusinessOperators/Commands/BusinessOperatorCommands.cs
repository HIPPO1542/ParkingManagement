using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Features.BusinessOperators.Commands;

public record CreateBusinessOperatorCommand(CreateBusinessOperatorDto Dto) : IRequest<BusinessOperatorDto>;
public record UpdateBusinessOperatorCommand(Guid Id, UpdateBusinessOperatorDto Dto) : IRequest<BusinessOperatorDto>;
public record DeleteBusinessOperatorCommand(Guid Id) : IRequest<bool>;

public class CreateBusinessOperatorCommandHandler : IRequestHandler<CreateBusinessOperatorCommand, BusinessOperatorDto>
{
    private readonly IBusinessOperatorRepository _operatorRepository;
    private readonly ISystemManagerRepository _managerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBusinessOperatorCommandHandler(
        IBusinessOperatorRepository operatorRepository,
        ISystemManagerRepository managerRepository,
        IUnitOfWork unitOfWork)
    {
        _operatorRepository = operatorRepository;
        _managerRepository = managerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BusinessOperatorDto> Handle(CreateBusinessOperatorCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var manager = await _managerRepository.GetByIdAsync(dto.SystemManagerId, cancellationToken);
        if (manager == null)
        {
            var allManagers = await _managerRepository.GetAllAsync(cancellationToken);
            var first = allManagers.FirstOrDefault();
            if (first != null) dto.SystemManagerId = first.Id;
        }

        var address = new Address(
            dto.Address.Street,
            dto.Address.Ward,
            dto.Address.District,
            dto.Address.City
        );

        var contact = new ContactInfo(
            dto.ContactInfo.PhoneNumber,
            dto.ContactInfo.Email
        );

        string username = !string.IsNullOrWhiteSpace(dto.Username)
            ? dto.Username.Trim()
            : (!string.IsNullOrWhiteSpace(dto.ContactInfo?.PhoneNumber) ? dto.ContactInfo.PhoneNumber.Trim() : $"op_{Guid.NewGuid():N}"[..10]);

        string password = !string.IsNullOrWhiteSpace(dto.Password)
            ? dto.Password.Trim()
            : "123@456";

        var op = new BusinessOperator(
            Guid.NewGuid(),
            dto.SystemManagerId,
            dto.Name,
            dto.Representative,
            address,
            contact,
            dto.IsActive,
            username,
            password
        );

        await _operatorRepository.AddAsync(op, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
        Username = op.Username
    };
}

public class UpdateBusinessOperatorCommandHandler : IRequestHandler<UpdateBusinessOperatorCommand, BusinessOperatorDto>
{
    private readonly IBusinessOperatorRepository _operatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBusinessOperatorCommandHandler(
        IBusinessOperatorRepository operatorRepository,
        IUnitOfWork unitOfWork)
    {
        _operatorRepository = operatorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BusinessOperatorDto> Handle(UpdateBusinessOperatorCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var dto = request.Dto;

        var op = await _operatorRepository.GetByIdAsync(id, cancellationToken);
        if (op == null)
            throw new EntityNotFoundException("Business operator", id);

        var address = new Address(
            dto.Address.Street,
            dto.Address.Ward,
            dto.Address.District,
            dto.Address.City
        );

        var contact = new ContactInfo(
            dto.ContactInfo.PhoneNumber,
            dto.ContactInfo.Email
        );

        op.UpdateInfo(
            dto.Name,
            dto.Representative,
            address,
            contact,
            dto.IsActive
        );

        string updatedUsername = !string.IsNullOrWhiteSpace(dto.Username) ? dto.Username.Trim() : op.Username;
        string updatedPassword = !string.IsNullOrWhiteSpace(dto.Password) ? dto.Password.Trim() : op.PasswordHash;
        op.SetupAccount(updatedUsername, updatedPassword);

        await _operatorRepository.UpdateAsync(op, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BusinessOperatorDto
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
            Username = op.Username
        };
    }
}

public class DeleteBusinessOperatorCommandHandler : IRequestHandler<DeleteBusinessOperatorCommand, bool>
{
    private readonly IBusinessOperatorRepository _operatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBusinessOperatorCommandHandler(
        IBusinessOperatorRepository operatorRepository,
        IUnitOfWork unitOfWork)
    {
        _operatorRepository = operatorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteBusinessOperatorCommand request, CancellationToken cancellationToken)
    {
        var op = await _operatorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (op == null)
            throw new EntityNotFoundException("Business operator", request.Id);

        await _operatorRepository.DeleteAsync(op, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
