using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class ChucVu
{
    public int IdNv { get; set; }

    public int? MaPhongBan { get; set; }

    public string? TenChucVu { get; set; }

    public virtual NhanVien IdNvNavigation { get; set; } = null!;

    public virtual PhongBan? MaPhongBanNavigation { get; set; }
}
