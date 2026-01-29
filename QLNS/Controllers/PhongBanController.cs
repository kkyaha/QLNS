using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNS.Models;

namespace QLNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongBanController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public PhongBanController(MyDbContext dbContext)
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
        public IActionResult GetAllPhongBan()
        {
            var list = dbContext.PhongBans
                        .Select(pb => new 
                        {
                            pb.MaPhongBan,
                            pb.TenPhong,
                            pb.SoLuongNv,
                            pb.MaTruongPhong,
                            NhanViens = pb.NhanViens.Select(nv => new { nv.MaNv, nv.HoTen }).ToList()
                        })
                        .ToList();
            return Ok(list);
        }

        [HttpGet("{maPhongBan}")]
        public IActionResult GetPhongBanById(int maPhongBan)
        {
            var phongBan = dbContext.PhongBans
                            .Where(pb => pb.MaPhongBan == maPhongBan)
                            .Select(pb => new 
                            {
                                pb.MaPhongBan,
                                pb.TenPhong,
                                pb.SoLuongNv,
                                pb.MaTruongPhong,
                                NhanViens = pb.NhanViens.Select(nv => new { nv.MaNv, nv.HoTen }).ToList()
                            })
                            .FirstOrDefault();
            if (phongBan == null) return NotFound("Phòng ban không tồn tại");
            return Ok(phongBan);
        }

        [HttpPost]
        public IActionResult AddPhongBan([FromBody] PhongBan phongBan)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền thêm phòng ban");

            if (phongBan == null) return BadRequest("Dữ liệu không hợp lệ");
            dbContext.PhongBans.Add(phongBan);
            dbContext.SaveChanges();
            return Ok(phongBan);
        }

        [HttpPut("{maPhongBan}")]
        public IActionResult UpdatePhongBan(int maPhongBan, [FromBody] PhongBan update)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền sửa phòng ban");

            var pb = dbContext.PhongBans.FirstOrDefault(x => x.MaPhongBan == maPhongBan);
            if (pb == null) return NotFound("Phòng ban không tồn tại");

            pb.TenPhong = update.TenPhong;
            pb.SoLuongNv = update.SoLuongNv;
            pb.MaTruongPhong = update.MaTruongPhong;

            dbContext.SaveChanges();
            return Ok(pb);
        }

        [HttpDelete("{maPhongBan}")]
        public IActionResult DeletePhongBan(int maPhongBan)
        {
            if (!IsAdmin()) return Forbid("Chỉ Admin mới có quyền xóa phòng ban");

            var pb = dbContext.PhongBans.FirstOrDefault(x => x.MaPhongBan == maPhongBan);
            if (pb == null) return NotFound("Phòng ban không tồn tại");

            dbContext.PhongBans.Remove(pb);
            dbContext.SaveChanges();
            return Ok("Xóa phòng ban thành công");
        }
    }
}
