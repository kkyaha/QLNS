using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class NhanLuong
{
    public int IdNv { get; set; }

    public int MaLuong { get; set; }

    public DateOnly? NgayNhan { get; set; }

    public virtual NhanVien IdNvNavigation { get; set; } = null!;

    public virtual Luong MaLuongNavigation { get; set; } = null!;
}
