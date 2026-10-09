using ParkingManagement.Application.Common;
using ParkingManagement.Application.DTOs;

namespace ParkingManagement.Application.Services;

public interface IParkingLotService
{
    Task<PagedResult<ParkingLotDto>> GetPagedAsync(Guid? operatorId = null, bool? isActive = null, int page = 1, int pageSize = 20);
    Task<ParkingLotDto> GetByIdAsync(Guid id);
    Task<ParkingLotDto> CreateAsync(CreateParkingLotDto dto);
    Task<ParkingLotDto> UpdateAsync(Guid id, UpdateParkingLotDto dto);
    Task DeleteAsync(Guid id);
}
