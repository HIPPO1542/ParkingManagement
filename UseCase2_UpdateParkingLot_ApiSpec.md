# API Quản lý bãi đỗ xe — Use Case 2: Chỉnh sửa thông tin bãi đỗ xe

## 1. Mục đích tài liệu

Tài liệu mô tả chi tiết API phục vụ **Use Case 2: Chỉnh sửa thông tin bãi đỗ xe**.  
Toàn bộ response được thống nhất theo dạng:

```json
{
  "code": 0,
  "message": "Success",
  "data": {
    "id": "77777777-7777-7777-7777-777777777777",
    "businessOperatorId": "33333333-3333-3333-3333-333333333333",
    "name": "Bãi xe KTX Khu A Mở Rộng",
    "address": {
      "street": "Đường Tạ Quang Bửu, Phường Linh Trung",
      "ward": "Phường Linh Trung",
      "district": "Thành phố Thủ Đức",
      "city": "Thành phố Hồ Chí Minh",
      "fullAddress": "Đường Tạ Quang Bửu, Phường Linh Trung, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh"
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

> Lưu ý:
>
> - Route dưới đây là **quy ước đề xuất** với tiền tố `/api/parking-lots/{id}`.
> - Bãi xe phải tồn tại trong hệ thống, nếu không sẽ trả về `404 Not Found`.
> - Nhà điều hành chỉ có quyền cập nhật thông tin bãi xe thuộc quyền sở hữu của đơn vị mình. Nếu cố tình sửa bãi xe của đơn vị khác, hệ thống sẽ từ chối với mã lỗi `403 Forbidden`.
> - Khi tổng sức chứa (`totalCapacity`) thay đổi, hệ thống sẽ tự động cập nhật lại số chỗ trống (`availableSpots`) dựa trên chênh lệch sức chứa thực tế.

---

## 2. Quy ước chung

- Method: `PUT`
- Content-Type request/response: `application/json`
- Xác thực: Bearer JWT Token (`Authorization: Bearer <token>`)
- Mã thành công:
  - `code = 0` (HTTP Status `200 OK`)
  - `message = "Success"`

### 2.1. Cấu trúc response chuẩn

| Trường     | Kiểu   | Bắt buộc | Mô tả                                       |
| ---------- | ------ | -------: | ------------------------------------------- |
| `code`     | number |       Có | Mã kết quả xử lý. `0` là thành công.        |
| `message`  | string |       Có | Thông điệp xử lý, ví dụ `Success`.          |
| `metadata` | object |    Không | Thông tin bổ trợ / phân trang (nếu có).     |
| `data`     | object |       Có | Dữ liệu chính trả về của API.               |

---

## 3. API Chỉnh sửa thông tin bãi đỗ xe

### 3.1. Mục đích

Tiếp nhận yêu cầu cập nhật các thông tin vận hành, quy mô hoặc địa chỉ của một bãi đỗ xe hiện hữu từ nhà điều hành hoặc quản trị viên.

### 3.2. Route đề xuất

`PUT /api/parking-lots/{id}`

- **Path Parameter:**
  - `id` (string - UUID, Bắt buộc): Mã định danh duy nhất của bãi đỗ xe cần chỉnh sửa.

### 3.3. Input parameters (Request Body)

| Tham số            | Kiểu    | Bắt buộc | Mô tả                                                                                     |
| ------------------ | ------- | -------: | ----------------------------------------------------------------------------------------- |
| `name`             | string  |       Có | Tên bãi đỗ xe cập nhật (Độ dài từ 3 - 150 ký tự).                                          |
| `address.street`   | string  |       Có | Địa chỉ chi tiết (số nhà, tên đường, khu phố).                                            |
| `address.ward`     | string  |    Không | Phường / Xã nơi đặt bãi xe.                                                               |
| `address.district` | string  |    Không | Quận / Huyện / Thành phố trực thuộc tỉnh.                                                 |
| `address.city`     | string  |       Có | Tỉnh / Thành phố trực thuộc trung ương.                                                   |
| `totalCapacity`    | number  |       Có | Tổng sức chứa thiết kế mới. Giá trị phải lớn hơn 0 (`> 0`).                               |
| `openTime`         | string  |    Không | Khung giờ mở cửa hoạt động hàng ngày (Định dạng: `HH:mm`, mặc định: `"06:00"`).           |
| `closeTime`        | string  |    Không | Khung giờ đóng cửa hoạt động hàng ngày (Định dạng: `HH:mm`, mặc định: `"22:00"`).         |
| `isActive`         | boolean |    Không | Trạng thái hoạt động của bãi đỗ xe (`true`: hoạt động, `false`: tạm ngưng).               |

### 3.4. Output

#### Data

| Trường                    | Kiểu    | Mô tả                                                                       |
| ------------------------- | ------- | --------------------------------------------------------------------------- |
| `data.id`                 | string  | Mã định danh duy nhất (UUID) của bãi xe.                                    |
| `data.businessOperatorId` | string  | Mã đơn vị kinh doanh sở hữu bãi đỗ xe.                                      |
| `data.name`               | string  | Tên chính thức sau khi cập nhật.                                            |
| `data.address.street`     | string  | Số nhà, tên đường bãi xe.                                                   |
| `data.address.ward`       | string  | Phường / Xã.                                                                |
| `data.address.district`   | string  | Quận / Huyện.                                                               |
| `data.address.city`       | string  | Tỉnh / Thành phố.                                                           |
| `data.fullAddress`        | string  | Chuỗi địa chỉ ghép đầy đủ.                                                  |
| `data.totalCapacity`      | number  | Tổng sức chứa thiết kế mới của bãi xe.                                      |
| `data.availableSpots`     | number  | Số lượng chỗ đỗ còn trống sau khi đã cân đối lại theo sức chứa mới.         |
| `data.openTime`           | string  | Giờ mở cửa hoạt động hàng ngày (`HH:mm`).                                   |
| `data.closeTime`          | string  | Giờ đóng cửa hoạt động hàng ngày (`HH:mm`).                                 |
| `data.isActive`           | boolean | Trạng thái hoạt động của bãi xe.                                            |

### 3.5. Response mẫu

#### a) Phản hồi thành công (HTTP Status 200 OK)

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

#### b) Phản hồi lỗi dữ liệu không hợp lệ (HTTP Status 400 Bad Request)

```json
{
  "code": 400,
  "message": "Total capacity must be greater than 0.",
  "data": null
}
```

#### c) Phản hồi lỗi không có quyền chỉnh sửa (HTTP Status 403 Forbidden)

```json
{
  "code": 403,
  "message": "Business operators can only update their own parking lots.",
  "data": null
}
```

#### d) Phản hồi lỗi không tìm thấy bãi đỗ xe (HTTP Status 404 Not Found)

```json
{
  "code": 404,
  "message": "Parking lot with ID '77777777-7777-7777-7777-777777777777' not found.",
  "data": null
}
```
