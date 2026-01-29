using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNS.DTOs;
using QLNS.Models;

namespace QLNS.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public NhanVienController(MyDbContext dbContext)
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
        public IActionResult GetAllNhanVien()
        {
            // Allow all authenticated users to view
            var allNhanVien = dbContext.NhanViens.ToList();
            return Ok(allNhanVien);
        }

        [HttpGet]
        [Route("{maNv}")]
        public IActionResult GetNhanVienById(string maNv)
        {
            var nhanVien = dbContext.NhanViens.FirstOrDefault(nv => nv.MaNv == maNv);
            if (nhanVien == null)
            {
                return NotFound("Nhân viên không tồn tại");
            }
            return Ok(nhanVien);
        }

        [HttpPost]
        public IActionResult AddNhanVien([FromBody] AddNhanVienDTO addNhanVienDTO)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền thêm nhân viên");

            if (addNhanVienDTO == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            // Validate MaPhongBan
            if (addNhanVienDTO.MaPhongBan.HasValue && !dbContext.PhongBans.Any(pb => pb.MaPhongBan == addNhanVienDTO.MaPhongBan))
            {
                return BadRequest("Phòng ban không tồn tại");
            }

            // Validate MaNv unique
            if (dbContext.NhanViens.Any(nv => nv.MaNv == addNhanVienDTO.MaNv))
            {
                return BadRequest("Mã nhân viên đã tồn tại");
            }

            var newNhanVien = new NhanVien
            {
                HoTen = addNhanVienDTO.HoTen,
                Sdt = addNhanVienDTO.Sdt,
                Email = addNhanVienDTO.Email,
                NgaySinh = addNhanVienDTO.NgaySinh,
                MaPhongBan = addNhanVienDTO.MaPhongBan,
                MaNv = addNhanVienDTO.MaNv
            };
            dbContext.NhanViens.Add(newNhanVien);
            dbContext.SaveChanges();
            return Ok(newNhanVien);           
        }

        [HttpPut]
        [Route("{maNv}")]
        public IActionResult UpdateNhanVien(string maNv, [FromBody] UpdateNhanVienDTO UpdateNhanVienDTO)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền sửa thông tin nhân viên");

            if (UpdateNhanVienDTO == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            var nhanVien = dbContext.NhanViens.FirstOrDefault(nv => nv.MaNv == maNv);
            if (nhanVien == null)
            {
                return NotFound("Nhân viên không tồn tại");
            }
            nhanVien.HoTen = UpdateNhanVienDTO.HoTen;
            nhanVien.Sdt = UpdateNhanVienDTO.Sdt;
            nhanVien.Email = UpdateNhanVienDTO.Email;
            nhanVien.NgaySinh = UpdateNhanVienDTO.NgaySinh;
            nhanVien.MaPhongBan = UpdateNhanVienDTO.MaPhongBan;
            dbContext.SaveChanges();
            return Ok(nhanVien);
        }

        [HttpDelete]
        [Route("{maNv}")]
        public IActionResult DeleteNhanVien(string maNv)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền xóa nhân viên");

            var nhanVien = dbContext.NhanViens.FirstOrDefault(nv => nv.MaNv == maNv);
            if (nhanVien == null)
            {
                return NotFound("Nhân viên không tồn tại");
            }
            dbContext.NhanViens.Remove(nhanVien);
            dbContext.SaveChanges();
            return Ok("Xóa nhân viên thành công");
        }

    }
}
