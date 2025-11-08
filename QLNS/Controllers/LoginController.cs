using Microsoft.AspNetCore.Mvc;
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
            if (dbContext.TaiKhoans.Any(u => u.TenDangNhap == dto.TenDangNhap))
                return BadRequest("Tên đăng nhập đã tồn tại");

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = dto.TenDangNhap,
                MatKhau = HashPassword(dto.MatKhau),
            };

            dbContext.TaiKhoans.Add(taiKhoan);
            dbContext.SaveChanges();

            return Ok("Đăng ký thành công");
        }

        // 🔹 Đăng nhập + JWT
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var user = dbContext.TaiKhoans.FirstOrDefault(u => u.TenDangNhap == dto.TenDangNhap);
            if (user == null || !VerifyPassword(dto.MatKhau, user.MatKhau))
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            // Sinh JWT
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Message = "Đăng nhập thành công",
                token = token,
                UserId = user.IdNv,
                TenDangNhap = user.TenDangNhap
            });
        }

        private string GenerateJwtToken(TaiKhoan user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim("UserId", user.IdNv.ToString())
            };

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
