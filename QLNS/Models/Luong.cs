using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class Luong
{
    public int MaLuong { get; set; }

    public int? IdNv { get; set; }

    public int? Thang { get; set; }

    public double? SoGioLam { get; set; }

    public double? SoGioOt { get; set; }

    public double? TongOt { get; set; }

    public double? HeSoOt { get; set; }

    public decimal? LuongCoBan { get; set; }

    public decimal? LuongThucNhan { get; set; }

    public byte? TrangThai { get; set; }

    public virtual NhanVien? IdNvNavigation { get; set; }

    public virtual ICollection<NhanLuong> NhanLuongs { get; set; } = new List<NhanLuong>();

    public virtual TrangThai? TrangThaiNavigation { get; set; }
}
