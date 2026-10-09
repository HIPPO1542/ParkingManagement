using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Services;

public interface IReportService
{
    Task<ParkingActivityReportDto?> GetActivityReportAsync(Guid parkingLotId, DateTime fromDate, DateTime toDate);
}

public class ReportService : IReportService
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;

    public ReportService(
        IParkingSessionRepository sessionRepository,
        IParkingLotRepository parkingLotRepository)
    {
        _sessionRepository = sessionRepository;
        _parkingLotRepository = parkingLotRepository;
    }

    public async Task<ParkingActivityReportDto?> GetActivityReportAsync(Guid parkingLotId, DateTime fromDate, DateTime toDate)
    {
        var fromUtc = fromDate.Kind == DateTimeKind.Local ? fromDate.ToUniversalTime() : fromDate;
        var toUtc = toDate.Kind == DateTimeKind.Local ? toDate.ToUniversalTime() : toDate;

        var futureThreshold = DateTime.UtcNow.Date.AddDays(1).AddHours(14);
        if (fromDate > toDate || fromUtc > futureThreshold || toUtc > futureThreshold)
        {
            throw new BusinessRuleViolationException("Invalid date range!");
        }

        var lot = await _parkingLotRepository.GetByIdAsync(parkingLotId);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", parkingLotId);

        var sessions = await _sessionRepository.GetSessionsInTimeRangeAsync(parkingLotId, fromDate, toDate);
        if (sessions == null || sessions.Count == 0)
        {
            return null;
        }

        int totalEntries = sessions.Count;
        int totalExits = sessions.Count(x => x.Status == ParkingStatus.Completed);
        int currentlyParked = await _sessionRepository.CountActiveSessionsInLotAsync(parkingLotId);
        decimal totalRevenue = sessions
            .Where(x => x.Status == ParkingStatus.Completed)
            .Sum(x => x.Fee.Amount);

        return new ParkingActivityReportDto
        {
            ParkingLotId = lot.Id,
            ParkingLotName = lot.Name,
            FromDate = fromDate,
            ToDate = toDate,
            TotalEntries = totalEntries,
            TotalExits = totalExits,
            CurrentlyParked = currentlyParked,
            AvailableSpots = lot.AvailableSpots,
            TotalCapacity = lot.TotalCapacity,
            TotalRevenue = totalRevenue,
            Currency = "VND",
            IsActive = lot.IsActive
        };
    }
}
