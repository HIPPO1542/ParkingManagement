# HỆ THỐNG QUẢN LÝ BÃI ĐỖ XE THÔNG MINH (SMART PARKING SYSTEM)

Hệ thống Web API và ứng dụng quản lý bãi đỗ xe thông minh xây dựng trên nền tảng **.NET 9 (C#)**, áp dụng các kiến trúc và mẫu thiết kế tiên tiến:
- **Clean Architecture (Onion Architecture)** phân tách 4 tầng độc lập.
- **Domain-Driven Design (DDD)** với các Aggregate Roots, Entities và Value Objects.
- **CQRS (Command Query Responsibility Segregation)** kết hợp **MediatR**.
- **REPR Pattern (Request-Endpoint-Response)** với **ASP.NET Core Minimal APIs**.
- Cơ sở dữ liệu **SQLite** quản lý bởi **Entity Framework Core**, kiểm soát tương tranh lạc quan (Optimistic Concurrency).
- Bộ kiểm thử tự động toàn diện **xUnit** với **55 ca kiểm thử đạt tỷ lệ Pass 100%**.

---

## 1. CẤU TRÚC DỰ ÁN (PROJECT STRUCTURE)

```
QL_BaiXe/
├── .vscode/                                      # Cấu hình IDE VS Code (Tasks, Launch Debugger)
│
├── Backend/                                      # MÃ NGUỒN HỆ THỐNG BACKEND (.NET 9)
│   ├── ParkingManagement.sln                    # Solution chính
│   │
│   ├── ParkingManagement.Domain/                 # [TẦNG 1] DOMAIN LAYER (Cốt lõi nghiệp vụ)
│   │   ├── Common/                               # IAggregateRoot, Entity base
│   │   ├── Entities/                             # Aggregate Roots (ParkingLot, BusinessOperator, ParkingSession, Tariff, VehicleType, SystemManager)
│   │   ├── Enums/                                # TariffType (PerHour, PerTurn, PerDay), SessionStatus
│   │   ├── Exceptions/                           # BusinessRuleViolationException, DomainException
│   │   ├── Repositories/                         # Repository Interfaces & IUnitOfWork
│   │   └── ValueObjects/                         # Value Objects bất biến (LicensePlate, Money, Address, ContactInfo)
│   │
│   ├── ParkingManagement.Application/            # [TẦNG 2] APPLICATION LAYER (Use Cases & CQRS)
│   │   ├── Common/                               # ApiResponse<T>, PagedResult<T>
│   │   ├── DTOs/                                 # Data Transfer Objects
│   │   ├── Features/                             # CQRS Features tổ chức theo từng Aggregate:
│   │   │   ├── Auth/                             # LoginCommand, GetCurrentUserProfileQuery
│   │   │   ├── BusinessOperators/                # Create, Update, Delete Operator (Commands/Queries)
│   │   │   ├── ParkingLots/                      # Create, Update, Delete Lot, GetLotById, GetLots (Commands/Queries)
│   │   │   ├── ParkingSessions/                  # CheckInCommand, CheckOutByIdCommand, CheckOutByPlateCommand, Queries
│   │   │   ├── Reports/                          # GetActivityReportQuery (Thống kê hoạt động, doanh thu)
│   │   │   ├── Tariffs/                          # Create, Update, Delete, GetTariffs (Commands/Queries)
│   │   │   └── VehicleTypes/                     # Create, GetAllVehicleTypes (Commands/Queries)
│   │   └── Services/                             # Token & Application Interfaces
│   │
│   ├── ParkingManagement.Infrastructure/         # [TẦNG 3] INFRASTRUCTURE LAYER (Hạ tầng kỹ thuật)
│   │   ├── Data/                                 # ParkingDbContext, DataSeeder (tự động nạp dữ liệu mẫu ban đầu)
│   │   ├── EntityConfigs/                        # EF Core Fluent API Configurations (Quan hệ, Indexes, Value Objects OwnsOne)
│   │   ├── Repositories/                         # Cài đặt thực tế Repository và UnitOfWork
│   │   └── Services/                             # JwtTokenGenerator, PasswordHasher
│   │
│   ├── ParkingManagement.Api/                    # [TẦNG 4] PRESENTATION LAYER (Minimal APIs)
│   │   ├── Endpoints/                            # Các Endpoint REPR độc lập:
│   │   │   ├── AuthEndpoints.cs                  # /api/auth (Login, Me)
│   │   │   ├── BusinessOperatorEndpoints.cs      # /api/business-operators
│   │   │   ├── ParkingLotEndpoints.cs            # /api/parking-lots
│   │   │   ├── ParkingSessionEndpoints.cs        # /api/parking-sessions (Check-in, Check-out)
│   │   │   ├── ReportEndpoints.cs                # /api/reports/activity
│   │   │   ├── TariffEndpoints.cs                # /api/tariffs
│   │   │   └── VehicleTypeEndpoints.cs           # /api/vehicle-types
│   │   ├── Middlewares/                          # GlobalExceptionMiddleware (Chuẩn hóa lỗi tiếng Việt)
│   │   └── Program.cs                            # Khởi tạo DI, Swagger UI, Middleware pipeline
│   │
│   └── ParkingManagement.Tests/                  # DỰ ÁN KIỂM THỬ TỰ ĐỘNG (55 TESTS - PASS 100%)
│       ├── AuthServiceTests.cs                   # Kiểm thử xác thực, phân quyền JWT, mã hóa mật khẩu
│       ├── BoundaryValueAnalysisTests.cs         # Phân tích giá trị biên 6 điểm (BVA) cho Dung lượng, Biển số, Tiền tệ
│       ├── BusinessUseCasesTests.cs              # Kiểm thử Use Cases, Soft/Hard Delete, Quyền sở hữu (Ownership), Báo cáo
│       ├── ConcurrencyAndIntegrityTests.cs       # Kiểm thử đối kháng (Race Condition 2 xe cùng vào), Rollback giao dịch
│       └── DomainTests.cs                        # Kiểm thử Entity Invariants và Công thức tính phí (Tariff Engine)
│
├── Database/                                     # DỮ LIỆU & SCRIPT KHỞI TẠO SQL
│   ├── init_schema.sql                           # Script DDL SQLite chuẩn
│   ├── parking.db                                # File cơ sở dữ liệu SQLite cục bộ
│   └── seed_parking_lots.sql                     # Script nạp danh mục mẫu
│
├── Frontend/                                     # GIAO DIỆN QUẢN TRỊ TRỰC QUAN (SPA)
│   ├── html/                                     # Các trang quản trị (checkin.html, dashboard.html, parking-lots.html...)
│   ├── js/                                       # Logic tương tác REST API (auth.js, checkin.js, api.js...)
│   └── index.html                                # Trang điều hướng trung tâm
│
├── TaskAPISpec.md                                # TÀI LIỆU ĐẶC TẢ CHI TIẾT API V2.0
├── UnitTestPlan.md                               # KẾ HOẠCH KIỂM THỬ TỔNG THỂ (MASTER TEST PLAN)
├── TestCases.md                                  # BÁO CÁO KẾT QUẢ 55 CA KIỂM THỬ (10 CỘT CHUẨN QA)
├── TestCases.csv                                 # DỮ LIỆU TEST CASES ĐỊNH DẠNG EXCEL (UTF-8 BOM)
├── DemoScript.md                                 # KỊCH BẢN THỰC THI & PHẢN BIỆN REVIEW
└── README.md                                     # Tài liệu tổng quan hệ thống
```

---

## 2. QUY TẮC NGHIỆP VỤ CỐT LÕI (CORE BUSINESS RULES)

### 2.1. Động cơ tính phí gửi xe (Tariff Engine)
* **Theo lượt (`PerTurn`)**: Thu một mức phí cố định khi xe ra bãi, không phụ thuộc vào thời gian đỗ (áp dụng cho xe máy).
* **Theo giờ (`PerHour`)**: Thu theo block giờ, không áp dụng ân hạn miễn phí:
  * Vào bãi và ra ngay (dưới 1 giờ, ví dụ 5 phút, 12 phút): Thu tròn tối thiểu **1 block giờ đầu tiên** (`hours = 1`).
  * Từ giờ thứ hai trở đi: Số phút lẻ được làm tròn lên block giờ kế tiếp (`Math.Ceiling`).
* **Theo ngày (`PerDay`)**: Tính theo block 24 giờ, vượt quá 24h làm tròn sang ngày tiếp theo (ví dụ 36 giờ đỗ tính 2 ngày).

### 2.2. Chính sách xóa bãi xe (Soft Delete vs Hard Delete)
* **Xóa cứng (Hard Delete - `204 No Content`)**: Chỉ áp dụng khi bãi xe **chưa từng phát sinh bất kỳ lượt gửi xe nào** trong lịch sử (`HistoricalSessions == 0`).
* **Xóa mềm (Soft Delete - `200 OK`)**: Khi bãi xe đã có lịch sử gửi xe trong quá khứ nhưng hiện tại không còn xe nào đang đỗ (`CurrentParked == 0`), hệ thống chuyển cờ `IsDeleted = true` và `IsActive = false` để bảo toàn lịch sử kế toán, doanh thu và tính toàn vẹn khóa ngoại.
* **Chặn xóa**: Nếu bãi xe đang có xe đỗ (`CurrentParked > 0`), hệ thống từ chối xóa và ném `BusinessRuleViolationException`.

### 2.3. Kiểm soát tương tranh & Toàn vẹn dữ liệu (Concurrency Control)
* Khi bãi chỉ còn đúng 1 chỗ trống (`AvailableSpots = 1`) và có 2 request quét thẻ cùng một tích tắc:
  * Cơ chế Database Transaction và Optimistic Concurrency Control (`RowVersion`) đảm bảo chỉ 1 xe thành công (chỗ trống về 0), xe thứ hai nhận lỗi thông báo bãi đã đầy. Chỗ trống không bao giờ bị âm.
* Khi có ngoại lệ xảy ra trong quá trình check-in, giao dịch tự động Rollback, không để lại bản ghi mồ côi (No orphan session).

### 2.4. Phân quyền và Bảo vệ tài nguyên (RBAC & Ownership Check)
* Hệ thống xác thực bằng **JWT Token** phân quyền 3 vai trò: `SystemManager`, `BusinessOperator`, `Attendant`.
* Phía máy chủ kiểm tra quyền sở hữu tài nguyên: Nhà điều hành (Business Operator) chỉ được sửa/xóa các bãi xe do chính mình sở hữu. Thao tác trên bãi của người khác sẽ bị từ chối ngay lập tức với mã lỗi **`403 Forbidden`**.

---

## 3. HỆ THỐNG TÀI LIỆU KIỂM THỬ & ĐẶC TẢ

Hệ thống được trang bị bộ tài liệu kỹ thuật hoàn chỉnh:
1. **[TaskAPISpec.md](TaskAPISpec.md)**: Đặc tả chi tiết 15 endpoints RESTful, mô tả Request/Response JSON, mã HTTP, luật kiểm tra phân quyền và bảng Changelog v2.0.
2. **[UnitTestPlan.md](UnitTestPlan.md)**: Kế hoạch kiểm thử tổng thể, chiến lược phân tích giá trị biên (BVA 6 điểm biên), phân vùng tương đương (EP), ma trận truy xuất nguồn gốc (Traceability Matrix).
3. **[TestCases.md](TestCases.md)**: Bảng đặc tả chi tiết **55 test cases** theo định dạng 10 cột chuẩn QA/QC quốc tế, kèm nhật ký thực thi thực tế đạt **100% Pass**.
4. **[TestCases.csv](TestCases.csv)**: File dữ liệu test cases chuẩn mã hóa UTF-8 BOM, hỗ trợ mở trực tiếp trên Microsoft Excel.
5. **[DemoScript.md](DemoScript.md)**: Kịch bản demo 5 bước và bộ câu hỏi phản biện chuyên sâu khi nghiệm thu dự án.

---

## 4. HƯỚNG DẪN KHỞI CHẠY VÀ KIỂM THỬ

### 4.1. Chạy toàn bộ 55 Unit Tests tự động
Mở Terminal tại thư mục gốc của dự án và chạy lệnh:
```powershell
dotnet test Backend/ParkingManagement.sln
```
*Để xem chi tiết danh sách từng hàm test chạy thành công:*
```powershell
dotnet test Backend/ParkingManagement.sln -v n
```

**Kết quả kiểm thử chính thức:**
```text
Test Run Successful.
Total tests: 55
     Passed: 55 (100%)
     Failed: 0
    Skipped: 0
 Total time: 1.0 giây
```

### 4.2. Khởi chạy Web API Backend
```powershell
dotnet run --project Backend/ParkingManagement.Api/ParkingManagement.Api.csproj
```
Truy cập tài liệu tương tác Swagger UI:
* **`http://localhost:5000/swagger`** hoặc **`http://localhost:5097/swagger`**

### 4.3. Mở giao diện Quản trị (Frontend)
Mở trực tiếp file `Frontend/index.html` trên trình duyệt để trải nghiệm hệ thống quản lý bãi đỗ xe trực quan.
