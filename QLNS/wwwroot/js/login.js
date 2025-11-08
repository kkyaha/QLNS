// Sử dụng auth.js để xử lý đăng nhập
document.addEventListener("DOMContentLoaded", function() {
    const form = document.getElementById("loginForm");
    const errorMessage = document.getElementById("errorMessage");
    const submitBtn = document.getElementById("submitBtn");
    const loading = document.getElementById("loading");
    
    // Kiểm tra nếu đã đăng nhập, chuyển hướng đến trang chủ
    if (isAuthenticated()) {
        window.location.href = "/index.html";
        return;
    }
    
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

        // Sử dụng hàm login từ auth.js
        const result = await login(username, password);

        if (result.success) {
            // Đăng nhập thành công
            submitBtn.textContent = "Đăng nhập thành công!";
            
            // Chuyển hướng đến trang chủ sau một chút delay
            setTimeout(() => {
                window.location.href = "/index.html";
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
});
