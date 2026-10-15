---
name: generate-api-lld-docs
description: >-
  Use this skill whenever the user asks to write, generate, or standardize API Specification documents (ApiSpec) and Low-Level Design (LLD) documents for a use case, feature, or system module following Mentor Tran Van Quang's standardized enterprise templates.
---

# Quy Trình Thiết Kế & Đặc Tả Tài Liệu Kỹ Thuật Chuẩn Doanh Nghiệp (API Spec & LLD)

Quy trình này hướng dẫn chi tiết cách biên soạn bộ đôi tài liệu kỹ thuật chuẩn hóa cho một Use Case hoặc phân hệ chức năng:
1. **File 1:** `[TênUseCase]_ApiSpec.md` — Đặc tả API dành cho đối tác tích hợp và đội ngũ Frontend.
2. **File 2:** `[TênUseCase]_LLD.md` — Thiết kế kỹ thuật chi tiết (Low-Level Design) lưu hành nội bộ Backend/Kiến trúc.

---

## 1. NGUYÊN TẮC CỐT LÕI (CORE PRINCIPLES)

1. **Phân tách 2 file độc lập:** Luôn tạo 2 file riêng biệt cho mỗi Use Case / Phân hệ để phục vụ đúng đối tượng độc giả.
2. **Chuẩn Response hệ thống thống nhất:**
   * **Thành công:** Luôn là `"code": 0` và `"message": "Success"`.
   * **Thất bại:** `"code": <mã lỗi số>` (`400`, `401`, `403`, `404`, `422`, `500`...) và `"message": "<mô tả lỗi ngắn gọn>"`.
   * **Cấu trúc vỏ bọc JSON:** `{ "code": 0, "message": "Success", "metadata": {}, "data": {} }`.
     * `metadata`: Chỉ dùng khi có phân trang (`pageNumber`, `pageSize`, `totalCount`, `totalPages`) hoặc bộ lọc thời gian/phạm vi (thường ở API danh sách).
     * `data`: Chứa dữ liệu nghiệp vụ chính.
3. **Quy tắc ngôn ngữ:**
   * Tên thuộc tính, endpoint route, kiểu dữ liệu: Giữ nguyên **tiếng Anh** chuẩn RESTful/OOP.
   * Tiêu đề, phần giải thích mô tả trong bảng và thông điệp lỗi (`message`): Viết bằng **tiếng Việt chuẩn mực, rõ ràng, dễ hiểu**.
4. **Nguyên tắc Kiến trúc DDD & CQRS:**
   * **DDD (Domain-Driven Design):** Thể hiện trong Sơ đồ lớp (Class Diagram) — chỉ vẽ các Thực thể nghiệp vụ (`<<Aggregate Root>>`, `<<Entity>>`, `<<Value Object>>`), bảo vệ các luật bất biến (Invariants), tuyệt đối không vẽ các class framework kỹ thuật.
   * **CQRS (Command Query Responsibility Segregation):** Thể hiện trong Sơ đồ tuần tự (Sequence Diagram) — phân tách rành mạch giữa luồng Ghi (`Command` $\rightarrow$ `CommandHandler` $\rightarrow$ `Domain Entity` $\rightarrow$ `UnitOfWork`) và luồng Đọc (`Query` $\rightarrow$ `QueryHandler` $\rightarrow$ `Repository/Read DB`).

---

## 2. QUY CHUẨN FILE 1: `[TênUseCase]_ApiSpec.md` (TÀI LIỆU API CHO ĐỐI TÁC TÍCH HỢP)

Mỗi file API Spec phải tuân thủ đúng cấu trúc đề mục và bảng biểu sau:

### 2.1. Đề mục tài liệu:
```markdown
# API [Tên phân hệ / Use Case]

## 1. Mục đích tài liệu
[Mô tả 1-2 câu tóm tắt nghiệp vụ]
Toàn bộ response được thống nhất theo dạng:
```json
{
  "code": 0,
  "message": "Success",
  "data": { ... }
}
```
> Lưu ý:
> - Các route dưới đây là quy ước đề xuất với tiền tố `/api/...`.
> - [Các lưu ý về phân quyền, ràng buộc nghiệp vụ cơ bản].

---

## 2. Quy ước chung
- Method: [POST / GET / PUT / DELETE]
- Content-Type request/response: `application/json`
- Xác thực: Bearer JWT Token (`Authorization: Bearer <token>`)
- Mã thành công: `code = 0`, `message = "Success"`

### 2.1. Cấu trúc response chuẩn
[BẢNG 4 CỘT]

### 2.2. Cấu trúc metadata (nếu có phân trang)
[BẢNG 4 CỘT]

---

## 3. API [Tên API chức năng]
### 3.1. Mục đích
### 3.2. Route đề xuất
### 3.3. Input parameters
[BẢNG 4 CỘT]
### 3.4. Output
#### Metadata (nếu có)
[BẢNG 3 CỘT]
#### Data
[BẢNG 3 CỘT]
### 3.5. Response mẫu
[JSON Thành công code: 0]
[JSON Lỗi nghiệp vụ code: 400, 422...]
```

### 2.2. Quy cách các bảng trong File 1:
1. **Bảng 2.1 Cấu trúc response chuẩn (4 cột):**
   `| Trường | Kiểu | Bắt buộc | Mô tả |`
   *(Các dòng chuẩn: `code` (number, Có), `message` (string, Có), `metadata` (object, Không), `data` (object/array, Có))*
2. **Bảng 3.3 Input parameters (4 cột):**
   `| Tham số | Kiểu | Bắt buộc | Mô tả |`
3. **Bảng 3.4 Output (3 cột):** *(Không có cột STT và Bắt buộc)*
   `| Trường | Kiểu | Mô tả |`
   *(Ký hiệu lồng nhau: `data.id`, `data.location.street`, `data[].name`...)*

---

## 3. QUY CHUẨN FILE 2: `[TênUseCase]_LLD.md` (THIẾT KẾ KỸ THUẬT CHI TIẾT)

Tài liệu LLD phải tuân thủ nghiêm ngặt **2 Phần chính** và các đề mục con sau:

### 3.1. PHẦN 1: DỮ LIỆU

#### 1.1. Dữ liệu đầu vào
* **`1.1.1. Cấu trúc dữ liệu` $\rightarrow$ CÓ BẢNG (5 cột):**
  | STT | Thuộc tính | Kiểu dữ liệu | Bắt buộc | Mô tả & Ràng buộc kỹ thuật |
* **`1.1.2. Định dạng dữ liệu` $\rightarrow$ GẠCH ĐẦU DÒNG (Không dùng bảng):**
  * Phương thức HTTP (POST/GET/PUT/DELETE)
  * Route: `/api/...`
  * Headers: `Authorization: Bearer <JWT_Token>`, `Content-Type: application/json`, `Accept: application/json`
* **`1.1.3. Ví dụ dữ liệu đầu vào` $\rightarrow$ KHỐI CODE (Không dùng bảng):**
  * `a) Định dạng HTTP Request:` (Khối raw HTTP, **bỏ `HTTP/1.1`** để tài liệu gọn gàng).
  * `b) Lệnh cURL:` (Khối cURL hoàn chỉnh có `-H` và `-d`).

#### 1.2. Dữ liệu đầu ra
* **`1.2.1. Cấu trúc dữ liệu` $\rightarrow$ CÓ BẢNG (5 cột):**
  | STT | Thuộc tính | Kiểu dữ liệu | Bắt buộc | Mô tả chi tiết |
* **`1.2.2. Ví dụ dữ liệu đầu ra` $\rightarrow$ KHỐI CODE (Không dùng bảng):**
  * `a) Phản hồi thành công:` Khối JSON chuẩn HTTP 200/201, `"code": 0`.
  * `b) Phản hồi lỗi dữ liệu không hợp lệ:` Khối JSON lỗi HTTP 400 hoặc 422, `"code": 400`/`422`, `"data": null`.

#### 1.3. Dữ liệu lưu trữ $\rightarrow$ CÓ BẢNG (8 cột chuẩn):**
Đặc tả chi tiết các bảng CSDL tham gia vào Use Case:
| STT | Tên cột | Kiểu dữ liệu | Khóa chính (PK) | Khóa ngoại (FK) | Nullable | Giá trị mặc định | Mô tả chi tiết |

---

### 3.2. PHẦN 2: SƠ ĐỒ

#### 2.1. Sơ đồ thiết kế lớp (Class Diagram)
* **Quy tắc vẽ Mermaid:** Chỉ vẽ các **Thực thể dữ liệu / Domain Entities / Value Objects** tham gia trực tiếp vào Use Case (theo đúng phong cách của Mentor).
* **Đặc tả rõ ràng:**
  * Lớp gốc gắn stereotype `<<Aggregate Root>>`.
  * Thực thể gắn stereotype `<<Entity>>`.
  * Đối tượng giá trị gắn stereotype `<<Value Object>>` (ví dụ: `Address`, `Money`).
  * Thực thể cha chứa Value Object bằng quan hệ Composition (`*--`).
  * Có ghi rõ thuộc tính của Value Object bên trong thực thể cha (ví dụ: `+Address address`).
  * Tuyệt đối **KHÔNG** vẽ các class kỹ thuật framework (Controller, Handlers, Commands, DTOs, Repository interfaces).
* **Bảng mô tả đi kèm sơ đồ $\rightarrow$ CÓ BẢNG (3 cột):**
  | Tên lớp / Đối tượng | Phân loại DDD | Vai trò & Đặc điểm kỹ thuật |

#### 2.2. Sơ đồ Sequence (Sequence Diagram)
* **Quy tắc vẽ Mermaid:**
  * Đánh số thứ tự tự động bằng `autonumber`.
  * **Quy ước đặt tên Participant:**
    * Tuyệt đối **KHÔNG** đặt tên theo kiểu MVC cổ điển như `XxxController` (ví dụ: `ParkingLotController`).
    * Luôn đặt tên tầng tiếp nhận là `participant API as 🌐 API ([METHOD] /route)` hoặc `🌐 Minimal API ([METHOD] /route)` theo đúng chuẩn Endpoint của Clean Architecture / CQRS.
  * **Áp dụng chuẩn kiến trúc CQRS (Command Query Responsibility Segregation):**
    * **Với Use Case GHI (POST, PUT, DELETE — Command):** Luồng bắt buộc: `Actor` $\rightarrow$ `API` $\rightarrow$ `Send([Action]Command)` $\rightarrow$ `[Action]CommandHandler` $\rightarrow$ `Repository (Kiểm tra dữ liệu)` $\rightarrow$ `Domain Entity (Thực thi & Validate luật bất biến)` $\rightarrow$ `Repository.Add/Update/Delete` $\rightarrow$ `IUnitOfWork (Commit Transaction)` $\rightarrow$ Trả về kết quả `200/201`.
    * **Với Use Case ĐỌC (GET List, GET Detail — Query):** Luồng đọc tối ưu: `Actor` $\rightarrow$ `API` $\rightarrow$ `Send([Action]Query)` $\rightarrow$ `[Action]QueryHandler` $\rightarrow$ `Repository / Read Database (AsNoTracking)` $\rightarrow$ Trả về DTO kết quả (không qua UnitOfWork hay Domain Entity để tối ưu tốc độ).
  * **Bắt buộc có khối phân nhánh `alt / else`** thể hiện đầy đủ cả:
    * Kịch bản thành công (`code: 0`, HTTP 200/201).
    * Các kịch bản lỗi nghiệp vụ (ví dụ: vi phạm validation `code: 400`, vi phạm quyền sở hữu `code: 403`, không tìm thấy `code: 404`, vi phạm luật nghiệp vụ trùng tên hoặc còn xe đang đỗ `code: 422`).

---

## 4. BẢNG TỔNG KẾT NHANH CÁC BẢNG & DANH SÁCH CỘT

| Tài liệu | Đề mục | Có bảng? | Danh sách các cột chính xác |
| :--- | :--- | :---: | :--- |
| **ApiSpec** | `2.1. Cấu trúc response chuẩn` | **CÓ** | `Trường` \| `Kiểu` \| `Bắt buộc` \| `Mô tả` |
| **ApiSpec** | `2.2. Cấu trúc metadata` | **CÓ** | `Trường` \| `Kiểu` \| `Bắt buộc` \| `Mô tả` |
| **ApiSpec** | `3.3. Input parameters` | **CÓ** | `Tham số` \| `Kiểu` \| `Bắt buộc` \| `Mô tả` |
| **ApiSpec** | `3.4. Output` | **CÓ** | `Trường` \| `Kiểu` \| `Mô tả` |
| **LLD** | `1.1.1. Cấu trúc dữ liệu` (Input) | **CÓ** | `STT` \| `Thuộc tính` \| `Kiểu dữ liệu` \| `Bắt buộc` \| `Mô tả & Ràng buộc kỹ thuật` |
| **LLD** | `1.1.2. Định dạng dữ liệu` | Không | *(Gạch đầu dòng HTTP Method, Route, Headers)* |
| **LLD** | `1.1.3. Ví dụ dữ liệu đầu vào` | Không | *(Khối code raw HTTP và cURL)* |
| **LLD** | `1.2.1. Cấu trúc dữ liệu` (Output)| **CÓ** | `STT` \| `Thuộc tính` \| `Kiểu dữ liệu` \| `Bắt buộc` \| `Mô tả chi tiết` |
| **LLD** | `1.2.2. Ví dụ dữ liệu đầu ra` | Không | *(Khối code JSON thành công và JSON lỗi)* |
| **LLD** | `1.3. Dữ liệu lưu trữ` (DB Schema) | **CÓ** | `STT` \| `Tên cột` \| `Kiểu dữ liệu` \| `Khóa chính (PK)` \| `Khóa ngoại (FK)` \| `Nullable` \| `Giá trị mặc định` \| `Mô tả chi tiết` |
| **LLD** | `2.1. Sơ đồ thiết kế lớp` | **CÓ** | `Tên lớp / Đối tượng` \| `Phân loại DDD` \| `Vai trò & Đặc điểm kỹ thuật` |
| **LLD** | `2.2. Sơ đồ Sequence` | Không | *(Sơ đồ Mermaid sequenceDiagram với alt/else theo chuẩn CQRS)* |
