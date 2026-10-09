/**
 * SMART PARKING - DASHBOARD PAGE SCRIPT
 * Quản lý logic báo cáo thống kê, KPI lượt vào/ra, số xe gửi, doanh thu.
 */

function setDateRange(type) {
    const now = new Date();
    let past = new Date();
    const pad = (n) => String(n).padStart(2, '0');
    const formatLocal = (d) => `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;

    if (type === 'today') {
        past.setHours(0, 0, 0, 0);
    } else if (type === '7days') {
        past = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
    } else if (type === '30days') {
        past = new Date(Date.now() - 30 * 24 * 60 * 60 * 1000);
    } else if (type === 'all2months') {
        past = new Date(2026, 7, 1, 0, 0); // 01/08/2026
    }

    const tuNgayEl = document.getElementById('tkTuNgay');
    const denNgayEl = document.getElementById('tkDenNgay');
    if (tuNgayEl) tuNgayEl.value = formatLocal(past);
    if (denNgayEl) denNgayEl.value = formatLocal(now);
    
    xemThongKe();
}

async function initPage() {
    const guestCard = document.getElementById('guestRestrictedCard');
    const authContent = document.getElementById('dashboardAuthContent');

    // 1. KIỂM TRA QUYỀN HẠN: Khách vãng lai KHÔNG được phép xem báo cáo thống kê
    if (!currentUser || !currentUser.token) {
        if (guestCard) guestCard.classList.remove('d-none');
        if (authContent) authContent.classList.add('d-none');
        return;
    }

    // 2. Người dùng hợp lệ (Quản lý hoặc Đơn vị kinh doanh)
    if (guestCard) guestCard.classList.add('d-none');
    if (authContent) authContent.classList.remove('d-none');

    setDateRange('all2months');
    await loadBaiXes();
    if (allBaiXes && allBaiXes.length > 0) {
        await xemThongKe();
    }
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

        const sel = document.getElementById('tkBaiXeSelect');
        if (sel) {
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
        }

        const scopeNote = document.getElementById('tkScopeNote');
        if (scopeNote) {
            if (currentUser && currentUser.role === 'DonViKinhDoanh') {
                scopeNote.innerText = `Đang lọc: ${allBaiXes.length} bãi trực thuộc đơn vị của bạn.`;
            } else {
                scopeNote.innerText = `Đang hiển thị toàn bộ ${allBaiXes.length} bãi trong hệ thống.`;
            }
        }
    } catch (err) {
        console.error("Lỗi tải bãi xe:", err);
    }
}

async function xemThongKe() {
    const maBai = document.getElementById('tkBaiXeSelect')?.value;
    if (!maBai) return;

    const tuNgayInput = document.getElementById('tkTuNgay')?.value;
    const denNgayInput = document.getElementById('tkDenNgay')?.value;
    if (!tuNgayInput || !denNgayInput) {
        showAlert("Vui lòng chọn khoảng thời gian hợp lệ!");
        return;
    }

    const tuDate = new Date(tuNgayInput);
    const denDate = new Date(denNgayInput);
    if (tuDate > denDate) {
        showAlert("Thời gian 'Từ Ngày' không được lớn hơn 'Đến Ngày'!", "warning");
        return;
    }

    const tuNgay = tuDate.toISOString();
    const denNgay = denDate.toISOString();

    try {
        const res = await fetch(`${getApiUrl()}/api/reports/activity?parkingLotId=${maBai}&fromDate=${encodeURIComponent(tuNgay)}&toDate=${encodeURIComponent(denNgay)}`, {
            headers: getAuthHeaders()
        });
        const json = await res.json();

        if (!res.ok) {
            showAlert(json.message || "Khoảng thời gian không hợp lệ!");
            document.getElementById('thongKeResult')?.classList.add('d-none');
            return;
        }

        if (!json.data) {
            showAlert(json.message || "Không có dữ liệu trong khoảng thời gian này.", "warning");
            document.getElementById('thongKeResult')?.classList.add('d-none');
            return;
        }

        const d = json.data;
        const totalEntries = d.totalEntries ?? d.tongSoLuotXeVao ?? 0;
        const totalExits = d.totalExits ?? d.tongSoLuotXeRa ?? 0;
        const currentlyParked = d.currentlyParked ?? d.soXeHienDangGuiTrongBai ?? 0;
        const availableSpots = d.availableSpots ?? d.soChoConTrongHienTai ?? 0;
        let revAmount = 0;
        let revCurrency = d.currency || d.donViTien || 'VND';

        if (typeof d.totalRevenue === 'number' || typeof d.totalRevenue === 'string') {
            revAmount = d.totalRevenue;
        } else if (d.totalRevenue && typeof d.totalRevenue === 'object') {
            revAmount = d.totalRevenue.amount ?? 0;
            revCurrency = d.totalRevenue.currency || revCurrency;
        } else if (d.revenue && typeof d.revenue === 'object') {
            revAmount = d.revenue.amount ?? 0;
            revCurrency = d.revenue.currency || revCurrency;
        } else if (d.tongDoanhThu != null) {
            revAmount = d.tongDoanhThu;
        }

        const revNum = Number(revAmount) || 0;
        document.getElementById('statLuotVao').innerText = totalEntries;
        document.getElementById('statLuotRa').innerText = totalExits;
        document.getElementById('statXeDangGui').innerText = currentlyParked;
        document.getElementById('statChoTrong').innerText = availableSpots;
        document.getElementById('statDoanhThu').innerText = `${revNum.toLocaleString('vi-VN')} ${revCurrency}`;

        document.getElementById('thongKeResult')?.classList.remove('d-none');
        closeAlert();
    } catch (err) {
        showAlert("Lỗi khi tải báo cáo thống kê: " + err.message);
    }
}

async function refreshCurrentPage() {
    await initPage();
}

document.addEventListener('DOMContentLoaded', initPage);
