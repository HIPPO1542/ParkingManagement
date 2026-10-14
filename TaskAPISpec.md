# Smart Parking Management API Specification
**Phiên bản:** 2.0 / 2026-10-13  
**Base URL:** `http://localhost:5097` (Phát triển) / `https://api.parkingmanagement.vn` (Sản xuất)  
**Kiến trúc cốt lõi:** Clean Architecture, CQRS (MediatR), REPR Pattern (Minimal API Endpoints)  
**Kho mã nguồn:** `https://github.com/HIPPO1542/ParkingManagement`  

---

## Mục Lục (Table of Contents)

1. [Quy Ước Kiến Trúc & Vòng Đời API](#1-quy-ước-kiến-trúc--vòng-đời-api)
2. [Chuẩn Dữ Liệu & Hợp Đồng Kỹ Thuật (API Contracts)](#2-chuẩn-dữ-liệu--hợp-đồng-kỹ-thuật-api-contracts)
3. [Ma Trận Phân Quyền & Kiểm Soát Sở Hữu (RBAC & Ownership Matrix)](#3-ma-trận-phân-quyền--kiểm-soát-sở-hữu-rbac--ownership-matrix)
4. [Tính Toàn Vẹn Dữ Liệu & Kiểm Soát Đồng Thời (Data Integrity & Concurrency)](#4-tính-toàn-vẹn-dữ-liệu--kiểm-soát-đồng-thời-data-integrity--concurrency)
5. [Quy Tắc & Công Thức Tính Phí Chi Tiết (Tariff Engine Specifications)](#5-quy-tắc--công-thức-tính-phí-chi-tiết-tariff-engine-specifications)
6. [Chi Tiết Các Nhóm Endpoint (Endpoint Specifications)](#6-chi-tiết-các-nhóm-endpoint-endpoint-specifications)
   - [6.1 Xác Thực Người Dùng (Auth Endpoints)](#61-xác-thực-người-dùng-auth-endpoints)
   - [6.2 Đơn Vị Kinh Doanh (Business Operator Endpoints)](#62-đơn-vị-kinh-doanh-business-operator-endpoints)
   - [6.3 Bãi Đỗ Xe (Parking Lot Endpoints)](#63-bãi-đỗ-xe-parking-lot-endpoints)
   - [6.4 Lượt Gửi Xe (Parking Session Endpoints)](#64-lượt-gửi-xe-parking-session-endpoints)
   - [6.5 Biểu Phí (Tariff Endpoints)](#65-biểu-phí-tariff-endpoints)
   - [6.6 Loại Phương Tiện (Vehicle Type Endpoints)](#66-loại-phương-tiện-vehicle-type-endpoints)
   - [6.7 Báo Cáo & Thống Kê (Report Endpoints)](#67-báo-cáo--thống-kê-report-endpoints)
7. [Thông Báo Sự Kiện Thời Gian Thực (Webhooks)](#7-thông-báo-sự-kiện-thời-gian-thực-webhooks)
8. [Lịch Sử Thay Đổi (Changelog)](#8-lịch-sử-thay-đổi-changelog)

---

## 1. Quy Ước Kiến Trúc & Vòng Đời API

Hệ thống thống nhất sử dụng kiến trúc **REPR (Request-Endpoint-Response)** trên nền tảng **ASP.NET Core Minimal APIs**, kết hợp mô hình **CQRS (Command Query Responsibility Segregation)** thông qua thư viện **MediatR**.

### 1.1 Nguyên Tắc Tổ Chức & Triển Khai
- **Không sử dụng Controller truyền thống:** Mọi route đều được tổ chức thành các file Endpoint độc lập tại `ParkingManagement.Api/Endpoints/`, kế thừa interface `IEndpoint` và được tự động đăng ký vào DI/Route table qua extension `MapEndpoints()`.
- **Tách biệt Ghi và Đọc (CQRS):**
  - **Luồng Ghi (Command):** Tiếp nhận HTTP Request ➔ Đóng gói Command ➔ Gửi qua `ISender.Send()` ➔ CommandHandler thực thi qua Domain Entities & Aggregates ➔ Commit qua `UnitOfWork`.
  - **Luồng Đọc (Query):** Tiếp nhận HTTP Request ➔ Đóng gói Query ➔ QueryHandler truy vấn trực tiếp từ Repository với chế độ `AsNoTracking()` ➔ Map dữ liệu trả về DTO.

```text
                           [ CLIENT / FRONTEND ]
                                     │
                                     ▼ (HTTP Request - JSON)
                         ┌───────────────────────┐
                         │ TẦNG API / ENDPOINTS  │
                         │ (Minimal API REPR)    │
                         └───────────┬───────────┘
                                     │ ISender.Send()
                                     ▼
                         ┌───────────────────────┐
                         │   TẦNG APPLICATION    │
                         │ (Command & Query)     │
                         └─────┬───────────┬─────┘
                               │           │
       [ LUỒNG GHI / COMMAND ] │           │ [ LUỒNG ĐỌC / QUERY ]
                               ▼           │
                     ┌──────────────────┐  │
                     │   TẦNG DOMAIN    │  │
                     │ (Entities/Rules) │  │
                     └─────────┬────────┘  │
                               │           │
         (Entity hợp lệ        │           │ (Truy vấn tối ưu
          chuyển qua lưu)      ▼           ▼  AsNoTracking)
                     ┌───────────────────────────┐
                     │    TẦNG INFRASTRUCTURE    │
                     │ (EF Core DbContext,       │
                     │  Repositories, UnitOfWork)│
                     └─────────────┬─────────────┘
                                   │ Commit SQL / Truy vấn
                                   ▼
                     ┌───────────────────────────┐
                     │       CƠ SỞ DỮ LIỆU       │
                     │       (SQLite/SQL)        │
                     └───────────────────────────┘
```

---

## 2. Chuẩn Dữ Liệu & Hợp Đồng Kỹ Thuật (API Contracts)

### 2.1 Quy Ước Kiểu Dữ Liệu & Múi Giờ
- **Định dạng ID:** Chuẩn **UUID v4** (GUID 36 ký tự có dấu gạch ngang, ví dụ: `33333333-3333-3333-3333-333333333333`).
- **Múi giờ (Timezone):** Tất cả các mốc thời gian lưu trữ và truyền nhận đều sử dụng chuẩn **UTC (ISO 8601)** có hậu tố `Z` (ví dụ: `2026-10-13T08:30:00Z`). Việc chuyển đổi sang giờ địa phương (`GMT+7`) thuộc trách nhiệm của tầng hiển thị (Frontend/Mobile).
- **Phân trang (Pagination Defaults):**
  - Tham số: `page` (mặc định: `1`, tối thiểu: `1`), `pageSize` (mặc định: `20`, tối đa: `100`).

### 2.2 Chuẩn Cấu Trúc Phản Hồi (Response Schemas)

#### A. Phản hồi Thành Công Đơn Lẻ (`ApiResponse<T>`):
```json
{
  "success": true,
  "message": "Dữ liệu được xử lý thành công.",
  "data": { ... },
  "errors": []
}
```

#### B. Phản hồi Phân Trang (`ApiResponse<PagedResult<T>>`):
```json
{
  "success": true,
  "message": "Lấy danh sách thành công.",
  "data": {
    "items": [ ... ],
    "page": 1,
    "pageSize": 20,
    "totalCount": 142,
    "totalPages": 8,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "errors": []
}
```

#### C. Phản hồi Lỗi Chuẩn (`ApiResponse<null>` / `ValidationProblemDetails`):
```json
{
  "success": false,
  "message": "Dữ liệu gửi lên không hợp lệ hoặc vi phạm quy tắc nghiệp vụ.",
  "data": null,
  "errors": [
    "Biển số xe 59-99988 hiện đang có phiên gửi xe đang hoạt động trong bãi!",
    "Sức chứa bãi xe phải là số nguyên dương lớn hơn 0."
  ]
}
```

### 2.3 Mã Lỗi HTTP Quy Ước
- **`200 OK`**: Yêu cầu xử lý thành công.
- **`201 Created`**: Tạo mới tài nguyên thành công; bắt buộc trả về Header `Location: /api/{resource}/{id}`.
- **`204 No Content`**: Xóa hoặc xử lý thành công, không trả nội dung.
- **`400 Bad Request`**: Dữ liệu gửi lên sai định dạng, thiếu trường, hoặc vi phạm Business Rules.
- **`401 Unauthorized`**: Thiếu Token hoặc Token không hợp lệ.
- **`403 Forbidden`**: Không có quyền truy cập chức năng hoặc không sở hữu tài nguyên được yêu cầu.
- **`404 Not Found`**: ID tài nguyên không tồn tại trong hệ thống.
- **`409 Conflict`**: Xung đột dữ liệu (ví dụ: bãi xe đã đầy ngay lúc commit, trùng lặp Unique Key).
- **`500 Internal Server Error`**: Lỗi hệ thống ngoài dự kiến.

---

## 3. Ma Trận Phân Quyền & Kiểm Soát Sở Hữu (RBAC & Ownership Matrix)

### 3.1 Ma Trận Phân Quyền Theo Endpoint
Ký hiệu:
- **`ALLOW`**: Được phép truy cập.
- **`OWN`**: Chỉ được phép thao tác trên tài nguyên **thuộc quyền sở hữu của chính mình**.
- **`DENY`**: Bị cấm truy cập (Trả về `403 Forbidden`).

| Endpoint | Phương thức | Quản trị viên (`Manager`) | Nhà điều hành (`BusinessOperator`) | Nhân viên soát vé (`Attendant`) |
| :--- | :---: | :---: | :---: | :---: |
| `/api/auth/login` | POST | ALLOW | ALLOW | ALLOW |
| `/api/auth/me` | GET | ALLOW | ALLOW | ALLOW |
| `/api/business-operators` | GET | ALLOW | DENY | DENY |
| `/api/business-operators/:id` | GET | ALLOW | OWN | DENY |
| `/api/business-operators` | POST | ALLOW | DENY | DENY |
| `/api/business-operators/:id` | PUT | ALLOW | OWN | DENY |
| `/api/business-operators/:id` | DELETE | ALLOW | DENY | DENY |
| `/api/parking-lots` | GET | ALLOW | OWN (Chỉ bãi của mình) | OWN (Chỉ bãi làm việc) |
| `/api/parking-lots/:id` | GET | ALLOW | OWN | OWN |
| `/api/parking-lots` | POST | ALLOW | OWN (Tạo cho mình) | DENY |
| `/api/parking-lots/:id` | PUT | ALLOW | OWN | DENY |
| `/api/parking-lots/:id` | DELETE | ALLOW | DENY | DENY |
| `/api/parking-sessions/check-in` | POST | ALLOW | OWN | OWN |
| `/api/parking-sessions/check-out` | POST | ALLOW | OWN | OWN |
| `/api/parking-sessions` | GET | ALLOW | OWN | OWN |
| `/api/tariffs` | GET | ALLOW | OWN | OWN |
| `/api/tariffs` | POST, PUT, DELETE | ALLOW | OWN | DENY |
| `/api/reports/activity` | GET | ALLOW | OWN | DENY |

### 3.2 Quy Tắc Kiểm Tra Quyền Sở Hữu Tại Máy Chủ (Server-side Ownership Enforcement)
- **Cơ chế xác thực không dừng lại ở kiểm tra Token:** Việc có Token hợp lệ chỉ chứng minh danh tính. Khi thực hiện các hành động sửa, xóa, xem chi tiết, hệ thống **bắt buộc kiểm tra quyền sở hữu tại Handler**:
  ```csharp
  // Ví dụ trong UpdateParkingLotCommandHandler:
  if (currentUser.Role == "BusinessOperator")
  {
      if (lot.BusinessOperatorId != currentUser.OperatorId)
          throw new UnauthorizedAccessException("Bạn không có quyền can thiệp vào bãi xe thuộc quyền sở hữu của đơn vị khác!");
  }
  ```

---

## 4. Tính Toàn Vẹn Dữ Liệu & Kiểm Soát Đồng Thời (Data Integrity & Concurrency)

### 4.1 Kiểm Soát Tranh Chấp Đồng Thời (Race Condition / Concurrency Control)
- **Vấn đề thực tế:** Khi bãi xe chỉ còn **1 chỗ trống duy nhất**, hai xe máy vào hai cổng cùng quét thẻ Check-in vào cùng một microsecond. Nếu không kiểm soát, cả 2 lượt đều thành công dẫn đến bãi xe bị âm chỗ (`AvailableSpots = -1`).
- **Giải pháp triển khai:**
  1. Sử dụng **Database Transaction** bao bọc toàn bộ chu trình: Kiểm tra chỗ ➔ Tạo session ➔ Cập nhật bãi xe ➔ `UnitOfWork.SaveChangesAsync()`.
  2. Áp dụng cơ chế **Optimistic Concurrency Control (OCC)** thông qua thuộc tính `RowVersion` / `ConcurrencyToken` trên Entity `ParkingLot`. Nếu có request khác đã thay đổi dữ liệu bãi xe trước, request sau sẽ nhận ngoại lệ `DbUpdateConcurrencyException` và tự động retry hoặc trả về `409 Conflict` an toàn.

### 4.2 Chính Sách Xóa Tài Nguyên (Soft Delete vs Hard Delete Policy)
- **Chính sách Xóa Bãi Xe (`ParkingLot`):**
  - **Chặn xóa khi còn xe đỗ:** Nếu bãi xe đang có phương tiện gửi bên trong (`CurrentParked > 0`), hệ thống tuyệt đối từ chối xóa và trả về lỗi `400 Bad Request`.
  - **Quy trình Xóa Mềm (Soft Delete):** Khi bãi xe không còn phương tiện đang đỗ (`CurrentParked == 0`) nhưng đã có lịch sử gửi xe trong quá khứ (`TotalPastSessions > 0`), hệ thống tự động chuyển sang chế độ Xóa mềm: đặt `IsActive = false`, ngừng tiếp nhận phương tiện nhưng bảo toàn 100% dữ liệu lịch sử và doanh thu kế toán. Endpoint phản hồi `204 No Content` (hoặc `200 OK`).
  - **Điều kiện Xóa Cứng (Hard Delete):** Chỉ cho phép xóa vật lý hoàn toàn khỏi cơ sở dữ liệu khi bãi xe vừa tạo nhầm và chưa từng phát sinh bất kỳ lượt gửi xe nào trong lịch sử (`TotalPastSessions == 0`). Hệ thống phản hồi `204 No Content`.

---

## 5. Quy Tắc & Công Thức Tính Phí Chi Tiết (Tariff Engine Specifications)

Hệ thống tính tiền tự động dựa trên thời gian đỗ thực tế và bảng giá có hiệu lực.

### 5.1 Các Quy Tắc Tính Phí Cốt Lõi
1. **Không áp dụng ân hạn miễn phí (Tính phí ngay khi xe vào bãi):**
   - Hệ thống **không có thời gian ân hạn miễn phí**. Phương tiện ngay khi vào bãi và được quét thẻ Check-in thành công (dù chỉ đỗ 1 phút, 5 phút hay ra ngay lập tức) đều bắt đầu tính tiền ngay:
     - Đối với hình thức tính theo lượt (`PerTurn`): Thu ngay mức phí cố định theo lượt (`Price.Amount`).
     - Đối với hình thức tính theo giờ (`PerHour`): Thu tối thiểu 1 block giờ đầu tiên (`hours >= 1`).
2. **Quy tắc làm tròn thời gian (Rounding Rule - Block 1 Giờ):**
   - Thời gian tính tiền theo giờ được làm tròn lên theo từng block 1 giờ đầy đủ (`Math.Ceiling`):
     $$\text{BilledHours} = \max\left(1, \left\lceil \text{DurationHours} \right\rceil\right)$$
3. **Thời điểm áp dụng Biểu phí (Tariff Effective Rule):**
   - Phí gửi xe được tính theo Biểu phí **có hiệu lực tại thời điểm Check-out** của bãi xe đó cho loại phương tiện tương ứng.
4. **Quy tắc Đỗ Xe Theo Ngày (`PerDay`):**
   - Làm tròn lên theo từng ngày (`Math.Ceiling(TotalDays)`), tối thiểu tính 1 ngày:
     $$\text{BilledDays} = \max\left(1, \left\lceil \text{DurationDays} \right\rceil\right)$$
     $$\text{TotalFee} = \text{BilledDays} \times \text{DailyRate}$$

### 5.2 Ba Ví Dụ Tính Tiền Cụ Thể (Test Scenarios)
*Giả sử biểu phí: Xe máy, `HourlyRate = 5.000 VND`, hình thức tính theo giờ (`PerHour`).*

| Trường hợp | Giờ vào ➔ Giờ ra | Thời gian thực tế | Số block tính tiền | Số tiền thanh toán | Giải thích nghiệp vụ |
| :--- | :--- | :---: | :---: | :---: | :--- |
| **Kịch bản A (Vào ra nhanh)** | `08:00` ➔ `08:12` | 12 phút | 1 giờ | **5.000 VND** | Không có ân hạn, vào bãi tính tối thiểu 1 block giờ đầu tiên. |
| **Kịch bản B (Làm tròn giờ)**| `08:00` ➔ `09:05` | 1 giờ 05 phút | 2 giờ | **10.000 VND** | Vượt quá 1 giờ ➔ Làm tròn lên 2 block giờ ($2 \times 5.000$). |
| **Kịch bản C (Đỗ qua ngày)** | `10/10 08:00` ➔ `11/10 12:00` | 28 giờ | 28 giờ | **140.000 VND** | $28 \text{ giờ} \times 5.000 \text{ VND} = 140.000 \text{ VND}$. |

---

## 6. Chi Tiết Các Nhóm Endpoint (Endpoint Specifications)

### 6.1 Xác Thực Người Dùng (Auth Endpoints)

#### `POST /api/auth/login`
- **Tóm tắt:** Đăng nhập hệ thống, cấp Access Token.
- **Request Body:**
  ```json
  {
    "username": "sgparking_admin",
    "password": "Password@123"
  }
  ```
- **Validation:**
  - `username`: Bắt buộc, chuỗi 4-50 ký tự.
  - `password`: Bắt buộc, chuỗi $\ge 6$ ký tự.
- **Phản hồi Thành Công (`200 OK`):**
  ```json
  {
    "success": true,
    "message": "Đăng nhập thành công.",
    "data": {
      "token": "eyJhbGciOiJIUzI1NiIsIn...",
      "userId": "11111111-1111-1111-1111-111111111111",
      "username": "sgparking_admin",
      "role": "BusinessOperator"
    },
    "errors": []
  }
  ```
- **Lỗi có thể xảy ra:**
  - `400 Bad Request`: Sai mật khẩu hoặc tài khoản đã bị vô hiệu hóa (`IsActive = false`).
  - `404 Not Found`: Tên đăng nhập không tồn tại.

---

### 6.2 Đơn Vị Kinh Doanh (Business Operator Endpoints)

#### `POST /api/business-operators`
- **Quyền hạn:** `Manager` only.
- **Request Body:**
  ```json
  {
    "name": "Công ty Bãi Xe Miền Đông",
    "representative": "Lê Văn C",
    "address": {
      "street": "292 Đinh Bộ Lĩnh",
      "ward": "Phường 26",
      "district": "Bình Thạnh",
      "city": "Hồ Chí Minh"
    },
    "contactInfo": {
      "phoneNumber": "02839998888",
      "email": "contact@bxmiendong.vn"
    },
    "username": "bxmiendong_op",
    "password": "Password@123",
    "isActive": true
  }
  ```
- **Ràng buộc Chống Trùng Lặp (Uniqueness Validation):**
  - Kiểm tra `username`: Phải là duy nhất trên toàn hệ thống ➔ Trả `400 Bad Request` nếu trùng.
  - Kiểm tra `name`: Không trùng với tên đơn vị kinh doanh đã đăng ký trước đó.
- **Phản hồi (`201 Created`):** Trả về đối tượng vừa tạo kèm Header `Location: /api/business-operators/{id}`.

---

### 6.3 Bãi Đỗ Xe (Parking Lot Endpoints)

#### `POST /api/parking-lots`
- **Quyền hạn:** `Manager`, `BusinessOperator` (chỉ tạo bãi cho chính mình).
- **Request Body:**
  ```json
  {
    "businessOperatorId": "22222222-2222-2222-2222-222222222222",
    "name": "Bãi xe Công viên 23/9",
    "address": {
      "street": "Phạm Ngũ Lão",
      "ward": "Phạm Ngũ Lão",
      "district": "Quận 1",
      "city": "Hồ Chí Minh"
    },
    "totalCapacity": 150,
    "openTime": "05:00",
    "closeTime": "23:00",
    "isActive": true
  }
  ```
- **Ràng buộc Biên & Chống Trùng Lặp:**
  - `totalCapacity`: Bắt buộc là số nguyên $> 0$.
  - `name`: Không được trùng lặp với tên bãi xe khác của cùng nhà điều hành này.
- **Phản hồi (`201 Created`):**
  ```json
  {
    "success": true,
    "message": "Bãi đỗ xe đã được tạo thành công.",
    "data": {
      "id": "77777777-7777-7777-7777-777777777777",
      "name": "Bãi xe Công viên 23/9",
      "totalCapacity": 150,
      "availableSpots": 150,
      "isActive": true
    },
    "errors": []
  }
  ```

---

### 6.4 Lượt Gửi Xe (Parking Session Endpoints)

#### `POST /api/parking-sessions/check-in`
- **Quyền hạn:** `Attendant`, `BusinessOperator`, `Manager`.
- **Request Body:**
  ```json
  {
    "parkingLotId": "33333333-3333-3333-3333-333333333333",
    "vehicleTypeId": "66666666-6666-6666-6666-666666666666",
    "provinceCode": "59",
    "plateNumber": "99988"
  }
  ```
- **Quy tắc Kiểm tra & Xử lý:**
  - Kiểm tra `AvailableSpots > 0` (Ném lỗi `400` nếu bãi đầy).
  - Kiểm tra biển số `59-99988` chưa có phiên đỗ hoạt động tại bãi.
  - Giảm `AvailableSpots` đi 1, tăng xe đang đỗ.
- **Phản hồi (`201 Created`):** Trả về chi tiết phiếu gửi xe.

#### `POST /api/parking-sessions/check-out`
- **Request Body:**
  ```json
  {
    "parkingLotId": "33333333-3333-3333-3333-333333333333",
    "provinceCode": "59",
    "plateNumber": "99988"
  }
  ```
- **Phản hồi (`200 OK`):** Trả về số giờ đỗ thực tế (`durationHours`), số tiền phí (`fee`), và cập nhật trạng thái `Completed`.

---

### 6.5 Biểu Phí (Tariff Endpoints)
- `GET /api/tariffs`: Lấy danh sách toàn bộ biểu phí.
- `GET /api/tariffs/parking-lot/{parkingLotId}`: Lấy danh sách biểu phí áp dụng cho 1 bãi xe cụ thể.
- `POST /api/tariffs`: Tạo mới biểu phí theo giờ (`PerHour`) hoặc theo lượt (`PerTurn`).
- `PUT /api/tariffs/{id}`: Cập nhật mức giá hoặc loại tính phí.
- `DELETE /api/tariffs/{id}`: Xóa biểu phí (chỉ cho phép nếu không có phiên đang hoạt động tham chiếu).

---

### 6.6 Loại Phương Tiện (Vehicle Type Endpoints)
- `GET /api/vehicle-types`: Lấy danh mục các loại xe (`Xe máy`, `Ô tô 4 chỗ`, `Ô tô 7 chỗ`, `Xe tải`).
- `POST /api/vehicle-types`: Thêm mới loại phương tiện vào danh mục chung.

---

### 6.7 Báo Cáo & Thống Kê (Report Endpoints)

#### `GET /api/reports/activity`
- **Query Parameters:**
  - `parkingLotId` (UUID, Bắt buộc)
  - `fromDate` (DateTime UTC, Mặc định: `00:00:00Z` hôm nay)
  - `toDate` (DateTime UTC, Mặc định: hiện tại)
- **Kiểm tra Thẩm quyền:** Chỉ chủ sở hữu bãi xe hoặc Manager mới được truy xuất báo cáo của bãi xe đó.
- **Phản hồi (`200 OK`):**
  ```json
  {
    "success": true,
    "message": "Lấy báo cáo hoạt động thành công.",
    "data": {
      "parkingLotId": "33333333-3333-3333-3333-333333333333",
      "parkingLotName": "Ben Thanh Central Parking Lot",
      "fromDate": "2026-10-13T00:00:00Z",
      "toDate": "2026-10-13T23:59:59Z",
      "totalEntries": 45,
      "totalExits": 5,
      "currentlyParked": 40,
      "availableSpots": 60,
      "totalCapacity": 100,
      "totalRevenue": 1450000.0,
      "currency": "VND",
      "isActive": true
    },
    "errors": []
  }
  ```

---

## 7. Thông Báo Sự Kiện Thời Gian Thực (Webhooks)

### 7.1 Danh Mục Sự Kiện (Webhook Events):
- `vehicle.checked_in`: Xe vừa quét thẻ vào bãi thành công.
- `vehicle.checked_out`: Xe vừa thanh toán và ra bãi.
- `parking_lot.full`: Bãi xe hết chỗ đỗ (`AvailableSpots = 0`).
- `parking_lot.available`: Bãi xe có chỗ trống trở lại.

### 7.2 Cấu Trúc Payload Mẫu:
```json
{
  "eventId": "evt_1f4bab4d1820400f9b50ed8bf2bb03b3",
  "eventType": "vehicle.checked_in",
  "timestamp": "2026-10-13T08:35:12Z",
  "parkingLotId": "33333333-3333-3333-3333-333333333333",
  "licensePlate": {
    "provinceCode": "59",
    "plateNumber": "99988",
    "fullPlateNumber": "59-99988"
  },
  "vehicleTypeId": "66666666-6666-6666-6666-666666666666",
  "vehicleTypeName": "Xe máy",
  "availableSpotsRemaining": 42
}
```

---

## 8. Lịch Sử Thay Đổi (Changelog)

* **Phiên bản 1.0 (2026-10-09):** Thiết kế ban đầu kiến trúc Controller truyền thống.
* **Phiên bản 1.3 (2026-10-12):** Tái cấu trúc hoàn toàn sang **CQRS với MediatR** và **Minimal API Endpoints (REPR Pattern)**.
* **Phiên bản 2.0 (2026-10-14):**
  - **Chuẩn hóa kiến trúc & luồng dữ liệu:** Sơ đồ dữ liệu chuẩn xác 100% dòng chảy tuần tự của Luồng ghi (Application ➔ Domain ➔ Infrastructure ➔ Cơ sở dữ liệu) và Luồng đọc tối ưu (Application ➔ Infrastructure ➔ Cơ sở dữ liệu).
  - **Chính sách Xóa mềm (Soft Delete) & Xóa cứng (Hard Delete):** Quy định rõ ràng điều kiện xóa cứng khi chưa có lịch sử gửi xe (`204 No Content`), chuyển xóa mềm bảo toàn lịch sử khi đã có dữ liệu và chặn xóa khi còn xe đỗ (`400 Bad Request`).
  - **Đồng bộ hợp đồng API & DTO:** Chuẩn hóa toàn bộ schema DTO, định dạng múi giờ UTC ISO 8601, mã UUID v4 và khớp từng trường báo cáo hoạt động (`totalEntries`, `totalExits`, `currentlyParked`, `totalRevenue`).
  - **Phân quyền chặt chẽ:** Quy định kiểm tra quyền sở hữu tài nguyên (Ownership Check) phía máy chủ và phân cấp quyền RBAC (`403 Forbidden`).

