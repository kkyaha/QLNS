using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNS.Models;

namespace QLNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucVuController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public ChucVuController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{idNv}")]
        public IActionResult GetChucVuByNhanVien(int idNv)
        {
            var cv = dbContext.ChucVus
                        .Include(c => c.MaPhongBanNavigation)
                        .FirstOrDefault(c => c.IdNv == idNv);

            if (cv == null) return NotFound("Chức vụ không tồn tại");
            return Ok(cv);
        }

        [HttpPost]
        public IActionResult AddChucVu([FromBody] ChucVu chucVu)
        {
            if (chucVu == null) return BadRequest("Dữ liệu không hợp lệ");

            dbContext.ChucVus.Add(chucVu);
            dbContext.SaveChanges();
            return Ok(chucVu);
        }

        [HttpPut("{idNv}")]
        public IActionResult UpdateChucVu(int idNv, [FromBody] ChucVu update)
        {
            var cv = dbContext.ChucVus.FirstOrDefault(c => c.IdNv == idNv);
            if (cv == null) return NotFound("Chức vụ không tồn tại");

            cv.TenChucVu = update.TenChucVu;
            cv.MaPhongBan = update.MaPhongBan;

            dbContext.SaveChanges();
            return Ok(cv);
        }

        [HttpDelete("{idNv}")]
        public IActionResult DeleteChucVu(int idNv)
        {
            var cv = dbContext.ChucVus.FirstOrDefault(c => c.IdNv == idNv);
            if (cv == null) return NotFound("Chức vụ không tồn tại");

            dbContext.ChucVus.Remove(cv);
            dbContext.SaveChanges();
            return Ok("Xóa chức vụ thành công");
        }
    }
}
