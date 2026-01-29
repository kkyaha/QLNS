namespace QLNS.DTOs
{
    public class LuongDTO
    {
        public int? IdNv { get; set; }
        public int? Thang { get; set; }
        public decimal? LuongCoBan { get; set; }
        public double? HeSoOt { get; set; }
        public byte? TrangThai { get; set; }
    }

    public class UpdateLuongDTO : LuongDTO
    {
        public int MaLuong { get; set; }
    }
}
