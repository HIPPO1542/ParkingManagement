/**
 * SMART PARKING - CORE API MODULE
 * Cung cấp cấu hình kết nối, header xác thực JWT và helper gọi API.
 */

const ApiConfig = {
    DEFAULT_API_URL: 'http://localhost:5097',
    STORAGE_KEY: 'parking_api_url'
};

/**
 * Lấy địa chỉ API server hiện tại (ưu tiên input trên UI, sau đó đến localStorage, cuối cùng là mặc định)
 */
function getApiUrl() {
    const input = document.getElementById('apiUrlInput');
    if (input && input.value.trim()) {
        return input.value.trim();
    }
    return localStorage.getItem(ApiConfig.STORAGE_KEY) || ApiConfig.DEFAULT_API_URL;
}

/**
 * Tạo HTTP Headers với Bearer JWT Token nếu người dùng đã đăng nhập
 */
function getAuthHeaders(extraHeaders = {}) {
    const headers = {
        'Content-Type': 'application/json',
        ...extraHeaders
    };
    if (typeof currentUser !== 'undefined' && currentUser && currentUser.token) {
        headers['Authorization'] = `Bearer ${currentUser.token}`;
    }
    return headers;
}

/**
 * Helper gọi API tiêu chuẩn
 */
async function apiFetch(endpoint, options = {}) {
    const baseUrl = getApiUrl();
    const url = endpoint.startsWith('http') ? endpoint : `${baseUrl}${endpoint}`;
    
    const defaultHeaders = getAuthHeaders();
    const config = {
        ...options,
        headers: {
            ...defaultHeaders,
            ...(options.headers || {})
        }
    };

    const response = await fetch(url, config);
    let data;
    try {
        data = await response.json();
    } catch (e) {
        data = null;
    }

    return {
        ok: response.ok,
        status: response.status,
        data: data
    };
}
