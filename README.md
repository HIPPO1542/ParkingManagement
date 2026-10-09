# HỆ THỐNG QUẢN LÝ BÃI ĐỖ XE (SMART PARKING SYSTEM)

Hệ thống Web API và ứng dụng quản lý bãi đỗ xe thông minh theo chuẩn **Domain-Driven Design (DDD)** và **Clean Architecture (Onion Architecture)**, xây dựng trên nền tảng **.NET 9 / C#**, cơ sở dữ liệu **SQLite** với **Entity Framework Core**.

---

## 1. CẤU TRÚC THƯ MỤC DỰ ÁN

Hệ thống được chuẩn hóa theo cấu trúc quản lý trực quan:

```
QL_BaiXe/
│
├── .vscode/                             # Cấu hình phát triển trên Visual Studio Code
│   ├── launch.json                      # Cấu hình F5 Debug Web API (.NET Core)
│   └── tasks.json                       # Cấu hình lệnh build và test tự động
│
├── Backend/                             # TOÀN BỘ MÃ NGUỒN BACKEND (DDD 4 TẦNG)
│   ├── QL_BaiXe.slnx                    # Solution file
│   │
│   ├── QL_BaiXe.Domain/                 # [TẦNG 1] DOMAIN LAYER
│   │   ├── Common/                      # Marker Interface (IAggregateRoot)
│   │   ├── Exceptions/                  # Domain Exceptions (BusinessRuleViolationException, ...)
│   │   ├── ValueObjects/                # Value Objects (DiaChi, BienSoXe, TienTe, ThongTinLienHe)
│   │   ├── Entities/                    # Aggregate Roots (BaiDoXe, DonViKinhDoanh, NhanVienQuanLy, LuotRaVao, BangGiaDichVu, LoaiXe)
│   │   └── Repositories/                # Repository Interfaces & IUnitOfWork
│   │
│   ├── QL_BaiXe.Application/            # [TẦNG 2] APPLICATION LAYER
│   │   ├── Common/                      # ApiResponse<T>, PagedResult<T>
│   │   ├── DTOs/                        # DTOs & Validation dữ liệu đầu vào
│   │   └── Services/                    # Application Use Cases (BaiDoXe, DonVi, LuotRaVao, ThongKe)
│   │
│   ├── QL_BaiXe.Infrastructure/         # [TẦNG 3] INFRASTRUCTURE LAYER
│   │   ├── Data/
│   │   │   ├── ParkingDbContext.cs      # EF Core Fluent API map chuẩn bảng/cột theo ERD
│   │   │   └── DataSeeder.cs            # Tự động tạo và nạp dữ liệu mẫu
│   │   └── Repositories/                # Cài đặt Repositories & UnitOfWork thực tế
│   │
│   ├── QL_BaiXe.Api/                    # [TẦNG 4] PRESENTATION LAYER (WEB API)
│   │   ├── Controllers/                 # RESTful Controllers
│   │   ├── Middlewares/                 # GlobalExceptionMiddleware xử lý lỗi tiếng Việt
│   │   ├── appsettings.json             # Cấu hình kết nối SQLite
│   │   └── Program.cs                   # Cấu hình Swagger UI, DI, Auto-migration
│   │
│   └── QL_BaiXe.Tests/                  # KIỂM THỬ TỰ ĐỘNG (XUNIT & EF CORE SQLITE)
│       ├── DomainTests.cs               # Kiểm thử Value Objects & Entity behaviors
│       └── BusinessUseCasesTests.cs     # Kiểm thử toàn diện 100% Sequence Diagrams
│
├── Database/                            # LƯU TRỮ VÀ KHỞI TẠO CƠ SỞ DỮ LIỆU
│   ├── init_schema.sql                  # Script SQL DDL tạo các bảng SQLite chuẩn theo ERD
│   └── parking.db                       # File SQLite database sẵn có dữ liệu mẫu
│
├── Frontend/                            # ỨNG DỤNG GIAO DIỆN TRỰC QUAN (WEB DASHBOARD)
│   └── index.html                       # Giao diện SPA (Bootstrap 5, FontAwesome) tương tác trực tiếp với API
│
└── README.md                            # Tài liệu hướng dẫn sử dụng và kiểm thử
```

---

## 2. ÁNH XẠ MÔ HÌNH DỮ LIỆU (ERD) VÀ VALUE OBJECTS

Ánh xạ chi tiết qua EF Core Fluent API:

| Bảng Cơ Sở Dữ Liệu | Khóa Chính | Value Objects được ánh xạ (`OwnsOne`) | Cột Dữ Liệu Chính |
| :--- | :--- | :--- | :--- |
| `NHAN_VIEN_QUAN_LY` | `ma_nhan_vien` | `ThongTinLienHe` -> `so_dien_thoai`, `email` | `ho_ten`, `chuc_vu`, `ten_dang_nhap`, `mat_khau_hash`, `trang_thai_hoat_dong` |
| `DON_VI_KINH_DOANH` | `ma_don_vi` | `DiaChi` -> 4 cột địa chỉ<br/>`ThongTinLienHe` -> `so_dien_thoai`, `email` | `ma_nhan_vien_ql` (FK), `ten_don_vi`, `nguoi_dai_dien`, `trang_thai_hoat_dong` |
| `BAI_DO_XE` | `ma_bai` | `DiaChi` -> `dia_chi_so_nha`, `dia_chi_phuong_xa`, `dia_chi_quan_huyen`, `dia_chi_tinh_thanh` | `ma_don_vi` (FK), `ten_bai`, `tong_suc_chua`, `so_cho_trong`, `gio_mo_cua`, `gio_dong_cua`, `dang_hoat_dong` |
| `LOAI_XE` | `ma_loai_xe` | - | `ten_loai_xe`, `mo_ta` |
| `GIA_VE_BAI_XE` | `ma_gia_ve` | `TienTe` -> `muc_gia`, `don_vi_tien` | `ma_bai` (FK), `ma_loai_xe` (FK), `hinh_thuc_tinh`, `ngay_ap_dung` |
| `LUOT_RA_VAO` | `ma_luot` | `BienSoXe` -> `bien_so_ma_tinh`, `bien_so_xe`<br/>`TienTe` -> `so_tien_thu`, `don_vi_tien` | `ma_bai` (FK), `ma_loai_xe` (FK), `thoi_gian_vao`, `thoi_gian_ra`, `trang_thai` |

---

## 3. CÁC QUY TẮC NGHIỆP VỤ & SEQUENCE DIAGRAMS

1. **Xóa Bãi Đỗ Xe (`xoaBaiDoXe`)**:
   - Query: `SELECT COUNT(*) FROM LUOT_RA_VAO WHERE ma_bai = ? AND trang_thai = 'Dang_gui'`.
   - Nếu `soXeDangGui > 0`: Ném `BusinessRuleViolationException`: *"Bãi xe còn xe đang gửi, không thể xóa!"*.
   - Nếu `soXeDangGui == 0`: Xóa bãi đỗ xe thành công.

2. **Xóa Đơn Vị Kinh Doanh (`xoaDonViKinhDoanh`)**:
   - Query: `SELECT COUNT(*) FROM BAI_DO_XE WHERE ma_don_vi = ? AND dang_hoat_dong = 1`.
   - Nếu `soBaiHoatDong > 0`: Ném `BusinessRuleViolationException`: *"Đơn vị đang quản lý bãi xe hoạt động, không thể xóa!"*.
   - Nếu `soBaiHoatDong == 0`: Xóa đơn vị kinh doanh thành công.

3. **Báo Cáo Thống Kê (`xemBaoCaoHoatDong`)**:
   - Endpoint: `GET /api/thong-ke/hoat-dong?maBai={guid}&tuNgay={date}&denNgay={date}`
   - Validate thời gian: Nếu `tuNgay > denNgay` hoặc thời gian ở tương lai -> Báo lỗi: *"Khoảng thời gian không hợp lệ!"*.
   - Nếu không có dữ liệu -> Trả về thông báo: *"Không có dữ liệu hợp lệ"*.
   - Nếu có dữ liệu -> Tổng hợp: Tổng lượt vào, tổng lượt ra, số xe đang gửi, chỗ trống hiện tại, tổng doanh thu, tình trạng hoạt động của bãi.

4. **Check-in & Check-out**:
   - Check-in: Kiểm tra xe trong bãi, kiểm tra bãi còn chỗ, giảm `so_cho_trong`, tạo lượt xe trạng thái `Dang_gui`.
   - Check-out: Tính thời gian gửi, tính tiền theo bảng giá `BangGiaDichVu` (theo Lượt hoặc Giờ), cập nhật trạng thái `Da_ra`, tăng lại `so_cho_trong`.

---

## 4. HƯỚNG DẪN KHỞI CHẠY VÀ KIỂM THỬ

### 1. Khởi chạy Backend Web API
Mở terminal tại thư mục gốc hoặc thư mục `Backend/`, chạy lệnh:
```powershell
cd Backend
dotnet run --project QL_BaiXe.Api/QL_BaiXe.Api.csproj
```
*(Chương trình tự động tạo file SQLite `parking.db` và nạp sẵn dữ liệu mẫu thực tế).*

### 2. Kiểm thử API trên Swagger UI
Mở trình duyệt:
```
http://localhost:5000/
hoặc
http://localhost:5097/
```

### 3. Trải nghiệm Giao diện Quản lý (Frontend)
Mở trực tiếp file `Frontend/index.html` bằng trình duyệt (hoặc dùng Live Server trong VS Code). Giao diện cho phép:
- Tra cứu báo cáo hoạt động bãi xe theo thời gian thực (Biểu đồ / Chỉ số doanh thu, lượt vào, lượt ra).
- Thao tác Check-in xe vào bãi và Check-out in biên lai thu tiền trực tiếp.
- Xem danh sách và thực hiện xóa bãi đỗ xe (áp dụng luật chặn xóa khi còn xe).
- Xem danh sách và thực hiện xóa đơn vị kinh doanh (áp dụng luật chặn xóa khi còn bãi hoạt động).

### 4. Chạy toàn bộ Unit & Integration Tests
```powershell
cd Backend
dotnet test
```
**Kết quả**: 15/15 tests **Passed** 100%.
