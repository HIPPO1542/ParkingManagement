using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Services;

public interface IBusinessOperatorService
{
    Task<IEnumerable<BusinessOperatorDto>> GetAllAsync();
    Task<BusinessOperatorDto> GetByIdAsync(Guid id);
    Task<BusinessOperatorDto> CreateAsync(CreateBusinessOperatorDto dto);
    Task<BusinessOperatorDto> UpdateAsync(Guid id, UpdateBusinessOperatorDto dto);
    Task DeleteAsync(Guid id);
}
