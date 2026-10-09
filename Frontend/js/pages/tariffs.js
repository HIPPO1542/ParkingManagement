/**
 * SMART PARKING - BẢNG GIÁ DỊCH VỤ PAGE SCRIPT
 * Quản lý danh sách biểu phí, bộ lọc đa năng, Thêm / Sửa / Xóa biểu phí dịch vụ.
 */

async function initPage() {
    await Promise.all([
        loadBaiXes(),
        loadLoaiXes()
    ]);
    await loadBangGiaDichVu();
    populateFilterOptions();
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

        // Cập nhật select thêm mới
        const sel = document.getElementById('mbgBaiXeSelect');
        if (sel) {
            sel.innerHTML = '';
            allBaiXes.forEach(b => {
                const opt = document.createElement('option');
                opt.value = b.id;
                opt.textContent = `${b.name || b.tenBai}`;
                sel.appendChild(opt);
            });
        }

        // Cập nhật select chỉnh sửa
        const esel = document.getElementById('ebgBaiXeSelect');
        if (esel) {
            esel.innerHTML = '';
            allBaiXes.forEach(b => {
                const opt = document.createElement('option');
                opt.value = b.id;
                opt.textContent = `${b.name || b.tenBai}`;
                esel.appendChild(opt);
            });
        }
    } catch (err) {
        console.error("Lỗi tải bãi xe:", err);
    }
}

async function loadLoaiXes() {
    try {
        const res = await fetch(`${getApiUrl()}/api/vehicle-types`, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) return;

        allLoaiXes = json.data || [];

        // Cập nhật select thêm mới
        const sel = document.getElementById('mbgLoaiXeSelect');
        if (sel) {
            sel.innerHTML = '';
            allLoaiXes.forEach(lx => {
                const opt = document.createElement('option');
                opt.value = lx.id;
                opt.textContent = lx.name || lx.tenLoaiXe;
                sel.appendChild(opt);
            });
        }

        // Cập nhật select chỉnh sửa
        const esel = document.getElementById('ebgLoaiXeSelect');
        if (esel) {
            esel.innerHTML = '';
            allLoaiXes.forEach(lx => {
                const opt = document.createElement('option');
                opt.value = lx.id;
                opt.textContent = lx.name || lx.tenLoaiXe;
                esel.appendChild(opt);
            });
        }
    } catch (err) {
        console.error("Lỗi tải loại xe:", err);
    }
}

function populateFilterOptions() {
    // Nạp bãi xe vào bộ lọc
    const fBaiXe = document.getElementById('filterBgBaiXe');
    if (fBaiXe) {
        fBaiXe.innerHTML = '<option value="">-- Tất cả bãi đỗ --</option>';
        allBaiXes.forEach(b => {
            const opt = document.createElement('option');
            const name = b.name || b.tenBai;
            opt.value = name.toLowerCase();
            opt.textContent = name;
            fBaiXe.appendChild(opt);
        });
    }

    // Nạp loại xe vào bộ lọc
    const fLoaiXe = document.getElementById('filterBgLoaiXe');
    if (fLoaiXe) {
        fLoaiXe.innerHTML = '<option value="">-- Tất cả loại xe --</option>';
        allLoaiXes.forEach(lx => {
            const opt = document.createElement('option');
            const name = lx.name || lx.tenLoaiXe;
            opt.value = name.toLowerCase();
            opt.textContent = name;
            fLoaiXe.appendChild(opt);
        });
    }
}

async function loadBangGiaDichVu() {
    try {
        const res = await fetch(`${getApiUrl()}/api/tariffs`, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) return;

        allBangGias = json.data || [];
        filterBangGias();
    } catch (err) {
        console.error("Lỗi tải bảng giá dịch vụ:", err);
    }
}

function filterBangGias() {
    const fBaiXe = (document.getElementById('filterBgBaiXe')?.value || '').toLowerCase().trim();
    const fLoaiXe = (document.getElementById('filterBgLoaiXe')?.value || '').toLowerCase().trim();
    const fMucGia = parseFloat(document.getElementById('filterBgMucGia')?.value);
    const fHinhThuc = (document.getElementById('filterBgHinhThuc')?.value || '').trim();
    const fNgay = document.getElementById('filterBgNgayApDung')?.value; // YYYY-MM-DD

    let baseList = allBangGias;
    if (currentUser && currentUser.role === 'DonViKinhDoanh' && allBaiXes.length > 0) {
        const myBaiIds = new Set(allBaiXes.map(b => b.id.toLowerCase()));
        baseList = allBangGias.filter(bg => {
            const lotId = bg.parkingLotId || bg.baiDoXeId;
            return lotId && myBaiIds.has(lotId.toLowerCase());
        });
    }

    const filtered = baseList.filter(bg => {
        const lotName = bg.parkingLotName || bg.tenBaiXe || '';
        const vName = bg.vehicleTypeName || bg.tenLoaiXe || '';
        const amount = bg.price ? bg.price.amount : (bg.mucGia ? bg.mucGia.soTien : 0);
        const pType = bg.pricingType || bg.hinhThucTinh || '';
        const effDate = bg.effectiveDate || bg.ngayApDung;

        if (fBaiXe && (!lotName || !lotName.toLowerCase().includes(fBaiXe))) {
            return false;
        }
        if (fLoaiXe && (!vName || !vName.toLowerCase().includes(fLoaiXe))) {
            return false;
        }
        if (!isNaN(fMucGia) && fMucGia > 0 && amount > fMucGia) {
            return false;
        }
        if (fHinhThuc) {
            const isMatch = (fHinhThuc === 'PerTurn' || fHinhThuc === 'Luot') ? (pType === 'PerTurn' || pType === 'Luot') :
                            (fHinhThuc === 'PerHour' || fHinhThuc === 'Gio') ? (pType === 'PerHour' || pType === 'Gio') :
                            (pType === 'PerDay' || pType === 'Ngay');
            if (!isMatch) return false;
        }
        if (fNgay && effDate) {
            const bgDate = new Date(effDate).toISOString().slice(0, 10);
            if (bgDate !== fNgay) return false;
        }
        return true;
    });

    renderBangGiaTable(filtered);
}

function resetFilterBangGia() {
    if (document.getElementById('filterBgBaiXe')) document.getElementById('filterBgBaiXe').value = '';
    if (document.getElementById('filterBgLoaiXe')) document.getElementById('filterBgLoaiXe').value = '';
    if (document.getElementById('filterBgMucGia')) document.getElementById('filterBgMucGia').value = '';
    if (document.getElementById('filterBgHinhThuc')) document.getElementById('filterBgHinhThuc').value = '';
    if (document.getElementById('filterBgNgayApDung')) document.getElementById('filterBgNgayApDung').value = '';
    filterBangGias();
}

function renderBangGiaTable(items) {
    const tbody = document.getElementById('bangGiaTable');
    if (!tbody) return;
    tbody.innerHTML = '';

    if (items.length === 0) {
        tbody.innerHTML = `<tr><td colspan="6" class="text-center text-muted py-4">Không tìm thấy biểu phí dịch vụ phù hợp với bộ lọc.</td></tr>`;
        return;
    }

    items.forEach(bg => {
        const tr = document.createElement('tr');
        const pType = bg.pricingType || bg.hinhThucTinh;
        const hinhThucText = (pType === 'PerTurn' || pType === 'Luot') ? 'Theo lượt xe' : ((pType === 'PerHour' || pType === 'Gio') ? 'Theo giờ' : 'Theo ngày');
        const amount = bg.price ? bg.price.amount : (bg.mucGia ? bg.mucGia.soTien : 0);
        const currency = bg.price ? bg.price.currency : (bg.mucGia ? bg.mucGia.donViTien : 'VND');
        const effDate = bg.effectiveDate || bg.ngayApDung;

        tr.innerHTML = `
            <td class="fw-bold text-dark">${bg.parkingLotName || bg.tenBaiXe}</td>
            <td><span class="badge bg-secondary bg-opacity-10 text-secondary border rounded-pill px-2.5">${bg.vehicleTypeName || bg.tenLoaiXe}</span></td>
            <td class="fw-bold text-success fs-6 font-monospace">${Number(amount).toLocaleString('vi-VN')} ${currency}</td>
            <td><span class="badge bg-light text-dark border rounded-pill px-2.5">${hinhThucText}</span></td>
            <td class="small text-muted font-monospace">${effDate ? new Date(effDate).toLocaleDateString('vi-VN') : '-'}</td>
            <td class="text-center">
                <div class="d-flex justify-content-center gap-1">
                    <button class="btn btn-sm btn-outline-primary rounded-pill px-2.5" onclick="openEditBangGiaModal('${bg.id}')" title="Sửa biểu phí">
                        <i class="fa-solid fa-pen-to-square me-1"></i> Sửa
                    </button>
                    <button class="btn btn-sm btn-outline-danger rounded-pill px-2.5" onclick="xoaBangGia('${bg.id}')" title="Xóa biểu phí">
                        <i class="fa-solid fa-trash me-1"></i> Xóa
                    </button>
                </div>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

function openAddBangGiaModal() {
    new bootstrap.Modal(document.getElementById('addBangGiaModal')).show();
}

function openEditBangGiaModal(id) {
    const bg = allBangGias.find(item => item.id.toLowerCase() === id.toLowerCase());
    if (!bg) {
        showAlert("Không tìm thấy thông tin biểu phí dịch vụ!", "warning");
        return;
    }

    const lotId = bg.parkingLotId || bg.baiDoXeId;
    const vTypeId = bg.vehicleTypeId || bg.loaiXeId;
    const amount = bg.price ? bg.price.amount : (bg.mucGia ? bg.mucGia.soTien : 0);
    const pType = bg.pricingType || bg.hinhThucTinh || 'PerTurn';
    const effDate = bg.effectiveDate || bg.ngayApDung;

    document.getElementById('ebgBangGiaId').value = bg.id;
    document.getElementById('ebgBaiXeSelect').value = lotId;
    document.getElementById('ebgLoaiXeSelect').value = vTypeId;
    document.getElementById('ebgDonGia').value = amount;
    
    // Convert Vietnamese select option value if needed
    const selectEl = document.getElementById('ebgHinhThuc');
    if (selectEl) {
        if (pType === 'Luot') selectEl.value = 'PerTurn';
        else if (pType === 'Gio') selectEl.value = 'PerHour';
        else if (pType === 'Ngay') selectEl.value = 'PerDay';
        else selectEl.value = pType;
    }

    if (effDate) {
        document.getElementById('ebgNgayApDung').value = new Date(effDate).toISOString().slice(0, 10);
    } else {
        document.getElementById('ebgNgayApDung').value = new Date().toISOString().slice(0, 10);
    }

    new bootstrap.Modal(document.getElementById('editBangGiaModal')).show();
}

function parsePricingType(val) {
    if (val === 'Luot' || val === 'PerTurn' || val === '0') return 'PerTurn';
    if (val === 'Gio' || val === 'PerHour' || val === '1') return 'PerHour';
    if (val === 'Ngay' || val === 'PerDay' || val === '2') return 'PerDay';
    return val || 'PerTurn';
}

async function handleCreateBangGia(e) {
    if (e && e.preventDefault) e.preventDefault();
    const body = {
        parkingLotId: document.getElementById('mbgBaiXeSelect').value,
        vehicleTypeId: document.getElementById('mbgLoaiXeSelect').value,
        price: {
            amount: parseFloat(document.getElementById('mbgDonGia').value),
            currency: "VND"
        },
        pricingType: parsePricingType(document.getElementById('mbgHinhThuc').value),
        effectiveDate: new Date().toISOString()
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/tariffs`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Thiết lập biểu phí dịch vụ thành công!", "success");
            const modalEl = document.getElementById('addBangGiaModal');
            if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadBangGiaDichVu();
        } else {
            showAlert(json.message || "Không thể thiết lập biểu phí!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối máy chủ API: " + err.message);
    }
}

async function handleUpdateBangGia(e) {
    if (e && e.preventDefault) e.preventDefault();
    const id = document.getElementById('ebgBangGiaId').value;
    const ngayValue = document.getElementById('ebgNgayApDung').value;

    const body = {
        parkingLotId: document.getElementById('ebgBaiXeSelect').value,
        vehicleTypeId: document.getElementById('ebgLoaiXeSelect').value,
        price: {
            amount: parseFloat(document.getElementById('ebgDonGia').value),
            currency: "VND"
        },
        pricingType: parsePricingType(document.getElementById('ebgHinhThuc').value),
        effectiveDate: ngayValue ? new Date(ngayValue).toISOString() : new Date().toISOString()
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/tariffs/${id}`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Cập nhật biểu phí dịch vụ thành công!", "success");
            const modalEl = document.getElementById('editBangGiaModal');
            if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadBangGiaDichVu();
        } else {
            showAlert(json.message || "Không thể cập nhật biểu phí!", "danger");
        }
    } catch (err) {
        showAlert("Lỗi kết nối máy chủ API: " + err.message, "danger");
    }
}

async function xoaBangGia(id) {
    if (!confirm("Bạn có chắc chắn muốn xóa biểu phí dịch vụ này không?")) return;
    try {
        const res = await fetch(`${getApiUrl()}/api/tariffs/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        const json = await res.json();
        if (res.ok) {
            showAlert("Xóa biểu phí dịch vụ thành công!", "success");
            await loadBangGiaDichVu();
        } else {
            showAlert(json.message || "Không thể xóa biểu phí!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối đến máy chủ: " + err.message);
    }
}

async function refreshCurrentPage() {
    await initPage();
}

document.addEventListener('DOMContentLoaded', initPage);
