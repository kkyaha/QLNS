using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("ToDoList")]
public partial class ToDoList
{
    [Key]
    [Column("ToDoID")]
    public int ToDoId { get; set; }

    [Column("MaNV")]
    public int? MaNv { get; set; }

    public int? MaNguoiGiao { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly? NgayTao { get; set; }

    public DateOnly? HanHoanThanh { get; set; }

    public string? GhiChu { get; set; }

    public byte? TrangThai { get; set; }

    [ForeignKey("MaNguoiGiao")]
    [InverseProperty("ToDoListMaNguoiGiaoNavigations")]
    public virtual NhanVien? MaNguoiGiaoNavigation { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("ToDoListMaNvNavigations")]
    public virtual NhanVien? MaNvNavigation { get; set; }

    [ForeignKey("TrangThai")]
    [InverseProperty("ToDoLists")]
    public virtual TrangThai? TrangThaiNavigation { get; set; }
}
