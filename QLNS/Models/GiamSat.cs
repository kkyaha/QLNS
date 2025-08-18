using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("GiamSat")]
public partial class GiamSat
{
    [Key]
    [Column("MaNV")]
    public int MaNv { get; set; }

    public int? BiGiamSat { get; set; }

    [ForeignKey("BiGiamSat")]
    [InverseProperty("GiamSatBiGiamSatNavigations")]
    public virtual NhanVien? BiGiamSatNavigation { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("GiamSatMaNvNavigation")]
    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
