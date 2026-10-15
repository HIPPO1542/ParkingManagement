---
name: scaffold-ddd-cqrs
description: >-
  Use this skill whenever the user asks to create, scaffold, architect, or structure a new project, microservice, or system module using Domain-Driven Design (DDD), Clean Architecture, and CQRS patterns.
---

# Quy Chuẩn Khởi Tạo & Tổ Chức Dự Án Chuẩn DDD + CQRS + Clean Architecture

Skill này hướng dẫn quy trình tiêu chuẩn để kiến tạo toàn diện một dự án phần mềm theo đúng mô hình kiến trúc **Clean Architecture kết hợp Domain-Driven Design (DDD) và Command Query Responsibility Segregation (CQRS)**.

---

## 1. CẤU TRÚC THƯ MỤC CHUẨN DOANH NGHIỆP (SOLUTION STRUCTURE)

Toàn bộ hệ thống được phân rã thành 4 tầng kiến trúc độc lập (nguyên tắc phụ thuộc đi từ ngoài vào trong: Presentation $\rightarrow$ Infrastructure $\rightarrow$ Application $\rightarrow$ Domain):

```text
SolutionName/
├── src/
│   ├── SolutionName.Domain/                     # Tầng cốt lõi: Không phụ thuộc bất kỳ thư viện ngoài nào
│   │   ├── Common/                              # IAggregateRoot, Entity<TId>, ValueObject
│   │   ├── Entities/                            # Aggregate Roots & Entities (chứa logic nghiệp vụ)
│   │   ├── ValueObjects/                        # Đối tượng giá trị bất biến (Address, Money, ...)
│   │   ├── Events/                              # Domain Events phát sinh khi thay đổi trạng thái
│   │   ├── Exceptions/                          # BusinessRuleViolationException, DomainException...
│   │   └── Repositories/                        # Interfaces (IRepository<T>, IUnitOfWork)
│   │
│   ├── SolutionName.Application/                # Tầng điều phối nghiệp vụ & Use Cases
│   │   ├── Common/                              # ApiResponse<T>, PagedResult<T>, Behaviors
│   │   ├── DTOs/                                # DTOs truyền nhận dữ liệu
│   │   ├── Features/                            # Tổ chức theo tính năng (Vertical Slices)
│   │   │   └── [FeatureName]/
│   │   │       ├── Commands/                    # C-QRS: Luồng Ghi (Create, Update, Delete)
│   │   │       │   ├── [Action]Command.cs       # Record DTO command
│   │   │       │   └── [Action]CommandHandler.cs# Xử lý logic nghiệp vụ
│   │   │       └── Queries/                     # CQ-R-S: Luồng Đọc (GetList, GetById)
│   │   │           ├── [Query]Query.cs          # Query parameters
│   │   │           └── [Query]QueryHandler.cs   # Đọc dữ liệu tối ưu
│   │   └── Validators/                          # Kiểm tra tính hợp lệ dữ liệu đầu vào (FluentValidation)
│   │
│   ├── SolutionName.Infrastructure/             # Tầng kỹ thuật & Truy cập cơ sở dữ liệu
│   │   ├── Persistence/                         # DbContext, Cấu hình EntityTypeConfiguration
│   │   ├── Repositories/                        # Hiện thực hóa các Repository Interfaces
│   │   └── Services/                            # Tích hợp bên thứ ba (SMS, Email, Payment, Cache)
│   │
│   └── SolutionName.Api/                        # Tầng giao diện ứng dụng (Presentation)
│       ├── Endpoints/                           # Minimal APIs hoặc Controllers phân nhóm
│       ├── Middlewares/                         # GlobalExceptionMiddleware, JwtMiddleware
│       ├── appsettings.json                     # Cấu hình môi trường, Connection Strings
│       └── Program.cs                           # Cấu hình Dependency Injection (DI Container)
│
└── tests/
    └── SolutionName.Tests/                      # Kiểm thử tự động (Unit Tests, Integration Tests)
```

---

## 2. NGUYÊN TẮC CỐT LÕI DDD & CQRS

### 2.1. Domain-Driven Design (DDD)
1. **Aggregate Root (Gốc tổng hợp):**
   * Luôn là điểm truy cập duy nhất để thay đổi dữ liệu của toàn bộ nhóm thực thể bên trong nó.
   * Chứa các phương thức nghiệp vụ thực thụ (ví dụ: `RecordVehicleEntry()`, `UpdateInfo()`), tuyệt đối **không dùng Anemic Domain Model** (chỉ có get/set rỗng).
   * Kiểm soát chặt chẽ các điều kiện bất biến (Invariants) trước khi cho phép thay đổi trạng thái.
2. **Value Object (Đối tượng giá trị):**
   * Không có định danh riêng (`id`).
   * Bất biến (Immutable) — một khi đã khởi tạo thì không thay đổi; muốn thay đổi phải tạo đối tượng mới.
   * Ví dụ: `Address` (gồm street, ward, district, city), `Money` (gồm amount, currency).
3. **Repository Pattern:**
   * Repository chỉ định nghĩa các Interface trong tầng Domain (`IParkingLotRepository`).
   * Việc hiện thực hóa xuống Database (EF Core, Dapper, SQL) chỉ nằm ở tầng Infrastructure.

### 2.2. Command Query Responsibility Segregation (CQRS)
1. **Commands (Tác vụ Ghi/Thay đổi):**
   * Đại diện cho ý định thay đổi trạng thái hệ thống: Tạo mới, Cập nhật, Xóa.
   * Đi kèm với Transaction (`IUnitOfWork.SaveChangesAsync()`).
2. **Queries (Tác vụ Đọc):**
   * Chỉ lấy dữ liệu hiển thị (Read-only).
   * Không làm thay đổi trạng thái cơ sở dữ liệu, tối ưu hóa tốc độ (AsNoTracking, Dapper).
3. **Phân tách qua MediatR:**
   * Mỗi Command/Query có một Handler riêng biệt, giúp code độc lập, dễ viết Unit Test và dễ mở rộng.

---

## 3. QUY ƯỚC API RESPONSE & XỬ LÝ LỖI CHUẨN HÓA

### 3.1. Lớp vỏ bọc phản hồi chuẩn (`ApiResponse<T>`):
Mọi Endpoint bắt buộc trả về định dạng vỏ bọc chung:
```csharp
public class ApiResponse<T>
{
    public int Code { get; set; } = 0;          // 0: Thành công, != 0: Lỗi
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "Success";
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}
```

### 3.2. Middleware bắt lỗi tập trung (`GlobalExceptionMiddleware`):
* Bắt các ngoại lệ nghiệp vụ (`BusinessRuleViolationException`, `EntityNotFoundException`).
* Tự động chuyển đổi ngoại lệ thành JSON chuẩn có `code = 400`, `403`, `404`, `422`, `500` mà không để ứng dụng bị crash.

---

## 4. QUY TRÌNH HÀNH ĐỘNG KHI TẠO PROJECT MỚI

Khi nhận yêu cầu khởi tạo hoặc tổ chức một dự án mới:
1. **Phân tích Nghiệp vụ:** Xác định các Bounded Contexts, Aggregate Roots, Entities và Value Objects.
2. **Thiết lập Cấu trúc Thư mục:** Tạo đầy đủ 4 project (.csproj) theo đúng kiến trúc Clean Architecture.
3. **Định nghĩa Tầng Domain:** Viết các Entity, Value Object có phương thức nghiệp vụ và Interface Repository trước tiên.
4. **Định nghĩa Tầng Application:** Thiết lập các Command/Query và Handler xử lý bằng MediatR.
5. **Định nghĩa Tầng Infrastructure:** Tạo DbContext, Mapping Entity và triển khai Repository.
6. **Định nghĩa Tầng Presentation:** Tạo Endpoint RESTful, cấu hình `GlobalExceptionMiddleware` và Swagger UI.
