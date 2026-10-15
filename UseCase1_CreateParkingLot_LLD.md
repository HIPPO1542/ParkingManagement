# TÀI LIỆU THIẾT KẾ KỸ THUẬT CHI TIẾT 
## Use Case 1: Thêm Bãi Đỗ Xe 

---

## 1. Dữ liệu

### 1.1. Dữ liệu đầu vào

#### 1.1.1. Cấu trúc dữ liệu 
Bảng đặc tả chi tiết các thuộc tính đầu vào của yêu cầu tạo bãi đỗ xe mới theo đúng mô hình DTO (`CreateParkingLotDto`):

| STT | Thuộc tính | Kiểu dữ liệu | Bắt buộc | Mô tả & Ràng buộc kỹ thuật |
| :---: | :--- | :--- | :---: | :--- |
| 1 | `businessOperatorId` | String (UUID) | Có | Định danh đơn vị kinh doanh sở hữu. Khóa ngoại tham chiếu đến bảng `business_operators`. |
| 2 | `name` | String | Có | Tên bãi đỗ xe (Độ dài từ 3 - 150 ký tự). Duy nhất trong cùng một đơn vị kinh doanh. |
| 3 | `address.street` | String | Có | Địa chỉ chi tiết (Số nhà, tên đường). Không được để trống. |
| 4 | `address.ward` | String | Không | Phường / Xã nơi đặt bãi xe. |
| 5 | `address.district` | String | Không | Quận / Huyện / Thành phố trực thuộc tỉnh. |
| 6 | `address.city` | String | Có | Tỉnh / Thành phố trực thuộc trung ương. |
| 7 | `totalCapacity` | Integer | Có | Tổng sức chứa thiết kế của bãi xe. Giá trị nguyên dương (`totalCapacity > 0`). |
| 8 | `openTime` | String (`HH:mm`) | Không | Khung giờ mở cửa hoạt động hàng ngày (Mặc định: `"06:00"`). |
| 9 | `closeTime` | String (`HH:mm`) | Không | Khung giờ đóng cửa hoạt động hàng ngày (Mặc định: `"22:00"`). |
| 10 | `isActive` | Boolean | Không | Trạng thái kích hoạt bãi xe ngay sau khi tạo (Mặc định: `true`). |

#### 1.1.2. Định dạng dữ liệu 
* **Phương thức HTTP:** `POST`
* **Route:** `/api/parking-lots`
* **Headers:**
  * `Authorization: Bearer <JWT_Token>`
  * `Content-Type: application/json`
  * `Accept: application/json`

#### 1.1.3. Ví dụ dữ liệu đầu vào

##### a) Định dạng HTTP Request:
```http
POST /api/parking-lots
Host: api.smartparking.vn
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "businessOperatorId": "33333333-3333-3333-3333-333333333333",
  "name": "Bãi xe KTX Khu A",
  "address": {
    "street": "Đường Tạ Quang Bửu, Khu phố 6",
    "ward": "Phường Linh Trung",
    "district": "Thành phố Thủ Đức",
    "city": "Thành phố Hồ Chí Minh"
  },
  "totalCapacity": 100,
  "openTime": "06:00",
  "closeTime": "22:00",
  "isActive": true
}
```

##### b) Lệnh cURL:
```bash
curl -X POST "https://api.smartparking.vn/api/parking-lots" \
  -H "Authorization: Bearer <access_token>" \
  -H "Content-Type: application/json" \
  -d '{
    "businessOperatorId": "33333333-3333-3333-3333-333333333333",
    "name": "Bãi xe KTX Khu A",
    "address": {
      "street": "Đường Tạ Quang Bửu, Khu phố 6",
      "ward": "Phường Linh Trung",
      "district": "Thành phố Thủ Đức",
      "city": "Thành phố Hồ Chí Minh"
    },
    "totalCapacity": 100,
    "openTime": "06:00",
    "closeTime": "22:00",
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
| 3 | `data.id` | String (UUID) | Có | Khóa chính của bản ghi bãi xe vừa tạo. |
| 4 | `data.businessOperatorId` | String (UUID) | Có | Mã đơn vị kinh doanh sở hữu bãi xe. |
| 5 | `data.name` | String | Có | Tên bãi đỗ xe. |
| 6 | `data.address.street` | String | Có | Số nhà, tên đường. |
| 7 | `data.address.ward` | String | Có | Phường / Xã. |
| 8 | `data.address.district` | String | Có | Quận / Huyện. |
| 9 | `data.address.city` | String | Có | Tỉnh / Thành phố. |
| 10 | `data.fullAddress` | String | Có | Chuỗi địa chỉ ghép đầy đủ. |
| 11 | `data.totalCapacity` | Integer | Có | Tổng sức chứa thiết kế. |
| 12 | `data.availableSpots` | Integer | Có | Số chỗ còn trống (khởi tạo mặc định bằng `totalCapacity`). |
| 13 | `data.openTime` | String | Có | Khung giờ mở cửa hoạt động (`HH:mm`). |
| 14 | `data.closeTime` | String | Có | Khung giờ đóng cửa hoạt động (`HH:mm`). |
| 15 | `data.isActive` | Boolean | Có | Trạng thái hoạt động (`true`: Đang hoạt động). |

#### 1.2.2. Ví dụ dữ liệu đầu ra

##### a) Phản hồi thành công (`201 Created`):
```json
{
  "code": 0,
  "message": "Success",
  "data": {
    "id": "77777777-7777-7777-7777-777777777777",
    "businessOperatorId": "33333333-3333-3333-3333-333333333333",
    "name": "Bãi xe KTX Khu A",
    "address": {
      "street": "Đường Tạ Quang Bửu, Khu phố 6",
      "ward": "Phường Linh Trung",
      "district": "Thành phố Thủ Đức",
      "city": "Thành phố Hồ Chí Minh"
    },
    "fullAddress": "Đường Tạ Quang Bửu, Khu phố 6, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh",
    "totalCapacity": 100,
    "availableSpots": 100,
    "openTime": "06:00",
    "closeTime": "22:00",
    "isActive": true
  }
}
```

##### b) Phản hồi lỗi dữ liệu không hợp lệ (`400 Bad Request`):
```json
{
  "code": 400,
  "message": "Sức chứa bãi xe (totalCapacity) phải lớn hơn 0.",
  "data": null
}
```

##### c) Phản hồi lỗi vi phạm trùng tên bãi đỗ (`422 Unprocessable Entity`):
```json
{
  "code": 422,
  "message": "Tên bãi xe 'Bãi xe KTX Khu A' đã tồn tại trong đơn vị kinh doanh này.",
  "data": null
}
```

---

### 1.3. Dữ liệu lưu trữ (Database Storage)

Dữ liệu của Use Case này được lưu trữ trực tiếp vào bảng `parking_lots` trong cơ sở dữ liệu SQLite:

#### Bảng: `parking_lots` (Lưu thông tin bãi đỗ xe)

| STT | Tên cột | Kiểu dữ liệu | Khóa chính (PK) | Khóa ngoại (FK) | Nullable | Giá trị mặc định | Mô tả chi tiết |
| :---: | :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| 1 | `id` | TEXT (UUID) | PK | | Không | | Mã định danh duy nhất của bãi đỗ xe. |
| 2 | `business_operator_id`| TEXT (UUID) | | FK | Không | | Khóa ngoại tham chiếu đến `business_operators(id)`. |
| 3 | `name` | TEXT (VARCHAR 150) | | | Không | | Tên bãi đỗ xe (Unique theo `business_operator_id`). |
| 4 | `street` | TEXT (VARCHAR 255) | | | Không | | Địa chỉ số nhà, tên đường. |
| 5 | `ward` | TEXT (VARCHAR 100) | | | Không | | Phường / Xã. |
| 6 | `district` | TEXT (VARCHAR 100) | | | Không | | Quận / Huyện / Thị xã. |
| 7 | `city` | TEXT (VARCHAR 100) | | | Không | | Tỉnh / Thành phố trực thuộc trung ương. |
| 8 | `total_capacity` | INTEGER | | | Không | | Tổng số chỗ đỗ xe thiết kế (`total_capacity > 0`). |
| 9 | `available_spots` | INTEGER | | | Không | | Số lượng vị trí còn trống (`available_spots == total_capacity` khi khởi tạo). |
| 10 | `open_time` | TEXT (CHAR 8) | | | Không | `'06:00'` | Khung giờ mở cửa hoạt động hàng ngày (`HH:mm`). |
| 11 | `close_time` | TEXT (CHAR 8) | | | Không | `'22:00'` | Khung giờ đóng cửa hoạt động hàng ngày (`HH:mm`). |
| 12 | `is_active` | INTEGER (TINYINT) | | | Không | `1` | Trạng thái hoạt động: `1` (Hoạt động), `0` (Tạm ngưng). |

---

## 2. Sơ đồ

### 2.1. Sơ đồ thiết kế lớp (Class Diagram)

Sơ đồ thiết kế lớp biểu diễn các Thực thể dữ liệu (Entities) và Đối tượng giá trị (Value Objects) tham gia trực tiếp vào việc xử lý nghiệp vụ của Use Case Thêm bãi đỗ xe:

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
        +RecordVehicleEntry()
        +RecordVehicleExit()
        +UpdateInfo()
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
| `ParkingLot` | **Aggregate Root** | Thực thể gốc quản lý vòng đời của bãi đỗ xe. Chịu trách nhiệm kiểm soát các quy tắc nghiệp vụ: tổng sức chứa (`total_capacity > 0`), tự động đồng bộ số chỗ trống ban đầu (`available_spots = total_capacity`), khung giờ mở/đóng cửa và trạng thái kích hoạt. |
| `BusinessOperator` | **Aggregate Root** | Đơn vị kinh doanh sở hữu bãi đỗ xe. Use Case cần tham chiếu tới đơn vị này để kiểm tra sự tồn tại và đảm bảo tên bãi đỗ là duy nhất trong phạm vi của đơn vị. |
| `Address` | **Value Object** | **Đối tượng giá trị (Value Object)** đại diện cho thông tin địa chỉ nơi đặt bãi xe. Không có định danh riêng (`id`), tính chất bất biến (Immutable), toàn vẹn dữ liệu (gồm đủ 4 thành phần: `street`, `ward`, `district`, `city`). Được nhúng trực tiếp (`Composition *--`) vào thực thể `ParkingLot`. |

---

### 2.2. Sơ đồ Sequence (Sequence Diagram)

Sơ đồ tuần tự chi tiết luồng thực thi khi Nhà điều hành thực hiện Use Case Thêm bãi đỗ xe:

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
        Handler->>Handler: Kiểm tra trùng tên (case-insensitive) -> Đã tồn tại
        Handler-->>API: Ném BusinessRuleViolationException ("Tên bãi xe đã tồn tại")
        API-->>Operator: 422 Unprocessable Entity
    else Tên bãi xe hợp lệ (chưa tồn tại)
        Handler->>Domain: Khởi tạo ParkingLot(id, operatorId, name, address, totalCapacity, openTime, closeTime)
        
        alt Kịch bản 2: Dung lượng không hợp lệ (totalCapacity <= 0)
            Domain-->>Handler: Ném ArgumentOutOfRangeException ("Sức chứa phải lớn hơn 0")
            Handler-->>API: 400 Bad Request
            API-->>Operator: 400 Bad Request
        else Dữ liệu hợp lệ thành công
            Domain->>Domain: Gán AvailableSpots = totalCapacity, IsActive = true
            Domain-->>Handler: Trả về đối tượng ParkingLot mới tạo
            Handler->>LotRepo: AddAsync(newLot)
            Handler->>UoW: SaveChangesAsync()
            UoW-->>Handler: Commit CSDL thành công
            Handler-->>API: Trả về ParkingLotDto (Kèm ID mới sinh)
            API-->>Operator: 201 Created (Kèm Response Body chi tiết)
        end
    end
```
