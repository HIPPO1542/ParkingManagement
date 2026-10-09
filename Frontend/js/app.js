/**
 * SMART PARKING - SHARED APPLICATION LOGIC & UTILITIES
 * Chứa các biến trạng thái dùng chung, hệ thống thông báo toast alert và khởi tạo ứng dụng.
 */

// Biến trạng thái toàn cục
let allBaiXes = [];
let allLoaiXes = [];
let allDonVis = [];
let allBangGias = [];

/**
 * Hiển thị thông báo Toast góc trên bên phải màn hình
 * @param {string} msg Nội dung thông báo
 * @param {string} type 'danger' | 'success' | 'warning' | 'info'
 */
function showAlert(msg, type = 'danger') {
    const box = document.getElementById('alertBox');
    if (!box) {
        alert(msg);
        return;
    }
    box.className = `alert floating-alert alert-${type} alert-dismissible fade show shadow-lg`;
    const msgEl = document.getElementById('alertMessage');
    if (msgEl) msgEl.innerText = msg;
    box.classList.remove('d-none');
    
    // Tự động ẩn sau 6 giây
    setTimeout(() => {
        if (box && !box.classList.contains('d-none')) {
            box.classList.add('d-none');
        }
    }, 6000);
}

/**
 * Đóng thông báo Toast thủ công
 */
function closeAlert() {
    const box = document.getElementById('alertBox');
    if (box) box.classList.add('d-none');
}

/**
 * Khởi tạo chung cho toàn bộ các trang khi DOM sẵn sàng
 */
document.addEventListener('DOMContentLoaded', () => {
    // 1. Khởi tạo xác thực & cập nhật menu theo quyền
    if (typeof initAuth === 'function') {
        initAuth();
    }

    // 2. Lắng nghe thay đổi API URL từ input (nếu có trên navbar)
    const apiUrlInput = document.getElementById('apiUrlInput');
    if (apiUrlInput) {
        const savedUrl = localStorage.getItem('parking_api_url') || 'http://localhost:5097';
        apiUrlInput.value = savedUrl;
        apiUrlInput.addEventListener('change', () => {
            const newUrl = apiUrlInput.value.trim();
            localStorage.setItem('parking_api_url', newUrl);
            showAlert(`Đã lưu URL máy chủ API: ${newUrl}`, 'info');
        });
    }
});
