# TÀI LIỆU CẤU TRÚC THƯ MỤC FRONTEND - SMART PARKING SYSTEM

Hệ thống Frontend được thiết kế theo cấu trúc module hóa (Modular Architecture), tách biệt rõ ràng giữa **Tầng giao diện (HTML/CSS)**, **Tầng tài nguyên tĩnh (Assets)**, **Tầng lõi hệ thống (Core JS)** và **Tầng xử lý nghiệp vụ theo từng trang (Page Logic JS)**.

---

## 1. CÂY THƯ MỤC TỔNG QUAN

```text
Frontend/
│
├── index.html                      # Entry-point: Tự động chuyển hướng vào màn hình chính
│
├── assets/                         # Thư mục tài nguyên tĩnh
│   ├── css/
│   │   └── style.css               # Bản sao CSS chuẩn phân phối
│   └── images/
│       └── logo.svg                # Logo vector thương hiệu Smart Parking
│
├── css/
│   └── style.css                   # Thiết kế UI, biến màu CSS, Typography, Layout, Responsive
│
├── html/                           # Tầng giao diện người dùng (Mỗi file đại diện 1 màn hình)
│   ├── dashboard.html              # Màn hình Báo cáo, thống kê lượt xe & doanh thu
│   ├── checkin.html                # Màn hình Check-in (vào bãi) & Check-out (ra bãi & thanh toán)
│   ├── parking-lots.html                  # Màn hình Quản lý danh sách bãi xe & tìm kiếm/lọc
│   ├── tariffs.html                # Màn hình Biểu phí dịch vụ & bộ lọc giá vé đa năng
│   ├── business-operators.html                  # Màn hình Quản lý đối tác / Đơn vị kinh doanh (Admin)
│   └── login.html                  # Màn hình Đăng nhập & danh sách tài khoản demo
│
└── js/                             # Tầng mã nguồn JavaScript xử lý tương tác & gọi API
    │
    ├── app.js                      # Điều phối chung: Quản lý Alert Toast, State toàn cục, Event DOM
    │
    ├── core/                       # Tầng lõi dùng chung cho toàn bộ ứng dụng
    │   ├── api.js                  # Cấu hình API Server, tự động gắn Bearer JWT Token, Fetch helper
    │   └── auth.js                 # Xử lý đăng nhập/đăng xuất, phân quyền người dùng (RBAC), menu động
    │
    └── pages/                      # Tầng xử lý nghiệp vụ riêng biệt cho từng màn hình
        ├── dashboard.js            # Gọi API thống kê, tính toán doanh thu & render KPI cards
        ├── checkin.js              # Nghiệp vụ Check-in, Check-out, cập nhật giá vé realtime, in hóa đơn
        ├── parking-lots.js                # Tìm kiếm, lọc bãi xe, Modal Thêm/Sửa/Xóa bãi đỗ xe
        ├── tariffs.js              # Lọc biểu phí theo bãi/loại xe/mức giá, Modal Thêm/Sửa/Xóa biểu phí
        ├── business-operators.js                # Quản lý đối tác doanh nghiệp, Modal Thêm/Xóa đơn vị kinh doanh
        └── login.js                # Tự động chuyển trang nếu đã login, focus input, phím tắt Enter
```

---

## 2. CHI TIẾT TỪNG TẦNG CHỨC NĂNG

### 🎯 Tầng Giao Diện (`html/` & `css/`)
* **`style.css`**:
  * Sử dụng hệ bảng màu hiện đại với CSS Variables (`--primary`, `--primary-gradient`, `--dark-surface`).
  * Tích hợp font chữ thời thượng Google Fonts **Plus Jakarta Sans**.
  * Chứa các thành phần UI độc quyền: Biển số xe chuẩn Việt Nam (`.plate-box`), Thẻ hóa đơn có đường xé răng cưa (`.receipt-card`), Thẻ KPI đổi màu gradient (`.kpi-card`), Thẻ giá vé voucher (`.price-voucher-card`).
* **Các file `.html`**:
  * Mã nguồn cực kỳ gọn gàng, thuần túy HTML và Modal Dialogs của Bootstrap 5.
  * **Không còn lẫn lộn hàng trăm dòng Javascript inline** bên trong thẻ HTML.

### ⚙️ Tầng Lõi Hệ Thống (`js/core/`)
* **`api.js`**:
  * `getApiUrl()`: Tự động lấy cấu hình URL máy chủ từ input hoặc `localStorage`.
  * `getAuthHeaders()`: Tự động đính kèm header `Authorization: Bearer <Token>` cho mọi request cần xác thực.
  * `apiFetch()`: Helper chuẩn hóa việc gọi API và parse kết quả JSON.
* **`auth.js`**:
  * `initAuth()` & `currentUser`: Quản lý session phiên đăng nhập qua `localStorage`.
  * `applyUserRoleUI()`: Cơ chế phân quyền giao diện RBAC (Role-Based Access Control):
    * **Quản lý hệ thống (`QuanLy`)**: Hiển thị menu Báo cáo, Quản lý bãi xe, Đơn vị kinh doanh; ẩn Check-in/Check-out.
    * **Đơn vị kinh doanh (`DonViKinhDoanh`)**: Hiển thị menu Báo cáo (thuộc quyền), Bãi xe của tôi, Check-in/Check-out, Biểu phí dịch vụ; ẩn Đơn vị kinh doanh.
    * **Khách vãng lai**: Chỉ hiển thị Báo cáo và Danh sách bãi xe công khai.
  * `doLogin()` & `quickLogin()`: Gọi API xác thực backend và lưu JWT Token.

### 🚀 Tầng Nghiệp Vụ Từng Màn Hình (`js/pages/`)
* Mỗi trang có 1 file JS riêng biệt tương ứng 1-1 với file HTML.
* Độc lập, dễ bảo trì, dễ sửa lỗi mà không sợ ảnh hưởng đến các màn hình khác.
* Dễ dàng viết Unit Test hoặc tích hợp thêm thư viện bên thứ 3 trong tương lai.

---

## 3. THỨ TỰ NẠP SCRIPT CHUẨN TRÊN MỖI TRANG

Mọi file HTML đều nạp các script theo thứ tự phân tầng chuẩn:
```html
<!-- 1. Thư viện ngoài -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

<!-- 2. Tầng Core (API & Auth) -->
<script src="../js/core/api.js"></script>
<script src="../js/core/auth.js"></script>

<!-- 3. Tầng Shared (Điều phối & Toast thông báo) -->
<script src="../js/app.js"></script>

<!-- 4. Tầng Page (Nghiệp vụ riêng của trang đó) -->
<script src="../js/pages/<tên_trang>.js"></script>
```
