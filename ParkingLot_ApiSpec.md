# API Phân hệ Quản lý Bãi đỗ xe

## 1. Mục đích tài liệu

Tài liệu mô tả chi tiết các API phục vụ Phân hệ Quản lý Bãi đỗ xe theo cấu trúc response chuẩn hóa.  
Toàn bộ response được thống nhất theo dạng:

```json
{
  "code": 0,
  "message": "Success",
  "metadata": {
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 50,
    "totalPages": 5
  },
  "data": [
    {
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
      "availableSpots": 85,
      "openTime": "06:00",
      "closeTime": "22:00",
      "isActive": true
    }
  ]
}
```

> Lưu ý:
>
> - Các route dưới đây là **quy ước đề xuất** với tiền tố `/api/parking-lots`.
> - Nếu hệ thống thực tế dùng route khác thì cập nhật lại theo route triển khai.
> - Quyền truy cập: Quản trị viên (`SystemManager`), Đơn vị kinh doanh (`BusinessOperator`), Nhân viên (`Attendant`).
> - Với đơn vị kinh doanh (`BusinessOperator`), hệ thống áp dụng cơ chế kiểm tra quyền sở hữu (*Ownership Check*): chỉ được xem, sửa, xóa các bãi xe do chính đơn vị mình sở hữu.
> - Giờ mở cửa (`openTime`) và giờ đóng cửa (`closeTime`) định dạng chuỗi giờ phút (`HH:mm`), mặc định lần lượt là `"06:00"` và `"22:00"`.

---

## 2. Quy ước chung

- Content-Type response: `application/json`
- Xác thực: Bearer JWT Token (`Authorization: Bearer <token>`)
- Mã thành công:
  - `code = 0`
  - `message = "Success"`

### 2.1. Cấu trúc response chuẩn

| Trường     | Kiểu   | Bắt buộc | Mô tả                                |
| ---------- | ------ | -------: | ------------------------------------ |
| `code`     | number |       Có | Mã kết quả xử lý. `0` là thành công. |
| `message`  | string |       Có | Thông điệp xử lý, ví dụ `Success`.   |
| `metadata` | object |    Không | Thông tin phân trang nếu có.         |
| `data`     | any    |       Có | Dữ liệu chính của API.               |

### 2.2. Cấu trúc `metadata` (áp dụng cho API danh sách)

| Trường                | Kiểu   | Bắt buộc | Mô tả                                  |
| --------------------- | ------ | -------: | -------------------------------------- |
| `metadata.pageNumber` | number |       Có | Số thứ tự trang hiện tại.              |
| `metadata.pageSize`   | number |       Có | Số lượng bản ghi trên một trang.       |
| `metadata.totalCount` | number |       Có | Tổng số bản ghi thỏa mãn điều kiện lọc.|
| `metadata.totalPages` | number |       Có | Tổng số trang.                         |

---

## 3. API Thêm bãi đỗ xe

### 3.1. Mục đích

Tiếp nhận yêu cầu khởi tạo bãi đỗ xe mới từ đơn vị kinh doanh hoặc quản trị viên.

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
| `isActive`           | boolean       |    Không | Trạng thái hoạt động của bãi đỗ (Mặc định: `true`).                                       |

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

---

## 4. API Cập nhật thông tin bãi đỗ xe

### 4.1. Mục đích

Cập nhật thông tin vận hành của bãi đỗ xe (áp dụng kiểm tra quyền sở hữu *Ownership Check*).

### 4.2. Route đề xuất

`PUT /api/parking-lots/{id}`

### 4.3. Input parameters

| Tham số            | Kiểu          | Bắt buộc | Mô tả                                                                   |
| ------------------ | ------------- | -------: | ----------------------------------------------------------------------- |
| `id`               | string (UUID) |       Có | Mã định danh duy nhất của bãi đỗ xe cần cập nhật (truyền qua URL path). |
| `name`             | string        |       Có | Tên bãi xe cập nhật. Không trùng lặp trong cùng đơn vị.                 |
| `address.street`   | string        |       Có | Tên đường, số nhà cập nhật.                                             |
| `address.ward`     | string        |    Không | Phường / Xã.                                                            |
| `address.district` | string        |    Không | Quận / Huyện.                                                           |
| `address.city`     | string        |       Có | Tỉnh / Thành phố.                                                       |
| `totalCapacity`    | number        |       Có | Tổng sức chứa mới của bãi xe.                                           |
| `openTime`         | string        |    Không | Giờ mở cửa mới (`HH:mm`).                                               |
| `closeTime`        | string        |    Không | Giờ đóng cửa mới (`HH:mm`).                                             |
| `isActive`         | boolean       |    Không | Trạng thái tiếp nhận xe (`true`: Hoạt động, `false`: Tạm ngưng).         |

### 4.4. Output

#### Data

| Trường                    | Kiểu    | Mô tả                                      |
| ------------------------- | ------- | ------------------------------------------ |
| `data.id`                 | string  | Mã định danh bãi xe.                       |
| `data.businessOperatorId` | string  | Mã đơn vị sở hữu.                          |
| `data.name`               | string  | Tên bãi xe sau cập nhật.                   |
| `data.address.street`     | string  | Địa chỉ số nhà, tên đường.                 |
| `data.address.ward`       | string  | Phường / Xã.                               |
| `data.address.district`   | string  | Quận / Huyện.                              |
| `data.address.city`       | string  | Tỉnh / Thành phố.                          |
| `data.fullAddress`        | string  | Chuỗi địa chỉ ghép đầy đủ.                 |
| `data.totalCapacity`      | number  | Tổng sức chứa.                             |
| `data.availableSpots`     | number  | Số chỗ trống hiện tại.                     |
| `data.openTime`           | string  | Giờ mở cửa.                                |
| `data.closeTime`          | string  | Giờ đóng cửa.                              |
| `data.isActive`           | boolean | Trạng thái hoạt động mới.                  |

### 4.5. Response mẫu

#### Thành công (`HTTP 200 OK`):

```json
{
  "code": 0,
  "message": "Success",
  "data": {
    "id": "77777777-7777-7777-7777-777777777777",
    "businessOperatorId": "33333333-3333-3333-3333-333333333333",
    "name": "Bãi xe KTX Khu A - Mở Rộng",
    "address": {
      "street": "Đường Tạ Quang Bửu, Khu phố 6",
      "ward": "Phường Linh Trung",
      "district": "Thành phố Thủ Đức",
      "city": "Thành phố Hồ Chí Minh"
    },
    "fullAddress": "Đường Tạ Quang Bửu, Khu phố 6, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh",
    "totalCapacity": 100,
    "availableSpots": 95,
    "openTime": "05:30",
    "closeTime": "23:30",
    "isActive": true
  }
}
```

---

## 5. API Xóa bãi đỗ xe

### 5.1. Mục đích

Xóa bãi đỗ xe theo 3 kịch bản tự động:
1. Chặn xóa nếu còn xe đang gửi (`CurrentParked > 0`).
2. Xóa mềm (`isDeleted = true, isActive = false`) nếu bãi rỗng nhưng đã có lịch sử gửi xe trong quá khứ.
3. Xóa cứng (xóa vĩnh viễn khỏi CSDL) nếu bãi xe mới tạo và chưa từng phát sinh bất kỳ lượt gửi xe nào.

### 5.2. Route đề xuất

`DELETE /api/parking-lots/{id}`

### 5.3. Input parameters

| Tham số | Kiểu          | Bắt buộc | Mô tả                                                        |
| ------- | ------------- | -------: | ------------------------------------------------------------ |
| `id`    | string (UUID) |       Có | Mã định danh duy nhất của bãi đỗ xe cần xóa (truyền qua URL path). |

### 5.4. Output

#### Data (áp dụng cho trường hợp xóa mềm)

| Trường           | Kiểu    | Mô tả                                                      |
| ---------------- | ------- | ---------------------------------------------------------- |
| `data.id`        | string  | Mã định danh bãi xe đã xóa mềm.                            |
| `data.isDeleted` | boolean | Trạng thái xóa mềm (`true`: Đã đánh dấu xóa).              |
| `data.isActive`  | boolean | Trạng thái hoạt động (`false`: Ngừng hoạt động hoàn toàn). |

### 5.5. Response mẫu

#### Xóa mềm thành công (`HTTP 200 OK`):

```json
{
  "code": 0,
  "message": "Bãi xe đã có lịch sử gửi xe, chuyển sang trạng thái xóa mềm thành công.",
  "data": {
    "id": "77777777-7777-7777-7777-777777777777",
    "isDeleted": true,
    "isActive": false
  }
}
```

#### Xóa cứng thành công (`HTTP 204 No Content`):
*Không có dữ liệu trả về.*

---

## 6. API Lấy danh sách bãi đỗ xe

### 6.1. Mục đích

Truy vấn danh sách các bãi đỗ xe có hỗ trợ phân trang, tìm kiếm từ khóa và lọc theo trạng thái hoạt động.

### 6.2. Route đề xuất

`GET /api/parking-lots`

### 6.3. Input parameters

| Tham số              | Kiểu          | Bắt buộc | Mô tả                                                                   |
| -------------------- | ------------- | -------: | ----------------------------------------------------------------------- |
| `businessOperatorId` | string (UUID) |    Không | Lọc theo mã đơn vị kinh doanh (Mặc định tự lọc theo token của chủ bãi). |
| `keyword`            | string        |    Không | Từ khóa tìm kiếm theo tên bãi xe hoặc địa chỉ.                          |
| `isActive`           | boolean       |    Không | Lọc theo trạng thái hoạt động (`true`: Mở, `false`: Tạm đóng).          |
| `pageNumber`         | number        |    Không | Số thứ tự trang cần lấy. Mặc định: `1`.                                 |
| `pageSize`           | number        |    Không | Số lượng bản ghi mỗi trang. Mặc định: `10`, tối đa `100`.               |

### 6.4. Output

#### Metadata

| Trường                | Kiểu   | Mô tả                            |
| --------------------- | ------ | -------------------------------- |
| `metadata.pageNumber` | number | Số thứ tự trang hiện tại.        |
| `metadata.pageSize`   | number | Số bản ghi trên mỗi trang.       |
| `metadata.totalCount` | number | Tổng số bản ghi thỏa mãn bộ lọc. |
| `metadata.totalPages` | number | Tổng số trang.                   |

#### Data

| Trường                     | Kiểu    | Mô tả                                           |
| -------------------------- | ------- | ----------------------------------------------- |
| `data[].id`                | string  | Mã định danh bãi xe.                            |
| `data[].businessOperatorId`| string  | Mã đơn vị sở hữu.                               |
| `data[].name`              | string  | Tên bãi đỗ xe.                                  |
| `data[].totalCapacity`     | number  | Tổng sức chứa thiết kế.                         |
| `data[].availableSpots`    | number  | Số chỗ trống còn lại.                           |
| `data[].fullAddress`       | string  | Chuỗi địa chỉ ghép đầy đủ.                      |
| `data[].openTime`          | string  | Giờ mở cửa (`HH:mm`).                           |
| `data[].closeTime`         | string  | Giờ đóng cửa (`HH:mm`).                         |
| `data[].isActive`          | boolean | Trạng thái hoạt động.                           |

### 6.5. Response mẫu

#### Thành công (`HTTP 200 OK`):

```json
{
  "code": 0,
  "message": "Success",
  "metadata": {
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 1,
    "totalPages": 1
  },
  "data": [
    {
      "id": "77777777-7777-7777-7777-777777777777",
      "businessOperatorId": "33333333-3333-3333-3333-333333333333",
      "name": "Bãi xe KTX Khu A",
      "totalCapacity": 100,
      "availableSpots": 42,
      "fullAddress": "Đường Tạ Quang Bửu, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh",
      "openTime": "06:00",
      "closeTime": "22:00",
      "isActive": true
    }
  ]
}
```

---

## 7. API Lấy thông tin chi tiết bãi đỗ xe

### 7.1. Mục đích

Truy vấn thông tin chi tiết một bãi đỗ xe thông qua mã định danh `id`.

### 7.2. Route đề xuất

`GET /api/parking-lots/{id}`

### 7.3. Input parameters

| Tham số | Kiểu          | Bắt buộc | Mô tả                                                        |
| ------- | ------------- | -------: | ------------------------------------------------------------ |
| `id`    | string (UUID) |       Có | Mã định danh duy nhất của bãi đỗ xe (truyền qua URL path).   |

### 7.4. Output

#### Data

| Trường                    | Kiểu    | Mô tả                                                           |
| ------------------------- | ------- | --------------------------------------------------------------- |
| `data.id`                 | string  | Mã định danh duy nhất của bãi xe.                               |
| `data.businessOperatorId` | string  | Mã đơn vị kinh doanh sở hữu.                                    |
| `data.name`               | string  | Tên bãi đỗ xe.                                                  |
| `data.address.street`     | string  | Tên đường, số nhà.                                              |
| `data.address.ward`       | string  | Phường / Xã.                                                    |
| `data.address.district`   | string  | Quận / Huyện.                                                   |
| `data.address.city`       | string  | Tỉnh / Thành phố.                                               |
| `data.fullAddress`        | string  | Chuỗi địa chỉ ghép đầy đủ.                                      |
| `data.totalCapacity`      | number  | Tổng sức chứa thiết kế.                                         |
| `data.availableSpots`     | number  | Số chỗ trống còn lại.                                           |
| `data.openTime`           | string  | Giờ mở cửa hoạt động (`HH:mm`).                                 |
| `data.closeTime`          | string  | Giờ đóng cửa hoạt động (`HH:mm`).                               |
| `data.isActive`           | boolean | Trạng thái hoạt động.                                           |

### 7.5. Response mẫu

#### Thành công (`HTTP 200 OK`):

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
    "availableSpots": 85,
    "openTime": "06:00",
    "closeTime": "22:00",
    "isActive": true
  }
}
```
