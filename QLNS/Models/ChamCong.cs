using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class ChamCong
{
    public int IdNv { get; set; }

    public DateOnly NgayChamCong { get; set; }
    public DateTime CheckIn { get; set; }   
    public DateTime? CheckOut { get; set; }

    public double? SoGioLam { get; set; }

    public double? SoGioOt { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual NhanVien IdNvNavigation { get; set; } = null!;
}
