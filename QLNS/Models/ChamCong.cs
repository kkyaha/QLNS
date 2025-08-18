using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[PrimaryKey("MaNv", "NgayChamCong")]
[Table("ChamCong")]
public partial class ChamCong
{
    [Key]
    [Column("MaNV")]
    public int MaNv { get; set; }

    [Key]
    public DateOnly NgayChamCong { get; set; }

    public double? SoGioLam { get; set; }

    [Column("SoGioOT")]
    public double? SoGioOt { get; set; }

    [StringLength(50)]
    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("ChamCongs")]
    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
