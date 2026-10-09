/**
 * SMART PARKING - CHECK-IN & CHECK-OUT PAGE SCRIPT
 * Quản lý nghiệp vụ cho xe vào, xe ra, tính toán cước phí và in hóa đơn.
 */

async function initPage() {
    await Promise.all([
        loadBaiXes(),
        loadLoaiXes(),
        loadBangGiaDichVu(),
        loadDangGui()
    ]);
    updateCheckInPrice();
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

        const selects = [
            document.getElementById('ciBaiXeSelect'),
            document.getElementById('coBaiXeSelect')
        ];

        selects.forEach(sel => {
            if (!sel) return;
            sel.innerHTML = '';
            allBaiXes.forEach(b => {
                const opt = document.createElement('option');
                opt.value = b.id;
                const name = b.name || b.tenBai;
                const avail = b.availableSpots ?? b.soChoHienTaiConTrong ?? 0;
                const total = b.totalCapacity ?? b.tongSucChua ?? 0;
                opt.textContent = `${name} (Trống: ${avail}/${total})`;
                sel.appendChild(opt);
            });
        });
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
        const sel = document.getElementById('ciLoaiXeSelect');
        if (sel) {
            sel.innerHTML = '';
            allLoaiXes.forEach(lx => {
                const opt = document.createElement('option');
                opt.value = lx.id;
                opt.textContent = lx.name || lx.tenLoaiXe;
                sel.appendChild(opt);
            });
        }
    } catch (err) {
        console.error("Lỗi tải loại xe:", err);
    }
}

async function loadBangGiaDichVu() {
    try {
        const res = await fetch(`${getApiUrl()}/api/tariffs`, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) return;

        allBangGias = json.data || [];
    } catch (err) {
        console.error("Lỗi tải bảng giá dịch vụ:", err);
    }
}

function updateCheckInPrice() {
    const baiId = document.getElementById('ciBaiXeSelect')?.value;
    const loaiXeId = document.getElementById('ciLoaiXeSelect')?.value;
    const ticketSelect = document.getElementById('ciGiaVeSelect');
    const badge = document.getElementById('ciTicketBadge');

    if (!ticketSelect) return;
    ticketSelect.innerHTML = '';

    if (!baiId || !loaiXeId) {
        ticketSelect.innerHTML = '<option value="">-- Vui lòng chọn bãi và loại xe --</option>';
        if (badge) badge.innerText = '0 gói';
        onSelectGiaVeChange();
        return;
    }

    const matchedTickets = (allBangGias || []).filter(bg => {
        const bgLotId = bg.parkingLotId || bg.baiDoXeId;
        const bgVId = bg.vehicleTypeId || bg.loaiXeId;
        return bgLotId && bgLotId.toLowerCase() === baiId.toLowerCase() &&
               bgVId && bgVId.toLowerCase() === loaiXeId.toLowerCase();
    });

    matchedTickets.sort((a, b) => new Date(b.effectiveDate || b.ngayApDung) - new Date(a.effectiveDate || a.ngayApDung));

    if (matchedTickets.length > 0) {
        if (badge) badge.innerText = `${matchedTickets.length} gói vé`;
        matchedTickets.forEach((bg, idx) => {
            const pricingType = bg.pricingType || bg.hinhThucTinh;
            const hinhThuc = (pricingType === 'PerTurn' || pricingType === 'Luot') ? 'Vé Lượt' : ((pricingType === 'PerHour' || pricingType === 'Gio') ? 'Vé Giờ' : 'Vé Ngày');
            const amount = bg.price ? bg.price.amount : (bg.mucGia ? bg.mucGia.soTien : 0);
            const currency = bg.price ? bg.price.currency : (bg.mucGia ? bg.mucGia.donViTien : 'VND');
            const giaFormat = `${Number(amount).toLocaleString('vi-VN')} ${currency}`;
            const unit = (pricingType === 'PerTurn' || pricingType === 'Luot') ? 'Lượt' : ((pricingType === 'PerHour' || pricingType === 'Gio') ? 'Giờ' : 'Ngày');

            const opt = document.createElement('option');
            opt.value = bg.id;
            opt.textContent = `${hinhThuc} - ${giaFormat} / ${unit}${idx === 0 ? ' (Mới nhất)' : ''}`;
            ticketSelect.appendChild(opt);
        });
        ticketSelect.selectedIndex = 0;
    } else {
        if (badge) badge.innerText = 'Mặc định';
        const opt = document.createElement('option');
        opt.value = 'default';
        opt.textContent = 'Vé tiêu chuẩn - 5.000 VND / Lượt (Mặc định)';
        ticketSelect.appendChild(opt);
    }

    onSelectGiaVeChange();
}

function onSelectGiaVeChange() {
    const ticketSelect = document.getElementById('ciGiaVeSelect');
    const displayEl = document.getElementById('ciGiaTienDisplay');
    const noteEl = document.getElementById('ciGiaTienNote');
    if (!displayEl || !noteEl) return;

    const selectedTicketId = ticketSelect?.value;
    if (!selectedTicketId || selectedTicketId === 'default') {
        const baiSelect = document.getElementById('ciBaiXeSelect');
        const tenBai = baiSelect && baiSelect.selectedIndex >= 0 ? baiSelect.options[baiSelect.selectedIndex].text.split('(')[0].trim() : '';
        const loaiXeSelect = document.getElementById('ciLoaiXeSelect');
        const tenLoai = loaiXeSelect && loaiXeSelect.selectedIndex >= 0 ? loaiXeSelect.options[loaiXeSelect.selectedIndex].text : '';

        displayEl.innerText = '5.000 VND / Lượt';
        noteEl.innerText = `Biểu phí chuẩn mặc định: ${tenLoai || 'Phương tiện'} tại ${tenBai || 'bãi xe'}.`;
        return;
    }

    const ticket = (allBangGias || []).find(bg => bg.id && bg.id.toLowerCase() === selectedTicketId.toLowerCase());
    if (ticket) {
        const pricingType = ticket.pricingType || ticket.hinhThucTinh;
        const hinhThuc = (pricingType === 'PerTurn' || pricingType === 'Luot') ? 'Lượt' : ((pricingType === 'PerHour' || pricingType === 'Gio') ? 'Giờ' : 'Ngày');
        const amount = ticket.price ? ticket.price.amount : (ticket.mucGia ? ticket.mucGia.soTien : 0);
        const currency = ticket.price ? ticket.price.currency : (ticket.mucGia ? ticket.mucGia.donViTien : 'VND');
        const giaFormat = `${Number(amount).toLocaleString('vi-VN')} ${currency}`;
        const effDate = ticket.effectiveDate || ticket.ngayApDung;
        const lotName = ticket.parkingLotName || ticket.tenBaiXe || '';
        const vName = ticket.vehicleTypeName || ticket.tenLoaiXe || '';

        displayEl.innerText = `${giaFormat} / ${hinhThuc}`;
        noteEl.innerText = `Biểu phí chuẩn: ${vName} tại ${lotName} (Áp dụng từ ${new Date(effDate).toLocaleDateString('vi-VN')})`;
    }
}

function getPlateNumber(item) {
    if (!item) return '-';
    if (item.fullPlateNumber) return item.fullPlateNumber;
    if (item.fullLicensePlate) return item.fullLicensePlate;
    if (item.licensePlate) {
        if (item.licensePlate.fullPlateNumber) return item.licensePlate.fullPlateNumber;
        if (item.licensePlate.fullPlate) return item.licensePlate.fullPlate;
        if (item.licensePlate.provinceCode && item.licensePlate.plateNumber) {
            return `${item.licensePlate.provinceCode}-${item.licensePlate.plateNumber}`;
        }
    }
    if (item.bienSoDayDu) return item.bienSoDayDu;
    return '-';
}

async function handleCheckIn(e) {
    if (e && e.preventDefault) e.preventDefault();
    const body = {
        parkingLotId: document.getElementById('ciBaiXeSelect').value,
        vehicleTypeId: document.getElementById('ciLoaiXeSelect').value,
        provinceCode: document.getElementById('ciMaTinh').value.trim(),
        plateNumber: document.getElementById('ciSoXe').value.trim()
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/parking-sessions/check-in`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            const plate = getPlateNumber(json.data);
            const lotName = json.data?.parkingLotName || json.data?.tenBaiXe;
            showAlert(`Check-in thành công cho xe [${plate}] vào bãi [${lotName}]!`, "success");
            const soXeInput = document.getElementById('ciSoXe');
            if (soXeInput) soXeInput.value = '';
            await Promise.all([loadBaiXes(), loadDangGui()]);
        } else {
            showAlert(json.message || "Check-in thất bại!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối API máy chủ: " + err.message);
    }
}

async function handleCheckOut(e) {
    if (e && e.preventDefault) e.preventDefault();
    const body = {
        parkingLotId: document.getElementById('coBaiXeSelect').value,
        provinceCode: document.getElementById('coMaTinh').value.trim(),
        plateNumber: document.getElementById('coSoXe').value.trim()
    };

    try {
        const res = await fetch(`${getApiUrl()}/api/parking-sessions/check-out`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        const json = await res.json();
        if (res.ok) {
            displayReceipt(json.data);
            const plate = getPlateNumber(json.data);
            showAlert(`Check-out thành công cho xe [${plate}]!`, "success");
            await Promise.all([loadBaiXes(), loadDangGui()]);
        } else {
            showAlert(json.message || "Check-out thất bại!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối API máy chủ: " + err.message);
    }
}

async function quickCheckOut(sessionId) {
    if (!confirm("Xác nhận Check-out xe và tính tiền gửi?")) return;

    try {
        const res = await fetch(`${getApiUrl()}/api/parking-sessions/${sessionId}/check-out`, {
            method: 'POST',
            headers: getAuthHeaders()
        });
        const json = await res.json();
        if (res.ok) {
            displayReceipt(json.data);
            const plate = getPlateNumber(json.data);
            showAlert(`Đã check-out xe [${plate}]. Vui lòng thu tiền theo hóa đơn!`, "success");
            await Promise.all([loadBaiXes(), loadDangGui()]);
        } else {
            showAlert(json.message || "Check-out thất bại!");
        }
    } catch (err) {
        showAlert("Lỗi kết nối API máy chủ: " + err.message);
    }
}

function displayReceipt(data) {
    const box = document.getElementById('receiptBox');
    if (!box) return;
    box.classList.remove('d-none');

    const plate = getPlateNumber(data);
    const lotName = data.parkingLotName || data.tenBaiXe || '-';
    const checkIn = data.checkInTime || data.gioVao;
    const checkOut = data.checkOutTime || data.gioRa;
    const duration = data.durationHours ?? data.thoiGianGuiGio ?? '0';
    const feeAmount = data.fee ? data.fee.amount : (data.soTienThu ? data.soTienThu.soTien : 0);
    const feeCurrency = data.fee ? data.fee.currency : (data.soTienThu ? data.soTienThu.donViTien : 'VND');

    document.getElementById('rcBienSo').innerText = plate;
    document.getElementById('rcTenBai').innerText = lotName;
    document.getElementById('rcGioVao').innerText = checkIn ? new Date(checkIn).toLocaleString('vi-VN') : '-';
    document.getElementById('rcGioRa').innerText = checkOut ? new Date(checkOut).toLocaleString('vi-VN') : '-';
    document.getElementById('rcThoiGianGui').innerText = duration;
    document.getElementById('rcSoTien').innerText = `${Number(feeAmount).toLocaleString('vi-VN')} ${feeCurrency}`;
    box.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
}

async function loadDangGui() {
    try {
        let url = `${getApiUrl()}/api/parking-sessions?status=Active&pageSize=100`;
        const res = await fetch(url, { headers: getAuthHeaders() });
        const json = await res.json();
        if (!json.success) return;

        let items = json.data.items || [];

        if (currentUser && currentUser.role === 'DonViKinhDoanh' && allBaiXes.length > 0) {
            const myBaiIds = new Set(allBaiXes.map(b => b.id.toLowerCase()));
            items = items.filter(v => {
                const lotId = v.parkingLotId || v.baiDoXeId;
                return lotId && myBaiIds.has(lotId.toLowerCase());
            });
        }

        const tbody = document.getElementById('parkingVehiclesTable');
        if (!tbody) return;
        tbody.innerHTML = '';

        if (items.length === 0) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-muted py-4">Hiện không có xe nào đang đỗ tại bãi.</td></tr>`;
            return;
        }

        items.forEach(v => {
            const tr = document.createElement('tr');
            const plate = getPlateNumber(v);
            const vType = v.vehicleTypeName || v.tenLoaiXe;
            const lotName = v.parkingLotName || v.tenBaiXe;
            const checkIn = v.checkInTime || v.gioVao;

            tr.innerHTML = `
                <td><span class="plate-box">${plate}</span></td>
                <td><span class="badge bg-primary bg-opacity-10 text-primary border border-primary border-opacity-25 rounded-pill px-2.5">${vType}</span></td>
                <td class="fw-semibold text-dark">${lotName}</td>
                <td class="small font-monospace text-secondary"><i class="fa-regular fa-clock me-1 text-muted"></i>${checkIn ? new Date(checkIn).toLocaleString('vi-VN') : '-'}</td>
                <td><span class="badge bg-warning bg-opacity-15 text-dark border border-warning border-opacity-25 rounded-pill px-2.5"><i class="fa-solid fa-spinner fa-spin me-1 text-warning"></i>Đang gửi</span></td>
                <td class="text-center">
                    <button class="btn btn-sm btn-danger fw-bold rounded-pill px-3 shadow-sm" onclick="quickCheckOut('${v.id}')" title="Check-out xe này ngay lập tức">
                        <i class="fa-solid fa-right-from-bracket me-1"></i> Check out
                    </button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error("Lỗi tải danh sách xe đang gửi:", err);
    }
}

async function refreshCurrentPage() {
    await initPage();
}

document.addEventListener('DOMContentLoaded', initPage);
