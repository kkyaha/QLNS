// Main page authentication and login handling

document.addEventListener("DOMContentLoaded", function() {
    checkAuthentication();
    setupLoginForm();
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
        showLoginForm();
    }
}

/**
 * Hiển thị form đăng nhập
 */
function showLoginForm() {
    document.getElementById("loginContainer").classList.remove("hidden");
    document.getElementById("mainContainer").classList.add("hidden");
}

/**
 * Hiển thị nội dung chính
 */
function showMainContent() {
    document.getElementById("loginContainer").classList.add("hidden");
    document.getElementById("mainContainer").classList.remove("hidden");
    
    // Hiển thị thông tin user
    const user = getCurrentUser();
    if (user.tenDangNhap) {
        document.getElementById("usernameDisplay").textContent = user.tenDangNhap;
        document.getElementById("welcomeUsername").textContent = user.tenDangNhap;
    }
}

/**
 * Thiết lập form đăng nhập
 */
function setupLoginForm() {
    const form = document.getElementById("loginForm");
    const errorMessage = document.getElementById("errorMessage");
    const submitBtn = document.getElementById("submitBtn");
    const loading = document.getElementById("loading");
    
    if (!form) return;
    
    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const username = document.getElementById("username").value.trim();
        const password = document.getElementById("password").value;

        // Xóa thông báo lỗi cũ
        errorMessage.textContent = "";
        errorMessage.style.display = "none";

        // Validate input
        if (!username || !password) {
            errorMessage.textContent = "Vui lòng nhập đầy đủ thông tin";
            errorMessage.style.display = "block";
            return;
        }

        // Hiển thị loading và vô hiệu hóa nút submit
        submitBtn.disabled = true;
        loading.style.display = "block";

        // Thực hiện đăng nhập
        const result = await login(username, password);

        if (result.success) {
            // Đăng nhập thành công
            submitBtn.textContent = "Đăng nhập thành công!";
            
            // Chuyển sang hiển thị nội dung chính sau một chút delay
            setTimeout(() => {
                showMainContent();
                form.reset();
                submitBtn.textContent = "Đăng nhập";
                submitBtn.disabled = false;
                loading.style.display = "none";
            }, 500);
        } else {
            // Đăng nhập thất bại
            errorMessage.textContent = result.error;
            errorMessage.style.display = "block";
            
            // Ẩn loading và kích hoạt lại nút submit
            loading.style.display = "none";
            submitBtn.disabled = false;
        }
    });
}

/**
 * Xử lý đăng xuất
 */
function handleLogout() {
    if (confirm("Bạn có chắc chắn muốn đăng xuất?")) {
        logout();
    }
}


