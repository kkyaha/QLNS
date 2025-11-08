using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class PhongBan
{
    public int MaPhongBan { get; set; }

    public string TenPhong { get; set; } = null!;

    public int? SoLuongNv { get; set; }

    public int? MaTruongPhong { get; set; }

    public virtual ICollection<ChucVu> ChucVus { get; set; } = new List<ChucVu>();

    public virtual NhanVien? MaTruongPhongNavigation { get; set; }

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
