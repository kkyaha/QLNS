using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNS.DTOs;
using QLNS.Models;
using System.Security.Claims;

namespace QLNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly MyDbContext dbContext;

        public ToDoListController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Helper to get current user ID from JWT
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        [HttpGet("my-tasks")]
        [Authorize]
        public IActionResult GetMyTasks()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Không xác định được người dùng");

            var tasks = dbContext.ToDoLists
                .Include(t => t.TrangThaiNavigation)
                .Include(t => t.MaNguoiGiaoNavigation)
                .Where(t => t.IdNv == userId)
                .OrderByDescending(t => t.NgayTao)
                .Select(t => new
                {
                    t.ToDoId,
                    t.NoiDung,
                    t.NgayTao,
                    t.HanHoanThanh,
                    t.GhiChu,
                    t.TrangThai,
                    TenTrangThai = t.TrangThaiNavigation != null ? t.TrangThaiNavigation.TenTrangThai : "",
                    NguoiGiao = t.MaNguoiGiaoNavigation != null ? t.MaNguoiGiaoNavigation.HoTen : "Tôi"
                })
                .ToList();

            return Ok(tasks);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddTask([FromBody] ToDoListDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Không xác định được người dùng");

            // If IdNv is not provided, assign to self
            var targetUserId = dto.IdNv ?? userId;

            // If assigning to another user, check permissions
            if (targetUserId != userId)
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role);
                var role = roleClaim?.Value;

                if (role == "1")
                {
                    // Admin can assign
                }
                else
                {
                    // Non-admin (Role 2) needs to be Manager/TruongPhong
                    bool isTruongPhong = dbContext.PhongBans.Any(pb => pb.MaTruongPhong == userId);
                     var chucVu = dbContext.ChucVus.FirstOrDefault(cv => cv.IdNv == userId);
                    bool isQuanLy = chucVu != null && !string.IsNullOrEmpty(chucVu.TenChucVu) && 
                                    (chucVu.TenChucVu.ToLower().Contains("quản lý") || 
                                     chucVu.TenChucVu.ToLower().Contains("trưởng phòng"));
                    
                    if (!isTruongPhong && !isQuanLy)
                    {
                        return Forbid("Bạn không có quyền giao công việc cho người khác");
                    }
                }
            }

            var newTask = new ToDoList
            {
                IdNv = targetUserId,
                MaNguoiGiao = userId, // Creator is the assigner
                NoiDung = dto.NoiDung,
                NgayTao = DateOnly.FromDateTime(DateTime.Now),
                HanHoanThanh = dto.HanHoanThanh,
                GhiChu = dto.GhiChu,
                TrangThai = 1 // Default: Mới tạo / Chưa hoàn thành
            };

            dbContext.ToDoLists.Add(newTask);
            dbContext.SaveChanges();

            return Ok(newTask);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateTask(int id, [FromBody] ToDoListDTO dto)
        {
            var userId = GetCurrentUserId();
            var task = dbContext.ToDoLists.Find(id);

            if (task == null) return NotFound("Công việc không tồn tại");

            // Only allow owner or assigner to update
            if (task.IdNv != userId && task.MaNguoiGiao != userId)
                return Forbid("Bạn không có quyền chỉnh sửa công việc này");

            task.NoiDung = dto.NoiDung ?? task.NoiDung;
            task.HanHoanThanh = dto.HanHoanThanh ?? task.HanHoanThanh;
            task.GhiChu = dto.GhiChu ?? task.GhiChu;
            if (dto.TrangThai.HasValue) task.TrangThai = dto.TrangThai.Value;

            dbContext.SaveChanges();
            return Ok(task);
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateToDoStatusDTO dto)
        {
            var userId = GetCurrentUserId();
            var task = dbContext.ToDoLists.Find(id);

            if (task == null) return NotFound("Công việc không tồn tại");
            
            if (task.IdNv != userId && task.MaNguoiGiao != userId)
                return Forbid();

            task.TrangThai = dto.TrangThai;
            dbContext.SaveChanges();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTask(int id)
        {
            var userId = GetCurrentUserId();
            var task = dbContext.ToDoLists.Find(id);

            if (task == null) return NotFound("Công việc không tồn tại");

             if (task.IdNv != userId && task.MaNguoiGiao != userId)
                return Forbid("Bạn không có quyền xóa công việc này");

            dbContext.ToDoLists.Remove(task);
            dbContext.SaveChanges();
            return Ok("Xóa thành công");
        }
    }
}
