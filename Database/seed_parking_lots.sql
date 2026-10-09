-- ====================================================================
-- TẮT TẠM THỜI KIỂM TRA RÀNG BUỘC KHÓA NGOẠI TRONG LÚC CHÈN DỮ LIỆU
-- ====================================================================
PRAGMA foreign_keys = OFF;

-- 1. ĐẢM BẢO BẢNG NHÂN VIÊN QUẢN LÝ (KHÓA GỐC) ĐÃ CÓ DỮ LIỆU
INSERT OR REPLACE INTO "NHAN_VIEN_QUAN_LY" (
    "ma_nhan_vien", "ho_ten", "chuc_vu", "so_dien_thoai", "email", 
    "ten_dang_nhap", "mat_khau_hash", "trang_thai_hoat_dong"
) VALUES (
    '11111111-1111-1111-1111-111111111111', 
    'Nguyễn Văn Quản Trị', 'Quản trị hệ thống', '0901234567', 'quantri@baixe.vn', 
    'admin', 'admin@123', 1
);

-- 2. ĐẢM BẢO BẢNG ĐƠN VỊ KINH DOANH ĐÃ CÓ DỮ LIỆU
INSERT OR REPLACE INTO "DON_VI_KINH_DOANH" (
    "ma_don_vi", "ma_nhan_vien_ql", "ten_don_vi", "nguoi_dai_dien", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "so_dien_thoai", "email", "trang_thai_hoat_dong"
) VALUES (
    '22222222-2222-2222-2222-222222222222', 
    '11111111-1111-1111-1111-111111111111', 
    'Tổng Công Ty Đô Thị Sài Gòn Parking', 'Trần Thị Giám Đốc', 
    '123 Đường Lê Lợi', 'Phường Bến Nghé', 'Quận 1', 'TP. Hồ Chí Minh', 
    '02838222333', 'lienhe@sgparking.vn', 1
);

-- 3. ĐẢM BẢO BẢNG DANH MỤC LOẠI XE ĐÃ CÓ DỮ LIỆU
INSERT OR REPLACE INTO "LOAI_XE" ("ma_loai_xe", "ten_loai_xe", "mo_ta") VALUES
('66666666-6666-6666-6666-666666666666', 'Xe máy', 'Phương tiện giao thông 2 bánh gắn máy'),
('77777777-7777-7777-7777-777777777777', 'Ô tô con (4 - 7 chỗ)', 'Xe du lịch gia đình dưới 9 chỗ ngồi'),
('88888888-8888-8888-8888-888888888888', 'Xe tải nhẹ', 'Xe vận chuyển hàng hóa dưới 2.5 tấn');

-- ====================================================================
-- BỘ DỮ LIỆU 10 BÃI ĐỖ XE HOẠT ĐỘNG TRONG 2 THÁNG (THÁNG 9 & THÁNG 10/2026)
-- ====================================================================

-- Bảng giá cho: Bãi Đỗ Xe TTTM Vincom Center Đồng Khởi
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-00-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-00-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Tòa Nhà Bitexco Financial Tower
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-01-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-01-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Quảng Trường Chợ Bến Thành
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-02-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-02-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Công Viên Thảo Cầm Viên
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-03-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-03-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Nhà Ga T1-T2 Sân Bay Tân Sơn Nhất
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-04-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-04-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Ga Đường Sắt Sài Gòn
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-05-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-05-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe TTTM Vạn Hạnh Mall
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-06-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-06-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Crescent Mall Phú Mỹ Hưng
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-07-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-07-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Đại Siêu Thị AEON Mall Bình Tân
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-08-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-08-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- Bảng giá cho: Bãi Đỗ Xe Khách Bến Xe Miền Đông Mới
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-may-09-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '5000', 'VND', 'Luot', '2026-08-01 00:00:00');
INSERT OR REPLACE INTO "GIA_VE_BAI_XE" ("ma_gia_ve", "ma_bai", "ma_loai_xe", "muc_gia", "don_vi_tien", "hinh_thuc_tinh", "ngay_ap_dung")
VALUES ('gv-oto-09-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '25000', 'VND', 'Gio', '2026-08-01 00:00:00');

-- ====================================================================
-- 1. THÊM 10 BÃI ĐỖ XE ĐANG HOẠT ĐỘNG
-- ====================================================================

INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0100000-0000-0000-0000-000000000001', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe TTTM Vincom Center Đồng Khởi',
    '72 Lê Thánh Tôn', 'Phường Bến Nghé', 'Quận 1', 'TP. Hồ Chí Minh',
    300, 297, '08:00', '22:30', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0200000-0000-0000-0000-000000000002', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Tòa Nhà Bitexco Financial Tower',
    '02 Hải Triều', 'Phường Bến Nghé', 'Quận 1', 'TP. Hồ Chí Minh',
    250, 248, '06:00', '23:00', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0300000-0000-0000-0000-000000000003', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Quảng Trường Chợ Bến Thành',
    'Đường Lê Lai', 'Phường Bến Thành', 'Quận 1', 'TP. Hồ Chí Minh',
    200, 197, '05:00', '23:59', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0400000-0000-0000-0000-000000000004', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Công Viên Thảo Cầm Viên',
    '02 Nguyễn Bỉnh Khiêm', 'Phường Bến Nghé', 'Quận 1', 'TP. Hồ Chí Minh',
    180, 178, '07:00', '18:30', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0500000-0000-0000-0000-000000000005', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Nhà Ga T1-T2 Sân Bay Tân Sơn Nhất',
    'Đường Trường Sơn', 'Phường 2', 'Quận Tân Bình', 'TP. Hồ Chí Minh',
    500, 497, '00:00', '23:59', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0600000-0000-0000-0000-000000000006', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Ga Đường Sắt Sài Gòn',
    '01 Nguyễn Thông', 'Phường 9', 'Quận 3', 'TP. Hồ Chí Minh',
    150, 148, '04:30', '23:00', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0700000-0000-0000-0000-000000000007', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe TTTM Vạn Hạnh Mall',
    '11 Sư Vạn Hạnh', 'Phường 12', 'Quận 10', 'TP. Hồ Chí Minh',
    350, 347, '08:30', '22:00', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0800000-0000-0000-0000-000000000008', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Crescent Mall Phú Mỹ Hưng',
    '101 Tôn Dật Tiên', 'Phường Tân Phú', 'Quận 7', 'TP. Hồ Chí Minh',
    400, 398, '08:00', '22:30', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b0900000-0000-0000-0000-000000000009', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Đại Siêu Thị AEON Mall Bình Tân',
    '01 Đường Số 17A', 'Phường Bình Trị Đông B', 'Quận Bình Tân', 'TP. Hồ Chí Minh',
    450, 447, '08:00', '22:00', 1
);
INSERT OR REPLACE INTO "BAI_DO_XE" (
    "ma_bai", "ma_don_vi", "ten_bai", 
    "dia_chi_so_nha", "dia_chi_phuong_xa", "dia_chi_quan_huyen", "dia_chi_tinh_thanh", 
    "tong_suc_chua", "so_cho_trong", "gio_mo_cua", "gio_dong_cua", "dang_hoat_dong"
) VALUES (
    'b1000000-0000-0000-0000-000000000010', '22222222-2222-2222-2222-222222222222', 'Bãi Đỗ Xe Khách Bến Xe Miền Đông Mới',
    '501 Hoàng Hữu Nam', 'Phường Long Bình', 'TP. Thủ Đức', 'TP. Hồ Chí Minh',
    300, 298, '00:00', '23:59', 1
);

-- ====================================================================
-- 2. THÊM 265 LƯỢT XE RA VÀO PHÂN BỔ TRONG 2 THÁNG (THÁNG 9 & 10)
-- ====================================================================

INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00101-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00102-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00103-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00104-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00105-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00106-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00107-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00108-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00109-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00110-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00111-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00112-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00113-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00114-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00115-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00116-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00117-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00118-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00119-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00120-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00121-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00122-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00123-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00124-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00125-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00126-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00127-0000-0000-000000000000', 'b0100000-0000-0000-0000-000000000001', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-10-05 09:00:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00128-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00129-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00130-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00131-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00132-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00133-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00134-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00135-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00136-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00137-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00138-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00139-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00140-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00141-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00142-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00143-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00144-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00145-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00146-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00147-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00148-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00149-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00150-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00151-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00152-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00153-0000-0000-000000000000', 'b0200000-0000-0000-0000-000000000002', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00154-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00155-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00156-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00157-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00158-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00159-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00160-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00161-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00162-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00163-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00164-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00165-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00166-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00167-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00168-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00169-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00170-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00171-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00172-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00173-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00174-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00175-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00176-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00177-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00178-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00179-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00180-0000-0000-000000000000', 'b0300000-0000-0000-0000-000000000003', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-10-05 09:00:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00181-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00182-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00183-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00184-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00185-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00186-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00187-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00188-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00189-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00190-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00191-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00192-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00193-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00194-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00195-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00196-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00197-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00198-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00199-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00200-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00201-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00202-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00203-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00204-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00205-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00206-0000-0000-000000000000', 'b0400000-0000-0000-0000-000000000004', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00207-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00208-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00209-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00210-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00211-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00212-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00213-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00214-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00215-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00216-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00217-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00218-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00219-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00220-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00221-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00222-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00223-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00224-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00225-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00226-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00227-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00228-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00229-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00230-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00231-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00232-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00233-0000-0000-000000000000', 'b0500000-0000-0000-0000-000000000005', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-10-05 09:00:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00234-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00235-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00236-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00237-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00238-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00239-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00240-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00241-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00242-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00243-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00244-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00245-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00246-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00247-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00248-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00249-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00250-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00251-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00252-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00253-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00254-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00255-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00256-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00257-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00258-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '66666666-6666-6666-6666-666666666666', '61', 'D2-55667',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00259-0000-0000-000000000000', 'b0600000-0000-0000-0000-000000000006', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00260-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00261-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00262-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00263-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00264-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00265-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00266-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00267-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00268-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00269-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00270-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00271-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00272-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00273-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00274-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00275-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00276-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00277-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00278-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00279-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00280-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00281-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00282-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00283-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00284-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'X1-77889',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00285-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00286-0000-0000-000000000000', 'b0700000-0000-0000-0000-000000000007', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-10-05 09:00:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00287-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00288-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00289-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00290-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00291-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00292-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00293-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00294-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00295-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00296-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00297-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00298-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00299-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00300-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00301-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00302-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00303-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00304-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00305-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00306-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00307-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00308-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00309-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00310-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00311-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '66666666-6666-6666-6666-666666666666', '59', 'Y2-99001',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00312-0000-0000-000000000000', 'b0800000-0000-0000-0000-000000000008', '77777777-7777-7777-7777-777777777777', '50', 'H-88888',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00313-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00314-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00315-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00316-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00317-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00318-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00319-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00320-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00321-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00322-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00323-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00324-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00325-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00326-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00327-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00328-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00329-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00330-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00331-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00332-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00333-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00334-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00335-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00336-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00337-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '51', 'Z3-22334',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00338-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '77777777-7777-7777-7777-777777777777', '51', 'G-99999',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00339-0000-0000-000000000000', 'b0900000-0000-0000-0000-000000000009', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-10-05 09:00:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00340-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-09-02 08:30:00', '2026-09-02 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00341-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-09-02 09:15:00', '2026-09-02 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00342-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '59', 'M1-66778',
    '2026-09-05 08:30:00', '2026-09-05 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00343-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '51', 'K-11111',
    '2026-09-05 09:15:00', '2026-09-05 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00344-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '59', 'N2-88990',
    '2026-09-09 08:30:00', '2026-09-09 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00345-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-09-09 09:15:00', '2026-09-09 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00346-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-09-12 08:30:00', '2026-09-12 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00347-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-09-12 09:15:00', '2026-09-12 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00348-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-09-16 08:30:00', '2026-09-16 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00349-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-09-16 09:15:00', '2026-09-16 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00350-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-09-20 08:30:00', '2026-09-20 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00351-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-09-20 09:15:00', '2026-09-20 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00352-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-09-24 08:30:00', '2026-09-24 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00353-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '51', 'C-66666',
    '2026-09-24 09:15:00', '2026-09-24 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00354-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '60', 'C1-33445',
    '2026-09-27 08:30:00', '2026-09-27 11:45:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00355-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '51', 'D-77777',
    '2026-09-27 09:15:00', '2026-09-27 13:15:00', '100000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00356-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '59', 'T1-12345',
    '2026-10-01 14:00:00', '2026-10-01 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00357-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '51', 'F-22222',
    '2026-10-01 10:00:00', '2026-10-01 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00358-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '59', 'K2-98765',
    '2026-10-02 14:00:00', '2026-10-02 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00359-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '50', 'E-33333',
    '2026-10-02 10:00:00', '2026-10-02 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00360-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '51', 'A3-45678',
    '2026-10-03 14:00:00', '2026-10-03 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00361-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '60', 'A-44444',
    '2026-10-03 10:00:00', '2026-10-03 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00362-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '50', 'B4-11223',
    '2026-10-04 14:00:00', '2026-10-04 17:30:00', '5000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00363-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '61', 'B-55555',
    '2026-10-04 10:00:00', '2026-10-04 12:00:00', '50000', 'VND', 'Da_ra'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00364-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '66666666-6666-6666-6666-666666666666', '52', 'H4-44556',
    '2026-10-05 07:15:00', NULL, '0', 'VND', 'Dang_gui'
);
INSERT OR REPLACE INTO "LUOT_RA_VAO" (
    "ma_luot", "ma_bai", "ma_loai_xe", "bien_so_ma_tinh", "bien_so_xe",
    "thoi_gian_vao", "thoi_gian_ra", "so_tien_thu", "don_vi_tien", "trang_thai"
) VALUES (
    'luot-00365-0000-0000-000000000000', 'b1000000-0000-0000-0000-000000000010', '77777777-7777-7777-7777-777777777777', '59', 'LD-12345',
    '2026-10-05 08:30:00', NULL, '0', 'VND', 'Dang_gui'
);

-- ====================================================================
-- BẬT LẠI KIỂM TRA RÀNG BUỘC KHÓA NGOẠI
-- ====================================================================
PRAGMA foreign_keys = ON;
