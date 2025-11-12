// API base URL
const API_BASE = '/api/chamcong';

// Check-in
async function checkIn() {
    const maNv = document.getElementById('maNvChamCong').value;
    
    if (!maNv) {
        alert('Vui lòng nhập mã nhân viên');
        return;
    }
    
    try {
        const token = localStorage.getItem('token');
        const response = await fetch(`${API_BASE}/checkin`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ maNv: maNv })
        });
        
        if (response.ok) {
            const data = await response.json();
            displayChamCongInfo(data, 'Check-in thành công!');
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi check-in');
    }
}

// Check-out
async function checkOut() {
    const maNv = document.getElementById('maNvChamCong').value;
    
    if (!maNv) {
        alert('Vui lòng nhập mã nhân viên');
        return;
    }
    
    try {
        const token = localStorage.getItem('token');
        const response = await fetch(`${API_BASE}/checkout`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ maNv: maNv })
        });
        
        if (response.ok) {
            const data = await response.json();
            displayChamCongInfo(data, 'Check-out thành công!');
        } else {
            const error = await response.text();
            alert('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Lỗi:', error);
        alert('Có lỗi xảy ra khi check-out');
    }
}

// Hiển thị thông tin chấm công
function displayChamCongInfo(data, message) {
    const infoBox = document.getElementById('chamCongInfo');
    
    const checkInTime = data.checkIn ? new Date(data.checkIn).toLocaleString('vi-VN') : 'Chưa có';
    const checkOutTime = data.checkOut ? new Date(data.checkOut).toLocaleString('vi-VN') : 'Chưa check-out';
    const soGioLam = data.soGioLam ? data.soGioLam.toFixed(2) : '0';
    const soGioOt = data.soGioOt ? data.soGioOt.toFixed(2) : '0';
    
    infoBox.innerHTML = `
        <div class="message success">
            <strong>${message}</strong>
        </div>
        <div style="margin-top: 15px;">
            <p><strong>ID Nhân viên:</strong> ${data.idNv}</p>
            <p><strong>Check-in:</strong> ${checkInTime}</p>
            <p><strong>Check-out:</strong> ${checkOutTime}</p>
            <p><strong>Số giờ làm:</strong> ${soGioLam} giờ</p>
            <p><strong>Số giờ OT:</strong> ${soGioOt} giờ</p>
            <p><strong>Trạng thái:</strong> ${data.trangThai || 'Đang làm việc'}</p>
            <p><strong>Ghi chú:</strong> ${data.ghiChu || ''}</p>
        </div>
    `;
}



