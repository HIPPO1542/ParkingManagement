using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.ValueObjects;
using Xunit;

namespace ParkingManagement.Tests;

/// <summary>
/// Unit Tests dựa trên kế hoạch kiểm thử Boundary Value Analysis (BVA) trong UnitTestPlan.md
/// Tập trung vào các giá trị biên: Min-1, Min, Min+1, Max-1, Max, Max+1
/// </summary>
public class BoundaryValueAnalysisTests
{
    private readonly Address _testAddress = new(
        "123 Nguyen Hue Street",
        "Ben Nghe Ward",
        "District 1",
        "Ho Chi Minh City"
    );

    #region Module 1: Sức chứa bãi xe (ParkingLot Capacity Boundary Tests)

    [Theory]
    [InlineData(-1)]  // Min - 1: Dưới biên nhỏ nhất (Số âm)
    [InlineData(-50)] // Số âm lớn
    public void ParkingLot_Capacity_NegativeValue_ThrowsArgumentOutOfRangeException(int negativeCapacity)
    {
        // TC_LOT_01: Sức chứa âm
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParkingLot(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Lot Negative",
                _testAddress,
                totalCapacity: negativeCapacity
            )
        );

        Assert.Equal("totalCapacity", ex.ParamName);
    }

    [Fact]
    public void ParkingLot_Capacity_Zero_ThrowsArgumentOutOfRangeException()
    {
        // TC_LOT_02: Sức chứa bằng 0 (Biên dưới không hợp lệ)
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParkingLot(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Lot Zero Capacity",
                _testAddress,
                totalCapacity: 0
            )
        );

        Assert.Equal("totalCapacity", ex.ParamName);
    }

    [Fact]
    public void ParkingLot_Capacity_MinimumValid_One_Success()
    {
        // TC_LOT_03: Sức chứa nhỏ nhất hợp lệ (Min = 1)
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Lot Min Capacity",
            _testAddress,
            totalCapacity: 1
        );

        Assert.Equal(1, lot.TotalCapacity);
        Assert.Equal(1, lot.AvailableSpots);
        Assert.Equal(0, lot.OccupiedSpots);
    }

    [Fact]
    public void ParkingLot_Capacity_VeryLargeValue_Success()
    {
        // TC_LOT_04: Sức chứa biên cực đại (Max = 50,000)
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Mega Lot",
            _testAddress,
            totalCapacity: 50000
        );

        Assert.Equal(50000, lot.TotalCapacity);
        Assert.Equal(50000, lot.AvailableSpots);
    }

    [Fact]
    public void ParkingLot_RecordVehicleEntry_WhenEmpty_OccupancyBecomesOne()
    {
        // TC_LOT_05: Cho xe vào khi bãi đang trống (Occupancy = 0)
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Lot Empty",
            _testAddress,
            totalCapacity: 100
        );

        lot.RecordVehicleEntry();

        Assert.Equal(99, lot.AvailableSpots);
        Assert.Equal(1, lot.OccupiedSpots);
    }

    [Fact]
    public void ParkingLot_RecordVehicleEntry_WhenOneSpotLeft_ReachesFullCapacity()
    {
        // TC_LOT_06: Cho xe vào khi bãi còn đúng 1 chỗ (Max - 1)
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Almost Full Lot",
            _testAddress,
            totalCapacity: 2
        );

        // Chiếm 1 chỗ
        lot.RecordVehicleEntry();
        Assert.Equal(1, lot.AvailableSpots);

        // Chiếm chỗ cuối cùng (Biên Max)
        lot.RecordVehicleEntry();
        Assert.Equal(0, lot.AvailableSpots);
        Assert.Equal(2, lot.OccupiedSpots);
    }

    [Fact]
    public void ParkingLot_RecordVehicleEntry_WhenCompletelyFull_ThrowsBusinessRuleViolationException()
    {
        // TC_LOT_07 & TC_LOT_08: Cho xe vào khi bãi đã đầy 100% (Max + 1)
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Full Lot",
            _testAddress,
            totalCapacity: 1
        );

        lot.RecordVehicleEntry(); // Đã đầy (0 chỗ trống)

        var ex = Assert.Throws<BusinessRuleViolationException>(() => lot.RecordVehicleEntry());
        Assert.Equal("Parking lot is full!", ex.Message);
    }

    [Fact]
    public void ParkingLot_RecordVehicleExit_WhenOneCarParked_ReturnsToEmpty()
    {
        // TC_LOT_09: Xe ra khi bãi đang có đúng 1 xe -> về lại mốc 0 xe
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Exit Test Lot",
            _testAddress,
            totalCapacity: 10
        );

        lot.RecordVehicleEntry();
        Assert.Equal(9, lot.AvailableSpots);

        lot.RecordVehicleExit();
        Assert.Equal(10, lot.AvailableSpots);
        Assert.Equal(0, lot.OccupiedSpots);
    }

    [Fact]
    public void ParkingLot_RecordVehicleExit_WhenAlreadyEmpty_DoesNotExceedTotalCapacity()
    {
        // TC_LOT_10: Xe ra khi bãi vốn dĩ đã rỗng (Biên Min = 0 xe) -> Không cho tràn số chỗ
        var lot = new ParkingLot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Already Empty Lot",
            _testAddress,
            totalCapacity: 5
        );

        lot.RecordVehicleExit(); // Thử gọi khi không có xe nào gửi

        Assert.Equal(5, lot.AvailableSpots);
        Assert.Equal(0, lot.OccupiedSpots);
    }

    #endregion

    #region Module 2: Lượt gửi xe (ParkingSession Boundary Tests)

    [Fact]
    public void ParkingSession_CheckOut_OnActiveSession_SetsStatusCompleted()
    {
        // TC_SES_01: Check-out thành công trên phiên đang hoạt động
        var session = new ParkingSession(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new LicensePlate("59", "99988"),
            DateTime.UtcNow
        );

        var fee = new Money(10000, "VND");
        session.CheckOut(fee);

        Assert.Equal(ParkingManagement.Domain.Enums.ParkingStatus.Completed, session.Status);
        Assert.Equal(10000, session.Fee.Amount);
        Assert.NotNull(session.CheckOutTime);
    }

    [Fact]
    public void ParkingSession_CheckOut_AlreadyCompleted_ThrowsBusinessRuleViolationException()
    {
        // TC_SES_06: Check-out lần 2 trên cùng 1 phiên đã hoàn thành (Biên trạng thái State Machine)
        var session = new ParkingSession(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new LicensePlate("59", "99988"),
            DateTime.UtcNow
        );

        session.CheckOut(new Money(10000, "VND")); // Check-out lần 1

        // Check-out lần 2 -> Phải ném ngoại lệ
        var ex = Assert.Throws<BusinessRuleViolationException>(() =>
            session.CheckOut(new Money(10000, "VND"))
        );
        Assert.Equal("This parking session has already been checked out!", ex.Message);
    }

    [Fact]
    public void ParkingSession_CheckOut_WithNullFee_ThrowsArgumentNullException()
    {
        var session = new ParkingSession(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new LicensePlate("59", "99988")
        );

        Assert.Throws<ArgumentNullException>(() => session.CheckOut(null!));
    }

    [Fact]
    public void ParkingSession_Creation_EmptyForeignKeys_ThrowsArgumentException()
    {
        var plate = new LicensePlate("59", "12345");

        // ParkingLotId rỗng
        Assert.Throws<ArgumentException>(() =>
            new ParkingSession(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), plate)
        );

        // VehicleTypeId rỗng
        Assert.Throws<ArgumentException>(() =>
            new ParkingSession(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, plate)
        );
    }

    #endregion

    #region Module 3: Định dạng Biển số xe (LicensePlate Boundary Tests)

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void LicensePlate_EmptyProvinceCode_ThrowsArgumentException(string? invalidProvince)
    {
        // TC_PLT_01: Mã tỉnh rỗng
        Assert.Throws<ArgumentException>(() =>
            new LicensePlate(invalidProvince!, "12345")
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void LicensePlate_EmptyPlateNumber_ThrowsArgumentException(string? invalidPlate)
    {
        Assert.Throws<ArgumentException>(() =>
            new LicensePlate("59", invalidPlate!)
        );
    }

    [Theory]
    [InlineData("59", "1234", "59-1234")]       // Ngắn nhất (Min)
    [InlineData("29A", "99988", "29A-99988")]   // Danh định (Nominal)
    [InlineData("80AA", "123456", "80AA-123456")] // Mã tỉnh 4 ký tự (Max)
    public void LicensePlate_ValidBoundaries_FormatsCorrectly(string province, string plate, string expectedFull)
    {
        // TC_PLT_02, TC_PLT_03, TC_PLT_04
        var licensePlate = new LicensePlate(province, plate);

        Assert.Equal(expectedFull, licensePlate.FullPlateNumber);
        Assert.Equal(expectedFull, licensePlate.ToString());
    }

    [Fact]
    public void LicensePlate_CaseInsensitiveEquality_ReturnsTrue()
    {
        // Kiểm tra tính bất biến và so sánh không phân biệt hoa thường
        var plate1 = new LicensePlate("59a", "99988");
        var plate2 = new LicensePlate("59A", "99988");

        Assert.Equal(plate1, plate2);
        Assert.True(plate1 == plate2);
    }

    #endregion

    #region Module 4: Giá trị Tiền tệ (Money Boundary Tests)

    [Theory]
    [InlineData(-0.01)] // Ngay dưới biên 0 (Min - 1)
    [InlineData(-1000)] // Số âm lớn
    public void Money_NegativeAmount_ThrowsArgumentOutOfRangeException(decimal negativeAmount)
    {
        // TC_MON_01: Số tiền âm
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Money(negativeAmount, "VND"));
        Assert.Equal("amount", ex.ParamName);
    }

    [Fact]
    public void Money_ZeroAmount_IsValid()
    {
        // TC_MON_02: Biên 0 đồng (Miễn phí)
        var zero = new Money(0, "VND");

        Assert.Equal(0, zero.Amount);
        Assert.Equal("VND", zero.Currency);
    }

    [Fact]
    public void Money_SmallestPositiveAmount_IsValid()
    {
        // TC_MON_03: Biên ngay trên 0 (Min + 1)
        var minMoney = new Money(1, "VND");

        Assert.Equal(1, minMoney.Amount);
    }

    [Fact]
    public void Money_Add_DifferentCurrencies_ThrowsInvalidOperationException()
    {
        // TC_MON_04: Ràng buộc cộng khác loại tiền tệ
        var vnd = new Money(10000, "VND");
        var usd = new Money(1, "USD");

        var ex = Assert.Throws<InvalidOperationException>(() => vnd.Add(usd));
        Assert.Contains("Cannot add money of different currencies", ex.Message);
    }

    #endregion
}
