# Smart Parking Management - Detailed Unit Test Cases Specification
**Dự án:** Hệ thống Quản lý Bãi đỗ xe Thông minh (Smart Parking Management)  
**Phương pháp thiết kế Test Case:** Phân tích Giá trị Biên (Boundary Value Analysis - BVA) & Phân vùng Tương đương (Equivalence Partitioning - EP)  
**Tập tin code thực thi:** `Backend/ParkingManagement.Tests/BoundaryValueAnalysisTests.cs`  
**Test Framework:** xUnit, .NET 9.0  
**Tổng số ca kiểm thử:** 29 test cases biên (Tổng suite: 47 test cases)  
**Kết quả thực thi:** 100% Passed (47/47 Passed, 0 Failed)  
**Ngày cập nhật:** 13/10/2026  

---

## 1. Module 1: Quản lý Sức chứa Bãi xe (`ParkingLot Capacity`)

### TC_LOT_01: Kiểm tra sức chứa bãi xe với giá trị âm ($Min - 1$)
* **Mô tả:** Đảm bảo hệ thống không cho phép khởi tạo bãi xe có sức chứa là số âm.
* **Tên phương thức Test:** `ParkingLot_Capacity_NegativeValue_ThrowsArgumentOutOfRangeException`
* **Loại kiểm thử:** Negative Boundary Test ($Min - 1$)
* **Dữ liệu đầu vào (Input):**
  - `negativeCapacity = -1`, `negativeCapacity = -50`
* **Các bước thực hiện:**
  1. Khởi tạo đối tượng `ParkingLot` với `totalCapacity = negativeCapacity`.
* **Kết quả mong đợi (Expected):**
  - Ném ra ngoại lệ `ArgumentOutOfRangeException`.
  - Tham số lỗi xác định đúng là `totalCapacity`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_02: Kiểm tra sức chứa bãi xe bằng 0
* **Mô tả:** Bãi đỗ xe bắt buộc phải có sức chứa tối thiểu > 0, không được bằng 0.
* **Tên phương thức Test:** `ParkingLot_Capacity_Zero_ThrowsArgumentOutOfRangeException`
* **Loại kiểm thử:** Negative Boundary Test
* **Dữ liệu đầu vào (Input):**
  - `totalCapacity = 0`
* **Các bước thực hiện:**
  1. Khởi tạo `ParkingLot` với `totalCapacity = 0`.
* **Kết quả mong đợi (Expected):**
  - Ném ra ngoại lệ `ArgumentOutOfRangeException`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_03: Khởi tạo bãi xe với sức chứa nhỏ nhất hợp lệ ($Min$)
* **Mô tả:** Đảm bảo bãi xe có thể tạo được với sức chứa tối thiểu là 1 chỗ đỗ.
* **Tên phương thức Test:** `ParkingLot_Capacity_MinimumValid_One_Success`
* **Loại kiểm thử:** Positive Boundary Test ($Min = 1$)
* **Dữ liệu đầu vào (Input):**
  - `totalCapacity = 1`
* **Các bước thực hiện:**
  1. Khởi tạo `ParkingLot` với `totalCapacity = 1`.
  2. Kiểm tra `TotalCapacity`, `AvailableSpots`, `OccupiedSpots`.
* **Kết quả mong đợi (Expected):**
  - Tạo thành công: `TotalCapacity = 1`, `AvailableSpots = 1`, `OccupiedSpots = 0`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_04: Khởi tạo bãi xe với sức chứa cực đại ($Max$)
* **Mô tả:** Đảm bảo hệ thống xử lý ổn định khi bãi xe có quy mô rất lớn mà không tràn bộ nhớ/số nguyên.
* **Tên phương thức Test:** `ParkingLot_Capacity_VeryLargeValue_Success`
* **Loại kiểm thử:** Positive Boundary Test ($Max = 50,000$)
* **Dữ liệu đầu vào (Input):**
  - `totalCapacity = 50000`
* **Kết quả mong đợi (Expected):**
  - `TotalCapacity = 50000`, `AvailableSpots = 50000`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_05: Xe vào khi bãi đang trống (Mốc bắt đầu)
* **Mô tả:** Ghi nhận 1 xe vào bãi khi bãi đang có 0 xe.
* **Tên phương thức Test:** `ParkingLot_RecordVehicleEntry_WhenEmpty_OccupancyBecomesOne`
* **Dữ liệu đầu vào (Input):**
  - Bãi có `totalCapacity = 100`, `AvailableSpots = 100`.
* **Các bước thực hiện:**
  1. Gọi `lot.RecordVehicleEntry()`.
* **Kết quả mong đợi (Expected):**
  - `AvailableSpots = 99`, `OccupiedSpots = 1`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_06: Xe vào khi bãi còn đúng 1 chỗ trống ($Max - 1$)
* **Mô tả:** Cho xe vào lấp đầy chỗ trống cuối cùng trong bãi.
* **Tên phương thức Test:** `ParkingLot_RecordVehicleEntry_WhenOneSpotLeft_ReachesFullCapacity`
* **Dữ liệu đầu vào (Input):**
  - Bãi có `totalCapacity = 2`.
* **Các bước thực hiện:**
  1. Gọi `RecordVehicleEntry()` lần 1 ➔ Còn 1 chỗ.
  2. Gọi `RecordVehicleEntry()` lần 2 (chỗ cuối cùng).
* **Kết quả mong đợi (Expected):**
  - `AvailableSpots = 0`, `OccupiedSpots = 2`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_07 & TC_LOT_08: Cố tình cho xe vào khi bãi đã đầy 100% ($Max + 1$)
* **Mô tả:** Đảm bảo quy tắc nghiệp vụ không bao giờ cho phép nhận thêm xe khi bãi đã hết chỗ.
* **Tên phương thức Test:** `ParkingLot_RecordVehicleEntry_WhenCompletelyFull_ThrowsBusinessRuleViolationException`
* **Loại kiểm thử:** Negative Boundary Test ($Max + 1$)
* **Dữ liệu đầu vào (Input):**
  - Bãi có `totalCapacity = 1`.
  - Đã gọi `RecordVehicleEntry()` ➔ `AvailableSpots = 0`.
* **Các bước thực hiện:**
  1. Cố tình gọi `lot.RecordVehicleEntry()` thêm một lần nữa.
* **Kết quả mong đợi (Expected):**
  - Ném ngoại lệ `BusinessRuleViolationException`.
  - Thông báo lỗi chính xác: `"Parking lot is full!"`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_09: Xe ra khi bãi đang có 1 xe
* **Mô tả:** Xe rời bãi, đưa số xe về mốc rỗng ban đầu.
* **Tên phương thức Test:** `ParkingLot_RecordVehicleExit_WhenOneCarParked_ReturnsToEmpty`
* **Kết quả mong đợi (Expected):**
  - `AvailableSpots = 10`, `OccupiedSpots = 0`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_LOT_10: Xe ra khi bãi vốn dĩ đã rỗng 0 xe (Chặn số âm)
* **Mô tả:** Đảm bảo số chỗ trống không bao giờ vượt quá tổng sức chứa khi có lệnh gọi ra bất thường.
* **Tên phương thức Test:** `ParkingLot_RecordVehicleExit_WhenAlreadyEmpty_DoesNotExceedTotalCapacity`
* **Kết quả mong đợi (Expected):**
  - `AvailableSpots` giữ nguyên ở mức `5`, không bị tăng lên `6`, `OccupiedSpots = 0`.
* **Trạng thái:** ✅ **PASSED**

---

## 2. Module 2: Lượt Gửi Xe (`ParkingSession`)

### TC_SES_01: Check-out thành công trên phiên đang hoạt động
* **Tên phương thức Test:** `ParkingSession_CheckOut_OnActiveSession_SetsStatusCompleted`
* **Dữ liệu đầu vào (Input):**
  - `ParkingSession` trạng thái `Active`.
  - `fee = new Money(10000, "VND")`.
* **Kết quả mong đợi (Expected):**
  - `Status` chuyển thành `ParkingStatus.Completed`.
  - `Fee.Amount = 10000`.
  - `CheckOutTime` được ghi nhận khác null.
* **Trạng thái:** ✅ **PASSED**

---

### TC_SES_02: Check-out lần thứ 2 trên phiên đã hoàn thành (Biên State Machine)
* **Mô tả:** Chống gian lận và lỗi hệ thống khi một phiên gửi xe đã hoàn tất lại bị gọi tính tiền lần 2.
* **Tên phương thức Test:** `ParkingSession_CheckOut_AlreadyCompleted_ThrowsBusinessRuleViolationException`
* **Kết quả mong đợi (Expected):**
  - Ném `BusinessRuleViolationException`.
  - Message: `"This parking session has already been checked out!"`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_SES_03: Check-out với số tiền null
* **Tên phương thức Test:** `ParkingSession_CheckOut_WithNullFee_ThrowsArgumentNullException`
* **Kết quả mong đợi (Expected):**
  - Ném `ArgumentNullException`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_SES_04: Khởi tạo phiên gửi xe với khóa ngoại rỗng (Guid.Empty)
* **Tên phương thức Test:** `ParkingSession_Creation_EmptyForeignKeys_ThrowsArgumentException`
* **Dữ liệu đầu vào (Input):**
  - `parkingLotId = Guid.Empty` hoặc `vehicleTypeId = Guid.Empty`.
* **Kết quả mong đợi (Expected):**
  - Ném `ArgumentException` chặn ngay từ constructor Domain.
* **Trạng thái:** ✅ **PASSED**

---

## 3. Module 3: Định dạng Biển Số Xe (`LicensePlate` Value Object)

### TC_PLT_01: Mã tỉnh rỗng hoặc chỉ có khoảng trắng
* **Tên phương thức Test:** `LicensePlate_EmptyProvinceCode_ThrowsArgumentException`
* **Dữ liệu đầu vào (Input):**
  - `provinceCode` nhận các giá trị: `""`, `"   "`, `null`.
* **Kết quả mong đợi (Expected):**
  - Ném `ArgumentException`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_PLT_02: Số xe rỗng hoặc chỉ có khoảng trắng
* **Tên phương thức Test:** `LicensePlate_EmptyPlateNumber_ThrowsArgumentException`
* **Dữ liệu đầu vào (Input):**
  - `plateNumber` nhận các giá trị: `""`, `"   "`, `null`.
* **Kết quả mong đợi (Expected):**
  - Ném `ArgumentException`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_PLT_03: Các giá trị biên độ dài biển số xe hợp lệ
* **Tên phương thức Test:** `LicensePlate_ValidBoundaries_FormatsCorrectly`
* **Bộ dữ liệu kiểm thử (InlineData):**
  1. `("59", "1234")` ➔ Ngắn nhất ($Min$): `"59-1234"`
  2. `("29A", "99988")` ➔ Danh định chuẩn ($Nominal$): `"29A-99988"`
  3. `("80AA", "123456")` ➔ Mã tỉnh dài nhất ($Max$): `"80AA-123456"`
* **Kết quả mong đợi (Expected):**
  - Thuộc tính `FullPlateNumber` và phương thức `ToString()` trả về định dạng chuẩn hoa.
* **Trạng thái:** ✅ **PASSED**

---

### TC_PLT_04: So sánh 2 biển số không phân biệt hoa thường (Case-insensitivity)
* **Tên phương thức Test:** `LicensePlate_CaseInsensitiveEquality_ReturnsTrue`
* **Dữ liệu đầu vào (Input):**
  - `plate1 = new LicensePlate("59a", "99988")`
  - `plate2 = new LicensePlate("59A", "99988")`
* **Kết quả mong đợi (Expected):**
  - `plate1.Equals(plate2) == true`
  - `plate1 == plate2` trả về `true`.
* **Trạng thái:** ✅ **PASSED**

---

## 4. Module 4: Giá Trị Tiền Tệ (`Money` Value Object)

### TC_MON_01: Số tiền âm ($Min - 1$)
* **Tên phương thức Test:** `Money_NegativeAmount_ThrowsArgumentOutOfRangeException`
* **Dữ liệu đầu vào (Input):**
  - `amount = -0.01` (ngay dưới biên 0)
  - `amount = -1000` (số âm lớn)
* **Kết quả mong đợi (Expected):**
  - Ném `ArgumentOutOfRangeException`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_MON_02: Số tiền bằng 0 đồng (Miễn phí)
* **Tên phương thức Test:** `Money_ZeroAmount_IsValid`
* **Dữ liệu đầu vào (Input):**
  - `amount = 0`, `currency = "VND"`
* **Kết quả mong đợi (Expected):**
  - Hợp lệ, `zero.Amount = 0`, `zero.Currency = "VND"`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_MON_03: Số tiền nhỏ nhất lớn hơn 0 ($Min + 1$)
* **Tên phương thức Test:** `Money_SmallestPositiveAmount_IsValid`
* **Dữ liệu đầu vào (Input):**
  - `amount = 1`, `currency = "VND"`
* **Kết quả mong đợi (Expected):**
  - `minMoney.Amount = 1`.
* **Trạng thái:** ✅ **PASSED**

---

### TC_MON_04: Cộng 2 số tiền khác mệnh giá tiền tệ
* **Tên phương thức Test:** `Money_Add_DifferentCurrencies_ThrowsInvalidOperationException`
* **Dữ liệu đầu vào (Input):**
  - `vnd = new Money(10000, "VND")`
  - `usd = new Money(1, "USD")`
* **Kết quả mong đợi (Expected):**
  - Ném `InvalidOperationException`.
  - Message chứa nội dung: `"Cannot add money of different currencies"`.
* **Trạng thái:** ✅ **PASSED**

---

## 5. Tổng Kết Ma Trận Kết Quả Thực Thi (Execution Summary)

| STT | Module | Số lượng Test Cases | Trạng thái (Status) | Tỷ lệ thành công |
| :---: | :--- | :---: | :---: | :---: |
| 1 | Sức chứa bãi xe (`ParkingLot`) | 10 | PASSED | 100% |
| 2 | Lượt gửi xe (`ParkingSession`) | 5 | PASSED | 100% |
| 3 | Biển số xe (`LicensePlate`) | 8 | PASSED | 100% |
| 4 | Giá trị tiền tệ (`Money`) | 6 | PASSED | 100% |
| **Tổng** | **Toàn bộ Test Cases Biên** | **29** | **PASSED** | **100%** |
