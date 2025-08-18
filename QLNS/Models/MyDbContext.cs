using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLNS.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChamCong> ChamCongs { get; set; }

    public virtual DbSet<ChucVu> ChucVus { get; set; }

    public virtual DbSet<GiamSat> GiamSats { get; set; }

    public virtual DbSet<Luong> Luongs { get; set; }

    public virtual DbSet<NhanLuong> NhanLuongs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhongBan> PhongBans { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ToDoList> ToDoLists { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=QLNS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChamCong>(entity =>
        {
            entity.HasKey(e => new { e.MaNv, e.NgayChamCong }).HasName("PK__ChamCong__B80B19CA7B96F6FC");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.ChamCongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChamCong__MaNV__72C60C4A");
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__ChucVu__2725D70AF822882B");

            entity.Property(e => e.MaNv).ValueGeneratedNever();

            entity.HasOne(d => d.MaNvNavigation).WithOne(p => p.ChucVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChucVu__MaNV__6477ECF3");

            entity.HasOne(d => d.MaPhongBanNavigation).WithMany(p => p.ChucVus).HasConstraintName("FK__ChucVu__MaPhongB__656C112C");
        });

        modelBuilder.Entity<GiamSat>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__GiamSat__2725D70AE2F261F1");

            entity.Property(e => e.MaNv).ValueGeneratedNever();

            entity.HasOne(d => d.BiGiamSatNavigation).WithMany(p => p.GiamSatBiGiamSatNavigations).HasConstraintName("FK__GiamSat__BiGiamS__7A672E12");

            entity.HasOne(d => d.MaNvNavigation).WithOne(p => p.GiamSatMaNvNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiamSat__MaNV__797309D9");
        });

        modelBuilder.Entity<Luong>(entity =>
        {
            entity.HasKey(e => e.MaLuong).HasName("PK__Luong__6609A48DB49D09BB");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Luongs).HasConstraintName("FK__Luong__MaNV__6FE99F9F");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.Luongs).HasConstraintName("FK_Luong_TrangThai");
        });

        modelBuilder.Entity<NhanLuong>(entity =>
        {
            entity.HasKey(e => new { e.MaNv, e.MaLuong }).HasName("PK__NhanLuon__E1454D42E270AC80");

            entity.HasOne(d => d.MaLuongNavigation).WithMany(p => p.NhanLuongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanLuong__MaLuo__76969D2E");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.NhanLuongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanLuong__MaNV__75A278F5");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70AA0CDE1AA");

            entity.HasOne(d => d.MaPhongBanNavigation).WithMany(p => p.NhanViens).HasConstraintName("FK__NhanVien__MaPhon__60A75C0F");
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.MaPhongBan).HasName("PK__PhongBan__D0910CC899BA9BEF");

            entity.HasOne(d => d.MaTruongPhongNavigation).WithMany(p => p.PhongBans).HasConstraintName("FK_PhongBan_TruongPhong");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap).HasName("PK__TaiKhoan__55F68FC17A841706");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.TaiKhoans).HasConstraintName("FK__TaiKhoan__MaNV__68487DD7");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans).HasConstraintName("FK__TaiKhoan__MaVaiT__693CA210");
        });

        modelBuilder.Entity<ToDoList>(entity =>
        {
            entity.HasKey(e => e.ToDoId).HasName("PK__ToDoList__21D08D200783ED6B");

            entity.HasOne(d => d.MaNguoiGiaoNavigation).WithMany(p => p.ToDoListMaNguoiGiaoNavigations).HasConstraintName("FK__ToDoList__MaNguo__6D0D32F4");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.ToDoListMaNvNavigations).HasConstraintName("FK__ToDoList__MaNV__6C190EBB");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.ToDoLists).HasConstraintName("FK_ToDoList_TrangThai");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("PK__TrangTha__AADE4138662E3168");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF30E7F714");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
