using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("TaiKhoan")]
public partial class TaiKhoan
{
    [Key]
    [StringLength(50)]
    public string TenDangNhap { get; set; } = null!;

    [StringLength(255)]
    public string MatKhau { get; set; } = null!;

    [Column("MaNV")]
    public int? MaNv { get; set; }

    public int? MaVaiTro { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("TaiKhoans")]
    public virtual NhanVien? MaNvNavigation { get; set; }

    [ForeignKey("MaVaiTro")]
    [InverseProperty("TaiKhoans")]
    public virtual VaiTro? MaVaiTroNavigation { get; set; }
}
