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
            entity.HasKey(e => new { e.IdNv, e.NgayChamCong }).HasName("PK__ChamCong__B80B19CA7B96F6FC");

            entity.ToTable("ChamCong");

            entity.Property(e => e.IdNv).HasColumnName("IdNV");
            entity.Property(e => e.CheckIn).HasColumnType("datetime");
            entity.Property(e => e.CheckOut).HasColumnType("datetime");
            entity.Property(e => e.SoGioOt).HasColumnName("SoGioOT");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChamCong__MaNV__72C60C4A");
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.IdNv).HasName("PK__ChucVu__2725D70AF822882B");

            entity.ToTable("ChucVu");

            entity.Property(e => e.IdNv)
                .ValueGeneratedNever()
                .HasColumnName("IdNV");
            entity.Property(e => e.TenChucVu).HasMaxLength(50);

            entity.HasOne(d => d.IdNvNavigation).WithOne(p => p.ChucVu)
                .HasForeignKey<ChucVu>(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChucVu__MaNV__6477ECF3");

            entity.HasOne(d => d.MaPhongBanNavigation).WithMany(p => p.ChucVus)
                .HasForeignKey(d => d.MaPhongBan)
                .HasConstraintName("FK__ChucVu__MaPhongB__656C112C");
        });

        modelBuilder.Entity<GiamSat>(entity =>
        {
            entity.HasKey(e => e.IdNv).HasName("PK__GiamSat__2725D70AE2F261F1");

            entity.ToTable("GiamSat");

            entity.Property(e => e.IdNv)
                .ValueGeneratedNever()
                .HasColumnName("IdNV");

            entity.HasOne(d => d.BiGiamSatNavigation).WithMany(p => p.GiamSatBiGiamSatNavigations)
                .HasForeignKey(d => d.BiGiamSat)
                .HasConstraintName("FK__GiamSat__BiGiamS__7A672E12");

            entity.HasOne(d => d.IdNvNavigation).WithOne(p => p.GiamSatIdNvNavigation)
                .HasForeignKey<GiamSat>(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiamSat__MaNV__797309D9");
        });

        modelBuilder.Entity<Luong>(entity =>
        {
            entity.HasKey(e => e.MaLuong).HasName("PK__Luong__6609A48DB49D09BB");

            entity.ToTable("Luong");

            entity.Property(e => e.HeSoOt).HasColumnName("HeSoOT");
            entity.Property(e => e.IdNv).HasColumnName("IdNV");
            entity.Property(e => e.LuongCoBan).HasColumnType("money");
            entity.Property(e => e.LuongThucNhan).HasColumnType("money");
            entity.Property(e => e.SoGioOt).HasColumnName("SoGioOT");
            entity.Property(e => e.TongOt).HasColumnName("TongOT");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.Luongs)
                .HasForeignKey(d => d.IdNv)
                .HasConstraintName("FK__Luong__MaNV__6FE99F9F");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.Luongs)
                .HasForeignKey(d => d.TrangThai)
                .HasConstraintName("FK_Luong_TrangThai");
        });

        modelBuilder.Entity<NhanLuong>(entity =>
        {
            entity.HasKey(e => new { e.IdNv, e.MaLuong }).HasName("PK__NhanLuon__E1454D42E270AC80");

            entity.ToTable("NhanLuong");

            entity.Property(e => e.IdNv).HasColumnName("IdNV");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.NhanLuongs)
                .HasForeignKey(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanLuong__MaNV__75A278F5");

            entity.HasOne(d => d.MaLuongNavigation).WithMany(p => p.NhanLuongs)
                .HasForeignKey(d => d.MaLuong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanLuong__MaLuo__76969D2E");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.IdNv).HasName("PK__NhanVien__2725D70AA0CDE1AA");

            entity.ToTable("NhanVien");

            entity.Property(e => e.IdNv).HasColumnName("IdNV");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .HasColumnName("MaNV");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");

            entity.HasOne(d => d.MaPhongBanNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaPhongBan)
                .HasConstraintName("FK__NhanVien__MaPhon__60A75C0F");
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.MaPhongBan).HasName("PK__PhongBan__D0910CC899BA9BEF");

            entity.ToTable("PhongBan");

            entity.Property(e => e.SoLuongNv).HasColumnName("SoLuongNV");
            entity.Property(e => e.TenPhong).HasMaxLength(100);

            entity.HasOne(d => d.MaTruongPhongNavigation).WithMany(p => p.PhongBans)
                .HasForeignKey(d => d.MaTruongPhong)
                .HasConstraintName("FK_PhongBan_TruongPhong");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap).HasName("PK__TaiKhoan__55F68FC17A841706");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.IdNv).HasColumnName("IdNV");
            entity.Property(e => e.MatKhau).HasMaxLength(255);

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.IdNv)
                .HasConstraintName("FK__TaiKhoan__MaNV__68487DD7");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("FK__TaiKhoan__MaVaiT__693CA210");
        });

        modelBuilder.Entity<ToDoList>(entity =>
        {
            entity.HasKey(e => e.ToDoId).HasName("PK__ToDoList__21D08D200783ED6B");

            entity.ToTable("ToDoList");

            entity.Property(e => e.ToDoId).HasColumnName("ToDoID");
            entity.Property(e => e.IdNv).HasColumnName("IdNV");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.ToDoListIdNvNavigations)
                .HasForeignKey(d => d.IdNv)
                .HasConstraintName("FK__ToDoList__MaNV__6C190EBB");

            entity.HasOne(d => d.MaNguoiGiaoNavigation).WithMany(p => p.ToDoListMaNguoiGiaoNavigations)
                .HasForeignKey(d => d.MaNguoiGiao)
                .HasConstraintName("FK__ToDoList__MaNguo__6D0D32F4");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.ToDoLists)
                .HasForeignKey(d => d.TrangThai)
                .HasConstraintName("FK_ToDoList_TrangThai");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("PK__TrangTha__AADE4138662E3168");

            entity.ToTable("TrangThai");

            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF30E7F714");

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
