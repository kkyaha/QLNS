using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("PhongBan")]
public partial class PhongBan
{
    [Key]
    public int MaPhongBan { get; set; }

    [StringLength(100)]
    public string TenPhong { get; set; } = null!;

    [Column("SoLuongNV")]
    public int? SoLuongNv { get; set; }

    public int? MaTruongPhong { get; set; }

    [InverseProperty("MaPhongBanNavigation")]
    public virtual ICollection<ChucVu> ChucVus { get; set; } = new List<ChucVu>();

    [ForeignKey("MaTruongPhong")]
    [InverseProperty("PhongBans")]
    public virtual NhanVien? MaTruongPhongNavigation { get; set; }

    [InverseProperty("MaPhongBanNavigation")]
    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
