using System;

namespace QLNS.DTOs
{
    public class ToDoListDTO
    {
        public int? IdNv { get; set; }
        public int? MaNguoiGiao { get; set; }
        public string? NoiDung { get; set; }
        public DateOnly? NgayTao { get; set; }
        public DateOnly? HanHoanThanh { get; set; }
        public string? GhiChu { get; set; }
        public byte? TrangThai { get; set; }
    }

    public class UpdateToDoStatusDTO
    {
        public byte TrangThai { get; set; }
    }
}
