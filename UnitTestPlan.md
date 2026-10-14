# Smart Parking Management - Kế Hoạch Kiểm Thử Toàn Diện (Comprehensive Test Plan)
**Dự án:** Hệ thống Quản lý Bãi đỗ xe Thông minh (Smart Parking Management)  
**Kiến trúc:** Clean Architecture, CQRS (MediatR), REPR Minimal API Endpoints  
**Phương pháp kiểm thử:** Phân tích Giá trị Biên (BVA), Phân vùng Tương đương (EP), Kiểm thử Đối kháng (Adversarial Testing) & Kiểm thử Tích hợp (Integration Testing)  
**Công cụ thực thi:** xUnit Core Framework (v2.9.2), xUnit.net VSTest Adapter (v2.8.2), .NET 9.0 SDK, SQLite In-Memory Database  
**Môi trường kiểm thử:** Nhánh `refactor/cqrs-endpoints`  
**Phiên bản kế hoạch:** 2.0 / 14-10-2026  
**Tổng số ca kiểm thử thực thi:** 55 ca kiểm thử (Khớp 100% với `TestCases.md`)  

---

## Mục Lục (Table of Contents)

1. [Mục Tiêu & Phạm Vi Kiểm Thử (Objectives & Scope)](#1-mục-tiêu--phạm-vi-kiểm-thử-objectives--scope)
2. [Chiến Lược & Phương Pháp Kiểm Thử (Test Strategy & Methodology)](#2-chiến-lược--phương-pháp-kiểm-thử-test-strategy--methodology)
3. [Quy Chuẩn Cấu Trúc Kiểm Thử (AAA Standard)](#3-quy-chuẩn-cấu-trúc-kiểm-thử-aaa-standard)
4. [Kế Hoạch & Ma Trận 55 Ca Kiểm Thử Chi Tiết](#4-kế-hoạch--ma-trận-55-ca-kiểm-thử-chi-tiết)
   - [4.1 Phân hệ Sức chứa Bãi xe & Biên (11 Ca)](#41-phân-hệ-sức-chứa-bãi-xe--biên-11-ca)
   - [4.2 Phân hệ Lượt gửi xe & Trạng thái (4 Ca)](#42-phân-hệ-lượt-gửi-xe--trạng-thái-4-ca)
   - [4.3 Phân hệ Định dạng Biển số xe (10 Ca)](#43-phân-hệ-định-dạng-biển-số-xe-10-ca)
   - [4.4 Phân hệ Tiền tệ & Biểu phí (11 Ca)](#44-phân-hệ-tiền-tệ--biểu-phí-11-ca)
   - [4.5 Phân hệ Nghiệp vụ, Chống trùng lặp & Phân quyền (12 Ca)](#45-phân-hệ-nghiệp-vụ-chống-trùng-lặp--phân-quyền-12-ca)
   - [4.6 Phân hệ Đối kháng, Tranh chấp đồng thời & Rollback (4 Ca)](#46-phân-hệ-đối-kháng-tranh-chấp-đồng-thời--rollback-4-ca)
   - [4.7 Phân hệ Xác thực & Phân quyền đăng nhập (3 Ca)](#47-phân-hệ-xác-thực--phân-quyền-đăng-nhập-3-ca)
5. [Môi Trường, Công Cụ & Quy Trình Thực Thi](#5-môi-trường-công-cụ--quy-trình-thực-thi)
6. [Tiêu Chuẩn Nghiệm Thu & Quản Lý Rủi Ro (Acceptance Criteria & Risks)](#6-tiêu-chuẩn-nghiệm-thu--quản-lý-rủi-ro-acceptance-criteria--risks)

---

## 1. Mục Tiêu & Phạm Vi Kiểm Thử (Objectives & Scope)

### 1.1 Mục Tiêu
- Đảm bảo tính đúng đắn và toàn vẹn của logic nghiệp vụ cốt lõi (Domain Entities, Value Objects) và tầng ứng dụng (Application CQRS Commands/Queries/Handlers).
- Ngăn chặn lỗi "off-by-one" (lệch 1 đơn vị), lỗi tràn số hoặc lỗi số lượng âm thông qua **Phân tích Giá trị Biên (BVA)**.
- Đảm bảo an toàn dữ liệu trong các tình huống **tranh chấp đồng thời (Concurrency)**, chính sách **Xóa mềm (Soft Delete)** bảo toàn lịch sử và tính **nguyên tử (Atomicity/Rollback)** của giao dịch.
- Đối soát 100% hợp đồng API quy định trong `TaskAPISpec.md` với các mã phản hồi HTTP: `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `403 Forbidden`, `404 Not Found`.

### 1.2 Phạm Vi Kiểm Thử (Scope)
- **Tầng Đối Tượng Miền (Domain Layer):**
  - Thực thể `ParkingLot`: Sức chứa (`TotalCapacity`), chỗ trống (`AvailableSpots`), tăng/giảm chỗ khi xe ra vào, chặn xe khi bãi đầy/ngừng hoạt động.
  - Thực thể `ParkingSession`: Vòng đời trạng thái (`Active` ➔ `Completed`), ghi nhận thời gian, tính tiền, chặn check-out lặp lại.
  - Đối tượng giá trị `LicensePlate`: Tính hợp lệ mã tỉnh, số xe, chuẩn hóa in hoa, tính bất biến và so sánh không phân biệt hoa thường.
  - Đối tượng giá trị `Money`: Chặn số tiền âm, hỗ trợ tiền 0 đồng, cộng cùng loại tiền tệ, chặn cộng lệch loại tiền tệ.
  - Thực thể `Tariff`: Tính phí theo giờ (`PerHour`) có làm tròn block giờ, tính phí đồng giá theo lượt (`PerTurn`), tính phí theo ngày (`PerDay`), và xử lý thời gian đỗ cực ngắn dưới 15 phút.
- **Tầng Nghiệp Vụ & CQRS (Application Layer):**
  - Chống trùng lặp tên bãi xe của cùng một nhà điều hành (`CreateParkingLotCommand`).
  - Chống trùng lặp tên đăng nhập tài khoản đơn vị kinh doanh (`CreateBusinessOperatorCommand`).
  - Chính sách Xóa mềm và Xóa cứng: Bãi xe chưa có lịch sử thì xóa cứng (`204 No Content`), bãi xe đã có lịch sử thì xóa mềm (`IsActive = false`), bãi còn xe đỗ thì chặn xóa (`400 Bad Request`).
  - Kiểm tra thẩm quyền và quyền sở hữu (Ownership / RBAC Check): Nhà điều hành không được sửa bãi xe của người khác (`403 Forbidden`), nhân viên không được xóa biểu phí (`403 Forbidden`).
  - Lập báo cáo hoạt động bãi xe và xử lý khoảng thời gian không hợp lệ (`GetActivityReportQuery`).
- **Kiểm Thử Đối Kháng & Đồng Thời (Concurrency & Adversarial):**
  - Tranh chấp cổng vào: Hai xe cùng check-in khi bãi còn đúng 1 chỗ trống duy nhất.
  - Gửi đúp request: Hai lệnh check-out cùng gọi trên một phiên gửi xe.
  - Chống gian lận: Xe đang đỗ cố tình quét thẻ check-in lần thứ hai.
  - Cơ chế Rollback: Hủy bỏ toàn bộ giao dịch khi gặp lỗi giữa chừng, không để lại bản ghi rác.

---

## 2. Chiến Lược & Phương Pháp Kiểm Thử (Test Strategy & Methodology)

### 2.1 Kỹ Thuật Phân Tích Giá Trị Biên (Boundary Value Analysis - BVA)
Tập trung vào 6 điểm biên: $Min - 1, Min, Min + 1, Max - 1, Max, Max + 1$ áp dụng cho sức chứa bãi xe, số tiền, và độ dài biển số.

### 2.2 Kỹ Thuật Phân Vùng Tương Đương (Equivalence Partitioning - EP)
Chia miền dữ liệu thành các lớp hợp lệ và không hợp lệ:
- Định dạng biển số xe: Chuỗi rỗng, khoảng trắng, null (không hợp lệ); biển 2 ký tự tỉnh + 4 số, 3 ký tự + 5 số, 4 ký tự + 6 số (hợp lệ).
- Giá trị tiền tệ: Số âm (không hợp lệ); 0 đồng, số dương (hợp lệ).

---

## 3. Quy Chuẩn Cấu Trúc Kiểm Thử (AAA Standard)

Tất cả 55 ca kiểm thử trong kế hoạch đều tuân thủ nghiêm ngặt mô hình **AAA**:
- **Arrange (Chuẩn bị):** Khởi tạo môi trường SQLite In-memory, nạp dữ liệu mẫu ban đầu, thiết lập bãi xe và tài khoản.
- **Act (Thực thi):** Kích hoạt phương thức xử lý nghiệp vụ hoặc lệnh Command/Query cần kiểm thử.
- **Assert (Xác minh):** Kiểm tra trạng thái thực tế sau xử lý, đối chiếu giá trị trả về và mã ngoại lệ mong đợi.

---

## 4. Kế Hoạch & Ma Trận 55 Ca Kiểm Thử Chi Tiết

### 4.1 Phân hệ Sức chứa Bãi xe & Biên (11 ca kiểm thử)
*Gồm 9 ca kiểm thử đơn lẻ (`Fact`) và 1 bộ dữ liệu biên (`Theory` gồm 2 giá trị).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Điều Kiện | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_LOT_01** | Sức chứa bãi xe số âm cận biên | `totalCapacity = -1` | Biên dưới không hợp lệ ($Min - 1$) | Ném `ArgumentOutOfRangeException` (400) |
| **TC_LOT_02** | Sức chứa bãi xe số âm lớn | `totalCapacity = -50` | Vùng giá trị âm không hợp lệ | Ném `ArgumentOutOfRangeException` (400) |
| **TC_LOT_03** | Sức chứa bãi xe bằng 0 | `totalCapacity = 0` | Biên dưới không hợp lệ | Ném `ArgumentOutOfRangeException` (400) |
| **TC_LOT_04** | Sức chứa nhỏ nhất hợp lệ | `totalCapacity = 1` | Biên tối thiểu hợp lệ ($Min = 1$) | Tạo bãi thành công, chỗ trống = 1 (201) |
| **TC_LOT_05** | Sức chứa cực đại | `totalCapacity = 50000` | Biên tối đa quy mô lớn ($Max$) | Tạo bãi thành công, chỗ trống = 50.000 (201) |
| **TC_LOT_06** | Xe vào khi bãi đang trống | `totalCapacity = 100`, 0 xe đỗ | Mốc bắt đầu vận hành | Chỗ trống giảm còn 99, xe đỗ = 1 (201) |
| **TC_LOT_07** | Xe vào khi còn đúng 1 chỗ | `totalCapacity = 100`, đã có 99 xe | Cận trên trước khi đầy ($Max - 1$) | Chỗ trống đạt mốc 0, xe đỗ = 100 (201) |
| **TC_LOT_08** | Cho xe vào khi bãi đã đầy 100% | Bãi 100 chỗ, đã có 100 xe đỗ | Vượt quá sức chứa ($Max + 1$) | Ném `BusinessRuleViolationException` (400) |
| **TC_LOT_09** | Xe ra khi bãi đang có đúng 1 xe | Bãi 100 chỗ, đang có 1 xe đỗ | Khôi phục mốc rỗng hoàn toàn | Chỗ trống khôi phục 100, xe đỗ = 0 (200) |
| **TC_LOT_10** | Xe ra khi bãi vốn dĩ đã rỗng 0 xe | Bãi 100 chỗ, 0 xe đỗ | Chặn tràn số chỗ trống (Min = 0 xe) | Chỗ trống giữ nguyên 100, xe đỗ = 0 (200) |
| **TC_LOT_11** | Xe vào khi bãi ngừng hoạt động | `isActive = false` | Trạng thái bãi đóng cửa | Ném `BusinessRuleViolationException` (400) |

---

### 4.2 Phân hệ Lượt gửi xe & Trạng thái (4 ca kiểm thử)
*Gồm 4 ca kiểm thử đơn lẻ (`Fact`).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Điều Kiện | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_SES_01** | Check-out hợp lệ trên phiên Active | Phiên đang gửi, phí 15.000đ | Chuyển đổi trạng thái kết thúc | Trạng thái `Completed`, phí 15.000đ (200) |
| **TC_SES_02** | Check-out lần 2 trên cùng 1 phiên | Phiên đã có trạng thái `Completed` | Biên máy trạng thái (State Machine) | Ném `BusinessRuleViolationException` (400) |
| **TC_SES_03** | Check-out với đối tượng phí rỗng | `fee = null` | Tham số bắt buộc bị rỗng | Ném `ArgumentNullException` (400) |
| **TC_SES_04** | Tạo phiên với khóa ngoại rỗng | `parkingLotId = Guid.Empty` | Khóa ngoại bắt buộc | Ném `ArgumentException` (400) |

---

### 4.3 Phân hệ Định dạng Biển số xe (10 ca kiểm thử)
*Gồm 1 ca đơn lẻ (`Fact`) và 3 bộ dữ liệu biên (`Theory` gồm 9 giá trị).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Điều Kiện | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_PLT_01** | Mã tỉnh là chuỗi rỗng | `provinceCode = ""` | Chiều dài bằng 0 ($Len = 0$) | Ném `ArgumentException` (400) |
| **TC_PLT_02** | Mã tỉnh chỉ có khoảng trắng | `provinceCode = "   "` | Chuỗi không có ký tự thực | Ném `ArgumentException` (400) |
| **TC_PLT_03** | Mã tỉnh là null | `provinceCode = null` | Giá trị null | Ném `ArgumentException` (400) |
| **TC_PLT_04** | Số xe là chuỗi rỗng | `plateNumber = ""` | Chiều dài bằng 0 ($Len = 0$) | Ném `ArgumentException` (400) |
| **TC_PLT_05** | Số xe chỉ có khoảng trắng | `plateNumber = "   "` | Chuỗi không có ký tự thực | Ném `ArgumentException` (400) |
| **TC_PLT_06** | Số xe là null | `plateNumber = null` | Giá trị null | Ném `ArgumentException` (400) |
| **TC_PLT_07** | Biển số ngắn nhất hợp lệ | Mã tỉnh "59", Số xe "1234" | Biên ngắn nhất ($Min$) | Định dạng chuẩn "59-1234" (Hợp lệ) |
| **TC_PLT_08** | Biển số tiêu chuẩn danh định | Mã tỉnh "29A", Số xe "99988" | Định dạng tiêu chuẩn ($Nominal$) | Định dạng chuẩn "29A-99988" (Hợp lệ) |
| **TC_PLT_09** | Biển số dài nhất hợp lệ | Mã tỉnh "80AA", Số xe "123456" | Biên dài nhất ($Max$) | Định dạng chuẩn "80AA-123456" (Hợp lệ) |
| **TC_PLT_10** | So sánh biển số không phân biệt hoa thường | "59-t1"/"1234" và "59-T1"/"1234" | Tính bất biến & Chuẩn hóa | Trả về `true` (Hai biển số bằng nhau) |

---

### 4.4 Phân hệ Tiền tệ & Biểu phí (11 ca kiểm thử)
*Gồm 9 ca đơn lẻ (`Fact`) và 1 bộ dữ liệu biên (`Theory` gồm 2 giá trị).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Điều Kiện | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_MON_01** | Số tiền âm cận biên 0 | `amount = -0.01` | Biên dưới không hợp lệ ($Min - 1$) | Ném `ArgumentOutOfRangeException` (400) |
| **TC_MON_02** | Số tiền âm lớn | `amount = -1000` | Giá trị âm không hợp lệ | Ném `ArgumentOutOfRangeException` (400) |
| **TC_MON_03** | Số tiền bằng 0 đồng (Miễn phí) | `amount = 0`, "VND" | Biên 0 đồng hợp lệ | Giá trị bằng 0, loại tiền "VND" (Hợp lệ) |
| **TC_MON_04** | Số tiền nhỏ nhất lớn hơn 0 | `amount = 0.01`, "VND" | Biên tối thiểu ($Min + 1$) | Giá trị bằng 0.01 VNĐ (Hợp lệ) |
| **TC_MON_05** | Chặn cộng tiền khác đơn vị | 100 VND + 5 USD | Xung đột loại tiền tệ | Ném `InvalidOperationException` (400) |
| **TC_MON_06** | Cộng hai khoản tiền cùng đơn vị | 20.000đ + 30.000đ | Cùng đơn vị tiền tệ "VND" | Tổng đạt chính xác 50.000 VNĐ |
| **TC_MON_07** | Khởi tạo tiền tệ 0 qua Money.Zero | "VND" | Tiện ích khởi tạo nhanh | Giá trị bằng 0, loại tiền "VND" |
| **TC_TAR_01** | Tính phí theo giờ làm tròn block | Đỗ 2.5 giờ, đơn giá 15.000đ/giờ | Làm tròn lên 3 block giờ | Tổng phí đạt 45.000 VNĐ |
| **TC_TAR_02** | Tính phí đồng giá theo lượt | Đỗ 8 giờ, đơn giá 5.000đ/lượt | Biểu phí cố định `PerTurn` | Tổng phí giữ nguyên 5.000 VNĐ |
| **TC_TAR_03** | Tính phí theo ngày làm tròn | Đỗ 36 giờ (1.5 ngày), giá 100.000đ/ngày | Làm tròn lên 2 block ngày | Tổng phí đạt 200.000 VNĐ |
| **TC_TAR_04** | Tính phí thời gian cực ngắn | Đỗ 12 phút, giá 20.000đ/giờ | Tính tối thiểu 1 block giờ | Tổng phí đạt 20.000 VNĐ |

---

### 4.5 Phân hệ Nghiệp vụ, Chống trùng lặp & Phân quyền (12 ca kiểm thử)
*Gồm 12 ca kiểm thử đơn lẻ (`Fact`).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Điều Kiện | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_BUS_01** | Xóa bãi xe khi còn xe đang gửi | Bãi còn 1 xe đang đỗ bên trong | Ràng buộc an toàn tài sản | Ném `BusinessRuleViolationException` (400) |
| **TC_BUS_02** | Xóa cứng bãi xe chưa có lịch sử | Bãi có 0 xe đỗ, chưa có session | Điều kiện Hard Delete sạch | Xóa vật lý thành công (204 No Content) |
| **TC_BUS_03** | Xóa mềm bãi xe đã có lịch sử | Bãi 0 xe đỗ, đã có session cũ | Chính sách Soft Delete bảo toàn | Đặt `IsActive = false` thành công (204 / 200) |
| **TC_BUS_04** | Xóa đơn vị kinh doanh khi còn bãi xe | Đơn vị đang có bãi xe hoạt động | Ràng buộc quản lý pháp nhân | Ném `BusinessRuleViolationException` (400) |
| **TC_BUS_05** | Xóa đơn vị kinh doanh khi không còn bãi | Đơn vị có 0 bãi xe trực thuộc | Đủ điều kiện xóa | Xóa thành công (204 / 200) |
| **TC_BUS_06** | Lập báo cáo khi ngày bắt đầu > kết thúc | `fromDate > toDate` | Khoảng thời gian sai logic | Ném `BusinessRuleViolationException` (400) |
| **TC_BUS_07** | Lập báo cáo khi không có lượt gửi | Khoảng thời gian không có xe | Dữ liệu rỗng | Trả về tổng tiền 0đ, tổng lượt 0 (200) |
| **TC_BUS_08** | Chu trình gửi xe trọn vẹn | Check-in -> Kiểm tra -> Check-out | Luồng nghiệp vụ liên hoàn | Hoàn thành, phí 5.000đ, phục hồi chỗ (200) |
| **TC_BUS_09** | Trùng tên bãi xe của cùng chủ bãi | Cùng chủ bãi, cùng tên bãi xe | Chống trùng lặp tên bãi xe | Ném `BusinessRuleViolationException` (400) |
| **TC_BUS_10** | Trùng tên đăng nhập đơn vị kinh doanh | Cố tình tạo trùng `username` | Chống trùng lặp tài khoản | Ném `BusinessRuleViolationException` (400) |
| **TC_BUS_11** | Kiểm tra quyền sở hữu bãi xe (Ownership) | Operator A sửa bãi của Operator B | Phân quyền tài nguyên cấp máy chủ | Ném `UnauthorizedAccessException` (403) |
| **TC_BUS_12** | Phân quyền vai trò nhân viên (RBAC) | Attendant cố tình xóa biểu phí | Phân cấp thẩm quyền chức vụ | Ném `UnauthorizedAccessException` (403) |

---

### 4.6 Phân hệ Đối kháng, Tranh chấp đồng thời & Rollback (4 ca kiểm thử)
*Gồm 4 ca kiểm thử đơn lẻ (`Fact`).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Tình Huống Đối Kháng | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_CON_01** | Tranh chấp cổng vào khi còn 1 chỗ | Bãi 1 chỗ, 2 xe cùng vào cạnh tranh | Tranh chấp tài nguyên đồng thời | Xe 1 thành công (201), Xe 2 bị từ chối (400) |
| **TC_CON_02** | Request Check-out gửi lặp lại | 2 lệnh check-out trên cùng phiên | Chống trừ tiền hai lần | Lần 1 thành công (200), Lần 2 bị chặn (400) |
| **TC_CON_03** | Xe đang đỗ cố tình Check-in tiếp | Cùng 1 biển số xe check-in lần hai | Chống gian lận biển số xe | Ném `BusinessRuleViolationException` (400) |
| **TC_CON_04** | Rollback giao dịch khi gặp lỗi | Exception phát sinh trước khi commit | Tính nguyên tử hóa (Atomicity) | Phục hồi 50 chỗ, không có bản ghi mồ côi |

---

### 4.7 Phân hệ Xác thực & Phân quyền đăng nhập (3 ca kiểm thử)
*Gồm 3 ca kiểm thử đơn lẻ (`Fact`).*

| Mã Ca Kiểm Thử | Tên Mục Tiêu Kiểm Thử | Dữ Liệu Đầu Vào | Điểm Biên / Điều Kiện | Hợp Đồng HTTP / Ngoại Lệ Kỳ Vọng |
| :--- | :--- | :--- | :--- | :--- |
| **TC_AUTH_01** | Đăng nhập tài khoản Quản trị viên | Tài khoản `admin`, đúng mật khẩu | Cấp quyền quản trị tối cao | Trả về vai trò `Manager` và JWT Token (200) |
| **TC_AUTH_02** | Đăng nhập tài khoản Nhà điều hành | Tài khoản `sgparking`, đúng mật khẩu | Cấp quyền nhà điều hành | Trả về vai trò `BusinessOperator` và Token (200) |
| **TC_AUTH_03** | Đăng nhập thất bại khi sai mật khẩu | Tài khoản đúng, sai mật khẩu | Xác thực không hợp lệ | Ném `BusinessRuleViolationException` (400) |

---

## 5. Môi Trường, Công Cụ & Quy Trình Thực Thi

- **Hệ điều hành & SDK:** Windows, Microsoft .NET 9.0 SDK.
- **Thư viện Kiểm thử (Test Frameworks & Runners):**
  - `xUnit Core Framework` (v2.9.2): Thư viện định nghĩa ca kiểm thử (Fact, Theory, Assertions).
  - `xUnit.net VSTest Adapter` (v2.8.2): Adapter điều phối phiên thực thi kiểm thử tích hợp với .NET SDK.
  - `Microsoft.NET.Test.Sdk` (v17.12.0).
  - `Microsoft.EntityFrameworkCore.Sqlite` (v9.0.2): Cơ sở dữ liệu SQLite In-memory giả lập độc lập cho từng phiên test.
- **Quy trình Chạy Kiểm Thử Tự Động:**
  ```powershell
  # Lệnh thực thi toàn bộ 55 ca kiểm thử:
  dotnet test Backend/ParkingManagement.Tests/ParkingManagement.Tests.csproj --logger "console;verbosity=normal"
  ```

---

## 6. Tiêu Chuẩn Nghiệm Thu & Quản Lý Rủi Ro (Acceptance Criteria & Risks)

### 6.1 Tiêu Chuẩn Nghiệm Thu (Acceptance Criteria)
1. **Tỷ lệ Đạt bắt buộc:** 100% (Đạt đủ 55/55 ca kiểm thử, 0 ca thất bại, 0 ca bỏ qua).
2. **Thời gian thực thi:** Toàn bộ bộ kiểm thử 55 ca phải hoàn thành trong thời gian dưới 5 giây (Thực tế đạt: **1.0 giây**).
3. **Tính nhất quán của dữ liệu:** Các ca kiểm thử đối kháng và rollback phải chứng minh cơ sở dữ liệu không bao giờ bị âm số chỗ đỗ và không bao giờ sót bản ghi mồ côi (Orphan records).

### 6.2 Quản Lý Rủi Ro Kiểm Thử
| Rủi Ro | Khả Năng Xảy Ra | Mức Độ Ảnh Hưởng | Biện Pháp Kiểm Soát & Khắc Phục |
| :--- | :---: | :---: | :--- |
| **Khóa tài nguyên đồng thời khi test** | Trung bình | Cao | Sử dụng kết nối `DataSource=:memory:` độc lập cho mỗi class test kèm `Dispose` giải phóng sau mỗi ca chạy. |
| **Lệch múi giờ giữa máy phát triển và máy chủ** | Thấp | Cao | Toàn bộ Entity và Test đều sử dụng chuẩn `DateTime.UtcNow` tuyệt đối. |
| **Thiếu tính cô lập dữ liệu giữa các test** | Thấp | Nghiêm trọng | Áp dụng Transaction rollback và tái tạo cấu trúc bảng SQLite sạch trước mỗi lượt chạy test. |
