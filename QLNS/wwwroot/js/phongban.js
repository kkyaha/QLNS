// API base URL
const API_BASE = '/api/phongban';

// Load danh sách phòng ban khi trang được tải
document.addEventListener('DOMContentLoaded', function() {
    loadPhongBan();
    
    // Xử lý form submit
    document.getElementById('phongBanForm').addEventListener('submit', function(e) {
        e.preventDefault();
        savePhongBan();
    });
});

// Load danh sách phòng ban
async function loadPhongBan() {
    // 1. Lấy token từ localStorage
    const token = localStorage.getItem("token");

    // 2. (Quan trọng) Kiểm tra xem có token không
    /*
    if (!token) {
        alert("Bạn chưa đăng nhập!");
        window.location.href = "/login.html"; // Chuyển về trang login
        return;
    }
    */

    // 3. Gọi fetch với header "Authorization"
    try {
        const response = await fetch('/api/PhongBan', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                // 4. Đây là mấu chốt: Gửi token theo chuẩn "Bearer"
                'Authorization': 'Bearer ' + token
            }
        });

        if (response.ok) {
            const data = await response.json();
            console.log(data); // Dữ liệu phòng ban sẽ ở đây
            // ... (Code của bạn để hiển thị dữ liệu lên table) ...
        } else if (response.status === 401) {
            // Nếu token hết hạn hoặc không hợp lệ
            alert("Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.");
            window.location.href = "/login.html";
        } else {
            // Các lỗi khác (500, 404...)
            console.error("Lỗi khi tải phòng ban:", response.status);
        }

    } catch (error) {
        console.error("Lỗi kết nối:", error);
    }
}

// Hiển thị danh sách phòng ban
function displayPhongBan(phongBans) {
    const tbody = document.getElementById('phongBanTableBody');
    tbody.innerHTML = '';
    
    if (phongBans.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" style="text-align: center;">Chưa có phòng ban nào</td></tr>';
        return;
    }
    
    phongBans.forEach(pb => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${pb.maPhongBan}</td>
            <td>${pb.tenPhong || ''}</td>
            <td>${pb.soLuongNv || 0}</td>
            <td>${pb.maTruongPhong || ''}</td>
            <td>
                <div class="action-buttons">
                    <button class="btn btn-primary" onclick="editPhongBan(${pb.maPhongBan})">Sửa</button>
                    <button class="btn btn-danger" onclick="deletePhongBan(${pb.maPhongBan})">Xóa</button>
                </div>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Lưu phòng ban (thêm mới hoặc cập nhật)
async function savePhongBan() {
    const editMaPhongBan = document.getElementById('editMaPhongBan').value;
    const token = localStorage.getItem('token');
    
    const data = {
        tenPhong: document.getElementById('tenPhong').value,
        soLuongNv: document.getElementById('soLuongNv').value ? parseInt(document.getElementById('soLuongNv').value) : null,
        maTruongPhong: document.getElementById('maTruongPhong').value ? parseInt(document.getElementById('maTruongPhong').value) : null
    };
    
    try {
        let response;
        if (editMaPhongBan) {
            // Cập nhật
            response = await fetch(`${API_BASE}/${editMaPhongBan}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data)
            });
        } else {
            // Thêm mới
            response = await fetch(API_BASE, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data)
            });
        }
        
        if (response.ok) {
            alert(editMaPhongBan ? 'Cập nhật phòng ban thành công!' : 'Thêm phòng ban thành công!');
            resetForm();
            loadPhongBan();
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi lưu phòng ban');
    }
}

// Sửa phòng ban
async function editPhongBan(maPhongBan) {
    try {
        const token = localStorage.getItem('token');
        const response = await fetch(`${API_BASE}/${maPhongBan}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        
        if (response.ok) {
            const pb = await response.json();
            document.getElementById('editMaPhongBan').value = pb.maPhongBan;
            document.getElementById('tenPhong').value = pb.tenPhong || '';
            document.getElementById('soLuongNv').value = pb.soLuongNv || '';
            document.getElementById('maTruongPhong').value = pb.maTruongPhong || '';
            
            document.getElementById('formTitle').textContent = 'Sửa phòng ban';
            
            // Scroll to form
            document.querySelector('.card').scrollIntoView({ behavior: 'smooth' });
        } else {
            alert('Không tìm thấy phòng ban');
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi tải thông tin phòng ban');
    }
}

// Xóa phòng ban
async function deletePhongBan(maPhongBan) {
    if (!confirm('Bạn có chắc chắn muốn xóa phòng ban này?')) {
        return;
    }
    
    try {
        const token = localStorage.getItem('token');
        const response = await fetch(`${API_BASE}/${maPhongBan}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        
        if (response.ok) {
            alert('Xóa phòng ban thành công!');
            loadPhongBan();
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi xóa phòng ban');
    }
}

// Reset form
function resetForm() {
    document.getElementById('phongBanForm').reset();
    document.getElementById('editMaPhongBan').value = '';
    document.getElementById('formTitle').textContent = 'Thêm phòng ban mới';
}


