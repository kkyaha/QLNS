using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class TaiKhoan
{
    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public int? IdNv { get; set; }

    public int? MaVaiTro { get; set; }

    public virtual NhanVien? IdNvNavigation { get; set; }

    public virtual VaiTro? MaVaiTroNavigation { get; set; }
}
