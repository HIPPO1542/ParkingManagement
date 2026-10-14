using MediatR;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Repositories;

namespace ParkingManagement.Application.Features.Reports.Queries;

public record GetActivityReportQuery(Guid ParkingLotId, DateTime FromDate, DateTime ToDate) : IRequest<ParkingActivityReportDto?>;

public class GetActivityReportQueryHandler : IRequestHandler<GetActivityReportQuery, ParkingActivityReportDto?>
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IParkingLotRepository _parkingLotRepository;

    public GetActivityReportQueryHandler(
        IParkingSessionRepository sessionRepository,
        IParkingLotRepository parkingLotRepository)
    {
        _sessionRepository = sessionRepository;
        _parkingLotRepository = parkingLotRepository;
    }

    public async Task<ParkingActivityReportDto?> Handle(GetActivityReportQuery request, CancellationToken cancellationToken)
    {
        var fromDate = request.FromDate;
        var toDate = request.ToDate;
        var parkingLotId = request.ParkingLotId;

        var fromUtc = fromDate.Kind == DateTimeKind.Local ? fromDate.ToUniversalTime() : fromDate;
        var toUtc = toDate.Kind == DateTimeKind.Local ? toDate.ToUniversalTime() : toDate;

        var futureThreshold = DateTime.UtcNow.Date.AddDays(1).AddHours(14);
        if (fromDate > toDate || fromUtc > futureThreshold || toUtc > futureThreshold)
        {
            throw new BusinessRuleViolationException("Invalid date range!");
        }

        var lot = await _parkingLotRepository.GetByIdAsync(parkingLotId, cancellationToken);
        if (lot == null)
            throw new EntityNotFoundException("Parking lot", parkingLotId);

        var sessions = await _sessionRepository.GetSessionsInTimeRangeAsync(parkingLotId, fromDate, toDate, cancellationToken);
        if (sessions == null || sessions.Count == 0)
        {
            return null;
        }

        int totalEntries = sessions.Count;
        int totalExits = sessions.Count(x => x.Status == ParkingStatus.Completed);
        int currentlyParked = await _sessionRepository.CountActiveSessionsInLotAsync(parkingLotId, cancellationToken);
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
