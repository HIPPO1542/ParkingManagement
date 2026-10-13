# Smart Parking Management - Unit Test Plan
**Dự án:** Hệ thống Quản lý Bãi đỗ xe Thông minh (Smart Parking Management)  
**Kỹ thuật kiểm thử chính:** Kỹ thuật Phân tích Giá trị Biên (Boundary Value Analysis - BVA) & Phân vùng Tương đương (Equivalence Partitioning - EP)  
**Framework kiểm thử:** xUnit, FluentAssertions, Moq (.NET 9.0)  
**Ngày lập:** 13/10/2026  
**Phiên bản:** 1.0  

---

## 1. Mục Tiêu & Phạm Vi Kiểm Thử (Objectives & Scope)

### 1.1 Mục tiêu
- Đảm bảo độ tin cậy và tính toàn vẹn của logic nghiệp vụ cốt lõi (Domain Entities, Value Objects) và tầng ứng dụng (Application CQRS Commands/Queries).
- Tập trung sâu vào **Kiểm thử Giá trị Biên (Boundary Value Testing)**: Các điểm giới hạn trên, giới hạn dưới, giá trị lân cận và giá trị vượt ngưỡng để phát hiện sớm các lỗi "off-by-one" (lệch 1 đơn vị), tràn dữ liệu hoặc rò rỉ nghiệp vụ.
- Đạt độ bao phủ kiểm thử (Code Coverage) tối thiểu 85% cho các nghiệp vụ trọng yếu.

### 1.2 Phạm vi kiểm thử
- **Domain Layer:**
  - Entity `ParkingLot`: Sức chứa (`TotalCapacity`), số lượng xe đỗ (`CurrentOccupancy`), logic kiểm tra còn chỗ (`HasAvailableCapacity`), đóng/mở cửa.
  - Entity `ParkingSession`: Tính toán thời gian đỗ xe (`DurationHours`), chuyển đổi trạng thái (`Active` ➔ `Completed`), tính phí đỗ xe.
  - Value Object `LicensePlate`: Độ dài mã tỉnh (`ProvinceCode`), số xe (`PlateNumber`), tính bất biến.
  - Value Object `Money`: Giá trị âm, bằng 0, cộng trừ tiền tệ khác loại.
- **Application Layer (CQRS Handlers):**
  - `CheckInCommandHandler`: Xe vào bãi khi bãi trống, bãi còn đúng 1 chỗ, bãi vừa đầy và bãi quá tải.
  - `CheckOutByPlateCommandHandler`: Xe ra bãi ngay lập tức (0 phút), dưới 1 giờ, qua khung giờ đêm, xe đã ra rồi mà check-out lại.
  - `CreateParkingLotCommandHandler`: Tạo bãi với sức chứa biên (1, 10.000, 0, số âm).

---

## 2. Chiến Lược & Kỹ Thuật Kiểm Thử (Testing Strategy)

### 2.1 Kỹ thuật Phân tích Giá trị Biên (Boundary Value Analysis - BVA)
Với một dải giá trị hợp lệ $[Min, Max]$, các điểm kiểm thử bắt buộc phải bao gồm:
1. **$Min - 1$**: Dưới ngưỡng tối thiểu (Không hợp lệ - Expect Exception/Failure)
2. **$Min$**: Điểm biên tối thiểu (Hợp lệ)
3. **$Min + 1$**: Ngay trên điểm biên tối thiểu (Hợp lệ)
4. **$Nominal$**: Giá trị danh định ở giữa dải (Hợp lệ)
5. **$Max - 1$**: Ngay dưới điểm biên tối đa (Hợp lệ)
6. **$Max$**: Điểm biên tối đa (Hợp lệ)
7. **$Max + 1$**: Vượt ngưỡng tối đa (Không hợp lệ - Expect Exception/Failure)

---

## 3. Ma Trận Kế Hoạch Test Cases Tập Trung Vào Các Biên (Test Matrix)

### Module 1: Quản lý Sức Chứa Bãi Xe (`ParkingLot` - Sức chứa danh định: 100)

| Test Case ID | Tên Kiểm Thử | Giá trị đầu vào / Trạng thái | Điểm Biên Được Kiểm Tra | Kết Quả Mong Đợi (Expected Result) |
| :--- | :--- | :--- | :--- | :--- |
| **TC_LOT_01** | Tạo bãi xe với sức chứa âm | `TotalCapacity = -1` | Biên dưới không hợp lệ ($Min - 1$) | Ném `ArgumentException` ("Capacity must be positive") |
| **TC_LOT_02** | Tạo bãi xe với sức chứa bằng 0 | `TotalCapacity = 0` | Biên dưới không hợp lệ | Ném `ArgumentException` |
| **TC_LOT_03** | Tạo bãi xe với sức chứa nhỏ nhất | `TotalCapacity = 1` | Biên dưới hợp lệ nhỏ nhất ($Min$) | Tạo bãi thành công, `CurrentOccupancy = 0` |
| **TC_LOT_04** | Tạo bãi xe với sức chứa cực lớn | `TotalCapacity = 50,000` | Biên trên rất lớn | Tạo bãi thành công |
| **TC_LOT_05** | Cho xe vào khi bãi trống | `CurrentOccupancy = 0`, `Capacity = 100` | Điểm bắt đầu của bãi | `HasAvailableCapacity() == true`, tăng occupancy lên 1 |
| **TC_LOT_06** | Cho xe vào khi bãi còn đúng 1 chỗ | `CurrentOccupancy = 99`, `Capacity = 100` | Ngay trước biên đầy ($Max - 1$) | Cho phép vào, `CurrentOccupancy` đạt 100 |
| **TC_LOT_07** | Kiểm tra trạng thái khi bãi vừa đầy | `CurrentOccupancy = 100`, `Capacity = 100` | Đúng điểm đầy bãi ($Max$) | `HasAvailableCapacity() == false` |
| **TC_LOT_08** | Cố tình cho xe vào khi bãi đã đầy | `CurrentOccupancy = 100`, yêu cầu thêm 1 xe | Vượt biên bãi ($Max + 1$) | Ném `InvalidOperationException` ("Parking lot is full") |
| **TC_LOT_09** | Xe ra khi bãi đang có đúng 1 xe | `CurrentOccupancy = 1`, xe ra | Về lại mốc trống ($Min$) | Giảm occupancy về 0 thành công |
| **TC_LOT_10** | Cố tình giảm xe khi bãi đang bằng 0 | `CurrentOccupancy = 0`, gọi giảm xe | Dưới mốc 0 ($Min - 1$) | Ném `InvalidOperationException` (Không cho phép occupancy âm) |

---

### Module 2: Lượt Gửi Xe & Tính Thời Gian Đỗ (`ParkingSession`)

Quy ước tính giờ đỗ xe: Làm tròn lên theo block giờ hoặc tính theo phút/giờ thực tế.

| Test Case ID | Tên Kiểm Thử | Giờ vào ➔ Giờ ra | Điểm Biên Được Kiểm Tra | Kết Quả Mong Đợi (Expected Result) |
| :--- | :--- | :--- | :--- | :--- |
| **TC_SES_01** | Ra ngay khi vừa vào (0 giây) | `CheckIn = T`, `CheckOut = T` | Điểm biên 0 thời gian | `DurationHours = 0.0`, không crash, tính giá tối thiểu hoặc miễn phí |
| **TC_SES_02** | Đỗ xe 59 phút 59 giây | `CheckIn = T`, `CheckOut = T + 59m59s` | Cận dưới mốc 1 giờ | Xử lý đúng block 1 giờ đầu tiên |
| **TC_SES_03** | Đỗ xe đúng 1 giờ 00 phút 00 giây | `CheckIn = T`, `CheckOut = T + 60m00s` | Đúng mốc 1 giờ | `DurationHours = 1.0` |
| **TC_SES_04** | Đỗ xe 1 giờ 00 phút 01 giây | `CheckIn = T`, `CheckOut = T + 60m01s` | Vừa chớm qua giờ thứ 2 | Hệ thống tính tròn sang block 2 giờ (nếu theo cơ chế block) |
| **TC_SES_05** | Giờ ra trước giờ vào | `CheckIn = 10:00`, `CheckOut = 09:59` | Giá trị âm / Sai logic thời gian | Ném `InvalidOperationException` ("Check-out time cannot be before check-in time") |
| **TC_SES_06** | Check-out lần thứ 2 trên cùng 1 xe | Phiên gửi đã `Completed`, gọi Check-out tiếp | Biên trạng thái State Machine | Ném `BusinessRuleViolationException` ("Session is already completed") |

---

### Module 3: Định Dạng Biển Số Xe (`LicensePlate` Value Object)

Quy định: `ProvinceCode` (2 đến 4 ký tự), `PlateNumber` (4 đến 10 ký tự).

| Test Case ID | Tên Kiểm Thử | Dữ liệu đầu vào | Điểm Biên Được Kiểm Tra | Kết Quả Mong Đợi (Expected Result) |
| :--- | :--- | :--- | :--- | :--- |
| **TC_PLT_01** | Mã tỉnh rỗng | `ProvinceCode = ""` | Dưới biên tối thiểu ($Len = 0$) | Ném `ArgumentException` |
| **TC_PLT_02** | Mã tỉnh ngắn nhất (2 ký tự) | `ProvinceCode = "59"` | Đúng biên tối thiểu ($Len = 2$) | Hợp lệ |
| **TC_PLT_03** | Mã tỉnh có chữ cái (3 ký tự) | `ProvinceCode = "29A"` | Danh định hợp lệ ($Len = 3$) | Hợp lệ |
| **TC_PLT_04** | Mã tỉnh dài nhất cho phép (4 ký tự)| `ProvinceCode = "80AA"` | Đúng biên tối đa ($Len = 4$) | Hợp lệ |
| **TC_PLT_05** | Mã tỉnh vượt quá 4 ký tự | `ProvinceCode = "12345"` | Vượt biên tối đa ($Len = 5$) | Ném `ArgumentException` |
| **TC_PLT_06** | Số xe ngắn nhất (4 ký tự) | `PlateNumber = "1234"` | Đúng biên tối thiểu ($Len = 4$) | Hợp lệ |
| **TC_PLT_07** | Số xe dài nhất (10 ký tự) | `PlateNumber = "123.456.78"` | Đúng biên tối đa ($Len = 10$) | Hợp lệ |
| **TC_PLT_08** | Số xe vượt quá 10 ký tự | `PlateNumber = "12345678901"` | Vượt biên tối đa ($Len = 11$) | Ném `ArgumentException` |

---

### Module 4: Giá Trị Tiền Tệ (`Money` Value Object)

| Test Case ID | Tên Kiểm Thử | Dữ liệu đầu vào | Điểm Biên Được Kiểm Tra | Kết Quả Mong Đợi (Expected Result) |
| :--- | :--- | :--- | :--- | :--- |
| **TC_MON_01** | Số tiền âm | `Amount = -0.01` | Biên dưới không hợp lệ | Ném `ArgumentOutOfRangeException` |
| **TC_MON_02** | Số tiền bằng 0 (Miễn phí) | `Amount = 0.0` | Điểm biên 0 | Hợp lệ, tạo Money(0, "VND") thành công |
| **TC_MON_03** | Số tiền nhỏ nhất | `Amount = 1.0` | Ngay trên biên 0 | Hợp lệ |
| **TC_MON_04** | Cộng 2 số tiền khác loại tiền tệ | `10,000 VND` + `5 USD` | Ràng buộc nghiệp vụ | Ném `InvalidOperationException` ("Cannot add different currencies") |

---

## 4. Môi Trường & Công Cụ Thực Thi Kiểm Thử (Environment & Tools)

- **Ngôn ngữ & SDK:** C# 13, .NET 9.0 SDK
- **Test Runner:** xUnit (`xunit.runner.visualstudio` v2.8+)
- **Thư viện Mocking:** `Moq` (Mock Repository & UnitOfWork)
- **Thư viện Assertion:** `FluentAssertions` (Giúp câu lệnh assert rõ ràng, dễ đọc)
- **Lệnh thực thi kiểm thử:**
  ```powershell
  # Chạy toàn bộ unit test và xuất kết quả chi tiết
  dotnet test Backend/ParkingManagement.Tests/ParkingManagement.Tests.csproj --logger "console;verbosity=detailed"
  ```

---

## 5. Tiêu Chuẩn Nghiệm Thu (Acceptance Criteria)
1. **Tỷ lệ Pass:** 100% các Test Case đều phải có màu xanh (Pass), không chấp nhận bất kỳ lỗi Failed nào.
2. **Xử lý Exception ở biên:** Tất cả các ca kiểm thử ở biên không hợp lệ ($Min-1, Max+1$) phải ném đúng loại ngoại lệ được đặc tả (`EntityNotFoundException`, `BusinessRuleViolationException`, `ArgumentException`), không được để xảy ra lỗi `NullReferenceException` hay Crash hệ thống.
3. **Thời gian thực thi:** Toàn bộ test suite phải hoàn thành dưới 5 giây.
