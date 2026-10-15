# API Quản lý bãi đỗ xe — Use Case 1: Thêm bãi đỗ xe

## 1. Mục đích tài liệu

Tài liệu mô tả chi tiết API phục vụ **Use Case 1: Thêm bãi đỗ xe**.  
Toàn bộ response được thống nhất theo dạng:

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
      "city": "Thành phố Hồ Chí Minh",
      "fullAddress": "Đường Tạ Quang Bửu, Khu phố 6, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh"
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

> Lưu ý:
>
> - Route dưới đây là **quy ước đề xuất** với tiền tố `/api/parking-lots`.
> - Nếu hệ thống thực tế dùng route khác thì cập nhật lại theo route triển khai.
> - Bãi xe mới tạo sẽ có số chỗ trống (`availableSpots`) mặc định bằng tổng sức chứa (`totalCapacity`).
> - Tên bãi xe không được phép trùng lặp trong cùng một đơn vị kinh doanh.

---

## 2. Quy ước chung

- Method: `POST`
- Content-Type request/response: `application/json`
- Xác thực: Bearer JWT Token (`Authorization: Bearer <token>`)
- Mã thành công:
  - `code = 0` (HTTP Status `201 Created`)
  - `message = "Success"`

### 2.1. Cấu trúc response chuẩn

| Trường     | Kiểu   | Bắt buộc | Mô tả                                       |
| ---------- | ------ | -------: | ------------------------------------------- |
| `code`     | number |       Có | Mã kết quả xử lý. `0` là thành công.        |
| `message`  | string |       Có | Thông điệp xử lý, ví dụ `Success`.          |
| `metadata` | object |    Không | Thông tin bổ trợ / phân trang (nếu có).     |
| `data`     | object |       Có | Dữ liệu chính trả về của API.               |

---

## 3. API Thêm bãi đỗ xe

### 3.1. Mục đích

Tiếp nhận yêu cầu khởi tạo bãi đỗ xe mới trong hệ thống từ đơn vị kinh doanh hoặc quản trị viên.

### 3.2. Route đề xuất

`POST /api/parking-lots`

### 3.3. Input parameters

| Tham số              | Kiểu          | Bắt buộc | Mô tả                                                                                     |
| -------------------- | ------------- | -------: | ----------------------------------------------------------------------------------------- |
| `businessOperatorId` | string (UUID) |       Có | Mã định danh đơn vị kinh doanh sở hữu bãi đỗ xe.                                          |
| `name`               | string        |       Có | Tên bãi đỗ xe (3 - 150 ký tự). Không được trùng tên với bãi xe khác cùng đơn vị sở hữu.   |
| `address.street`     | string        |       Có | Địa chỉ chi tiết (số nhà, tên đường, khu phố).                                            |
| `address.ward`       | string        |    Không | Phường / Xã nơi đặt bãi xe.                                                               |
| `address.district`   | string        |    Không | Quận / Huyện / Thành phố trực thuộc tỉnh.                                                 |
| `address.city`       | string        |       Có | Tỉnh / Thành phố trực thuộc trung ương.                                                   |
| `totalCapacity`      | number        |       Có | Tổng sức chứa thiết kế của bãi xe. Giá trị phải lớn hơn 0 (`> 0`).                        |
| `openTime`           | string        |    Không | Khung giờ mở cửa hoạt động hàng ngày (Định dạng: `HH:mm`, mặc định: `"06:00"`).           |
| `closeTime`          | string        |    Không | Khung giờ đóng cửa hoạt động hàng ngày (Định dạng: `HH:mm`, mặc định: `"22:00"`).         |
| `isActive`           | boolean       |    Không | Trạng thái kích hoạt bãi xe ngay sau khi tạo (Mặc định: `true`).                          |

### 3.4. Output

#### Data

| Trường                    | Kiểu    | Mô tả                                                                       |
| ------------------------- | ------- | --------------------------------------------------------------------------- |
| `data.id`                 | string  | Mã định danh duy nhất (UUID) của bãi xe vừa tạo.                            |
| `data.businessOperatorId` | string  | Mã đơn vị kinh doanh sở hữu bãi đỗ xe.                                      |
| `data.name`               | string  | Tên chính thức của bãi đỗ xe.                                               |
| `data.address.street`     | string  | Số nhà, tên đường bãi xe.                                                   |
| `data.address.ward`       | string  | Phường / Xã.                                                                |
| `data.address.district`   | string  | Quận / Huyện.                                                               |
| `data.address.city`       | string  | Tỉnh / Thành phố.                                                           |
| `data.fullAddress`        | string  | Chuỗi địa chỉ ghép đầy đủ.                                                  |
| `data.totalCapacity`      | number  | Tổng sức chứa thiết kế.                                                     |
| `data.availableSpots`     | number  | Số chỗ đỗ hiện còn trống (khởi tạo bằng `totalCapacity`).                   |
| `data.openTime`           | string  | Khung giờ mở cửa hoạt động (`HH:mm`).                                       |
| `data.closeTime`          | string  | Khung giờ đóng cửa hoạt động (`HH:mm`).                                     |
| `data.isActive`           | boolean | Trạng thái hoạt động của bãi đỗ (`true`: Đang hoạt động, sẵn sàng nhận xe).    |

### 3.5. Response mẫu

#### Thành công (`HTTP 201 Created`):

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

#### Lỗi dữ liệu không hợp lệ (`HTTP 400 Bad Request`):

```json
{
  "code": 400,
  "message": "Sức chứa bãi xe (totalCapacity) phải lớn hơn 0.",
  "data": null
}
```

#### Lỗi trùng tên bãi đỗ xe (`HTTP 422 Unprocessable Entity`):

```json
{
  "code": 422,
  "message": "Tên bãi xe 'Bãi xe KTX Khu A' đã tồn tại trong đơn vị kinh doanh này.",
  "data": null
}
```
