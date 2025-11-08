// API base URL
const API_BASE = '/api/chucvu';

// Load form submit
document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('chucVuForm').addEventListener('submit', function(e) {
        e.preventDefault();
        saveChucVu();
    });
});

// Tìm kiếm chức vụ theo nhân viên
async function searchChucVu() {
    const idNv = document.getElementById('searchIdNv').value;
    
    if (!idNv) {
        alert('Vui lòng nhập ID nhân viên');
        return;
    }
    
    try {
        const token = localStorage.getItem('token');
        const response = await fetch(`${API_BASE}/${idNv}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        
        if (response.ok) {
            const data = await response.json();
            displayChucVuInfo(data);
            // Điền vào form để có thể sửa
            document.getElementById('editIdNv').value = data.idNv;
            document.getElementById('idNv').value = data.idNv;
            document.getElementById('tenChucVu').value = data.tenChucVu || '';
            document.getElementById('maPhongBanChucVu').value = data.maPhongBan || '';
            document.getElementById('formTitle').textContent = 'Sửa chức vụ';
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
            document.getElementById('chucVuInfo').innerHTML = '<p>Không tìm thấy chức vụ</p>';
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi tìm kiếm chức vụ');
    }
}

// Hiển thị thông tin chức vụ
function displayChucVuInfo(data) {
    const infoBox = document.getElementById('chucVuInfo');
    const actionsBox = document.getElementById('chucVuActions');
    infoBox.innerHTML = `
        <div class="message success">
            <strong>Tìm thấy chức vụ!</strong>
        </div>
        <div style="margin-top: 15px;">
            <p><strong>ID Nhân viên:</strong> ${data.idNv}</p>
            <p><strong>Tên chức vụ:</strong> ${data.tenChucVu || 'Chưa có'}</p>
            <p><strong>Mã phòng ban:</strong> ${data.maPhongBan || 'Chưa có'}</p>
            ${data.maPhongBanNavigation ? `<p><strong>Tên phòng ban:</strong> ${data.maPhongBanNavigation.tenPhong || ''}</p>` : ''}
        </div>
    `;
    actionsBox.style.display = 'block';
    actionsBox.setAttribute('data-idnv', data.idNv);
}

// Lưu chức vụ (thêm mới hoặc cập nhật)
async function saveChucVu() {
    const editIdNv = document.getElementById('editIdNv').value;
    const token = localStorage.getItem('token');
    
    const data = {
        idNv: parseInt(document.getElementById('idNv').value),
        tenChucVu: document.getElementById('tenChucVu').value || null,
        maPhongBan: document.getElementById('maPhongBanChucVu').value ? parseInt(document.getElementById('maPhongBanChucVu').value) : null
    };
    
    try {
        let response;
        if (editIdNv) {
            // Cập nhật
            response = await fetch(`${API_BASE}/${editIdNv}`, {
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
            alert(editIdNv ? 'Cập nhật chức vụ thành công!' : 'Thêm chức vụ thành công!');
            resetForm();
            if (editIdNv) {
                searchChucVu();
            }
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi lưu chức vụ');
    }
}

// Xóa chức vụ
async function deleteChucVu(idNv) {
    if (!confirm('Bạn có chắc chắn muốn xóa chức vụ này?')) {
        return;
    }
    
    try {
        const token = localStorage.getItem('token');
        const response = await fetch(`${API_BASE}/${idNv}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        
        if (response.ok) {
            alert('Xóa chức vụ thành công!');
            resetForm();
            document.getElementById('chucVuInfo').innerHTML = '<p>Vui lòng tìm kiếm chức vụ theo ID nhân viên</p>';
            document.getElementById('chucVuActions').style.display = 'none';
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi xóa chức vụ');
    }
}

// Xóa chức vụ theo ID từ button
function deleteChucVuById() {
    const actionsBox = document.getElementById('chucVuActions');
    const idNv = actionsBox.getAttribute('data-idnv');
    if (idNv) {
        deleteChucVu(parseInt(idNv));
    }
}

// Reset form
function resetForm() {
    document.getElementById('chucVuForm').reset();
    document.getElementById('editIdNv').value = '';
    document.getElementById('formTitle').textContent = 'Thêm chức vụ mới';
    document.getElementById('chucVuActions').style.display = 'none';
    document.getElementById('chucVuInfo').innerHTML = '<p>Vui lòng tìm kiếm chức vụ theo ID nhân viên</p>';
}

