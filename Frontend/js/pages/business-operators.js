/**
 * SMART PARKING - ĐƠN VỊ KINH DOANH PAGE SCRIPT
 * Quản lý đối tác / đơn vị kinh doanh vận hành bãi xe.
 * - Quản lý hệ thống: Xem toàn bộ, thêm mới, sửa thông tin & tài khoản, xóa.
 * - Đơn vị kinh doanh: Xem thông tin của đơn vị mình và chỉnh sửa thông tin / đổi mật khẩu.
 */

async function initPage() {
    await loadDonVis();
}

async function loadDonVis() {
    try {
        const res = await fetch(`${getApiUrl()}/api/business-operators`, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) {
            console.error("Không thể tải đơn vị kinh doanh:", json.message);
            return;
        }

        allDonVis = json.data || [];

        // Điều chỉnh tiêu đề và nút bấm theo vai trò
        const isOperator = (currentUser && currentUser.role === 'DonViKinhDoanh');
        const titleEl = document.getElementById('donViPageTitle');
        const subtitleEl = document.getElementById('donViPageSubtitle');
        const addBtnContainer = document.getElementById('btnAddDonViContainer');

        if (isOperator) {
            if (titleEl) titleEl.innerHTML = `<i class="fa-solid fa-building-user me-2"></i>Thông Tin Đơn Vị Của Bạn`;
            if (subtitleEl) subtitleEl.innerText = 'Xem chi tiết và chỉnh sửa thông tin doanh nghiệp, đổi mật khẩu tài khoản của bạn';
            if (addBtnContainer) addBtnContainer.classList.add('d-none');
        } else {
            if (titleEl) titleEl.innerHTML = `<i class="fa-solid fa-building me-2"></i>Danh Sách Đơn Vị Kinh Doanh`;
            if (subtitleEl) subtitleEl.innerText = 'Quản lý và cấp quyền cho các doanh nghiệp vận hành bãi đỗ xe';
            if (addBtnContainer) addBtnContainer.classList.remove('d-none');
        }

        // Lọc danh sách hiển thị
        let displayList = allDonVis;
        if (isOperator) {
            const myOp = allDonVis.filter(d => 
                (currentUser.businessOperatorId && d.id === currentUser.businessOperatorId) || 
                (currentUser.donViId && d.id === currentUser.donViId) || 
                (currentUser.userId && d.id === currentUser.userId) || 
                (currentUser.username && d.username === currentUser.username)
            );
            if (myOp.length > 0) {
                displayList = myOp;
            }
        }

        const tbody = document.getElementById('donViTable');
        if (!tbody) return;
        tbody.innerHTML = '';
        if (displayList.length === 0) {
            tbody.innerHTML = `<tr><td colspan="8" class="text-center text-muted py-4">Chưa có thông tin đơn vị kinh doanh nào.</td></tr>`;
            return;
        }

        displayList.forEach(d => {
            const countBai = d.parkingLots ? d.parkingLots.length : 0;
            const tr = document.createElement('tr');
            
            let actionButtons = '';
            if (isOperator) {
                actionButtons = `
                    <button class="btn btn-sm btn-primary rounded-pill px-3 shadow-sm" onclick="openEditDonViModal('${d.id}')" title="Chỉnh sửa thông tin & đổi mật khẩu">
                        <i class="fa-solid fa-pen-to-square me-1"></i> Chỉnh Sửa & Đổi Mật Khẩu
                    </button>
                `;
            } else {
                actionButtons = `
                    <button class="btn btn-sm btn-outline-primary rounded-pill px-3 me-1" onclick="openEditDonViModal('${d.id}')" title="Chỉnh sửa thông tin & tài khoản">
                        <i class="fa-solid fa-pen-to-square me-1"></i> Sửa
                    </button>
                    <button class="btn btn-sm btn-outline-danger rounded-pill px-3" onclick="xoaDonVi('${d.id}')" title="Xóa đơn vị kinh doanh">
                        <i class="fa-solid fa-trash me-1"></i> Xóa
                    </button>
                `;
            }

            const phone = d.contactInfo?.phoneNumber || '-';
            const email = d.contactInfo?.email || '-';
            const fullAddr = d.address?.fullAddress || [d.address?.street, d.address?.ward, d.address?.district, d.address?.city].filter(Boolean).join(', ') || '-';

            tr.innerHTML = `
                <td class="fw-bold text-dark">${d.name || '-'}</td>
                <td><code class="bg-light px-2 py-1 rounded text-primary fw-bold">${d.username || '-'}</code></td>
                <td>${d.representative || '-'}</td>
                <td class="small">
                    <div><i class="fa-solid fa-phone me-1 text-muted"></i>${phone}</div>
                    <div><i class="fa-solid fa-envelope me-1 text-muted"></i>${email}</div>
                </td>
                <td class="small text-muted">${fullAddr}</td>
                <td><span class="badge bg-primary bg-opacity-10 text-primary border border-primary border-opacity-25 rounded-pill px-2.5">${countBai} bãi</span></td>
                <td><span class="badge ${d.isActive ? 'badge-active' : 'badge-inactive'}">${d.isActive ? '● Hoạt động' : 'Tạm dừng'}</span></td>
                <td class="text-center text-nowrap">${actionButtons}</td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error("Lỗi tải đơn vị kinh doanh:", err);
        const tbody = document.getElementById('donViTable');
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="8" class="text-center text-danger py-4">Không thể kết nối đến máy chủ API (${getApiUrl()}). Vui lòng kiểm tra lại backend.</td></tr>`;
        }
    }
}

function openAddDonViModal() {
    if (!currentUser || currentUser.role !== 'QuanLy') {
        showAlert("Chỉ Quản lý hệ thống mới có quyền thêm mới đơn vị kinh doanh!", "warning");
        return;
    }
    // Reset form
    document.getElementById('mdvTenDonVi').value = '';
    document.getElementById('mdvNguoiDaiDien').value = '';
    document.getElementById('mdvSdt').value = '';
    document.getElementById('mdvEmail').value = '';
    document.getElementById('mdvSoNha').value = '';
    document.getElementById('mdvPhuongXa').value = '';
    document.getElementById('mdvQuanHuyen').value = '';
    document.getElementById('mdvTinhThanh').value = 'TP. Hồ Chí Minh';
    if (document.getElementById('mdvTenDangNhap')) document.getElementById('mdvTenDangNhap').value = '';
    if (document.getElementById('mdvMatKhau')) document.getElementById('mdvMatKhau').value = '';

    new bootstrap.Modal(document.getElementById('addDonViModal')).show();
}

async function handleCreateDonVi(e) {
    if (e && e.preventDefault) e.preventDefault();
    const sdt = document.getElementById('mdvSdt').value.trim();
    const tenDangNhap = document.getElementById('mdvTenDangNhap')?.value.trim() || sdt;
    const matKhau = document.getElementById('mdvMatKhau')?.value.trim() || '123@456';

    const body = {
        systemManagerId: (currentUser && currentUser.userId) ? currentUser.userId : "11111111-1111-1111-1111-111111111111",
        name: document.getElementById('mdvTenDonVi').value.trim(),
        representative: document.getElementById('mdvNguoiDaiDien').value.trim(),
        address: {
            street: document.getElementById('mdvSoNha').value.trim(),
            ward: document.getElementById('mdvPhuongXa').value.trim(),
            district: document.getElementById('mdvQuanHuyen').value.trim(),
            city: document.getElementById('mdvTinhThanh').value.trim()
        },
        contactInfo: {
            phoneNumber: sdt,
            email: document.getElementById('mdvEmail').value.trim()
        },
        isActive: true,
        username: tenDangNhap,
        password: matKhau
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/business-operators`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            showAlert(`Thêm mới đơn vị thành công! Tài khoản: ${tenDangNhap} | Mật khẩu: ${matKhau}`, "success");
            const modalEl = document.getElementById('addDonViModal');
            if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadDonVis();
        } else {
            showAlert(json.message || "Không thể thêm đơn vị kinh doanh!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối máy chủ API: " + err.message);
    }
}

function openEditDonViModal(id) {
    const isOperator = (currentUser && currentUser.role === 'DonViKinhDoanh');
    const isAdmin = (currentUser && currentUser.role === 'QuanLy');

    if (!isAdmin && !isOperator) {
        showAlert("Vui lòng đăng nhập để thực hiện thao tác!", "warning");
        return;
    }

    const d = allDonVis.find(x => x.id === id);
    if (!d) {
        showAlert("Không tìm thấy thông tin đơn vị kinh doanh!", "warning");
        return;
    }

    document.getElementById('edvId').value = d.id;
    document.getElementById('edvTenDonVi').value = d.name || '';
    document.getElementById('edvNguoiDaiDien').value = d.representative || '';
    document.getElementById('edvSdt').value = d.contactInfo?.phoneNumber || '';
    document.getElementById('edvEmail').value = d.contactInfo?.email || '';

    const addr = d.address || {};
    document.getElementById('edvSoNha').value = addr.street || '';
    document.getElementById('edvPhuongXa').value = addr.ward || '';
    document.getElementById('edvQuanHuyen').value = addr.district || '';
    document.getElementById('edvTinhThanh').value = addr.city || 'TP. Hồ Chí Minh';

    document.getElementById('edvTenDangNhap').value = d.username || '';
    document.getElementById('edvMatKhau').value = '';
    document.getElementById('edvTrangThai').value = (d.isActive ?? true) ? 'true' : 'false';

    // Nếu là Đơn vị kinh doanh tự sửa thì khóa ô Trạng Thái (không cho tự đổi trạng thái)
    const trangThaiSelect = document.getElementById('edvTrangThai');
    if (trangThaiSelect) {
        trangThaiSelect.disabled = isOperator;
    }

    new bootstrap.Modal(document.getElementById('editDonViModal')).show();
}

async function handleUpdateDonVi(e) {
    if (e && e.preventDefault) e.preventDefault();
    const id = document.getElementById('edvId').value;
    const sdt = document.getElementById('edvSdt').value.trim();
    const tenDangNhap = document.getElementById('edvTenDangNhap').value.trim() || sdt;
    const matKhau = document.getElementById('edvMatKhau').value.trim();

    const existing = allDonVis.find(x => x.id === id);
    const managerId = existing?.systemManagerId || (currentUser?.userId || "11111111-1111-1111-1111-111111111111");

    const body = {
        systemManagerId: managerId,
        name: document.getElementById('edvTenDonVi').value.trim(),
        representative: document.getElementById('edvNguoiDaiDien').value.trim(),
        address: {
            street: document.getElementById('edvSoNha').value.trim(),
            ward: document.getElementById('edvPhuongXa').value.trim(),
            district: document.getElementById('edvQuanHuyen').value.trim(),
            city: document.getElementById('edvTinhThanh').value.trim()
        },
        contactInfo: {
            phoneNumber: sdt,
            email: document.getElementById('edvEmail').value.trim()
        },
        isActive: document.getElementById('edvTrangThai').value === 'true',
        username: tenDangNhap,
        password: matKhau
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/business-operators/${id}`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Cập nhật thông tin đơn vị kinh doanh thành công!", "success");
            const modalEl = document.getElementById('editDonViModal');
            if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadDonVis();
        } else {
            showAlert(json.message || "Không thể cập nhật đơn vị kinh doanh!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối máy chủ API: " + err.message);
    }
}

async function xoaDonVi(id) {
    if (!currentUser || currentUser.role !== 'QuanLy') {
        showAlert("Chỉ Quản lý hệ thống mới có quyền xóa đơn vị kinh doanh!", "warning");
        return;
    }
    if (!confirm("Bạn có chắc chắn muốn xóa đơn vị kinh doanh này không? Nếu đơn vị đang có bãi xe hoạt động sẽ bị chặn xóa theo quy tắc nghiệp vụ.")) return;
    try {
        const res = await fetch(`${getApiUrl()}/api/business-operators/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Xóa đơn vị kinh doanh thành công!", "success");
            await loadDonVis();
        } else {
            showAlert(json.message || "Không thể xóa đơn vị kinh doanh!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối đến máy chủ: " + err.message);
    }
}

async function refreshCurrentPage() {
    await initPage();
}

document.addEventListener('DOMContentLoaded', initPage);
