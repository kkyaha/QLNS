using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLNS.DTOs;
using QLNS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QLNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        private readonly IConfiguration config;

        public LoginController(MyDbContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            this.config = config;
        }

        private string HashPassword(string Password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string Password, string storedHash)
        {
            var hash = HashPassword(Password);
            return hash == storedHash;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginDTO dto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ");

            // Trim và validate tên đăng nhập
            if (string.IsNullOrWhiteSpace(dto.TenDangNhap))
                return BadRequest("Tên đăng nhập không được để trống");

            dto.TenDangNhap = dto.TenDangNhap.Trim();

            if (dto.TenDangNhap.Length < 3)
                return BadRequest("Tên đăng nhập phải có ít nhất 3 ký tự");

            if (dto.TenDangNhap.Length > 50)
                return BadRequest("Tên đăng nhập không được vượt quá 50 ký tự");

            // Kiểm tra định dạng tên đăng nhập (chỉ cho phép chữ, số, dấu gạch dưới)
            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.TenDangNhap, @"^[a-zA-Z0-9_]+$"))
                return BadRequest("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới");

            // Validate mật khẩu
            if (string.IsNullOrWhiteSpace(dto.MatKhau))
                return BadRequest("Mật khẩu không được để trống");

            if (dto.MatKhau.Length < 6)
                return BadRequest("Mật khẩu phải có ít nhất 6 ký tự");

            if (dto.MatKhau.Length > 100)
                return BadRequest("Mật khẩu không được vượt quá 100 ký tự");

            // Kiểm tra tên đăng nhập đã tồn tại
            if (dbContext.TaiKhoans.Any(u => u.TenDangNhap == dto.TenDangNhap))
                return BadRequest("Tên đăng nhập đã tồn tại");

            // Tạo tài khoản mới
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = dto.TenDangNhap,
                MatKhau = HashPassword(dto.MatKhau),
            };

            try
            {
                dbContext.TaiKhoans.Add(taiKhoan);
                dbContext.SaveChanges();

                return Ok("Đăng ký thành công");
            }
            catch (Exception ex)
            {
                // Log lỗi (có thể thêm logging service ở đây)
                return StatusCode(500, "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại sau.");
            }
        }

        // 🔹 Đăng nhập + JWT
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var user = dbContext.TaiKhoans
                .Include(u => u.MaVaiTroNavigation)
                .FirstOrDefault(u => u.TenDangNhap == dto.TenDangNhap);

            if (user == null || !VerifyPassword(dto.MatKhau, user.MatKhau))
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            // Sinh JWT
            var token = GenerateJwtToken(user);

            // Calculate permissions
            bool canAssign = false;
            if (user.MaVaiTro == 1)
            {
                canAssign = true;
            }
            else
            {
                // Check if user is Trưởng phòng (Head of Dept)
                bool isTruongPhong = dbContext.PhongBans.Any(pb => pb.MaTruongPhong == user.IdNv);
                
                // Check if user has "Quản lý" or "Trưởng phòng" in position title
                // Note: ChucVu has IdNv as PK
                var chucVu = dbContext.ChucVus.FirstOrDefault(cv => cv.IdNv == user.IdNv);
                bool isQuanLy = chucVu != null && !string.IsNullOrEmpty(chucVu.TenChucVu) && 
                                (chucVu.TenChucVu.ToLower().Contains("quản lý") || 
                                 chucVu.TenChucVu.ToLower().Contains("trưởng phòng"));

                canAssign = isTruongPhong || isQuanLy;
            }

            return Ok(new
            {
                Message = "Đăng nhập thành công",
                token = token,
                UserId = user.IdNv,
                TenDangNhap = user.TenDangNhap,
                MaVaiTro = user.MaVaiTro,
                TenVaiTro = user.MaVaiTroNavigation?.TenVaiTro,
                CanAssign = canAssign
            });
        }

        private string GenerateJwtToken(TaiKhoan user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim("UserId", user.IdNv.ToString())
            };

            if (user.MaVaiTro.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.MaVaiTro.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
