# TÀI LIỆU THIẾT KẾ KỸ THUẬT CHI TIẾT (LOW-LEVEL DESIGN - LLD)
## Phân hệ: Quản lý Thông tin Bãi đỗ xe (Smart Parking System)

---

## 1. Tổng quan phân hệ & Kiến trúc hệ thống

Phân hệ **Quản lý Thông tin Bãi đỗ xe** được thiết kế dựa trên các nguyên lý cốt lõi:
* **Domain-Driven Design (DDD):** Phân định rõ ranh giới giữa Aggregate Root, Entity, Value Object và các quy tắc nghiệp vụ bất biến (Invariants).
* **Clean Architecture / CQRS:** Độc lập giữa Domain Logic, Use Cases (Application), Cơ sở dữ liệu và Giao diện API.
* **Cơ chế phân quyền & kiểm tra sở hữu (Ownership Enforcement):** Đảm bảo tính bảo mật và toàn vẹn dữ liệu đa khách hàng (Multi-tenant).

---

## 2. Sơ đồ lớp (Class Diagram - Domain Model DDD)

### 2.1. Sơ đồ Mermaid Class Diagram

```mermaid
classDiagram
    class BusinessOperator {
        <<Aggregate Root>>
        +Guid Id
        +Guid SystemManagerId
        +string Name
        +string Representative
        +Address OfficeAddress
        +ContactInfo ContactInfo
        +string Username
        +string PasswordHash
        +bool IsActive
    }

    class ParkingLot {
        <<Aggregate Root>>
        +Guid Id
        +Guid BusinessOperatorId
        +string Name
        +Address Address
        +int TotalCapacity
        +int AvailableSpots
        +string OpenTime
        +string CloseTime
        +bool IsActive
        +bool IsDeleted
        +RecordVehicleEntry()
        +RecordVehicleExit()
        +UpdateInfo(string name, Address address, int totalCapacity, string openTime, string closeTime, bool isActive)
        +SetStatus(bool isActive)
        +DeactivateAndSoftDelete()
    }

    class ParkingSession {
        <<Aggregate Root>>
        +Guid Id
        +Guid ParkingLotId
        +Guid VehicleTypeId
        +LicensePlate Plate
        +DateTime CheckInTime
        +DateTime? CheckOutTime
        +Money Fee
        +SessionStatus Status
        +Complete(Money fee)
    }

    class Tariff {
        <<Entity>>
        +Guid Id
        +Guid ParkingLotId
        +Guid VehicleTypeId
        +Money Price
        +PricingType PricingType
        +DateTime EffectiveDate
        +CalculateFee(TimeSpan duration) Money
    }

    class VehicleType {
        <<Entity>>
        +Guid Id
        +string Name
        +string Description
    }

    %% Value Objects (Không có Identity riêng, Bất biến)
    class Address {
        <<Value Object>>
        +string Street
        +string Ward
        +string District
        +string City
        +GetFullAddress() string
    }

    class ContactInfo {
        <<Value Object>>
        +string PhoneNumber
        +string Email
    }

    class LicensePlate {
        <<Value Object>>
        +string ProvinceCode
        +string PlateNumber
        +GetFormattedPlate() string
    }

    class Money {
        <<Value Object>>
        +decimal Amount
        +string Currency
        +Add(Money other) Money
        +Zero() Money
    }

    %% Mối quan hệ Aggregate và Composition
    BusinessOperator "1" *-- "1" Address : địa chỉ trụ sở
    BusinessOperator "1" *-- "1" ContactInfo : thông tin liên hệ
    BusinessOperator "1" --> "*" ParkingLot : quản lý các bãi xe

    ParkingLot "1" *-- "1" Address : đặt tại (Address)
    ParkingLot "1" --> "*" Tariff : áp dụng bảng giá
    ParkingLot "1" --> "*" ParkingSession : ghi nhận lượt đỗ xe

    Tariff "1" *-- "1" Money : đơn giá (Price)
    Tariff "*" --> "1" VehicleType : áp dụng cho loại xe

    ParkingSession "1" *-- "1" LicensePlate : biển số xe
    ParkingSession "1" *-- "0..1" Money : số tiền thanh toán
    ParkingSession "*" --> "1" VehicleType : loại xe thực tế
```

### 2.2. Đặc tả chi tiết các phần tử trong Mô hình Miền (Domain Model)

| Phân loại | Tên phần tử | Kiểu | Mô tả vai trò & Quy tắc nghiệp vụ |
| :--- | :--- | :--- | :--- |
| **Aggregate Root** | `BusinessOperator` | Lớp gốc | Đại diện cho đối tác / đơn vị vận hành bãi xe. Quản lý thông tin pháp nhân và danh mục bãi đỗ thuộc quyền sở hữu. |
| **Aggregate Root** | `ParkingLot` | Lớp gốc | Quản lý thông tin vật lý của bãi đỗ, sức chứa (`totalCapacity`), số chỗ trống thực tế (`availableSpots`), giờ mở cửa (`openTime`) và giờ đóng cửa (`closeTime`). |
| **Aggregate Root** | `ParkingSession` | Lớp gốc | Quản lý vòng đời lượt gửi xe từ lúc Check-in đến khi thanh toán Check-out. |
| **Entity** | `Tariff` | Thực thể | Chính sách giá đỗ xe áp dụng cho từng loại phương tiện tại bãi. |
| **Entity** | `VehicleType` | Thực thể | Danh mục phân loại phương tiện (Xe máy, Ô tô con, Xe tải). |
| **Value Object** | `Address` | Đối tượng giá trị | Gói thông tin địa chỉ gồm: `Street`, `Ward`, `District`, `City`. Không thể tách rời. |
| **Value Object** | `ContactInfo` | Đối tượng giá trị | Thông tin liên lạc (Số điện thoại, Email) có validation định dạng. |
| **Value Object** | `LicensePlate` | Đối tượng giá trị | Biển số xe gồm Mã tỉnh/thành (`ProvinceCode`) và Dãy số đăng ký (`PlateNumber`). |
| **Value Object** | `Money` | Đối tượng giá trị | Đơn vị tiền tệ chuẩn (Số tiền thập phân và Mã tiền tệ `VND`). |

---

## 3. Sơ đồ cơ sở dữ liệu (ERD) & Dữ liệu lưu trữ

### 3.1. Sơ đồ quan hệ thực thể (Mermaid ERD)

```mermaid
erDiagram
    SYSTEM_MANAGERS ||--o{ BUSINESS_OPERATORS : "quản lý"
    BUSINESS_OPERATORS ||--o{ PARKING_LOTS : "sở hữu"
    PARKING_LOTS ||--o{ TARIFFS : "áp dụng"
    PARKING_LOTS ||--o{ PARKING_SESSIONS : "ghi nhận"
    VEHICLE_TYPES ||--o{ TARIFFS : "định giá cho"
    VEHICLE_TYPES ||--o{ PARKING_SESSIONS : "phân loại cho"

    SYSTEM_MANAGERS {
        TEXT id PK "Mã quản trị viên"
        TEXT full_name "Họ và tên"
        TEXT position "Chức vụ"
        TEXT phone_number "Số điện thoại"
        TEXT email "Email liên hệ"
        TEXT username UK "Tên đăng nhập"
        TEXT password_hash "Mật khẩu mã hóa"
        INTEGER role "Vai trò quản trị"
        INTEGER is_active "Trạng thái kích hoạt"
    }

    BUSINESS_OPERATORS {
        TEXT id PK "Mã đơn vị kinh doanh"
        TEXT system_manager_id FK "Mã quản trị viên phụ trách"
        TEXT name "Tên công ty / đơn vị"
        TEXT representative "Người đại diện pháp luật"
        TEXT street "Địa chỉ trụ sở: Đường"
        TEXT ward "Địa chỉ trụ sở: Phường"
        TEXT district "Địa chỉ trụ sở: Quận"
        TEXT city "Địa chỉ trụ sở: Thành phố"
        TEXT phone_number "Số điện thoại"
        TEXT email "Email liên hệ"
        TEXT username UK "Tên đăng nhập"
        TEXT password_hash "Mật khẩu mã hóa"
        INTEGER is_active "Trạng thái hoạt động"
    }

    PARKING_LOTS {
        TEXT id PK "Mã bãi đỗ xe (UUID)"
        TEXT business_operator_id FK "Mã đơn vị sở hữu"
        TEXT name "Tên bãi đỗ xe"
        TEXT street "Địa chỉ: Đường"
        TEXT ward "Địa chỉ: Phường"
        TEXT district "Địa chỉ: Quận"
        TEXT city "Địa chỉ: Thành phố"
        INTEGER total_capacity "Tổng sức chứa"
        INTEGER available_spots "Số chỗ trống"
        TEXT open_time "Giờ mở cửa (HH:mm)"
        TEXT close_time "Giờ đóng cửa (HH:mm)"
        INTEGER is_active "Trạng thái hoạt động"
    }

    TARIFFS {
        TEXT id PK "Mã bảng giá (UUID)"
        TEXT parking_lot_id FK "Mã bãi đỗ xe"
        TEXT vehicle_type_id FK "Mã loại phương tiện"
        TEXT price_amount "Đơn giá định mức"
        TEXT price_currency "Loại tiền tệ"
        INTEGER pricing_type "Loại hình tính phí"
        TEXT effective_date "Ngày hiệu lực"
    }

    VEHICLE_TYPES {
        TEXT id PK "Mã loại phương tiện"
        TEXT name "Tên loại xe"
        TEXT description "Mô tả chi tiết"
    }

    PARKING_SESSIONS {
        TEXT id PK "Mã phiên gửi xe (UUID)"
        TEXT parking_lot_id FK "Mã bãi đỗ xe"
        TEXT vehicle_type_id FK "Mã loại phương tiện"
        TEXT province_code "Mã tỉnh biển số"
        TEXT plate_number "Dãy số biển kiểm soát"
        TEXT check_in_time "Thời điểm vào bãi"
        TEXT check_out_time "Thời điểm ra bãi"
        TEXT fee_amount "Số tiền thực tế thanh toán"
        TEXT fee_currency "Loại tiền tệ"
        INTEGER status "Trạng thái phiên đỗ xe"
    }
```

---

### 3.2. Dữ liệu lưu trữ (Database Schema Data Dictionary)

#### Bảng: `parking_lots` (Bảng chính lưu trữ thông tin bãi đỗ xe)

| STT | Tên cột | Kiểu dữ liệu | Khóa chính (PK) | Khóa ngoại (FK) | Nullable | Giá trị mặc định | Mô tả chi tiết |
| :---: | :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| 1 | `id` | TEXT (UUID) | PK | | Không | | Mã định danh duy nhất của bãi đỗ xe. |
| 2 | `business_operator_id`| TEXT (UUID) | | FK | Không | | Khóa ngoại tham chiếu tới `business_operators(id)`. |
| 3 | `name` | TEXT (VARCHAR 150) | | | Không | | Tên bãi đỗ xe (Duy nhất trong cùng đơn vị kinh doanh). |
| 4 | `street` | TEXT (VARCHAR 255) | | | Không | | Địa chỉ số nhà, tên đường bãi xe tọa lạc. |
| 5 | `ward` | TEXT (VARCHAR 100) | | | Không | | Phường / Xã. |
| 6 | `district` | TEXT (VARCHAR 100) | | | Không | | Quận / Huyện / Thị xã. |
| 7 | `city` | TEXT (VARCHAR 100) | | | Không | | Tỉnh / Thành phố trực thuộc trung ương. |
| 8 | `total_capacity` | INTEGER | | | Không | | Tổng số lượng vị trí đỗ xe thiết kế (`total_capacity > 0`). |
| 9 | `available_spots` | INTEGER | | | Không | | Số lượng vị trí còn trống sẵn sàng tiếp nhận xe (`0 <= available_spots <= total_capacity`). |
| 10 | `open_time` | TEXT (CHAR 8) | | | Không | `'06:00'` | Khung giờ mở cửa hoạt động hàng ngày (`HH:mm`). |
| 11 | `close_time` | TEXT (CHAR 8) | | | Không | `'22:00'` | Khung giờ đóng cửa hoạt động hàng ngày (`HH:mm`). |
| 12 | `is_active` | INTEGER (TINYINT) | | | Không | `1` | Trạng thái hoạt động: `1` (Đang mở đón khách), `0` (Tạm ngưng phục vụ). |

#### Bảng: `business_operators` (Đơn vị kinh doanh / Chủ sở hữu bãi đỗ)

| STT | Tên cột | Kiểu dữ liệu | Khóa chính (PK) | Khóa ngoại (FK) | Nullable | Giá trị mặc định | Mô tả chi tiết |
| :---: | :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| 1 | `id` | TEXT (UUID) | PK | | Không | | Mã định danh đơn vị kinh doanh. |
| 2 | `system_manager_id` | TEXT (UUID) | | FK | Không | | Quản trị viên hệ thống phê duyệt / phụ trách. |
| 3 | `name` | TEXT (VARCHAR 200) | | | Không | | Tên doanh nghiệp / Đơn vị vận hành bãi xe. |
| 4 | `representative` | TEXT (VARCHAR 100) | | | Không | | Người đại diện theo pháp luật. |
| 5 | `street` | TEXT (VARCHAR 255) | | | Không | | Địa chỉ trụ sở văn phòng. |
| 6 | `ward` | TEXT (VARCHAR 100) | | | Không | | Phường / Xã. |
| 7 | `district` | TEXT (VARCHAR 100) | | | Không | | Quận / Huyện. |
| 8 | `city` | TEXT (VARCHAR 100) | | | Không | | Tỉnh / Thành phố. |
| 9 | `phone_number` | TEXT (VARCHAR 20) | | | Không | | Số điện thoại liên hệ chính thức. |
| 10 | `email` | TEXT (VARCHAR 100) | | | Không | | Địa chỉ hòm thư điện tử. |
| 11 | `username` | TEXT (VARCHAR 50) | | | Không | | Tên đăng nhập hệ thống (Unique). |
| 12 | `password_hash` | TEXT (VARCHAR 255) | | | Không | | Mật khẩu tài khoản đã mã hóa bcrypt. |
| 13 | `is_active` | INTEGER (TINYINT) | | | Không | `1` | Trạng thái hoạt động (`1`: Đang hoạt động, `0`: Bị khóa). |

---

## 4. Sơ đồ tuần tự (Sequence Diagrams)

### 4.1. Luồng 1: Khởi tạo bãi đỗ xe mới (Create Parking Lot)

```mermaid
sequenceDiagram
    autonumber
    actor Operator as 👤 Nhà điều hành (Business Operator)
    participant API as 🌐 Minimal API (POST /api/parking-lots)
    participant Handler as ⚙️ CreateParkingLotCommandHandler
    participant LotRepo as 🗄️ IParkingLotRepository
    participant Domain as 🧠 ParkingLot (Domain Entity)
    participant UoW as 🗄️ IUnitOfWork (DbContext)

    Operator->>API: POST /api/parking-lots (Name, Address, TotalCapacity, OpenTime, CloseTime)
    API->>Handler: Send(CreateParkingLotCommand)
    
    Handler->>LotRepo: GetAllAsync(operatorId)
    alt Kịch bản 1: Trùng tên bãi xe trong cùng đơn vị kinh doanh
        Handler->>Handler: So khớp tên bãi xe -> Đã tồn tại
        Handler-->>API: Ném BusinessRuleViolationException ("Tên bãi xe đã tồn tại")
        API-->>Operator: 422 Unprocessable Entity
    else Tên bãi hợp lệ
        Handler->>Domain: Khởi tạo ParkingLot(id, name, capacity, address, openTime, closeTime)
        alt Kịch bản 2: Dung lượng không hợp lệ (capacity <= 0)
            Domain-->>Handler: Ném ArgumentOutOfRangeException ("Sức chứa phải > 0")
            Handler-->>API: 400 Bad Request
            API-->>Operator: 400 Bad Request
        else Dữ liệu hợp lệ thành công
            Domain->>Domain: Gán AvailableSpots = totalCapacity, IsActive = true
            Handler->>LotRepo: AddAsync(newLot)
            Handler->>UoW: SaveChangesAsync()
            UoW-->>Handler: Commit thành công
            Handler-->>API: Trả về ParkingLotDto
            API-->>Operator: 201 Created (Trả về chi tiết bãi xe kèm ID mới)
        end
    end
```

---

### 4.2. Luồng 2: Cập nhật thông tin & Kiểm tra quyền sở hữu (Update Parking Lot - Ownership Check)

```mermaid
sequenceDiagram
    autonumber
    actor Operator as 👤 Nhà điều hành A
    participant API as 🌐 Minimal API (PUT /api/parking-lots/{id})
    participant Handler as ⚙️ UpdateParkingLotCommandHandler
    participant LotRepo as 🗄️ IParkingLotRepository
    participant Domain as 🧠 ParkingLot (Domain Entity)
    participant UoW as 🗄️ IUnitOfWork

    Operator->>API: PUT /api/parking-lots/{id} (Kèm Bearer JWT Token của Operator A)
    API->>Handler: Send(UpdateParkingLotCommand)
    
    Handler->>LotRepo: GetByIdAsync(id)
    alt Kịch bản 1: Không tìm thấy bãi xe
        LotRepo-->>Handler: null
        Handler-->>API: 404 Not Found
        API-->>Operator: 404 Not Found ("Không tìm thấy bãi đỗ xe")
    else Tìm thấy bãi xe
        LotRepo-->>Handler: ParkingLot Entity (thuộc sở hữu Operator B)
        Handler->>Handler: So sánh Claim CurrentUser.OperatorId == lot.BusinessOperatorId
        alt Kịch bản 2: Operator A cố tình sửa bãi xe của Operator B (Vi phạm sở hữu)
            Handler-->>API: Trả về lỗi 403 Forbidden
            API-->>Operator: 403 Forbidden ("Chỉ được phép chỉnh sửa bãi xe của chính mình")
        else Kịch bản 3: Hợp lệ - Đúng quyền sở hữu
            Handler->>Domain: lot.UpdateInfo(name, address, capacity, openTime, closeTime, isActive)
            Handler->>UoW: SaveChangesAsync()
            UoW-->>Handler: Commit cập nhật CSDL thành công
            Handler-->>API: Trả về ParkingLotDto
            API-->>Operator: 200 OK ("Cập nhật thông tin bãi đỗ xe thành công")
        end
    end
```

---

### 4.3. Luồng 3: Phân nhánh xóa bãi đỗ xe (Delete Parking Lot)

```mermaid
sequenceDiagram
    autonumber
    actor Operator as 👤 Nhà điều hành
    participant API as 🌐 Minimal API (DELETE /api/parking-lots/{id})
    participant Handler as ⚙️ DeleteParkingLotCommandHandler
    participant LotRepo as 🗄️ IParkingLotRepository
    participant SessionRepo as 🗄️ IParkingSessionRepository
    participant Domain as 🧠 ParkingLot (Domain Entity)
    participant UoW as 🗄️ IUnitOfWork

    Operator->>API: DELETE /api/parking-lots/{id}
    API->>Handler: Send(DeleteParkingLotCommand)
    
    Handler->>LotRepo: GetByIdAsync(id)
    alt Không tìm thấy bãi xe
        LotRepo-->>Handler: null
        Handler-->>API: 404 Not Found
        API-->>Operator: 404 Not Found
    else Tìm thấy bãi xe
        LotRepo-->>Handler: ParkingLot Entity
        Handler->>SessionRepo: CountActiveVehiclesAsync(lotId)
        
        alt KỊCH BẢN 1: Bãi xe vẫn còn xe đang đỗ (CurrentParked > 0)
            SessionRepo-->>Handler: > 0 (Còn xe trong bãi)
            Handler-->>API: Ném BusinessRuleViolationException ("Bãi xe còn xe đang gửi, không thể xóa!")
            API-->>Operator: 422 Unprocessable Entity
        else Bãi xe hiện không có xe nào đang đỗ (CurrentParked == 0)
            Handler->>SessionRepo: HasHistoricalSessionsAsync(lotId)
            
            alt KỊCH BẢN 2: Bãi đã có lịch sử gửi xe trong quá khứ (HistoricalSessions > 0)
                SessionRepo-->>Handler: true (Đã từng phát sinh vé gửi xe)
                Handler->>Domain: lot.DeactivateAndSoftDelete()
                Handler->>UoW: SaveChangesAsync()
                UoW-->>Handler: Lưu cờ xóa mềm thành công
                Handler-->>API: 200 OK (Soft Deleted)
                API-->>Operator: 200 OK ("Bãi xe đã có lịch sử, chuyển sang trạng thái xóa mềm thành công")
            else KỊCH BẢN 3: Bãi xe mới tạo nhầm, chưa từng có lịch sử (HistoricalSessions == 0)
                SessionRepo-->>Handler: false (Chưa từng phát sinh vé xe nào)
                Handler->>LotRepo: HardDeleteAsync(lot)
                Handler->>UoW: SaveChangesAsync()
                UoW-->>Handler: Xóa vĩnh viễn bản ghi khỏi CSDL
                Handler-->>API: 204 No Content (Hard Deleted)
                API-->>Operator: 204 No Content
            end
        end
    end
```
