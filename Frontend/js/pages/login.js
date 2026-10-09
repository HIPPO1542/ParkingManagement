/**
 * SMART PARKING - LOGIN PAGE SCRIPT
 * Xử lý giao diện trang đăng nhập, phím tắt và tự động chuyển hướng nếu đã đăng nhập.
 */

document.addEventListener('DOMContentLoaded', () => {
    // Nếu đã đăng nhập từ trước, tự động chuyển hướng
    if (currentUser && currentUser.token) {
        if (currentUser.role === 'QuanLy') {
            window.location.replace('dashboard.html');
        } else {
            window.location.replace('checkin.html');
        }
        return;
    }

    // Tự động focus vào ô Tên đăng nhập
    const usernameInput = document.getElementById('loginUsername');
    if (usernameInput) {
        usernameInput.focus();
    }
});
