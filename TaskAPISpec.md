# Smart Parking Management API Specification
**Version:** 1.0 / 2026-10-13  
**Base URL:** `http://localhost:5097` (hoặc `https://api.parkingmanagement.vn`)  
**Mô tả:** Hệ thống Web API quản lý bãi đỗ xe thông minh theo kiến trúc Clean Architecture, CQRS (MediatR) và REPR Minimal API Endpoints.

---

## 1. Common (Quy ước chung)

### 1.1 URLs & Format
- Mọi tài nguyên API đều bắt đầu bằng prefix: `/api/...`
- Tất cả dữ liệu gửi đi (Request Body) và nhận về (Response Body) đều sử dụng định dạng **JSON** (`Content-Type: application/json; charset=utf-8`).

### 1.2 Authentication & Authorization
Hệ thống sử dụng cơ chế bảo mật **JWT (JSON Web Token)** Bearer.
- Token được gửi kèm trong HTTP Request Header:
  ```http
  Authorization: Bearer <your_jwt_token>
  ```
- **Phân quyền (Roles):**
  - `Manager` (Quản trị viên hệ thống): Toàn quyền thêm, sửa, xóa bãi xe, tài khoản và nhà điều hành.
  - `BusinessOperator` (Nhà điều hành bãi xe): Quản lý các bãi xe thuộc sở hữu của mình, xem báo cáo doanh thu.
  - `Attendant` (Nhân viên bãi xe): Thực hiện nghiệp vụ Check-in, Check-out xe.

### 1.3 Standard Response Format (`ApiResponse<T>`)
Tất cả các API đều trả về cấu trúc bọc chung:
```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": { ... },
  "errors": []
}
```

### 1.4 Response Status Codes (Mã phản hồi HTTP)
- **`200 OK`**: Yêu cầu xử lý thành công (thường dùng cho `GET`, `PUT`).
- **`201 Created`**: Tạo mới thành công bản ghi dữ liệu (dùng cho `POST`), kèm Header `Location`.
- **`204 No Content`**: Xóa thành công, không có nội dung trả về.
- **`400 Bad Request`**: Dữ liệu gửi lên không hợp lệ, thiếu trường bắt buộc hoặc vi phạm Business Rule.
- **`401 Unauthorized`**: Chưa đăng nhập hoặc Token JWT không hợp lệ/hết hạn.
- **`403 Forbidden`**: Không có quyền truy cập vào chức năng (ví dụ: Attendant gọi API xóa bãi xe).
- **`404 Not Found`**: Không tìm thấy tài nguyên (Bãi xe, Lượt gửi xe, Loại xe không tồn tại).
- **`500 Internal Server Error`**: Lỗi hệ thống ngoài ý muốn (được bắt qua `GlobalExceptionMiddleware`).

---

## 2. Authentication API (`/api/auth`)

### 2.1 Login Endpoint
* **URL:** `POST /api/auth/login`
* **Quyền hạn:** Public
* **Mô tả:** Đăng nhập vào hệ thống để lấy chuỗi Access Token JWT.

#### Request Body:
```json
{
  "username": "admin",
  "password": "Password@123"
}
```

#### Fields:
| Field name | Type | Description | Validation |
| :--- | :--- | :--- | :--- |
| `username` | String | Tên đăng nhập tài khoản | Bắt buộc, tối đa 50 ký tự |
| `password` | String | Mật khẩu truy cập | Bắt buộc, tối thiểu 6 ký tự |

#### Response (200 OK):
```json
{
  "success": true,
  "message": "Login successfully.",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "userId": "11111111-1111-1111-1111-111111111111",
    "username": "admin",
    "role": "Manager"
  },
  "errors": []
}
```

---

### 2.2 Get Current User Profile Endpoint
* **URL:** `GET /api/auth/me`
* **Quyền hạn:** Yêu cầu đăng nhập (`Authorize`)
* **Mô tả:** Lấy thông tin tài khoản hiện tại từ Token JWT.

#### Response (200 OK):
```json
{
  "success": true,
  "message": "User profile retrieved successfully.",
  "data": {
    "id": "11111111-1111-1111-1111-111111111111",
    "username": "admin",
    "fullName": "Nguyen Van Quan Tri",
    "role": "Manager"
  },
  "errors": []
}
```

---

## 3. Parking Sessions API (`/api/parking-sessions`)

Quản lý toàn bộ vòng đời lượt gửi xe (Check-in, Check-out, Lịch sử ra vào).

### 3.1 Check-In Endpoint
* **URL:** `POST /api/parking-sessions/check-in`
* **Mô tả:** Ghi nhận xe vào bãi, kiểm tra sức chứa bãi xe và tạo phiên gửi xe mới.
* **CQRS Mapping:** `CheckInCommand` ➔ `CheckInCommandHandler`

#### Request Body:
```json
{
  "parkingLotId": "33333333-3333-3333-3333-333333333333",
  "vehicleTypeId": "66666666-6666-6666-6666-666666666666",
  "provinceCode": "59",
  "plateNumber": "99988"
}
```

#### Fields:
| Field name | Type | Description | Validation |
| :--- | :--- | :--- | :--- |
| `parkingLotId` | UUID (Guid) | Mã định danh bãi đỗ xe | Bắt buộc, phải tồn tại trong DB |
| `vehicleTypeId` | UUID (Guid) | Mã loại xe (xe máy, ô tô...) | Bắt buộc, phải tồn tại trong DB |
| `provinceCode` | String | Mã tỉnh/thành phố trên biển số (VD: 59, 29) | Bắt buộc, tối đa 20 ký tự |
| `plateNumber` | String | Dãy số trên biển kiểm soát (VD: 99988) | Bắt buộc, tối đa 50 ký tự |

#### Response (201 Created):
```json
{
  "success": true,
  "message": "Check-in successfully.",
  "data": {
    "id": "9a781e79-1b43-45dd-89f6-f55e42aa03e5",
    "parkingLotId": "33333333-3333-3333-3333-333333333333",
    "parkingLotName": "Ben Thanh Central Parking Lot",
    "vehicleTypeId": "66666666-6666-6666-6666-666666666666",
    "vehicleTypeName": "Motorbike",
    "fullPlateNumber": "59-99988",
    "checkInTime": "2026-10-13T10:45:00Z",
    "checkOutTime": null,
    "fee": {
      "amount": 0.0,
      "currency": "VND"
    },
    "status": "Active"
  },
  "errors": []
}
```

---

### 3.2 Check-Out by Plate Endpoint
* **URL:** `POST /api/parking-sessions/check-out`
* **Mô tả:** Xe rời bãi theo biển số. Hệ thống tự động tính tiền gửi dựa trên Bảng giá (Tariff) và thời gian đỗ, đồng thời giảm số lượng xe trong bãi.
* **CQRS Mapping:** `CheckOutByPlateCommand` ➔ `CheckOutByPlateCommandHandler`

#### Request Body:
```json
{
  "parkingLotId": "33333333-3333-3333-3333-333333333333",
  "provinceCode": "59",
  "plateNumber": "99988"
}
```

#### Response (200 OK):
```json
{
  "success": true,
  "message": "Check-out successfully.",
  "data": {
    "id": "9a781e79-1b43-45dd-89f6-f55e42aa03e5",
    "fullPlateNumber": "59-99988",
    "checkInTime": "2026-10-13T08:00:00Z",
    "checkOutTime": "2026-10-13T11:00:00Z",
    "durationHours": 3.0,
    "fee": {
      "amount": 10000.0,
      "currency": "VND"
    },
    "status": "Completed"
  },
  "errors": []
}
```

---

### 3.3 List Parking Sessions Endpoint
* **URL:** `GET /api/parking-sessions`
* **Mô tả:** Lấy danh sách lịch sử phiên gửi xe có lọc và phân trang.
* **CQRS Mapping:** `GetParkingSessionsQuery` ➔ `GetParkingSessionsQueryHandler`

#### Query Parameters:
| Parameter | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `parkingLotId` | UUID | null | Lọc theo bãi xe cụ thể |
| `status` | String | null | Lọc trạng thái (`Active` hoặc `Completed`) |
| `page` | Integer | 1 | Số thứ tự trang hiện tại |
| `pageSize` | Integer | 10 | Số lượng bản ghi trên một trang |

---

## 4. Parking Lots API (`/api/parking-lots`)

Quản lý danh sách và thông tin các bãi đỗ xe.

### 4.1 List Parking Lots Endpoint
* **URL:** `GET /api/parking-lots`
* **Query Parameters:** `operatorId` (UUID), `isActive` (Boolean), `page` (Int), `pageSize` (Int)
* **Response (200 OK):** Danh sách bãi xe kèm metadata phân trang (`totalCount`, `items`).

---

### 4.2 Detail Parking Lot Endpoint
* **URL:** `GET /api/parking-lots/:id`
* **Response (200 OK):** Chi tiết 1 bãi xe bao gồm: Tên, địa chỉ, sức chứa tối đa (`totalCapacity`), số chỗ hiện còn trống (`availableCapacity`).

---

### 4.3 Create Parking Lot Endpoint
* **URL:** `POST /api/parking-lots`
* **Mô tả:** Thêm mới một bãi đỗ xe vào hệ thống.
* **CQRS Mapping:** `CreateParkingLotCommand` ➔ `CreateParkingLotCommandHandler`

#### Request Body:
```json
{
  "businessOperatorId": "22222222-2222-2222-2222-222222222222",
  "name": "Bãi xe Nhà hát Thành phố",
  "address": {
    "street": "07 Lam Sơn",
    "ward": "Bến Nghé",
    "district": "Quận 1",
    "city": "Hồ Chí Minh"
  },
  "totalCapacity": 200,
  "openTime": "06:00:00",
  "closeTime": "23:00:00",
  "isActive": true
}
```

#### Fields:
| Field name | Type | Description | Validation |
| :--- | :--- | :--- | :--- |
| `businessOperatorId` | UUID | ID của nhà điều hành sở hữu | Bắt buộc, tồn tại trong DB |
| `name` | String | Tên bãi đỗ xe | Bắt buộc, tối đa 200 ký tự |
| `address` | Object | Thông tin địa chỉ bãi xe | Bắt buộc (gồm street, ward, district, city) |
| `totalCapacity` | Integer | Sức chứa tối đa của bãi | Bắt buộc, số nguyên > 0 |
| `openTime` | TimeSpan / String | Giờ mở cửa | Định dạng `hh:mm:ss` |
| `closeTime` | TimeSpan / String | Giờ đóng cửa | Định dạng `hh:mm:ss` |
| `isActive` | Boolean | Trạng thái hoạt động | Mặc định `true` |

---

### 4.4 Update Parking Lot Endpoint
* **URL:** `PUT /api/parking-lots/:id`
* **Mô tả:** Cập nhật thông tin bãi xe (Sức chứa, giờ mở/đóng cửa, tên bãi).

---

### 4.5 Delete Parking Lot Endpoint
* **URL:** `DELETE /api/parking-lots/:id`
* **Quyền hạn:** `Manager` only
* **Response:** `200 OK` hoặc `204 No Content`.

---

## 5. Tariffs API (`/api/tariffs`)

Quản lý cơ chế biểu phí tính tiền gửi xe cho từng loại phương tiện.

### 5.1 Endpoints List:
- `GET /api/tariffs`: Lấy tất cả bảng giá đang áp dụng.
- `GET /api/tariffs/parking-lot/:parkingLotId`: Lấy bảng giá áp dụng cho riêng một bãi xe.
- `POST /api/tariffs`: Tạo quy tắc tính giá mới.
- `PUT /api/tariffs/:id`: Cập nhật biểu phí.
- `DELETE /api/tariffs/:id`: Xóa biểu phí.

#### Tariff Request Example:
```json
{
  "parkingLotId": "33333333-3333-3333-3333-333333333333",
  "vehicleTypeId": "66666666-6666-6666-6666-666666666666",
  "name": "Giá xe máy ban ngày",
  "hourlyRate": 5000.0,
  "fixedRate": 0.0,
  "pricingType": "Hourly",
  "startTime": "06:00:00",
  "endTime": "18:00:00"
}
```

---

## 6. Reports API (`/api/reports`)

Cung cấp các báo cáo thống kê, phân tích phục vụ nhà quản lý.

### 6.1 Parking Activity Report Endpoint
* **URL:** `GET /api/reports/activity`
* **Quyền hạn:** Yêu cầu đăng nhập (`Authorize`)
* **Query Parameters:**
  - `parkingLotId` (UUID, Bắt buộc): Mã bãi xe cần báo cáo.
  - `fromDate` (DateTime, Tuỳ chọn): Từ ngày (Mặc định: đầu ngày hôm nay).
  - `toDate` (DateTime, Tuỳ chọn): Đến ngày (Mặc định: hiện tại).

#### Response (200 OK):
```json
{
  "success": true,
  "message": "Activity report retrieved successfully.",
  "data": {
    "totalEntries": 150,
    "totalExits": 120,
    "currentlyParked": 30,
    "availableSpots": 70,
    "totalCapacity": 100,
    "totalRevenue": 750000.0,
    "currency": "VND",
    "isActive": true
  },
  "errors": []
}
```

---

## 7. Changelog
* **1.0.0 (2026-10-13):**
  - Khởi tạo tài liệu đặc tả Smart Parking Management API Specification.
  - Tái cấu trúc toàn bộ API Layer sang mô hình **REPR Minimal API Endpoints** (`IEndpoint`).
  - Áp dụng mô hình **CQRS** sử dụng thư viện **MediatR** (Tách biệt Commands và Queries).
