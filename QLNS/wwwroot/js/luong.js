const API_URL = "https://localhost:7158/api/Luong";
const NV_API_URL = "https://localhost:7158/api/NhanVien";

document.addEventListener("DOMContentLoaded", () => {
    checkLogin();
    loadSalaryTable();
});

// Load and render salary table
async function loadSalaryTable() {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(API_URL, {
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("Không thể tải danh sách lương");

        const data = await response.json();
        renderTable(data);
    } catch (error) {
        console.error("Lỗi:", error);
        // alert("Có lỗi khi tải dữ liệu lương");
    }
}

function renderTable(data) {
    const tableBody = document.getElementById("salaryTableBody");
    if (!tableBody) return;

    tableBody.innerHTML = "";

    data.forEach(item => {
        const formattedBase = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.luongCoBan || 0);
        const formattedTotal = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.luongThucNhan || 0);
        const formattedOT = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.tongOt || 0);

        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${item.maLuong}</td>
            <td>${item.tenNhanVien || 'N/A'}</td>
            <td>${formattedBase}</td>
            <td>${item.soGioOt || 0}</td>
            <td>${item.heSoOt || 1.5}</td>
            <td>${formattedOT}</td>
            <td class="font-bold text-success">${formattedTotal}</td>
            <td>${item.trangThai || 'Mới'}</td>
            <td>
                <div class="action-buttons">
                    <button class="btn-edit" onclick="calculateSalary(${item.maLuong})" title="Tính lại lương">🔄 Tính</button>
                    <!-- <button class="btn-delete" onclick="deleteSalary(${item.maLuong})" title="Xóa">🗑️</button> -->
                </div>
            </td>
        `;
        tableBody.appendChild(tr);
    });
}

// Trigger backend calculation
async function calculateSalary(id) {
    if (!confirm("Bạn muốn tính lại lương cho nhân viên này dựa trên chấm công?")) return;

    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`${API_URL}/calculate/${id}`, {
            method: "POST",
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (response.ok) {
            alert("Tính lương thành công!");
            loadSalaryTable();
        } else {
            const err = await response.text();
            alert("Lỗi: " + err);
        }
    } catch (error) {
        console.error("Lỗi:", error);
        alert("Có lỗi xảy ra");
    }
}
