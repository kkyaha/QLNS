using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[PrimaryKey("MaNv", "MaLuong")]
[Table("NhanLuong")]
public partial class NhanLuong
{
    [Key]
    [Column("MaNV")]
    public int MaNv { get; set; }

    [Key]
    public int MaLuong { get; set; }

    public DateOnly? NgayNhan { get; set; }

    [ForeignKey("MaLuong")]
    [InverseProperty("NhanLuongs")]
    public virtual Luong MaLuongNavigation { get; set; } = null!;

    [ForeignKey("MaNv")]
    [InverseProperty("NhanLuongs")]
    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
