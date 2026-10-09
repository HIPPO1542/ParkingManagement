using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;
using ParkingManagement.Domain.ValueObjects;

namespace ParkingManagement.Application.Services;

public class BusinessOperatorService : IBusinessOperatorService
{
    private readonly IBusinessOperatorRepository _operatorRepository;
    private readonly ISystemManagerRepository _managerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BusinessOperatorService(
        IBusinessOperatorRepository operatorRepository,
        ISystemManagerRepository managerRepository,
        IUnitOfWork unitOfWork)
    {
        _operatorRepository = operatorRepository;
        _managerRepository = managerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BusinessOperatorDto>> GetAllAsync()
    {
        var items = await _operatorRepository.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<BusinessOperatorDto> GetByIdAsync(Guid id)
    {
        var op = await _operatorRepository.GetByIdAsync(id);
        if (op == null)
            throw new EntityNotFoundException("Business operator", id);

        return MapToDto(op);
    }

    public async Task<BusinessOperatorDto> CreateAsync(CreateBusinessOperatorDto dto)
    {
        var manager = await _managerRepository.GetByIdAsync(dto.SystemManagerId);
        if (manager == null)
        {
            var allManagers = await _managerRepository.GetAllAsync();
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

        // Username defaults to Phone Number if not provided, password defaults to 123@456
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

        await _operatorRepository.AddAsync(op);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(op);
    }

    public async Task<BusinessOperatorDto> UpdateAsync(Guid id, UpdateBusinessOperatorDto dto)
    {
        var op = await _operatorRepository.GetByIdAsync(id);
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

        await _operatorRepository.UpdateAsync(op);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(op);
    }

    public async Task DeleteAsync(Guid id)
    {
        var op = await _operatorRepository.GetByIdAsync(id);
        if (op == null)
            throw new EntityNotFoundException("Business operator", id);

        int activeLots = await _operatorRepository.CheckActiveParkingLotsAsync(id);
        if (activeLots > 0)
            throw new BusinessRuleViolationException($"Cannot delete business operator while {activeLots} parking lot(s) are active!");

        await _operatorRepository.DeleteAsync(op);
        await _unitOfWork.SaveChangesAsync();
    }

    private static BusinessOperatorDto MapToDto(BusinessOperator op)
    {
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
            Username = op.Username,
            ParkingLots = op.ParkingLots?.Select(p => new ParkingLotDto
            {
                Id = p.Id,
                BusinessOperatorId = p.BusinessOperatorId,
                Name = p.Name,
                TotalCapacity = p.TotalCapacity,
                AvailableSpots = p.AvailableSpots,
                OpenTime = p.OpenTime,
                CloseTime = p.CloseTime,
                IsActive = p.IsActive,
                FullAddress = p.Address.ToString()
            }).ToList() ?? new List<ParkingLotDto>()
        };
    }
}
