using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class ToDoList
{
    public int ToDoId { get; set; }

    public int? IdNv { get; set; }

    public int? MaNguoiGiao { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly? NgayTao { get; set; }

    public DateOnly? HanHoanThanh { get; set; }

    public string? GhiChu { get; set; }

    public byte? TrangThai { get; set; }

    public virtual NhanVien? IdNvNavigation { get; set; }

    public virtual NhanVien? MaNguoiGiaoNavigation { get; set; }

    public virtual TrangThai? TrangThaiNavigation { get; set; }
}
