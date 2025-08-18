using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    }
}
