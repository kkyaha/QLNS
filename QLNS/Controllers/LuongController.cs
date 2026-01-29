using Microsoft.AspNetCore.Mvc;
using QLNS.Models;
using QLNS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuongController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public LuongController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Helper to check if user is admin (MaVaiTro = 1)
        private bool IsAdmin()
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role);
            return roleClaim != null && roleClaim.Value == "1";
        }

        [HttpGet]
        public IActionResult GetAllLuong()
        {
            var luongs = dbContext.Luongs
                .Include(l => l.IdNvNavigation)
                .Include(l => l.TrangThaiNavigation)
                .Select(l => new
                {
                    l.MaLuong,
                    ExcludeIdNv = l.IdNv,
                    TenNhanVien = l.IdNvNavigation != null ? l.IdNvNavigation.HoTen : "",
                    l.LuongCoBan,
                    l.LuongThucNhan,
                    l.SoGioOt,
                    l.HeSoOt,
                    l.TongOt,
                    TrangThai = l.TrangThaiNavigation != null ? l.TrangThaiNavigation.TenTrangThai : ""
                })
                .ToList();
            return Ok(luongs);
        }

        [HttpGet("{id}")]
        public IActionResult GetLuongById(int id)
        {
            var luong = dbContext.Luongs
                .Include(l => l.IdNvNavigation)
                .FirstOrDefault(l => l.MaLuong == id);

            if (luong == null) return NotFound("Không tìm thấy bảng lương");

            return Ok(new
            {
                luong.MaLuong,
                luong.IdNv,
                TenNhanVien = luong.IdNvNavigation?.HoTen,
                luong.LuongCoBan,
                luong.LuongThucNhan,
                luong.SoGioOt,
                luong.HeSoOt,
                luong.TongOt,
                luong.TrangThai
            });
        }

        [HttpPost]
        public IActionResult AddLuong([FromBody] LuongDTO dto)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền thêm lương");

            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");

            var nhanVien = dbContext.NhanViens.Find(dto.IdNv);
            if (nhanVien == null) return BadRequest("Nhân viên không tồn tại");

            var newLuong = new Luong
            {
                IdNv = dto.IdNv,
                Thang = dto.Thang ?? DateTime.Now.Month,
                LuongCoBan = dto.LuongCoBan,
                HeSoOt = dto.HeSoOt ?? 1.5, // Default HS OT
                TrangThai = dto.TrangThai ?? 1, // Default status
                SoGioOt = 0,
                TongOt = 0,
                LuongThucNhan = dto.LuongCoBan // Init without OT
            };

            dbContext.Luongs.Add(newLuong);
            dbContext.SaveChanges();

            return Ok(newLuong);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLuong(int id, [FromBody] LuongDTO dto)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền sửa lương");

            var luong = dbContext.Luongs.Find(id);
            if (luong == null) return NotFound("Không tìm thấy bảng lương");

            luong.LuongCoBan = dto.LuongCoBan ?? luong.LuongCoBan;
            luong.HeSoOt = dto.HeSoOt ?? luong.HeSoOt;
            luong.TrangThai = dto.TrangThai ?? luong.TrangThai;
            luong.Thang = dto.Thang ?? luong.Thang;

            // Recalculate if needed (simplified logic)
            if (luong.SoGioOt.HasValue && luong.HeSoOt.HasValue && luong.LuongCoBan.HasValue)
            {
                double hourlyRate = (double)(luong.LuongCoBan.Value / 26 / 8); 
                luong.TongOt = hourlyRate * luong.SoGioOt.Value * luong.HeSoOt.Value;
                luong.LuongThucNhan = luong.LuongCoBan + (decimal)luong.TongOt;
            }

            dbContext.SaveChanges();
            return Ok(luong);
        }
        
        [HttpPost("calculate/{id}")]
        public IActionResult CalculateLuong(int id)
        {
             if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền tính lương");

             var luong = dbContext.Luongs.Find(id);
            if (luong == null) return NotFound("Không tìm thấy bảng lương");
            
            var nvId = luong.IdNv;
            if (nvId != null) {
                // Determine Month and Year
                int month = luong.Thang ?? DateTime.Now.Month;
                int year = DateTime.Now.Year; // Assuming current year since model lacks Year

                var totalOt = dbContext.ChamCongs
                    .Where(cc => cc.IdNv == nvId && cc.NgayChamCong.Month == month && cc.NgayChamCong.Year == year && cc.SoGioOt > 0) 
                    .Sum(cc => cc.SoGioOt) ?? 0;
                
                luong.SoGioOt = totalOt;
                
                // Formula: (Basic / 26 / 8) * OT_Hours * Coeff
                double hourlyRate = (double)(luong.LuongCoBan.GetValueOrDefault() / 26 / 8); 
                luong.TongOt = hourlyRate * totalOt * luong.HeSoOt.GetValueOrDefault(1.5);
                luong.LuongThucNhan = luong.LuongCoBan + (decimal)luong.TongOt;
                
                dbContext.SaveChanges();
            }
            
            return Ok(luong);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLuong(int id)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền xóa lương");

            var luong = dbContext.Luongs.Find(id);
            if (luong == null) return NotFound("Không tìm thấy");

            dbContext.Luongs.Remove(luong);
            dbContext.SaveChanges();
            return Ok("Xóa thành công");
        }
    }
}
