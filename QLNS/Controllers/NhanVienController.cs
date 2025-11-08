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

        [HttpGet]
        public IActionResult GetAllNhanVien()
        {
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
            if (addNhanVienDTO == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
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
