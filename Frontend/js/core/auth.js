/**
 * SMART PARKING - AUTHENTICATION & ROLE-BASED ACCESS CONTROL (RBAC)
 * Quản lý phiên đăng nhập, JWT token, phân quyền và hiển thị giao diện theo vai trò.
 */

let currentUser = null; // { token, role, username, fullName, businessOperatorId, donViId, userId, displayRole }

/**
 * Khởi tạo trạng thái xác thực từ localStorage
 */
function initAuth() {
    const saved = localStorage.getItem('currentUser');
    if (saved) {
        try {
            currentUser = JSON.parse(saved);
        } catch (e) {
            currentUser = null;
        }
    }
    applyUserRoleUI();
}

/**
 * Cập nhật giao diện thanh điều hướng, banner và các nút chức năng theo quyền hạn
 */
function applyUserRoleUI() {
    const navInfo = document.getElementById('navUserInfo');
    const navLogin = document.getElementById('navLoginBtn');
    const bannerTitle = document.getElementById('bannerRoleTitle');
    const bannerDesc = document.getElementById('bannerRoleDesc');
    const bannerIcon = document.getElementById('bannerRoleIcon');

    // Menu links
    const navDashboard = document.getElementById('navItem-dashboard');
    const navCheckIn = document.getElementById('navItem-checkin');
    const navBaiXe = document.getElementById('navItem-baixe');
    const navDonVi = document.getElementById('navItem-donvi');
    const navBangGia = document.getElementById('navItem-banggia');
    const labelBaiXe = document.getElementById('labelNavBaiXe');

    if (currentUser) {
        if (navInfo) {
            navInfo.classList.remove('d-none');
            navInfo.classList.add('d-flex');
        }
        if (navLogin) navLogin.classList.add('d-none');

        const fullNameEl = document.getElementById('navUserFullName');
        const roleEl = document.getElementById('navUserRole');
        if (fullNameEl) fullNameEl.innerText = currentUser.fullName || currentUser.username;
        if (roleEl) roleEl.innerText = currentUser.displayRole || currentUser.role;

        if (currentUser.role === 'QuanLy') {
            const roleIcon = document.getElementById('userRoleIcon');
            if (roleIcon) roleIcon.className = 'fa-solid fa-user-shield me-2 text-danger fs-5';
            if (bannerIcon) bannerIcon.className = 'fa-solid fa-user-shield fs-3 text-danger';
            if (bannerTitle) bannerTitle.innerHTML = `<span class="badge bg-danger me-2">QUẢN LÝ HỆ THỐNG</span> Chào mừng ${currentUser.fullName}`;
            if (bannerDesc) bannerDesc.innerHTML = `Quyền hạn: Xem báo cáo thống kê tất cả các bãi, CRUD Đơn vị kinh doanh, CRUD Bãi đỗ xe hệ thống.`;

            // Menu Quản lý: Thấy Báo cáo, Bãi đỗ xe, Đơn vị kinh doanh
            if (navDashboard) navDashboard.classList.remove('d-none');
            if (navBaiXe) navBaiXe.classList.remove('d-none');
            if (navDonVi) navDonVi.classList.remove('d-none');
            if (labelBaiXe) labelBaiXe.innerText = 'Quản Lý Bãi Đỗ Xe';

            // Menu Quản lý: ẨN Check-in và Bảng giá
            if (navCheckIn) navCheckIn.classList.add('d-none');
            if (navBangGia) navBangGia.classList.add('d-none');

            // Nút thêm mới bãi đỗ xe
            const btnAddBai = document.getElementById('btnGroupAdminBaiXe');
            if (btnAddBai) btnAddBai.classList.remove('d-none');

        } else if (currentUser.role === 'DonViKinhDoanh') {
            const roleIcon = document.getElementById('userRoleIcon');
            if (roleIcon) roleIcon.className = 'fa-solid fa-building me-2 text-success fs-5';
            if (bannerIcon) bannerIcon.className = 'fa-solid fa-building fs-3 text-success';
            if (bannerTitle) bannerTitle.innerHTML = `<span class="badge bg-success me-2">ĐƠN VỊ KINH DOANH</span> ${currentUser.fullName}`;
            if (bannerDesc) bannerDesc.innerHTML = `Quyền hạn: Quản lý bãi xe trực thuộc, thực hiện Check-in / Check-out, và CRUD biểu phí dịch vụ cho bãi của mình.`;

            // Menu Đơn vị: Thấy Báo cáo, Bãi xe của tôi, Check-in, Bảng giá
            if (navDashboard) navDashboard.classList.remove('d-none');
            if (navBaiXe) navBaiXe.classList.remove('d-none');
            if (navCheckIn) navCheckIn.classList.remove('d-none');
            if (navBangGia) navBangGia.classList.remove('d-none');
            if (labelBaiXe) labelBaiXe.innerText = 'Bãi Xe Của Tôi';

            // Menu Đơn vị: Hiển thị mục "Thông Tin Đơn Vị" để xem & đổi mật khẩu
            if (navDonVi) {
                navDonVi.classList.remove('d-none');
                navDonVi.innerHTML = '<i class="fa-solid fa-building-user"></i> Thông Tin Đơn Vị';
            }

            // Ẩn nút thêm mới bãi đỗ xe đối với đơn vị kinh doanh
            const btnAddBai = document.getElementById('btnGroupAdminBaiXe');
            if (btnAddBai) btnAddBai.classList.add('d-none');
        }
    } else {
        if (navInfo) {
            navInfo.classList.remove('d-flex');
            navInfo.classList.add('d-none');
        }
        if (navLogin) navLogin.classList.remove('d-none');

        if (bannerIcon) bannerIcon.className = 'fa-solid fa-circle-info fs-3 text-primary';
        if (bannerTitle) bannerTitle.innerText = 'Chế độ xem: Khách vãng lai';
        if (bannerDesc) bannerDesc.innerText = 'Khách vãng lai chỉ có quyền tra cứu danh sách bãi đỗ xe và chỗ trống công khai. Báo cáo thống kê, doanh thu và các nghiệp vụ điều hành yêu cầu đăng nhập tài khoản.';

        // Khách vãng lai: ẨN Thống kê / Báo cáo, chỉ xem Bãi xe công khai
        if (navDashboard) navDashboard.classList.add('d-none');
        if (navBaiXe) navBaiXe.classList.remove('d-none');
        if (navCheckIn) navCheckIn.classList.add('d-none');
        if (navDonVi) navDonVi.classList.add('d-none');
        if (navBangGia) navBangGia.classList.add('d-none');
        const btnAddBai = document.getElementById('btnGroupAdminBaiXe');
        if (btnAddBai) btnAddBai.classList.add('d-none');
    }
}

/**
 * Hiển thị Modal đăng nhập hoặc chuyển hướng đến login.html
 */
function showLoginModal() {
    const modalEl = document.getElementById('loginModal');
    if (modalEl) {
        new bootstrap.Modal(modalEl).show();
    } else {
        window.location.href = 'login.html';
    }
}

/**
 * Đăng nhập nhanh bằng 1 click từ giao diện demo
 */
async function quickLogin(u, p) {
    await doLogin(u, p);
}

/**
 * Xử lý sự kiện submit form đăng nhập
 */
async function handleFormLogin(e) {
    if (e && e.preventDefault) e.preventDefault();
    const u = document.getElementById('loginUsername')?.value.trim();
    const p = document.getElementById('loginPassword')?.value.trim();
    if (!u || !p) {
        showAlert("Vui lòng nhập tên đăng nhập và mật khẩu!", "warning");
        return;
    }
    await doLogin(u, p);
}

/**
 * Thực hiện gọi API đăng nhập backend và lưu trữ token
 */
async function doLogin(username, password) {
    const errBox = document.getElementById('loginErrorMsg');
    if (errBox) errBox.classList.add('d-none');

    try {
        const payload = {
            username: username,
            password: password
        };

        const res = await fetch(`${getApiUrl()}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });
        const json = await res.json();

        // Backend trả về trực tiếp LoginResponseDto hoặc bọc trong { data: ... }
        const data = json.data ? json.data : json;

        if (res.ok && (data.token || json.success)) {
            const isManager = (data.role === 'Manager' || data.role === 'QuanLy');
            const normalizedRole = isManager ? 'QuanLy' : 'DonViKinhDoanh';
            currentUser = {
                token: data.token,
                role: normalizedRole,
                rawRole: data.role,
                username: data.username,
                fullName: data.fullName,
                businessOperatorId: data.businessOperatorId || null,
                donViId: data.businessOperatorId || null,
                userId: data.userId,
                displayRole: (normalizedRole === 'QuanLy') ? 'Quản Lý Hệ Thống' : 'Đơn Vị Kinh Doanh'
            };
            localStorage.setItem('currentUser', JSON.stringify(currentUser));

            const modalEl = document.getElementById('loginModal');
            if (modalEl) {
                const modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) modal.hide();
            }

            showAlert(`Đăng nhập thành công với vai trò: ${currentUser.displayRole}!`, "success");
            applyUserRoleUI();

            // Nếu đang ở trang login, chuyển hướng tới trang phù hợp
            if (window.location.pathname.endsWith('login.html')) {
                if (currentUser.role === 'QuanLy') {
                    window.location.href = 'dashboard.html';
                } else {
                    window.location.href = 'checkin.html';
                }
                return;
            }

            // Gọi hàm refresh trang hiện tại nếu có
            if (typeof refreshCurrentPage === 'function') {
                await refreshCurrentPage();
            }
        } else {
            let msg = json.message;
            if (!msg && json.errors) {
                msg = Object.values(json.errors).flat().join(", ");
            }
            if (!msg) msg = "Tên đăng nhập hoặc mật khẩu không chính xác!";

            if (errBox) {
                errBox.innerText = msg;
                errBox.classList.remove('d-none');
            } else {
                showAlert(msg, "danger");
            }
        }
    } catch (err) {
        if (errBox) {
            errBox.innerText = "Lỗi kết nối đến máy chủ API: " + err.message;
            errBox.classList.remove('d-none');
        } else {
            showAlert("Lỗi kết nối máy chủ API.", "danger");
        }
    }
}

/**
 * Đăng xuất tài khoản
 */
function handleLogout() {
    currentUser = null;
    localStorage.removeItem('currentUser');
    showAlert("Bạn đã đăng xuất khỏi hệ thống thành công.", "info");
    applyUserRoleUI();
    if (typeof refreshCurrentPage === 'function') {
        refreshCurrentPage();
    }
}

/**
 * Chuyển nhanh giữa tài khoản Quản lý và Đơn vị kinh doanh
 */
async function quickSwitchUser(username, password) {
    await doLogin(username, password);
    if (username === 'admin') {
        window.location.href = 'business-operators.html';
    } else {
        window.location.href = 'checkin.html';
    }
}
