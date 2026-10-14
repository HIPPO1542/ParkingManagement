# Hệ Thống Quản Lý Bãi Đỗ Xe Thông Minh - Đặc Tả Ca Kiểm Thử (Test Cases Specification)
**Dự án:** Hệ thống Quản lý Bãi đỗ xe Thông minh  
**Phương pháp thiết kế:** Phân tích Giá trị Biên (BVA), Phân vùng Tương đương (EP), Kiểm thử Đối kháng & Kiểm thử Tích hợp  
**Định dạng chuẩn:** Bảng 10 cột chuẩn QA/QC theo mẫu biểu kiểm thử thực tế (Test Case ID, Test Scenario, Test Case, Pre-condition, Test Steps, Test Data, Expected Result, Post Condition, Actual Result, Status)  
**Công cụ thực thi:** xUnit Core Framework (v2.9.2), xUnit.net VSTest Adapter (v2.8.2), .NET 9.0 SDK, SQLite In-Memory  
**Môi trường kiểm thử:** Nhánh `refactor/cqrs-endpoints`  
**Thời điểm chạy kiểm thử:** 14/10/2026 08:13:23 (GMT+7)  
**Tổng số ca kiểm thử thực thi:** 55 ca kiểm thử  
**Kết quả thực thi:** 100% ĐẠT (55/55 Pass, 0 Fail, Thời gian: 1.0 giây)  

---

## Mục Lục

1. [Phân Hệ 1: Sức Chứa Bãi Xe & Giá Trị Biên (11 Ca)](#1-phân-hệ-1-sức-chứa-bãi-xe--giá-trị-biên-11-ca)
2. [Phân Hệ 2: Lượt Gửi Xe & Vòng Đời Trạng Thái (4 Ca)](#2-phân-hệ-2-lượt-gửi-xe--vòng-đời-trạng-thái-4-ca)
3. [Phân Hệ 3: Định Dạng Biển Số Xe (10 Ca)](#3-phân-hệ-3-định-dạng-biển-số-xe-10-ca)
4. [Phân Hệ 4: Giá Trị Tiền Tệ & Biểu Phí (11 Ca)](#4-phân-hệ-4-giá-trị-tiền-tệ--biểu-phí-11-ca)
5. [Phân Hệ 5: Quy Tắc Nghiệp Vụ, Chống Trùng Lặp & Phân Quyền (12 Ca)](#5-phân-hệ-5-quy-tắc-nghiệp-vụ-chống-trùng-lặp--phân-quyền-12-ca)
6. [Phân Hệ 6: Đối Kháng, Tranh Chấp Đồng Thời & Rollback (4 Ca)](#6-phân-hệ-6-đối-kháng-tranh-chấp-đồng-thời--rollback-4-ca)
7. [Phân Hệ 7: Xác Thực & Cấp Quyền Đăng Nhập (3 Ca)](#7-phân-hệ-7-xác-thực--cấp-quyền-đăng-nhập-3-ca)
8. [Bảng Ma Trận Tổng Hợp 55 Ca Kiểm Thử](#8-bảng-ma-trận-tổng-hợp-55-ca-kiểm-thử)
9. [Bằng Chứng Thực Nghiệm & Log Thực Thi Từ Hệ Thống](#9-bằng-chứng-thực-nghiệm--log-thực-thi-từ-hệ-thống)

---

## 1. Phân Hệ 1: Sức Chứa Bãi Xe & Giá Trị Biên (11 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_LOT_01** | Khởi tạo sức chứa bãi xe | Sức chứa bãi xe bằng số âm nhỏ (-1) | Hệ thống đang hoạt động, có Đơn vị kinh doanh hợp lệ | 1. Chuẩn bị thông tin bãi xe<br>2. Gán sức chứa totalCapacity = -1<br>3. Khởi tạo thực thể ParkingLot | `totalCapacity = -1` | Ném ngoại lệ ArgumentOutOfRangeException, từ chối tạo bãi xe (HTTP 400) | Bãi xe không được tạo trong hệ thống | Ném ArgumentOutOfRangeException với ParamName là totalCapacity | **Pass** |
| **TC_LOT_02** | Khởi tạo sức chứa bãi xe | Sức chứa bãi xe bằng số âm lớn (-50) | Hệ thống đang hoạt động, có Đơn vị kinh doanh hợp lệ | 1. Chuẩn bị thông tin bãi xe<br>2. Gán sức chứa totalCapacity = -50<br>3. Khởi tạo thực thể ParkingLot | `totalCapacity = -50` | Ném ngoại lệ ArgumentOutOfRangeException, từ chối tạo bãi xe (HTTP 400) | Bãi xe không được tạo trong hệ thống | Ném ArgumentOutOfRangeException với ParamName là totalCapacity | **Pass** |
| **TC_LOT_03** | Khởi tạo sức chứa bãi xe | Sức chứa bãi xe bằng 0 | Hệ thống đang hoạt động, có Đơn vị kinh doanh hợp lệ | 1. Chuẩn bị thông tin bãi xe<br>2. Gán sức chứa totalCapacity = 0<br>3. Khởi tạo thực thể ParkingLot | `totalCapacity = 0` | Ném ngoại lệ ArgumentOutOfRangeException, bãi xe phải có ít nhất 1 chỗ đỗ (HTTP 400) | Bãi xe không được tạo trong hệ thống | Ném ArgumentOutOfRangeException với ParamName là totalCapacity | **Pass** |
| **TC_LOT_04** | Khởi tạo sức chứa bãi xe | Khởi tạo bãi xe với sức chứa nhỏ nhất hợp lệ (1 chỗ) | Đơn vị kinh doanh hợp lệ | 1. Chuẩn bị thông tin bãi xe<br>2. Gán sức chứa totalCapacity = 1<br>3. Khởi tạo thực thể ParkingLot | `totalCapacity = 1` | Bãi xe được tạo thành công, TotalCapacity = 1, AvailableSpots = 1, CurrentParked = 0 (HTTP 201) | Bãi xe sẵn sàng tiếp nhận tối đa 1 phương tiện | Khởi tạo thành công, TotalCapacity = 1, AvailableSpots = 1 | **Pass** |
| **TC_LOT_05** | Khởi tạo sức chứa bãi xe | Khởi tạo bãi xe với sức chứa cực đại (50.000 chỗ) | Đơn vị kinh doanh hợp lệ | 1. Chuẩn bị thông tin bãi xe quy mô lớn<br>2. Gán totalCapacity = 50000<br>3. Khởi tạo thực thể ParkingLot | `totalCapacity = 50000` | Bãi xe tạo thành công, không tràn số, TotalCapacity = 50000, AvailableSpots = 50000 (HTTP 201) | Bãi xe sẵn sàng vận hành với quy mô 50.000 chỗ | Khởi tạo thành công, TotalCapacity = 50000, AvailableSpots = 50000 | **Pass** |
| **TC_LOT_06** | Ghi nhận xe vào bãi | Xe vào khi bãi đang trống hoàn toàn | Bãi xe có TotalCapacity = 100, AvailableSpots = 100, CurrentParked = 0, IsActive = true | 1. Quét thẻ xe vào cổng bãi<br>2. Gọi phương thức RecordVehicleEntry() | `ParkingLotId` hợp lệ | AvailableSpots giảm từ 100 xuống 99, CurrentParked tăng từ 0 lên 1 (HTTP 201) | AvailableSpots = 99, CurrentParked = 1 | AvailableSpots = 99, CurrentParked = 1 | **Pass** |
| **TC_LOT_07** | Ghi nhận xe vào bãi | Xe vào khi bãi còn đúng 1 chỗ trống cuối cùng | Bãi xe có TotalCapacity = 100, AvailableSpots = 1, CurrentParked = 99, IsActive = true | 1. Xe quét thẻ vào cổng<br>2. Gọi phương thức RecordVehicleEntry() | `ParkingLotId` hợp lệ | AvailableSpots giảm về 0, CurrentParked tăng lên 100, bãi xe chuyển trạng thái đầy (HTTP 201) | AvailableSpots = 0, CurrentParked = 100 | AvailableSpots = 0, CurrentParked = 100 | **Pass** |
| **TC_LOT_08** | Ghi nhận xe vào bãi | Xe vào khi bãi đã hết chỗ hoàn toàn (AvailableSpots = 0) | Bãi xe có TotalCapacity = 100, AvailableSpots = 0, CurrentParked = 100 | 1. Xe cố gắng quét thẻ vào cổng<br>2. Gọi phương thức RecordVehicleEntry() | `ParkingLotId` đầy chỗ | Ném ngoại lệ BusinessRuleViolationException("Parking lot is full!"), từ chối cho xe vào (HTTP 400) | Số chỗ AvailableSpots giữ nguyên = 0, không có xe vào thêm | Ném BusinessRuleViolationException với thông báo Parking lot is full! | **Pass** |
| **TC_LOT_09** | Ghi nhận xe ra bãi | Xe ra khi bãi đang có đúng 1 xe đỗ | Bãi xe có TotalCapacity = 100, AvailableSpots = 99, CurrentParked = 1 | 1. Xe thanh toán và qua barrier ra<br>2. Gọi phương thức RecordVehicleExit() | `ParkingLotId` hợp lệ | AvailableSpots tăng từ 99 lên 100, CurrentParked giảm từ 1 về 0 (HTTP 200) | AvailableSpots = 100, CurrentParked = 0 | AvailableSpots = 100, CurrentParked = 0 | **Pass** |
| **TC_LOT_10** | Ghi nhận xe ra bãi | Xe ra khi bãi đã trống hoàn toàn (AvailableSpots = TotalCapacity) | Bãi xe có TotalCapacity = 100, AvailableSpots = 100, CurrentParked = 0 | 1. Cố ý gọi phương thức RecordVehicleExit() khi CurrentParked = 0 | `ParkingLotId` trống xe | Số chỗ giữ nguyên bằng TotalCapacity = 100, không bị tràn vượt quá tổng sức chứa | Sức chứa giữ nguyên, CurrentParked không bị âm | AvailableSpots giữ nguyên 100 | **Pass** |
| **TC_LOT_11** | Ghi nhận xe vào bãi | Xe vào bãi xe đang ở trạng thái ngừng hoạt động (IsActive = false) | Bãi xe có IsActive = false (đang bảo trì hoặc đóng cửa) | 1. Quét thẻ xe vào cổng<br>2. Gọi phương thức RecordVehicleEntry() | `ParkingLotId` có `IsActive = false` | Ném ngoại lệ BusinessRuleViolationException("Parking lot is currently inactive!"), từ chối check-in (HTTP 400) | Không có xe nào được ghi nhận vào bãi | Ném BusinessRuleViolationException với thông báo Parking lot is currently inactive! | **Pass** |

---

## 2. Phân Hệ 2: Lượt Gửi Xe & Vòng Đời Trạng Thái (4 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_SES_01** | Vòng đời lượt gửi xe | Check-out thành công cho phiên đỗ xe đang hoạt động (Active) | Phiên gửi xe có Status = Active, CheckInTime đã ghi nhận | 1. Chuẩn bị mức phí hợp lệ 15.000 VNĐ<br>2. Gọi phương thức session.CheckOut(fee) | `fee = 15000 VND` | Trạng thái chuyển sang Completed, CheckOutTime được gán thời gian hiện tại, Fee = 15.000 VNĐ (HTTP 200) | Status = Completed, CheckOutTime != null | Status = Completed, CheckOutTime được cập nhật chính xác | **Pass** |
| **TC_SES_02** | Vòng đời lượt gửi xe | Check-out lần 2 trên phiên gửi xe đã hoàn thành (Completed) | Phiên gửi xe đã ở trạng thái Completed từ trước | 1. Chuẩn bị mức phí 20.000 VNĐ<br>2. Tiếp tục gọi session.CheckOut(fee) lần thứ hai | Phiên đỗ xe đã Completed | Ném ngoại lệ BusinessRuleViolationException("Parking session is already completed!"), từ chối thanh toán lặp lại (HTTP 400) | Dữ liệu phiên không bị ghi đè, trạng thái vẫn là Completed | Ném BusinessRuleViolationException với thông báo Parking session is already completed! | **Pass** |
| **TC_SES_03** | Vòng đời lượt gửi xe | Check-out với giá trị tiền phí bị null | Phiên gửi xe có Status = Active | 1. Truyền tham số fee = null vào phương thức CheckOut<br>2. Gọi session.CheckOut(null!) | `fee = null` | Ném ngoại lệ ArgumentNullException với tham số lỗi là "fee" (HTTP 400) | Phiên vẫn giữ nguyên trạng thái Active, không bị kết thúc sai lệch | Ném ArgumentNullException với ParamName là fee | **Pass** |
| **TC_SES_04** | Khởi tạo lượt gửi xe | Khởi tạo phiên gửi xe với khóa ngoại rỗng (Guid.Empty) | Đã có thông tin biển số xe hợp lệ | 1. Truyền parkingLotId = Guid.Empty hoặc vehicleTypeId = Guid.Empty<br>2. Khởi tạo new ParkingSession(...) | `parkingLotId = Guid.Empty` | Ném ngoại lệ ArgumentException báo lỗi mã bãi xe hoặc mã loại xe không được để trống (HTTP 400) | Không có phiên gửi xe nào được tạo | Ném ArgumentException | **Pass** |

---

## 3. Phân Hệ 3: Định Dạng Biển Số Xe (10 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_PLT_01** | Xác thực biển số xe | Mã tỉnh bị chuỗi rỗng ("") | Chuẩn bị số xe hợp lệ "99988" | 1. Gán provinceCode = ""<br>2. Khởi tạo new LicensePlate("", "99988") | `provinceCode = ""` | Ném ngoại lệ ArgumentException("Province code cannot be empty.") (HTTP 400) | Đối tượng biển số không được tạo | Ném ArgumentException với thông báo Province code cannot be empty | **Pass** |
| **TC_PLT_02** | Xác thực biển số xe | Mã tỉnh chỉ chứa khoảng trắng ("   ") | Chuẩn bị số xe hợp lệ "99988" | 1. Gán provinceCode = "   "<br>2. Khởi tạo new LicensePlate("   ", "99988") | `provinceCode = "   "` | Ném ngoại lệ ArgumentException (HTTP 400) | Đối tượng không được tạo | Ném ArgumentException | **Pass** |
| **TC_PLT_03** | Xác thực biển số xe | Mã tỉnh nhận giá trị null | Chuẩn bị số xe hợp lệ "99988" | 1. Gán provinceCode = null<br>2. Khởi tạo new LicensePlate(null!, "99988") | `provinceCode = null` | Ném ngoại lệ ArgumentException (HTTP 400) | Đối tượng không được tạo | Ném ArgumentException | **Pass** |
| **TC_PLT_04** | Xác thực biển số xe | Số xe bị chuỗi rỗng ("") | Chuẩn bị mã tỉnh hợp lệ "59" | 1. Gán plateNumber = ""<br>2. Khởi tạo new LicensePlate("59", "") | `plateNumber = ""` | Ném ngoại lệ ArgumentException("Plate number cannot be empty.") (HTTP 400) | Đối tượng không được tạo | Ném ArgumentException với thông báo Plate number cannot be empty | **Pass** |
| **TC_PLT_05** | Xác thực biển số xe | Số xe chỉ chứa khoảng trắng ("   ") | Chuẩn bị mã tỉnh hợp lệ "59" | 1. Gán plateNumber = "   "<br>2. Khởi tạo new LicensePlate("59", "   ") | `plateNumber = "   "` | Ném ngoại lệ ArgumentException (HTTP 400) | Đối tượng không được tạo | Ném ArgumentException | **Pass** |
| **TC_PLT_06** | Xác thực biển số xe | Số xe nhận giá trị null | Chuẩn bị mã tỉnh hợp lệ "59" | 1. Gán plateNumber = null<br>2. Khởi tạo new LicensePlate("59", null!) | `plateNumber = null` | Ném ngoại lệ ArgumentException (HTTP 400) | Đối tượng không được tạo | Ném ArgumentException | **Pass** |
| **TC_PLT_07** | Chuẩn hóa biển số xe | Khởi tạo biển số hợp lệ cận dưới (2 ký tự tỉnh, 4 số xe) | Dữ liệu biển số xe chuẩn | 1. Nhập provinceCode = "59", plateNumber = "1234"<br>2. Khởi tạo new LicensePlate("59", "1234") | `provinceCode = "59"`, `plateNumber = "1234"` | Khởi tạo thành công, FullPlateNumber = "59-1234" (HTTP 200/201) | Biển số được chuẩn hóa thành chuỗi "59-1234" | Khởi tạo thành công, FullPlateNumber = "59-1234" | **Pass** |
| **TC_PLT_08** | Chuẩn hóa biển số xe | Khởi tạo biển số hợp lệ có sê-ri chữ (3 ký tự tỉnh/sê-ri, 5 số xe) | Dữ liệu biển số xe chuẩn | 1. Nhập provinceCode = "29A", plateNumber = "99988"<br>2. Khởi tạo new LicensePlate("29A", "99988") | `provinceCode = "29A"`, `plateNumber = "99988"` | Khởi tạo thành công, FullPlateNumber = "29A-99988" (HTTP 200/201) | Biển số được chuẩn hóa thành chuỗi "29A-99988" | Khởi tạo thành công, FullPlateNumber = "29A-99988" | **Pass** |
| **TC_PLT_09** | Chuẩn hóa biển số xe | Khởi tạo biển số đặc biệt (4 ký tự tỉnh/sê-ri, 6 số xe) | Dữ liệu biển số xe đặc thù | 1. Nhập provinceCode = "80AA", plateNumber = "123456"<br>2. Khởi tạo new LicensePlate("80AA", "123456") | `provinceCode = "80AA"`, `plateNumber = "123456"` | Khởi tạo thành công, FullPlateNumber = "80AA-123456" (HTTP 200/201) | Biển số được chuẩn hóa thành chuỗi "80AA-123456" | Khởi tạo thành công, FullPlateNumber = "80AA-123456" | **Pass** |
| **TC_PLT_10** | So sánh và định danh biển số | So sánh không phân biệt hoa thường (Case-insensitive) | Hai chuỗi ký tự biển số khác nhau chữ hoa/thường: "59-t1"/"1234" vs "59-T1"/"1234" | 1. Tạo plate1 = new LicensePlate("59-t1", "1234")<br>2. Tạo plate2 = new LicensePlate("59-T1", "1234")<br>3. Kiểm tra so sánh plate1 == plate2 | `plate1 = "59-t1"/"1234"`, `plate2 = "59-T1"/"1234"` | Hai đối tượng được xác định là trùng khớp (True), cùng mã băm GetHashCode | Ngăn chặn gian lận cùng xe quét vào 2 lần do chữ hoa/thường | plate1 == plate2 trả về True, HashCode trùng khớp | **Pass** |

---

## 4. Phân Hệ 4: Giá Trị Tiền Tệ & Biểu Phí (11 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_MON_01** | Khởi tạo giá trị tiền tệ | Số tiền âm nhỏ (-0.01 VNĐ) | Loại tiền tệ "VND" | 1. Gán amount = -0.01<br>2. Khởi tạo new Money(-0.01m, "VND") | `amount = -0.01m` | Ném ngoại lệ ArgumentOutOfRangeException("Amount cannot be negative") (HTTP 400) | Không có đối tượng Money nào được tạo | Ném ArgumentOutOfRangeException với thông báo Amount cannot be negative | **Pass** |
| **TC_MON_02** | Khởi tạo giá trị tiền tệ | Số tiền âm lớn (-1.000 VNĐ) | Loại tiền tệ "VND" | 1. Gán amount = -1000<br>2. Khởi tạo new Money(-1000m, "VND") | `amount = -1000m` | Ném ngoại lệ ArgumentOutOfRangeException (HTTP 400) | Không có đối tượng Money nào được tạo | Ném ArgumentOutOfRangeException | **Pass** |
| **TC_MON_03** | Khởi tạo giá trị tiền tệ | Số tiền bằng 0 đồng (0 VNĐ - Miễn phí) | Loại tiền tệ "VND" | 1. Gán amount = 0<br>2. Khởi tạo new Money(0m, "VND") | `amount = 0m` | Khởi tạo thành công, Amount = 0, Currency = "VND" | Hỗ trợ nghiệp vụ gửi xe miễn phí | Khởi tạo thành công, Amount = 0 | **Pass** |
| **TC_MON_04** | Khởi tạo giá trị tiền tệ | Số tiền dương nhỏ nhất (0.01 VNĐ) | Loại tiền tệ "VND" | 1. Gán amount = 0.01<br>2. Khởi tạo new Money(0.01m, "VND") | `amount = 0.01m` | Khởi tạo thành công, Amount = 0.01, Currency = "VND" | Đối tượng Money được tạo hợp lệ | Khởi tạo thành công, Amount = 0.01 | **Pass** |
| **TC_MON_05** | Tính toán số tiền | Chặn cộng tiền khác loại tiền tệ | m1 = Money(100, "VND") và m2 = Money(5, "USD") | 1. Gọi phương thức m1.Add(m2) | `100 VND + 5 USD` | Ném ngoại lệ InvalidOperationException("Cannot add money with different currencies") | Ngăn chặn sai lệch tiền tệ | Ném InvalidOperationException | **Pass** |
| **TC_MON_06** | Tính toán số tiền | Cộng hai số tiền cùng loại tiền tệ | Hai số tiền 20.000 VNĐ và 30.000 VNĐ | 1. Tạo m1 = Money(20000, "VND")<br>2. Tạo m2 = Money(30000, "VND")<br>3. Tính sum = m1.Add(m2) | `m1 = 20000 VND`, `m2 = 30000 VND` | Kết quả sum.Amount = 50000, Currency = "VND" | Giá trị cộng chính xác tuyệt đối | sum.Amount = 50000, sum.Currency = "VND" | **Pass** |
| **TC_MON_07** | Khởi tạo giá trị tiền tệ | Khởi tạo tiền tệ 0 qua thuộc tính tiện ích Money.Zero | Tiền tệ "VND" | 1. Gọi Money.Zero() | `Money.Zero()` | Trả về đối tượng Money có Amount = 0, Currency = "VND" | Hỗ trợ gán tiền mặc định nhanh | Amount = 0, Currency = "VND" | **Pass** |
| **TC_TAR_01** | Tính phí đỗ xe | Tính phí theo giờ (PerHour) với thời gian lẻ (2.5 giờ) | Biểu phí theo giờ 15.000 VNĐ/giờ, PricingType = PerHour | 1. Tạo TimeSpan duration = 2.5 giờ<br>2. Gọi tariff.CalculateFee(duration) | `Price = 15000 VND`, `duration = 2.5h` | Làm tròn lên 3 block giờ đầy đủ, tiền phí = 15.000 x 3 = 45.000 VNĐ | Kết quả tiền phí = 45.000 VNĐ | Tiền phí tính ra đúng bằng 45.000 VNĐ | **Pass** |
| **TC_TAR_02** | Tính phí đỗ xe | Tính phí theo lượt (PerTurn) đỗ thời gian dài (8 giờ) | Biểu phí theo lượt 5.000 VNĐ/lượt, PricingType = PerTurn | 1. Tạo TimeSpan duration = 8 giờ<br>2. Gọi tariff.CalculateFee(duration) | `Price = 5000 VND`, `duration = 8h` | Thu cố định mức phí lượt = 5.000 VNĐ (không nhân theo thời gian đỗ) | Kết quả tiền phí = 5.000 VNĐ | Tiền phí tính ra đúng bằng 5.000 VNĐ | **Pass** |
| **TC_TAR_03** | Tính phí đỗ xe | Tính phí theo ngày (PerDay) với thời gian 36 giờ (1.5 ngày) | Biểu phí theo ngày 100.000 VNĐ/ngày, PricingType = PerDay | 1. Tạo TimeSpan duration = 36 giờ<br>2. Gọi tariff.CalculateFee(duration) | `Price = 100000 VND`, `duration = 36h` | Làm tròn lên 2 block ngày đầy đủ, tiền phí = 100.000 x 2 = 200.000 VNĐ | Kết quả tiền phí = 200.000 VNĐ | Tiền phí tính ra đúng bằng 200.000 VNĐ | **Pass** |
| **TC_TAR_04** | Tính phí đỗ xe | Tính phí theo giờ cho thời gian đỗ cực ngắn (12 phút) | Biểu phí theo giờ 20.000 VNĐ/giờ, PricingType = PerHour | 1. Tạo TimeSpan duration = 12 phút<br>2. Gọi tariff.CalculateFee(duration) | `Price = 20000 VND`, `duration = 12m` | Không áp dụng ân hạn miễn phí, tính đủ 1 block giờ đầu tiên = 20.000 VNĐ | Kết quả tiền phí = 20.000 VNĐ | Tiền phí tính ra đúng bằng 20.000 VNĐ | **Pass** |

---

## 5. Phân Hệ 5: Quy Tắc Nghiệp Vụ, Chống Trùng Lặp & Phân Quyền (12 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_BUS_01** | Xóa bãi đỗ xe | Xóa bãi đỗ xe khi đang có phương tiện gửi (CurrentParked > 0) | Bãi xe đang có 1 xe đỗ bên trong | 1. Gửi lệnh DeleteParkingLotCommand cho bãi xe này | `ParkingLotId` có xe đỗ | Ném ngoại lệ BusinessRuleViolationException("Cannot delete parking lot with currently parked vehicles"), từ chối xóa (HTTP 400) | Bãi xe không bị xóa khỏi cơ sở dữ liệu | Ném BusinessRuleViolationException | **Pass** |
| **TC_BUS_02** | Xóa bãi đỗ xe | Xóa cứng bãi xe khi chưa từng có lịch sử gửi xe (Hard Delete) | Bãi xe chưa từng phát sinh bất kỳ lượt gửi nào | 1. Gửi lệnh DeleteParkingLotCommand cho bãi xe này | `ParkingLotId` chưa có session | Xóa cứng bãi xe thành công, phản hồi HTTP 204 No Content | Bãi xe bị xóa hoàn toàn khỏi cơ sở dữ liệu | Bãi xe được xóa sạch khỏi cơ sở dữ liệu | **Pass** |
| **TC_BUS_03** | Xóa bãi đỗ xe | Xóa mềm bãi xe khi đã có lịch sử gửi xe trong quá khứ (Soft Delete) | Bãi 0 xe đỗ nhưng đã có lịch sử phiên gửi xe | 1. Thực hiện lệnh xóa bãi xe đã có lịch sử | `ParkingLotId` có lịch sử | Bãi xe được chuyển sang trạng thái xóa mềm (`IsActive = false`), bảo toàn dữ liệu lịch sử | Bãi xe không bị xóa vật lý, `IsActive = false` | Bãi xe được vô hiệu hóa an toàn, không mất dữ liệu | **Pass** |
| **TC_BUS_04** | Xóa đơn vị kinh doanh | Xóa đơn vị kinh doanh khi vẫn còn bãi xe trực thuộc hoạt động | Đơn vị kinh doanh đang sở hữu ít nhất 1 bãi xe đang hoạt động | 1. Gửi lệnh DeleteBusinessOperatorCommand | `BusinessOperatorId` còn bãi xe | Ném ngoại lệ BusinessRuleViolationException("Cannot delete business operator that still has active parking lots"), từ chối xóa (HTTP 400) | Đơn vị kinh doanh không bị xóa | Ném BusinessRuleViolationException | **Pass** |
| **TC_BUS_05** | Xóa đơn vị kinh doanh | Xóa đơn vị kinh doanh khi không còn bãi xe nào trực thuộc | Đơn vị kinh doanh không có bãi xe nào | 1. Gửi lệnh DeleteBusinessOperatorCommand | `BusinessOperatorId` không có bãi xe | Xóa đơn vị kinh doanh thành công, trả về HTTP 204 No Content / 200 OK | Đơn vị kinh doanh bị xóa khỏi cơ sở dữ liệu | Xóa thành công | **Pass** |
| **TC_BUS_06** | Lập báo cáo hoạt động bãi xe | Lập báo cáo khi ngày bắt đầu lớn hơn ngày kết thúc (FromDate > ToDate) | Đã có dữ liệu hoạt động trong bãi xe | 1. Gọi GetActivityReportQuery với fromDate = ngày mai, toDate = hôm nay | `fromDate > toDate` | Ném ngoại lệ BusinessRuleViolationException("Start date cannot be after end date"), từ chối truy vấn (HTTP 400) | Không sinh dữ liệu báo cáo | Ném BusinessRuleViolationException | **Pass** |
| **TC_BUS_07** | Lập báo cáo hoạt động bãi xe | Lập báo cáo cho khoảng thời gian không có lượt gửi xe nào | Khoảng thời gian hợp lệ trong quá khứ không có xe ra vào | 1. Gọi GetActivityReportQuery với khoảng thời gian này | `fromDate`, `toDate` trong quá khứ | Trả về null an toàn hoặc báo cáo rỗng với TotalEntries = 0, TotalExits = 0, TotalRevenue = 0 (HTTP 200) | Báo cáo an toàn không gây lỗi hệ thống | Xử lý an toàn, không có dữ liệu rác | **Pass** |
| **TC_BUS_08** | Chu trình gửi xe trọn vẹn | Luồng trọn vẹn từ lúc xe vào đến lúc xe ra và thanh toán | Bãi xe có 100 chỗ trống, biểu phí theo lượt 5.000 VNĐ | 1. Gửi CheckInCommand cho xe 59-99988<br>2. Kiểm tra bãi giảm còn 99 chỗ<br>3. Gửi CheckOutByPlateCommand cho xe 59-99988 | Xe máy 59-99988, biểu phí 5.000 VNĐ | Check-in trả về HTTP 201; Check-out trả về HTTP 200, phí 5.000 VNĐ, bãi xe phục hồi đủ 100 chỗ | Phiên hoàn thành, bãi xe phục hồi 100 chỗ trống | Chu trình thành công 100%, phí 5.000 VNĐ, AvailableSpots = 100 | **Pass** |
| **TC_BUS_09** | Chống trùng lặp dữ liệu bãi xe | Tạo bãi xe mới có tên trùng lặp trong cùng một Đơn vị kinh doanh | Đơn vị kinh doanh A đã có bãi xe tên "Bến Thành Central" | 1. Gửi CreateParkingLotCommand với cùng OperatorId và cùng tên bãi xe | `OperatorId = A`, `Name = "Bến Thành Central"` | Ném ngoại lệ BusinessRuleViolationException("Parking lot name already exists for this operator"), từ chối tạo trùng (HTTP 400) | Không có bãi xe trùng tên nào được tạo | Ném BusinessRuleViolationException | **Pass** |
| **TC_BUS_10** | Chống trùng lặp tài khoản | Tạo đơn vị kinh doanh mới với tên đăng nhập (Username) đã tồn tại | Hệ thống đã có đơn vị kinh doanh với Username = "sgparking" | 1. Gửi CreateBusinessOperatorCommand với Username = "sgparking" | `Username = "sgparking"` | Ném ngoại lệ BusinessRuleViolationException("Username already exists in the system"), từ chối đăng ký (HTTP 400) | Không tạo đơn vị kinh doanh trùng lặp | Ném BusinessRuleViolationException | **Pass** |
| **TC_BUS_11** | Kiểm tra quyền sở hữu (Ownership) | Nhà điều hành A cố tình sửa thông tin bãi xe của Nhà điều hành B | Tài khoản đăng nhập thuộc Operator A, bãi xe thuộc sở hữu của Operator B | 1. Gửi lệnh UpdateParkingLotCommand sửa bãi xe của người khác | `OperatorId != Lot.BusinessOperatorId` | Ném ngoại lệ UnauthorizedAccessException, từ chối quyền thao tác (HTTP 403 Forbidden) | Bãi xe của Operator B không bị thay đổi | Ném UnauthorizedAccessException với thông báo phù hợp | **Pass** |
| **TC_BUS_12** | Phân quyền chức vụ (RBAC) | Nhân viên bãi xe (Attendant) cố ý gửi lệnh xóa biểu phí | Tài khoản đăng nhập có vai trò `Attendant` | 1. Gửi lệnh DeleteTariffCommand | Role = `Attendant` | Ném ngoại lệ UnauthorizedAccessException, chặn thao tác xóa giá của nhân viên (HTTP 403 Forbidden) | Biểu phí không bị xóa | Ném UnauthorizedAccessException | **Pass** |

---

## 6. Phân Hệ 6: Đối Kháng, Tranh Chấp Đồng Thời & Rollback (4 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_CON_01** | Kiểm soát tranh chấp cổng vào (Race Condition) | Hai request check-in cùng lúc khi bãi chỉ còn đúng 1 chỗ trống duy nhất | Bãi xe có TotalCapacity = 10, AvailableSpots = 1 | 1. Phát đồng thời 2 task Check-in bằng Task.WhenAll cho 2 xe khác nhau<br>2. Kiểm tra kết quả 2 task | Xe 1: 59-11111<br>Xe 2: 59-22222 | Đúng 1 request thành công (HTTP 201), 1 request thất bại ném ngoại lệ (HTTP 400/409), AvailableSpots = 0 | Bãi xe có AvailableSpots = 0, không bị âm số chỗ | 1 task thành công, 1 task nhận ngoại lệ, AvailableSpots = 0 | **Pass** |
| **TC_CON_02** | Kiểm soát request lặp lại (Idempotency) | Hai request check-out liên tiếp cùng gọi trên một phiên gửi xe | Phiên gửi xe đang ở trạng thái Active | 1. Gửi request Check-out thứ nhất thành công<br>2. Gửi tiếp request Check-out thứ hai trên cùng phiên đó | `SessionId` hợp lệ | Lần 1 trả về HTTP 200 OK; Lần 2 ném BusinessRuleViolationException, từ chối thanh toán trùng lặp (HTTP 400) | Trạng thái phiên giữ nguyên Completed, số tiền không bị trừ 2 lần | Lần 1 thành công, lần 2 ném BusinessRuleViolationException | **Pass** |
| **TC_CON_03** | Chống gian lận biển số xe | Xe đang đỗ trong bãi cố tình gửi request check-in lần thứ hai | Xe biển số 59-99988 đang có một phiên đỗ Active trong bãi | 1. Gửi tiếp CheckInCommand cho xe biển số 59-99988 vào cùng bãi xe đó | Biển số "59-99988" | Ném ngoại lệ BusinessRuleViolationException("Vehicle is currently parked in this parking lot"), từ chối check-in (HTTP 400) | Không tạo thêm phiên đỗ thứ hai, sức chứa bãi không bị trừ sai | Ném BusinessRuleViolationException | **Pass** |
| **TC_CON_04** | Tính nguyên tử và Rollback | Lỗi xảy ra trong quá trình ghi dữ liệu, cơ chế Rollback hoạt động | Bãi xe có 50 chỗ trống | 1. Mở Database Transaction<br>2. Giảm chỗ trống bãi xe<br>3. Gặp lỗi ngoại lệ trước khi commit<br>4. Thực thi transaction.RollbackAsync() | Exception phát sinh giữa chu trình | Toàn bộ thay đổi bị hủy bỏ, số chỗ trống bãi xe phục hồi nguyên vẹn 50, không có bản ghi mồ côi | Dữ liệu bãi xe và phiên đỗ trở về trạng thái ban đầu | Rollback hoàn tất, AvailableSpots phục hồi = 50, không có phiên mồ côi | **Pass** |

---

## 7. Phân Hệ 7: Xác Thực & Cấp Quyền Đăng Nhập (3 Ca)

| TEST CASE ID | TEST SCENARIO | TEST CASE | PRE-CONDITION | TEST STEPS | TEST DATA | EXPECTED RESULT | POST CONDITION | ACTUAL RESULT | STATUS (PASS/FAIL) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **TC_AUTH_01** | Đăng nhập hệ thống | Đăng nhập thành công với tài khoản Quản trị viên (Manager) | Tài khoản admin đã tồn tại với mật khẩu admin@123 | 1. Gửi LoginDto với username = "admin", password = "admin@123"<br>2. Gọi AuthService.LoginAsync() | Username: "admin"<br>Password: "admin@123" | Đăng nhập thành công (HTTP 200), trả về Role = "Manager", chuỗi Token JWT hợp lệ | Người dùng nhận token xác thực vai trò Manager | Trả về Role Manager, JWT token sinh thành công | **Pass** |
| **TC_AUTH_02** | Đăng nhập hệ thống | Đăng nhập thành công với tài khoản Nhà điều hành bãi xe (BusinessOperator) | Tài khoản sgparking đã tồn tại với mật khẩu 123456 | 1. Gửi LoginDto với username = "sgparking", password = "123456"<br>2. Gọi AuthService.LoginAsync() | Username: "sgparking"<br>Password: "123456" | Đăng nhập thành công (HTTP 200), trả về Role = "BusinessOperator", chuỗi Token JWT hợp lệ | Người dùng nhận token xác thực vai trò BusinessOperator | Trả về Role BusinessOperator, JWT token sinh thành công | **Pass** |
| **TC_AUTH_03** | Đăng nhập hệ thống | Đăng nhập thất bại khi nhập sai mật khẩu | Tài khoản admin tồn tại | 1. Gửi LoginDto với username = "admin", password = "wrong_password"<br>2. Gọi AuthService.LoginAsync() | Username: "admin"<br>Password: "wrong_password" | Ném ngoại lệ BusinessRuleViolationException("Invalid username or password"), từ chối cấp token (HTTP 400) | Không có token nào được cấp, đăng nhập bị từ chối | Ném BusinessRuleViolationException | **Pass** |

---

## 8. Bảng Ma Trận Tổng Hợp 55 Ca Kiểm Thử

| Phân Hệ | Số Ca Đơn Lẻ (`Fact`) | Số Bộ Dữ Liệu Biên (`Theory`) | Tổng Số Ca Thực Thi | Trạng Thái | Tỷ Lệ Đạt |
| :--- | :---: | :---: | :---: | :---: | :---: |
| **1. Sức chứa bãi xe & Biên** | 9 ca | 2 bộ dữ liệu (2 giá trị) | **11 ca** | Pass | 100% |
| **2. Lượt gửi xe & Trạng thái** | 4 ca | 0 bộ | **4 ca** | Pass | 100% |
| **3. Định dạng Biển số xe** | 1 ca | 3 bộ dữ liệu (9 giá trị) | **10 ca** | Pass | 100% |
| **4. Giá trị Tiền tệ & Biểu phí** | 9 ca | 1 bộ dữ liệu (2 giá trị) | **11 ca** | Pass | 100% |
| **5. Nghiệp vụ, Chống trùng lặp & Phân quyền** | 12 ca | 0 bộ | **12 ca** | Pass | 100% |
| **6. Đối kháng, Tranh chấp & Rollback** | 4 ca | 0 bộ | **4 ca** | Pass | 100% |
| **7. Xác thực & Phân quyền đăng nhập** | 3 ca | 0 bộ | **3 ca** | Pass | 100% |
| **TỔNG CỘNG HỆ THỐNG** | **42 ca** | **13 ca từ Theory** | **55 CA** | **PASS** | **100%** |

---

## 9. Bằng Chứng Thực Nghiệm & Log Thực Thi Từ Hệ Thống

```text
================================================================================
THÔNG TIN PHIÊN KIỂM THỬ HỆ THỐNG (TEST EXECUTION SESSION)
Thời điểm thực thi: 2026-10-14 08:13:23 (GMT+7)
Môi trường: .NET SDK 10.0.300 / Microsoft .NET Core 9.0 (x64)
Test Runner: xUnit.net VSTest Adapter v2.8.2 (64-bit .NET 9.0.16)
Core Framework: xUnit.net Core Framework v2.9.2
Nhánh Git: refactor/cqrs-endpoints
Mã băm commit tương ứng: 65a6270b22a01bc15456b3c9429188e4420077ab
Lệnh thực thi: dotnet test Backend/ParkingManagement.Tests/ParkingManagement.Tests.csproj
================================================================================

[xUnit.net]   Discovering: ParkingManagement.Tests
[xUnit.net]   Discovered:  55 test cases
[xUnit.net]   Starting:    ParkingManagement.Tests

  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_Capacity_NegativeValue(-50) [< 5 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_Capacity_NegativeValue(-1) [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_Capacity_Zero [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_Capacity_MinimumValid_One [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_Capacity_VeryLargeValue [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_RecordVehicleEntry_WhenEmpty [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_RecordVehicleEntry_WhenOneSpotLeft [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_RecordVehicleEntry_WhenCompletelyFull [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_RecordVehicleExit_WhenOneCarParked [< 3 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingLot_RecordVehicleExit_WhenAlreadyEmpty [< 1 ms]
  Passed ParkingManagement.Tests.DomainTests.ParkingLot_RecordVehicleEntry_WhenInactive [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingSession_CheckOut_OnActiveSession [< 3 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingSession_CheckOut_AlreadyCompleted [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingSession_CheckOut_WithNullFee [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.ParkingSession_Creation_EmptyForeignKeys [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_EmptyProvinceCode("") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_EmptyProvinceCode("   ") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_EmptyProvinceCode(null) [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_EmptyPlateNumber("") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_EmptyPlateNumber("   ") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_EmptyPlateNumber(null) [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_ValidBoundaries("59", "1234") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_ValidBoundaries("29A", "99988") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_ValidBoundaries("80AA", "123456") [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.LicensePlate_CaseInsensitiveEquality [< 3 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.Money_NegativeAmount(-0.01) [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.Money_NegativeAmount(-1000) [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.Money_ZeroAmount_IsValid [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.Money_SmallestPositiveAmount_IsValid [< 1 ms]
  Passed ParkingManagement.Tests.BoundaryValueAnalysisTests.Money_Add_DifferentCurrencies [< 1 ms]
  Passed ParkingManagement.Tests.DomainTests.Money_Add_SameCurrency [< 6 ms]
  Passed ParkingManagement.Tests.DomainTests.Money_Zero_HasZeroAmount [< 1 ms]
  Passed ParkingManagement.Tests.DomainTests.Tariff_CalculateFee_PerHour [< 1 ms]
  Passed ParkingManagement.Tests.DomainTests.Tariff_CalculateFee_PerTurn [< 1 ms]
  Passed ParkingManagement.Tests.DomainTests.Tariff_CalculateFee_PerDay [< 1 ms]
  Passed ParkingManagement.Tests.DomainTests.Tariff_CalculateFee_PerHour_ShortDuration [< 1 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.DeleteParkingLot_WhenVehiclesParked [< 15 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.DeleteParkingLot_WhenNoVehiclesParked [< 33 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.DeleteParkingLot_WhenHasPastSessions_PerformsSoftDelete [< 20 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.DeleteBusinessOperator_WhenActiveLotsExist [< 1 s]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.DeleteBusinessOperator_WhenNoActiveLots [< 28 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.Report_InvalidDateRange [< 12 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.Report_NoDataFound [< 30 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.CheckIn_And_CheckOut_FullCycle [< 55 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.CreateParkingLot_DuplicateNameSameOperator [< 68 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.CreateBusinessOperator_DuplicateUsername [< 41 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.UpdateParkingLot_WhenOperatorIsNotOwner [< 18 ms]
  Passed ParkingManagement.Tests.BusinessUseCasesTests.DeleteTariff_WhenAttendantRole [< 10 ms]
  Passed ParkingManagement.Tests.ConcurrencyAndIntegrityTests.CheckIn_WhenOnlyOneSpotAvailable_SecondRequestThrows [< 19 ms]
  Passed ParkingManagement.Tests.ConcurrencyAndIntegrityTests.CheckOut_RepeatedRequest_SameSession [< 87 ms]
  Passed ParkingManagement.Tests.ConcurrencyAndIntegrityTests.CheckIn_SameLicensePlate_WhileAlreadyActive [< 40 ms]
  Passed ParkingManagement.Tests.ConcurrencyAndIntegrityTests.CheckIn_Rollback_WhenExceptionOccurs_NoOrphanSession [< 1 s]
  Passed ParkingManagement.Tests.AuthServiceTests.LoginAsync_ValidAdmin [< 1 s]
  Passed ParkingManagement.Tests.AuthServiceTests.LoginAsync_ValidBusinessOperator [< 16 ms]
  Passed ParkingManagement.Tests.AuthServiceTests.LoginAsync_WrongPassword [< 77 ms]

================================================================================
KẾT QUẢ TỔNG HỢP:
Tổng số ca kiểm thử: 55
Đạt (Passed): 55 (100%)
Thất bại (Failed): 0 (0%)
Bỏ qua (Skipped): 0
Tổng thời gian thực thi: 1.0 giây
Trạng thái kiểm tra: ĐẠT TOÀN DIỆN (TEST RUN SUCCESSFUL)
================================================================================
```
