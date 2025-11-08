using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class GiamSat
{
    public int IdNv { get; set; }

    public int? BiGiamSat { get; set; }

    public virtual NhanVien? BiGiamSatNavigation { get; set; }

    public virtual NhanVien IdNvNavigation { get; set; } = null!;
}
