using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNS.DTOs;
using QLNS.Models;

namespace QLNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamCongController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public ChamCongController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ChamCongController is working");
        }


        [HttpPost("checkin")]
        public IActionResult CheckIn([FromBody] ChamCongDTO dto)
        {
            var nhanVien = dbContext.NhanViens.FirstOrDefault(nv => nv.MaNv == dto.MaNv);
            if (nhanVien == null)
                return NotFound("Không tìm thấy nhân viên");

            var today = DateTime.Today;
            var homNay = DateOnly.FromDateTime(DateTime.Today);
            var existingChamCong = dbContext.ChamCongs
                .FirstOrDefault(c => c.IdNv == nhanVien.IdNv && c.CheckIn.Date == today);
            if (existingChamCong != null)
                return BadRequest("Nhân viên đã check-in hôm nay");

            var chamCong = new ChamCong
            {
                IdNv = nhanVien.IdNv,
                NgayChamCong = homNay,
                CheckIn = DateTime.Now,
                GhiChu = "Check-in"
            };

            dbContext.ChamCongs.Add(chamCong);
            dbContext.SaveChanges();

            return Ok(chamCong);
        }
        
        [HttpPost("checkout")]
        public IActionResult CheckOut([FromBody] ChamCongDTO dto)
        {
            var nhanVien = dbContext.NhanViens.FirstOrDefault(nv => nv.MaNv == dto.MaNv);
            if (nhanVien == null)
                return NotFound("Không tìm thấy nhân viên");

            var today = DateTime.Today;
            var chamCong = dbContext.ChamCongs
                .FirstOrDefault(c => c.IdNv == nhanVien.IdNv && c.CheckIn.Date == today);
            if (chamCong == null)
                return BadRequest("Nhân viên chưa check-in hôm nay");

            if (chamCong.CheckOut != null)
                return BadRequest("Nhân viên đã check-out rồi");

            chamCong.CheckOut = DateTime.Now;

            // Tính số giờ làm
            var duration = (chamCong.CheckOut.Value - chamCong.CheckIn).TotalHours;
            chamCong.SoGioLam = duration > 8 ? 8 : duration;
            chamCong.SoGioOt = duration > 8 ? duration - 8 : 0;
            chamCong.TrangThai = "Hoàn thành";

            dbContext.SaveChanges();

            return Ok(chamCong);
        }
    }
}
