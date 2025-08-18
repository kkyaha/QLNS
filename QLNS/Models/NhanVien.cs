using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("NhanVien")]
public partial class NhanVien
{
    [Key]
    [Column("MaNV")]
    public int MaNv { get; set; }

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    [Column("SDT")]
    [StringLength(15)]
    public string? Sdt { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public int? MaPhongBan { get; set; }

    [InverseProperty("MaNvNavigation")]
    public virtual ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();

    [InverseProperty("MaNvNavigation")]
    public virtual ChucVu? ChucVu { get; set; }

    [InverseProperty("BiGiamSatNavigation")]
    public virtual ICollection<GiamSat> GiamSatBiGiamSatNavigations { get; set; } = new List<GiamSat>();

    [InverseProperty("MaNvNavigation")]
    public virtual GiamSat? GiamSatMaNvNavigation { get; set; }

    [InverseProperty("MaNvNavigation")]
    public virtual ICollection<Luong> Luongs { get; set; } = new List<Luong>();

    [ForeignKey("MaPhongBan")]
    [InverseProperty("NhanViens")]
    public virtual PhongBan? MaPhongBanNavigation { get; set; }

    [InverseProperty("MaNvNavigation")]
    public virtual ICollection<NhanLuong> NhanLuongs { get; set; } = new List<NhanLuong>();

    [InverseProperty("MaTruongPhongNavigation")]
    public virtual ICollection<PhongBan> PhongBans { get; set; } = new List<PhongBan>();

    [InverseProperty("MaNvNavigation")]
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();

    [InverseProperty("MaNguoiGiaoNavigation")]
    public virtual ICollection<ToDoList> ToDoListMaNguoiGiaoNavigations { get; set; } = new List<ToDoList>();

    [InverseProperty("MaNvNavigation")]
    public virtual ICollection<ToDoList> ToDoListMaNvNavigations { get; set; } = new List<ToDoList>();
}
