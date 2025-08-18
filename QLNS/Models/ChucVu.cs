using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("ChucVu")]
public partial class ChucVu
{
    [Key]
    [Column("MaNV")]
    public int MaNv { get; set; }

    public int? MaPhongBan { get; set; }

    [StringLength(50)]
    public string? TenChucVu { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("ChucVu")]
    public virtual NhanVien MaNvNavigation { get; set; } = null!;

    [ForeignKey("MaPhongBan")]
    [InverseProperty("ChucVus")]
    public virtual PhongBan? MaPhongBanNavigation { get; set; }
}
