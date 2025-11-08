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

        [HttpGet]
        public IActionResult GetAllPhongBan()
        {
            var list = dbContext.PhongBans
                        .Include(pb => pb.NhanViens) // load kèm nhân viên
                        .ToList();
            return Ok(list);
        }

        [HttpGet("{maPhongBan}")]
        public IActionResult GetPhongBanById(int maPhongBan)
        {
            var phongBan = dbContext.PhongBans
                            .Include(pb => pb.NhanViens)
                            .FirstOrDefault(pb => pb.MaPhongBan == maPhongBan);
            if (phongBan == null) return NotFound("Phòng ban không tồn tại");
            return Ok(phongBan);
        }

        [HttpPost]
        public IActionResult AddPhongBan([FromBody] PhongBan phongBan)
        {
            if (phongBan == null) return BadRequest("Dữ liệu không hợp lệ");
            dbContext.PhongBans.Add(phongBan);
            dbContext.SaveChanges();
            return Ok(phongBan);
        }

        [HttpPut("{maPhongBan}")]
        public IActionResult UpdatePhongBan(int maPhongBan, [FromBody] PhongBan update)
        {
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
            var pb = dbContext.PhongBans.FirstOrDefault(x => x.MaPhongBan == maPhongBan);
            if (pb == null) return NotFound("Phòng ban không tồn tại");

            dbContext.PhongBans.Remove(pb);
            dbContext.SaveChanges();
            return Ok("Xóa phòng ban thành công");
        }
    }
}
