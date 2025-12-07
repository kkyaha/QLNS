// Main page authentication and login handling

document.addEventListener("DOMContentLoaded", function() {
    checkAuthentication();
});

/**
 * Kiểm tra trạng thái đăng nhập và hiển thị UI tương ứng
 */
function checkAuthentication() {
    if (isAuthenticated()) {
        // Đã đăng nhập - hiển thị nội dung chính
        showMainContent();
    } else {
        // Chưa đăng nhập - hiển thị form đăng nhập
        window.location.href = "/pages/login.html";
    }
}

/**
 * Hiển thị nội dung chính
 */
function showMainContent() {
    const mainContainer = document.getElementById("mainContainer");
    if (mainContainer) {
        mainContainer.classList.remove("hidden");
        
        // Hiển thị thông tin user
        const user = getCurrentUser();
        if (user.tenDangNhap) {
            const usernameDisplay = document.getElementById("usernameDisplay");
            const welcomeUsername = document.getElementById("welcomeUsername");
            
            if (usernameDisplay) {
                usernameDisplay.textContent = user.tenDangNhap;
            }
            if (welcomeUsername) {
                welcomeUsername.textContent = user.tenDangNhap;
            }
        }
    }
}


/**
 * Xử lý đăng xuất
 */
function handleLogout() {
    if (confirm("Bạn có chắc chắn muốn đăng xuất?")) {
        logout();
    }
}




