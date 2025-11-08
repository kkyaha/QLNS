using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class NhanVien
{
    public int IdNv { get; set; }

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public int? MaPhongBan { get; set; }

    public string? MaNv { get; set; }

    public virtual ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();

    public virtual ChucVu? ChucVu { get; set; }

    public virtual ICollection<GiamSat> GiamSatBiGiamSatNavigations { get; set; } = new List<GiamSat>();

    public virtual GiamSat? GiamSatIdNvNavigation { get; set; }

    public virtual ICollection<Luong> Luongs { get; set; } = new List<Luong>();

    public virtual PhongBan? MaPhongBanNavigation { get; set; }

    public virtual ICollection<NhanLuong> NhanLuongs { get; set; } = new List<NhanLuong>();

    public virtual ICollection<PhongBan> PhongBans { get; set; } = new List<PhongBan>();

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();

    public virtual ICollection<ToDoList> ToDoListIdNvNavigations { get; set; } = new List<ToDoList>();

    public virtual ICollection<ToDoList> ToDoListMaNguoiGiaoNavigations { get; set; } = new List<ToDoList>();
}
