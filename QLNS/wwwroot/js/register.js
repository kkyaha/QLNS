// Sử dụng auth.js để kiểm tra authentication
document.addEventListener("DOMContentLoaded", function() {
    const form = document.getElementById("registerForm");
    const errorMessage = document.getElementById("errorMessage");
    const submitBtn = document.getElementById("submitBtn");
    const loading = document.getElementById("loading");
    
    // Kiểm tra nếu đã đăng nhập, chuyển hướng đến trang chủ
    if (isAuthenticated()) {
        window.location.href = "/index.html";
        return;
    }
    
    form.addEventListener("submit", async function (e) {
        // 1. Chặn hành vi load lại trang mặc định của HTML
        e.preventDefault();

        // 2. Lấy dữ liệu từ các ô input
        const username = document.getElementById("username").value.trim();
        const password = document.getElementById("password").value;
        const confirmPassword = document.getElementById("confirmPassword").value;

        // Xóa thông báo lỗi cũ
        errorMessage.textContent = "";
        errorMessage.style.display = "none";

        // 3. Kiểm tra phía Client (Validation)
        // Kiểm tra các trường bắt buộc
        if (!username || !password || !confirmPassword) {
            errorMessage.textContent = "Vui lòng nhập đầy đủ thông tin";
            errorMessage.style.display = "block";
            return;
        }

        // Kiểm tra độ dài tên đăng nhập
        if (username.length < 3) {
            errorMessage.textContent = "Tên đăng nhập phải có ít nhất 3 ký tự";
            errorMessage.style.display = "block";
            return;
        }

        if (username.length > 50) {
            errorMessage.textContent = "Tên đăng nhập không được vượt quá 50 ký tự";
            errorMessage.style.display = "block";
            return;
        }

        // Kiểm tra định dạng tên đăng nhập (chỉ cho phép chữ, số, dấu gạch dưới)
        const usernameRegex = /^[a-zA-Z0-9_]+$/;
        if (!usernameRegex.test(username)) {
            errorMessage.textContent = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới";
            errorMessage.style.display = "block";
            return;
        }

        // Kiểm tra mật khẩu nhập lại có khớp không
        if (password !== confirmPassword) {
            errorMessage.textContent = "Mật khẩu nhập lại không khớp!";
            errorMessage.style.display = "block";
            return;
        }

        // Kiểm tra độ dài mật khẩu
        if (password.length < 6) {
            errorMessage.textContent = "Mật khẩu phải có ít nhất 6 ký tự!";
            errorMessage.style.display = "block";
            return;
        }

        if (password.length > 100) {
            errorMessage.textContent = "Mật khẩu không được vượt quá 100 ký tự";
            errorMessage.style.display = "block";
            return;
        }

        // Hiển thị loading và vô hiệu hóa nút submit
        submitBtn.disabled = true;
        loading.style.display = "block";

        // 4. Gửi dữ liệu lên Server (.NET API)
        try {
            const response = await fetch("/api/login/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                // Dữ liệu gửi đi phải khớp với LoginDTO trong C#
                body: JSON.stringify({
                    tenDangNhap: username,
                    matKhau: password
                })
            });

            // 5. Xử lý kết quả trả về
            if (response.ok) {
                // Trường hợp thành công (HTTP 200)
                submitBtn.textContent = "Đăng ký thành công!";
                
                // Chuyển hướng đến trang đăng nhập sau một chút delay
                setTimeout(() => {
                    window.location.href = "/pages/login.html";
                }, 1000);
            } else {
                // Trường hợp thất bại (HTTP 400, 500...)
                // Đọc tin nhắn lỗi từ server
                let errorText = "Đăng ký thất bại. Vui lòng thử lại sau.";
                try {
                    const error = await response.text();
                    if (error) {
                        errorText = error;
                    }
                } catch (parseError) {
                    console.error("Lỗi khi đọc phản hồi:", parseError);
                }
                
                errorMessage.textContent = errorText;
                errorMessage.style.display = "block";
                
                // Ẩn loading và kích hoạt lại nút submit
                loading.style.display = "none";
                submitBtn.disabled = false;
            }
        } catch (error) {
            // Trường hợp lỗi mạng hoặc server sập
            console.error("Lỗi:", error);
            errorMessage.textContent = "Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối mạng và thử lại.";
            errorMessage.style.display = "block";
            
            // Ẩn loading và kích hoạt lại nút submit
            loading.style.display = "none";
            submitBtn.disabled = false;
        }
    });
});