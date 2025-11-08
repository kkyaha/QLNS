// Auth utility functions for JWT handling

/**
 * Kiểm tra xem JWT token có hợp lệ không
 */
function isTokenValid(token) {
    if (!token) return false;
    
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const exp = payload.exp * 1000; // Convert to milliseconds
        return Date.now() < exp;
    } catch (error) {
        console.error("Lỗi khi kiểm tra token:", error);
        return false;
    }
}

/**
 * Lấy token từ localStorage và kiểm tra tính hợp lệ
 */
function getValidToken() {
    const token = localStorage.getItem("token");
    if (!token) return null;
    
    if (isTokenValid(token)) {
        return token;
    } else {
        // Token đã hết hạn, xóa nó
        localStorage.removeItem("token");
        localStorage.removeItem("userId");
        localStorage.removeItem("tenDangNhap");
        return null;
    }
}

/**
 * Kiểm tra xem user đã đăng nhập chưa
 */
function isAuthenticated() {
    return getValidToken() !== null;
}

/**
 * Đăng nhập
 */
async function login(username, password) {
    try {
        const response = await fetch("/api/login/login", {
            method: "POST",
            headers: { 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify({ 
                tenDangNhap: username.trim(), 
                matKhau: password 
            })
        });

        if (response.ok) {
            const data = await response.json();
            
            // Lưu thông tin vào localStorage
            if (data.token) {
                localStorage.setItem("token", data.token);
            }
            if (data.UserId) {
                localStorage.setItem("userId", data.UserId);
            }
            if (data.TenDangNhap) {
                localStorage.setItem("tenDangNhap", data.TenDangNhap);
            }
            
            return { success: true, data: data };
        } else {
            let errorText = "Sai tài khoản hoặc mật khẩu";
            try {
                const error = await response.text();
                if (error) {
                    errorText = error;
                }
            } catch (parseError) {
                console.error("Lỗi khi đọc phản hồi:", parseError);
            }
            
            return { success: false, error: errorText };
        }
    } catch (error) {
        console.error("Lỗi:", error);
        return { success: false, error: "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại sau." };
    }
}

/**
 * Đăng xuất
 */
function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("tenDangNhap");
    window.location.reload();
}

/**
 * Lấy thông tin user từ localStorage
 */
function getCurrentUser() {
    return {
        userId: localStorage.getItem("userId"),
        tenDangNhap: localStorage.getItem("tenDangNhap"),
        token: getValidToken()
    };
}

/**
 * Lấy Authorization header cho API calls
 */
function getAuthHeaders() {
    const token = getValidToken();
    if (!token) {
        return { "Content-Type": "application/json" };
    }
    return {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    };
}

