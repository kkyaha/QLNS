namespace QLNS.DTOs
{
    public class UpdateNhanVienDTO
    {
        public string HoTen { get; set; } = null!;

        public DateOnly? NgaySinh { get; set; }

        public string? Sdt { get; set; }

        public string? Email { get; set; }

        public int? MaPhongBan { get; set; }

        public string? MaNv { get; set; }
    }
}
