# TÀI LIỆU THIẾT KẾ KỸ THUẬT CHI TIẾT
## Use Case 2: Chỉnh Sửa Thông Tin Bãi Đỗ Xe

---

## 1. Dữ liệu

### 1.1. Dữ liệu đầu vào

#### 1.1.1. Cấu trúc dữ liệu
Bảng đặc tả chi tiết các thuộc tính đầu vào của yêu cầu cập nhật bãi đỗ xe theo mô hình `UpdateParkingLotDto` và tham số định danh URL:

| STT | Thuộc tính | Kiểu dữ liệu | Bắt buộc | Mô tả & Ràng buộc kỹ thuật |
| :---: | :--- | :--- | :---: | :--- |
| 1 | `id` | String (UUID) | Có | Mã định danh duy nhất của bãi đỗ xe cần chỉnh sửa (truyền qua Path Parameter). |
| 2 | `name` | String | Có | Tên bãi đỗ xe cập nhật (Độ dài từ 3 - 150 ký tự). Không được để trống. |
| 3 | `address.street` | String | Có | Địa chỉ chi tiết (Số nhà, tên đường). Không được để trống. |
| 4 | `address.ward` | String | Không | Phường / Xã nơi đặt bãi xe. |
| 5 | `address.district` | String | Không | Quận / Huyện / Thành phố trực thuộc tỉnh. |
| 6 | `address.city` | String | Có | Tỉnh / Thành phố trực thuộc trung ương. |
| 7 | `totalCapacity` | Integer | Có | Tổng sức chứa thiết kế mới. Giá trị nguyên dương (`totalCapacity > 0`). |
| 8 | `openTime` | String (`HH:mm`) | Không | Khung giờ mở cửa hoạt động hàng ngày (Mặc định: `"06:00"`). |
| 9 | `closeTime` | String (`HH:mm`) | Không | Khung giờ đóng cửa hoạt động hàng ngày (Mặc định: `"22:00"`). |
| 10 | `isActive` | Boolean | Không | Trạng thái kích hoạt bãi xe (`true`: Hoạt động, `false`: Tạm ngưng). |

#### 1.1.2. Định dạng dữ liệu
* **Phương thức HTTP:** `PUT`
* **Route:** `/api/parking-lots/{id}`
* **Headers:**
  * `Authorization: Bearer <JWT_Token>`
  * `Content-Type: application/json`
  * `Accept: application/json`

#### 1.1.3. Ví dụ dữ liệu đầu vào

##### a) Định dạng HTTP Request:
```http
PUT /api/parking-lots/77777777-7777-7777-7777-777777777777
Host: api.smartparking.vn
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "name": "Bãi xe KTX Khu A Mở Rộng",
  "address": {
    "street": "Đường Tạ Quang Bửu, Phường Linh Trung",
    "ward": "Phường Linh Trung",
    "district": "Thành phố Thủ Đức",
    "city": "Thành phố Hồ Chí Minh"
  },
  "totalCapacity": 150,
  "openTime": "05:30",
  "closeTime": "23:00",
  "isActive": true
}
```

##### b) Lệnh cURL:
```bash
curl -X PUT "https://api.smartparking.vn/api/parking-lots/77777777-7777-7777-7777-777777777777" \
  -H "Authorization: Bearer <access_token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Bãi xe KTX Khu A Mở Rộng",
    "address": {
      "street": "Đường Tạ Quang Bửu, Phường Linh Trung",
      "ward": "Phường Linh Trung",
      "district": "Thành phố Thủ Đức",
      "city": "Thành phố Hồ Chí Minh"
    },
    "totalCapacity": 150,
    "openTime": "05:30",
    "closeTime": "23:00",
    "isActive": true
  }'
```

---

### 1.2. Dữ liệu đầu ra

#### 1.2.1. Cấu trúc dữ liệu (Data Structure)
Bảng đặc tả cấu trúc dữ liệu trả về theo mô hình `ParkingLotDto`:

| STT | Thuộc tính | Kiểu dữ liệu | Bắt buộc | Mô tả chi tiết |
| :---: | :--- | :--- | :---: | :--- |
| 1 | `code` | Integer | Có | Mã kết quả thực thi (`0`: Thành công, khác `0`: Thất bại). |
| 2 | `message` | String | Có | Thông điệp kết quả xử lý. |
| 3 | `data.id` | String (UUID) | Có | Khóa chính của bản ghi bãi xe được cập nhật. |
| 4 | `data.businessOperatorId` | String (UUID) | Có | Mã đơn vị kinh doanh sở hữu bãi xe. |
| 5 | `data.name` | String | Có | Tên bãi đỗ xe sau khi cập nhật. |
| 6 | `data.address.street` | String | Có | Số nhà, tên đường. |
| 7 | `data.address.ward` | String | Có | Phường / Xã. |
| 8 | `data.address.district` | String | Có | Quận / Huyện. |
| 9 | `data.address.city` | String | Có | Tỉnh / Thành phố. |
| 10 | `data.fullAddress` | String | Có | Chuỗi địa chỉ ghép đầy đủ. |
| 11 | `data.totalCapacity` | Integer | Có | Tổng sức chứa thiết kế mới. |
| 12 | `data.availableSpots` | Integer | Có | Số chỗ còn trống được tự động tính toán lại theo độ chênh lệch dung lượng. |
| 13 | `data.openTime` | String | Có | Khung giờ mở cửa hoạt động (`HH:mm`). |
| 14 | `data.closeTime` | String | Có | Khung giờ đóng cửa hoạt động (`HH:mm`). |
| 15 | `data.isActive` | Boolean | Có | Trạng thái hoạt động (`true`: Hoạt động, `false`: Tạm ngưng). |

#### 1.2.2. Ví dụ dữ liệu đầu ra

##### a) Phản hồi thành công (`200 OK`):
```json
{
  "code": 0,
  "message": "Parking lot updated successfully.",
  "data": {
    "id": "77777777-7777-7777-7777-777777777777",
    "businessOperatorId": "33333333-3333-3333-3333-333333333333",
    "name": "Bãi xe KTX Khu A Mở Rộng",
    "address": {
      "street": "Đường Tạ Quang Bửu, Phường Linh Trung",
      "ward": "Phường Linh Trung",
      "district": "Thành phố Thủ Đức",
      "city": "Thành phố Hồ Chí Minh"
    },
    "fullAddress": "Đường Tạ Quang Bửu, Phường Linh Trung, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh",
    "totalCapacity": 150,
    "availableSpots": 150,
    "openTime": "05:30",
    "closeTime": "23:00",
    "isActive": true
  }
}
```

##### b) Phản hồi lỗi dữ liệu không hợp lệ (`400 Bad Request`):
```json
{
  "code": 400,
  "message": "Total capacity must be greater than 0.",
  "data": null
}
```

##### c) Phản hồi lỗi không có quyền can thiệp (`403 Forbidden`):
```json
{
  "code": 403,
  "message": "Business operators can only update their own parking lots.",
  "data": null
}
```

##### d) Phản hồi lỗi không tìm thấy bãi đỗ xe (`404 Not Found`):
```json
{
  "code": 404,
  "message": "Parking lot with ID '77777777-7777-7777-7777-777777777777' not found.",
  "data": null
}
```

---

### 1.3. Dữ liệu lưu trữ (Database Storage)

Dữ liệu cập nhật của Use Case này tác động trực tiếp vào bản ghi tương ứng trong bảng `parking_lots` của cơ sở dữ liệu:

#### Bảng: `parking_lots` (Lưu thông tin bãi đỗ xe)

| STT | Tên cột | Kiểu dữ liệu | Khóa chính (PK) | Khóa ngoại (FK) | Nullable | Giá trị mặc định | Mô tả chi tiết |
| :---: | :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| 1 | `id` | TEXT (UUID) | PK | | Không | | Mã định danh duy nhất của bãi đỗ xe. |
| 2 | `business_operator_id`| TEXT (UUID) | | FK | Không | | Khóa ngoại tham chiếu đến `business_operators(id)`. |
| 3 | `name` | TEXT (VARCHAR 150) | | | Không | | Tên bãi đỗ xe. |
| 4 | `street` | TEXT (VARCHAR 255) | | | Không | | Địa chỉ số nhà, tên đường. |
| 5 | `ward` | TEXT (VARCHAR 100) | | | Không | | Phường / Xã. |
| 6 | `district` | TEXT (VARCHAR 100) | | | Không | | Quận / Huyện / Thị xã. |
| 7 | `city` | TEXT (VARCHAR 100) | | | Không | | Tỉnh / Thành phố trực thuộc trung ương. |
| 8 | `total_capacity` | INTEGER | | | Không | | Tổng số chỗ đỗ xe sau khi cập nhật (`total_capacity > 0`). |
| 9 | `available_spots` | INTEGER | | | Không | | Số lượng vị trí còn trống được điều chỉnh theo độ lệch dung lượng. |
| 10 | `open_time` | TEXT (CHAR 8) | | | Không | `'06:00'` | Khung giờ mở cửa hoạt động hàng ngày (`HH:mm`). |
| 11 | `close_time` | TEXT (CHAR 8) | | | Không | `'22:00'` | Khung giờ đóng cửa hoạt động hàng ngày (`HH:mm`). |
| 12 | `is_active` | INTEGER (TINYINT) | | | Không | `1` | Trạng thái hoạt động: `1` (Hoạt động), `0` (Tạm ngưng). |

---

## 2. Sơ đồ

### 2.1. Sơ đồ thiết kế lớp (Class Diagram)

Sơ đồ thiết kế lớp biểu diễn các Thực thể dữ liệu (Entities) và Đối tượng giá trị (Value Objects) tham gia trực tiếp vào việc xử lý nghiệp vụ của Use Case Chỉnh sửa thông tin bãi đỗ xe:

```mermaid
classDiagram
    class BusinessOperator {
        <<Aggregate Root>>
        +Guid id
        +string name
        +string representative
        +Address address
        +string phone_number
        +string email
        +bool is_active
    }

    class ParkingLot {
        <<Aggregate Root>>
        +Guid id
        +Guid business_operator_id
        +string name
        +Address address
        +int total_capacity
        +int available_spots
        +string open_time
        +string close_time
        +bool is_active
        +UpdateInfo(string name, Address address, int totalCapacity, string openTime, string closeTime, bool isActive)
        +RecordVehicleEntry()
        +RecordVehicleExit()
        +SetStatus(bool isActive)
    }

    class Address {
        <<Value Object>>
        +string street
        +string ward
        +string district
        +string city
        +GetFullAddress() string
    }

    BusinessOperator "1" --> "*" ParkingLot : sở hữu (owns)
    ParkingLot "1" *-- "1" Address : vị trí (location)
```

#### Bảng mô tả chi tiết các lớp và đối tượng:

| Tên lớp / Đối tượng | Phân loại DDD | Vai trò & Đặc điểm kỹ thuật |
| :--- | :--- | :--- |
| `ParkingLot` | **Aggregate Root** | Thực thể gốc quản lý toàn bộ vòng đời và luật nghiệp vụ của bãi đỗ xe. Phương thức `UpdateInfo` chịu trách nhiệm bảo vệ luật bất biến: kiểm tra `name` không rỗng, `totalCapacity > 0`, và tự động cân đối lại `availableSpots` theo công thức `availableSpots = Math.Clamp(availableSpots + delta, 0, totalCapacity)`. |
| `BusinessOperator` | **Aggregate Root** | Đơn vị kinh doanh sở hữu bãi đỗ xe. Đóng vai trò đối tượng bảo vệ quyền sở hữu, ngăn chặn hành vi nhà điều hành sửa đổi trái phép bãi xe của đơn vị khác. |
| `Address` | **Value Object** | **Đối tượng giá trị (Value Object)** mô tả địa chỉ chuẩn của bãi đỗ xe gồm 4 trường (`street`, `ward`, `district`, `city`). Có tính bất biến (Immutable), mỗi lần cập nhật địa chỉ sẽ thay thế hoàn toàn bằng một thực thể `Address` mới. |

---

### 2.2. Sơ đồ Sequence (Sequence Diagram)

Sơ đồ tuần tự chi tiết luồng xử lý theo chuẩn **CQRS Command** khi thực hiện Use Case Chỉnh sửa thông tin bãi đỗ xe:

```mermaid
sequenceDiagram
    autonumber
    actor Operator as 👤 Nhà điều hành (Business Operator)
    participant API as 🌐 Minimal API (PUT /api/parking-lots/{id})
    participant Handler as ⚙️ UpdateParkingLotCommandHandler
    participant LotRepo as 🗄️ IParkingLotRepository
    participant Domain as 🧠 ParkingLot (Domain Entity)
    participant UoW as 🗄️ IUnitOfWork (DbContext)

    Operator->>API: PUT /api/parking-lots/{id} (UpdateParkingLotDto)
    
    alt Kiểm tra quyền sở hữu (Ownership Check)
        API->>LotRepo: GetByIdAsync(id)
        LotRepo-->>API: Trả về ParkingLot hiện tại
        alt Bãi xe không thuộc sở hữu của nhà điều hành gửi yêu cầu
            API-->>Operator: 403 Forbidden ("Business operators can only update their own parking lots.")
        end
    end

    API->>Handler: Send(UpdateParkingLotCommand(id, dto))
    Handler->>LotRepo: GetByIdAsync(id)
    
    alt Kịch bản 1: Không tìm thấy bãi đỗ xe
        LotRepo-->>Handler: null
        Handler-->>API: Ném EntityNotFoundException("Parking lot", id)
        API-->>Operator: 404 Not Found
    else Tìm thấy bãi đỗ xe hợp lệ
        LotRepo-->>Handler: Đối tượng ParkingLot hiện tại
        Handler->>Handler: Tạo mới Address(street, ward, district, city)
        Handler->>Domain: UpdateInfo(name, address, totalCapacity, openTime, closeTime, isActive)
        
        alt Kịch bản 2: Dữ liệu vi phạm bất biến (totalCapacity <= 0 hoặc name rỗng)
            Domain-->>Handler: Ném ArgumentOutOfRangeException / ArgumentException
            Handler-->>API: 400 Bad Request
            API-->>Operator: 400 Bad Request
        else Dữ liệu hợp lệ thành công
            Domain->>Domain: Tính delta = newCapacity - oldCapacity
            Domain->>Domain: Cập nhật AvailableSpots = Clamp(AvailableSpots + delta, 0, newCapacity)
            Domain->>Domain: Cập nhật Name, Address, OpenTime, CloseTime, IsActive
            Domain-->>Handler: Hoàn tất cập nhật thực thể
            
            Handler->>LotRepo: UpdateAsync(lot)
            Handler->>UoW: SaveChangesAsync()
            UoW-->>Handler: Commit CSDL thành công
            Handler-->>API: Trả về ParkingLotDto cập nhật
            API-->>Operator: 200 OK (code: 0, Message: "Parking lot updated successfully.")
        end
    end
```
