﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("VaiTro")]
public partial class VaiTro
{
    [Key]
    public int MaVaiTro { get; set; }

    [StringLength(50)]
    public string TenVaiTro { get; set; } = null!;

    [InverseProperty("MaVaiTroNavigation")]
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
