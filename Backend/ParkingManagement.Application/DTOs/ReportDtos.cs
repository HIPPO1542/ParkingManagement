namespace ParkingManagement.Application.DTOs;

public class ParkingActivityReportDto
{
    public Guid ParkingLotId { get; set; }
    public string ParkingLotName { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalEntries { get; set; }
    public int TotalExits { get; set; }
    public int CurrentlyParked { get; set; }
    public int AvailableSpots { get; set; }
    public int TotalCapacity { get; set; }
    public decimal TotalRevenue { get; set; }
    public string Currency { get; set; } = "VND";
    public bool IsActive { get; set; }
    public string StatusDescription => IsActive ? "Active" : "Inactive";
}
