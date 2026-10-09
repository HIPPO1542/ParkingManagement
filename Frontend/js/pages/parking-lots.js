/**
 * SMART PARKING - BÃI ĐỖ XE PAGE SCRIPT
 * Quản lý danh sách bãi xe, tìm kiếm & bộ lọc, xem chi tiết, Thêm / Sửa / Xóa bãi đỗ xe.
 */

async function initPage() {
    await Promise.all([
        loadBaiXes(),
        loadDonVis()
    ]);
}

async function loadBaiXes() {
    try {
        let url = `${getApiUrl()}/api/parking-lots?pageSize=100`;
        const opId = currentUser?.businessOperatorId || currentUser?.donViId;
        if (currentUser && currentUser.role === 'DonViKinhDoanh' && opId) {
            url += `&operatorId=${opId}`;
        }

        const res = await fetch(url, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) return;

        allBaiXes = json.data.items || [];
        filterBaiXes();
    } catch (err) {
        console.error("Lỗi tải bãi xe:", err);
    }
}

function filterBaiXes() {
    const tenQuery = (document.getElementById('filterTenBai')?.value || '').toLowerCase().trim();
    const diaChiQuery = (document.getElementById('filterDiaChi')?.value || '').toLowerCase().trim();

    const filtered = allBaiXes.filter(b => {
        const name = (b.name || b.tenBai || '').toLowerCase();
        const address = (b.fullAddress || b.diaChiDayDu || '').toLowerCase();
        const matchTen = !tenQuery || name.includes(tenQuery);
        const matchDiaChi = !diaChiQuery || address.includes(diaChiQuery);
        return matchTen && matchDiaChi;
    });

    renderBaiXeTable(filtered);
}

function resetFilterBaiXes() {
    if (document.getElementById('filterTenBai')) document.getElementById('filterTenBai').value = '';
    if (document.getElementById('filterDiaChi')) document.getElementById('filterDiaChi').value = '';
    renderBaiXeTable(allBaiXes);
}

function renderBaiXeTable(items) {
    const tbody = document.getElementById('baiXeTable');
    if (!tbody) return;
    tbody.innerHTML = '';

    if (items.length === 0) {
        tbody.innerHTML = `<tr><td colspan="8" class="text-center text-muted py-4">Không tìm thấy bãi đỗ xe phù hợp với bộ lọc.</td></tr>`;
        return;
    }

    const isQuanLy = currentUser && currentUser.role === 'QuanLy';
    const isDonVi = currentUser && currentUser.role === 'DonViKinhDoanh';

    items.forEach(b => {
        const tr = document.createElement('tr');
        const totalCapacity = b.totalCapacity ?? b.tongSucChua ?? 0;
        const availableSpots = b.availableSpots ?? b.soChoHienTaiConTrong ?? 0;
        const soXeDangGui = Math.max(0, totalCapacity - availableSpots);
        const name = b.name || b.tenBai || '-';
        const address = b.fullAddress || b.diaChiDayDu || '-';
        const openTime = b.openTime || b.gioMoCua || '00:00';
        const closeTime = b.closeTime || b.gioDongCua || '23:59';
        const isActive = b.isActive ?? b.dangHoatDong ?? true;

        let actionHtml = '';
        if (isQuanLy || isDonVi) {
            actionHtml = `
                <div class="d-flex justify-content-center gap-1">
                    <button class="btn btn-sm btn-outline-primary rounded-pill px-2.5" onclick="openEditBaiXeModal('${b.id}')" title="Sửa thông tin bãi">
                        <i class="fa-solid fa-pen-to-square me-1"></i> Sửa
                    </button>
                    ${isQuanLy ? `
                    <button class="btn btn-sm btn-outline-danger rounded-pill px-2.5" onclick="xoaBaiXe('${b.id}')" title="Xóa bãi xe">
                        <i class="fa-solid fa-trash me-1"></i> Xóa
                    </button>` : ''}
                </div>
            `;
        } else {
            actionHtml = `<span class="badge bg-light text-secondary border rounded-pill px-2.5">Chỉ xem</span>`;
        }

        tr.innerHTML = `
            <td class="fw-bold text-primary">${name}</td>
            <td class="small text-muted">${address}</td>
            <td><span class="badge bg-secondary bg-opacity-10 text-secondary border rounded-pill px-2.5">${totalCapacity} chỗ</span></td>
            <td><span class="badge ${availableSpots > 0 ? 'bg-success bg-opacity-10 text-success border border-success border-opacity-25' : 'bg-danger bg-opacity-10 text-danger border border-danger border-opacity-25'} rounded-pill px-2.5">${availableSpots} trống</span></td>
            <td><span class="badge bg-warning bg-opacity-15 text-dark border border-warning border-opacity-25 rounded-pill px-2.5">${soXeDangGui} xe</span></td>
            <td><span class="small text-secondary font-monospace"><i class="fa-regular fa-clock me-1 text-muted"></i>${openTime} - ${closeTime}</span></td>
            <td><span class="badge ${isActive ? 'badge-active' : 'badge-inactive'}">${isActive ? '● Đang hoạt động' : 'Tạm ngưng'}</span></td>
            <td class="text-center">${actionHtml}</td>
        `;
        tbody.appendChild(tr);
    });
}

function openEditBaiXeModal(id) {
    const bai = allBaiXes.find(b => b.id.toLowerCase() === id.toLowerCase());
    if (!bai) {
        showAlert("Không tìm thấy thông tin bãi xe!", "warning");
        return;
    }

    const addr = bai.address || {};
    document.getElementById('ebBaiXeId').value = bai.id;
    document.getElementById('ebTenBai').value = bai.name || bai.tenBai || '';
    document.getElementById('ebSoNha').value = addr.street || addr.soNhaTenDuong || '';
    document.getElementById('ebPhuongXa').value = addr.ward || addr.phuongXa || '';
    document.getElementById('ebQuanHuyen').value = addr.district || addr.quanHuyen || '';
    document.getElementById('ebTinhThanh').value = addr.city || addr.tinhThanh || 'TP. Hồ Chí Minh';
    document.getElementById('ebTongSucChua').value = bai.totalCapacity ?? bai.tongSucChua ?? 0;
    document.getElementById('ebGioMo').value = bai.openTime || bai.gioMoCua || '06:00';
    document.getElementById('ebGioDong').value = bai.closeTime || bai.gioDongCua || '23:00';
    document.getElementById('ebDangHoatDong').checked = bai.isActive ?? bai.dangHoatDong ?? true;

    new bootstrap.Modal(document.getElementById('editBaiXeModal')).show();
}

async function handleUpdateBaiXe(e) {
    if (e && e.preventDefault) e.preventDefault();
    const id = document.getElementById('ebBaiXeId').value;
    const body = {
        name: document.getElementById('ebTenBai').value.trim(),
        address: {
            street: document.getElementById('ebSoNha').value.trim(),
            ward: document.getElementById('ebPhuongXa').value.trim(),
            district: document.getElementById('ebQuanHuyen').value.trim(),
            city: document.getElementById('ebTinhThanh').value.trim()
        },
        totalCapacity: parseInt(document.getElementById('ebTongSucChua').value, 10),
        openTime: document.getElementById('ebGioMo').value.trim(),
        closeTime: document.getElementById('ebGioDong').value.trim(),
        isActive: document.getElementById('ebDangHoatDong').checked
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/parking-lots/${id}`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Cập nhật thông tin bãi đỗ xe thành công!", "success");
            const modalEl = document.getElementById('editBaiXeModal');
            if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadBaiXes();
        } else {
            showAlert(json.message || "Không thể cập nhật thông tin bãi xe!", "danger");
        }
    } catch (err) {
        showAlert("Lỗi kết nối máy chủ API: " + err.message, "danger");
    }
}

async function loadDonVis() {
    try {
        const res = await fetch(`${getApiUrl()}/api/business-operators`, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) return;

        allDonVis = json.data || [];

        const mbDonVi = document.getElementById('mbDonViSelect');
        if (mbDonVi) {
            mbDonVi.innerHTML = '';
            allDonVis.forEach(d => {
                const opt = document.createElement('option');
                opt.value = d.id;
                opt.textContent = d.name || d.tenDonVi;
                mbDonVi.appendChild(opt);
            });
        }
    } catch (err) {
        console.error("Lỗi tải đơn vị kinh doanh:", err);
    }
}

function openAddBaiXeModal() {
    if (!currentUser || currentUser.role !== 'QuanLy') {
        showAlert("Chỉ Quản lý hệ thống mới có quyền thêm mới bãi đỗ xe!", "warning");
        return;
    }
    new bootstrap.Modal(document.getElementById('addBaiXeModal')).show();
}

async function handleCreateBaiXe(e) {
    if (e && e.preventDefault) e.preventDefault();
    const body = {
        businessOperatorId: document.getElementById('mbDonViSelect').value,
        name: document.getElementById('mbTenBai').value.trim(),
        address: {
            street: document.getElementById('mbSoNha').value.trim(),
            ward: document.getElementById('mbPhuongXa').value.trim(),
            district: document.getElementById('mbQuanHuyen').value.trim(),
            city: document.getElementById('mbTinhThanh').value.trim()
        },
        totalCapacity: parseInt(document.getElementById('mbTongSucChua').value, 10),
        openTime: document.getElementById('mbGioMo').value.trim(),
        closeTime: document.getElementById('mbGioDong').value.trim(),
        isActive: true
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/parking-lots`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Thêm mới bãi đỗ xe thành công!", "success");
            const modalEl = document.getElementById('addBaiXeModal');
            if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadBaiXes();
        } else {
            showAlert(json.message || "Không thể thêm bãi xe!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối máy chủ API: " + err.message);
    }
}

async function xoaBaiXe(id) {
    if (!currentUser || currentUser.role !== 'QuanLy') {
        showAlert("Chỉ Quản lý hệ thống mới có quyền xóa bãi xe!", "warning");
        return;
    }
    if (!confirm("Bạn có chắc chắn muốn xóa bãi đỗ xe này không? Nếu bãi còn xe đang gửi sẽ bị chặn xóa theo quy định.")) return;
    try {
        const res = await fetch(`${getApiUrl()}/api/parking-lots/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Xóa bãi đỗ xe thành công!", "success");
            await loadBaiXes();
        } else {
            showAlert(json.message || "Không thể xóa bãi xe!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối đến máy chủ: " + err.message);
    }
}

async function refreshCurrentPage() {
    await initPage();
}

document.addEventListener('DOMContentLoaded', initPage);
