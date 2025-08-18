using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

[Table("TrangThai")]
public partial class TrangThai
{
    [Key]
    public byte MaTrangThai { get; set; }

    [StringLength(50)]
    public string TenTrangThai { get; set; } = null!;

    [InverseProperty("TrangThaiNavigation")]
    public virtual ICollection<Luong> Luongs { get; set; } = new List<Luong>();

    [InverseProperty("TrangThaiNavigation")]
    public virtual ICollection<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
}
