using System;
using System.Collections.Generic;

namespace QLNS.Models;

public partial class TrangThai
{
    public byte MaTrangThai { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public virtual ICollection<Luong> Luongs { get; set; } = new List<Luong>();

    public virtual ICollection<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
}
