const API_URL = "https://localhost:7158/api/todolist";
const NV_API_URL = "https://localhost:7158/api/NhanVien";

document.addEventListener("DOMContentLoaded", () => {
    checkLogin();
    loadTasks();
    loadEmployees(); // New: Load employees for dropdown

    // Set min date for due date picker to today
    const today = new Date().toISOString().split('T')[0];
    document.getElementById("newTaskDueDate").setAttribute('min', today);
});

async function loadEmployees() {
    // Check role first: Only Admin (1) or Manager (2) can see this
    if (!hasRole([1, 2])) {
        const select = document.getElementById("assigneeSelect");
        if (select) {
            select.closest('.add-task-form').querySelector('select').style.display = 'none';
            // Better: Hide the select, or just don't populate it and hide it
            select.style.display = 'none';
        }
        return;
    }

    const token = localStorage.getItem("token");
    try {
        const response = await fetch(NV_API_URL, {
            headers: { "Authorization": `Bearer ${token}` }
        });
        if (response.ok) {
            const employees = await response.json();
            const select = document.getElementById("assigneeSelect");
            employees.forEach(nv => {
                const option = document.createElement("option");
                option.value = nv.idNv;
                option.textContent = `${nv.hoTen} (${nv.maNv})`;
                select.appendChild(option);
            });
        }
    } catch (e) {
        console.error("Failed to load employees", e);
    }
}

async function loadTasks() {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`${API_URL}/my-tasks`, {
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("Không thể tải danh sách công việc");

        const tasks = await response.json();
        renderTasks(tasks);
        updateTaskCount(tasks.length);
    } catch (error) {
        console.error("Lỗi:", error);
        alert("Có lỗi khi tải danh sách công việc");
    }
}

function renderTasks(tasks) {
    const list = document.getElementById("taskList");
    list.innerHTML = "";

    tasks.forEach(task => {
        const isCompleted = task.trangThai === 2; // Assuming 2 is 'Completed'
        const li = document.createElement("li");
        li.className = `task-item ${isCompleted ? 'completed' : ''}`;

        const dueDate = task.hanHoanThanh ? new Date(task.hanHoanThanh).toLocaleDateString('vi-VN') : 'Không thời hạn';

        li.innerHTML = `
            <div class="task-info">
                <div class="task-content">${escapeHtml(task.noiDung)}</div>
                <div class="task-meta">
                    <span>📅 Hạn: ${dueDate}</span>
                    <span class="status-badge ${isCompleted ? 'done' : 'pending'}">
                        ${isCompleted ? 'Hoàn thành' : 'Đang thực hiện'}
                    </span>
                    ${task.nguoiGiao !== 'Tôi' ? `<span>👤 Giao bởi: ${task.nguoiGiao}</span>` : ''}
                </div>
            </div>
            <div class="task-actions">
                <button class="btn-check" onclick="toggleTaskStatus(${task.toDoId}, ${task.trangThai})" title="${isCompleted ? 'Đánh dấu chưa xong' : 'Đánh dấu hoàn thành'}">
                    ${isCompleted ? '↩️' : '✓'}
                </button>
                <button class="btn-delete" onclick="deleteTask(${task.toDoId})" title="Xóa">
                    🗑️
                </button>
            </div>
        `;
        list.appendChild(li);
    });
}

async function addTask() {
    const contentInput = document.getElementById("newTaskContent");
    const dueDateInput = document.getElementById("newTaskDueDate");
    const assigneeSelect = document.getElementById("assigneeSelect");

    const content = contentInput.value.trim();
    const dueDate = dueDateInput.value;
    const assigneeId = assigneeSelect.value;

    if (!content) return alert("Vui lòng nhập nội dung công việc");

    const token = localStorage.getItem("token");
    const payload = {
        noiDung: content,
        hanHoanThanh: dueDate || null,
        ghiChu: "",
        idNv: assigneeId ? parseInt(assigneeId) : null
    };

    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(payload)
        });

        if (!response.ok) throw new Error("Không thể thêm công việc");

        contentInput.value = "";
        dueDateInput.value = "";
        assigneeSelect.value = "";
        loadTasks();
    } catch (error) {
        console.error("Lỗi:", error);
        alert("Có lỗi khi thêm công việc");
    }
}

async function toggleTaskStatus(id, currentStatus) {
    const newStatus = currentStatus === 2 ? 1 : 2; // Toggle between 1 (New) and 2 (Done)
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(`${API_URL}/${id}/status`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({ trangThai: newStatus })
        });

        if (!response.ok) throw new Error("Không thể cập nhật trạng thái");
        loadTasks();
    } catch (error) {
        console.error("Lỗi:", error);
        alert("Có lỗi khi cập nhật trạng thái");
    }
}

async function deleteTask(id) {
    if (!confirm("Bạn có chắc muốn xóa công việc này?")) return;

    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: "DELETE",
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("Không thể xóa công việc");
        loadTasks();
    } catch (error) {
        console.error("Lỗi:", error);
        alert("Có lỗi khi xóa công việc");
    }
}

function updateTaskCount(count) {
    document.getElementById("taskCount").textContent = `${count} công việc`;
}

function escapeHtml(text) {
    if (!text) return "";
    return text
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}
