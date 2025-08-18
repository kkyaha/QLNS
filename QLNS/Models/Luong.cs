using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("Luong")]
public partial class Luong
{
    [Key]
    public int MaLuong { get; set; }

    [Column("MaNV")]
    public int? MaNv { get; set; }

    public int? Thang { get; set; }

    public double? SoGioLam { get; set; }

    [Column("SoGioOT")]
    public double? SoGioOt { get; set; }

    [Column("TongOT")]
    public double? TongOt { get; set; }

    [Column("HeSoOT")]
    public double? HeSoOt { get; set; }

    [Column(TypeName = "money")]
    public decimal? LuongCoBan { get; set; }

    [Column(TypeName = "money")]
    public decimal? LuongThucNhan { get; set; }

    public byte? TrangThai { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("Luongs")]
    public virtual NhanVien? MaNvNavigation { get; set; }

    [InverseProperty("MaLuongNavigation")]
    public virtual ICollection<NhanLuong> NhanLuongs { get; set; } = new List<NhanLuong>();

    [ForeignKey("TrangThai")]
    [InverseProperty("Luongs")]
    public virtual TrangThai? TrangThaiNavigation { get; set; }
}
