// API base URL
const API_BASE = '/api/nhanvien';

// Load danh sách nhân viên khi trang được tải
document.addEventListener('DOMContentLoaded', function() {
    // Kiểm tra authentication trước
    if (!isAuthenticated()) {
        window.location.href = '/index.html';
        return;
    }
    
    loadNhanVien();
    
    // Xử lý form submit
    document.getElementById('nhanVienForm').addEventListener('submit', function(e) {
        e.preventDefault();
        saveNhanVien();
    });
});

// Load danh sách nhân viên
async function loadNhanVien() {
    try {
        const response = await fetch(API_BASE, {
            headers: getAuthHeaders()
        });
        
        if (response.ok) {
            const data = await response.json();
            displayNhanVien(data);
        } else if (response.status === 401) {
            // Token không hợp lệ, chuyển về trang đăng nhập
            logout();
            window.location.href = '/index.html';
        } else {
            console.error('Lỗi khi load nhân viên:', response.statusText);
        }
    } catch (error) {
        console.error('Lỗi:', error);
    }
}

// Hiển thị danh sách nhân viên
function displayNhanVien(nhanViens) {
    const tbody = document.getElementById('nhanVienTableBody');
    tbody.innerHTML = '';
    
    if (nhanViens.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center;">Chưa có nhân viên nào</td></tr>';
        return;
    }
    
    nhanViens.forEach(nv => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${nv.maNv || ''}</td>
            <td>${nv.hoTen || ''}</td>
            <td>${nv.ngaySinh ? new Date(nv.ngaySinh).toLocaleDateString('vi-VN') : ''}</td>
            <td>${nv.sdt || ''}</td>
            <td>${nv.email || ''}</td>
            <td>${nv.maPhongBan || ''}</td>
            <td>
                <div class="action-buttons">
                    <button class="btn btn-primary" onclick="editNhanVien('${nv.maNv}')">Sửa</button>
                    <button class="btn btn-danger" onclick="deleteNhanVien('${nv.maNv}')">Xóa</button>
                </div>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Lưu nhân viên (thêm mới hoặc cập nhật)
async function saveNhanVien() {
    const editMaNv = document.getElementById('editMaNv').value;
    
    const data = {
        maNv: document.getElementById('maNv').value,
        hoTen: document.getElementById('hoTen').value,
        ngaySinh: document.getElementById('ngaySinh').value || null,
        sdt: document.getElementById('sdt').value || null,
        email: document.getElementById('email').value || null,
        maPhongBan: document.getElementById('maPhongBan').value ? parseInt(document.getElementById('maPhongBan').value) : null
    };
    
    try {
        let response;
        if (editMaNv) {
            // Cập nhật
            response = await fetch(`${API_BASE}/${editMaNv}`, {
                method: 'PUT',
                headers: getAuthHeaders(),
                body: JSON.stringify(data)
            });
        } else {
            // Thêm mới
            response = await fetch(API_BASE, {
                method: 'POST',
                headers: getAuthHeaders(),
                body: JSON.stringify(data)
            });
        }
        
        if (response.ok) {
            alert(editMaNv ? 'Cập nhật nhân viên thành công!' : 'Thêm nhân viên thành công!');
            resetForm();
            loadNhanVien();
        } else if (response.status === 401) {
            logout();
            window.location.href = '/index.html';
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi lưu nhân viên');
    }
}

// Sửa nhân viên
async function editNhanVien(maNv) {
    try {
        const response = await fetch(`${API_BASE}/${maNv}`, {
            headers: getAuthHeaders()
        });
        
        if (response.ok) {
            const nv = await response.json();
            document.getElementById('editMaNv').value = nv.maNv;
            document.getElementById('maNv').value = nv.maNv;
            document.getElementById('hoTen').value = nv.hoTen || '';
            document.getElementById('ngaySinh').value = nv.ngaySinh ? nv.ngaySinh.split('T')[0] : '';
            document.getElementById('sdt').value = nv.sdt || '';
            document.getElementById('email').value = nv.email || '';
            document.getElementById('maPhongBan').value = nv.maPhongBan || '';
            
            document.getElementById('formTitle').textContent = 'Sửa nhân viên';
            document.getElementById('maNv').disabled = true;
            
            // Scroll to form
            document.querySelector('.card').scrollIntoView({ behavior: 'smooth' });
        } else if (response.status === 401) {
            logout();
            window.location.href = '/index.html';
        } else {
            alert('Không tìm thấy nhân viên');
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi tải thông tin nhân viên');
    }
}

// Xóa nhân viên
async function deleteNhanVien(maNv) {
    if (!confirm('Bạn có chắc chắn muốn xóa nhân viên này?')) {
        return;
    }
    
    try {
        const response = await fetch(`${API_BASE}/${maNv}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        
        if (response.ok) {
            alert('Xóa nhân viên thành công!');
            loadNhanVien();
        } else if (response.status === 401) {
            logout();
            window.location.href = '/index.html';
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi xóa nhân viên');
    }
}

// Reset form
function resetForm() {
    document.getElementById('nhanVienForm').reset();
    document.getElementById('editMaNv').value = '';
    document.getElementById('formTitle').textContent = 'Thêm nhân viên mới';
    document.getElementById('maNv').disabled = false;
}

