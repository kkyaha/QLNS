using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNS.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrangThai",
                columns: table => new
                {
                    MaTrangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    TenTrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrangTha__AADE4138662E3168", x => x.MaTrangThai);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaiTro__C24C41CF30E7F714", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "ChamCong",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false),
                    NgayChamCong = table.Column<DateOnly>(type: "date", nullable: false),
                    SoGioLam = table.Column<double>(type: "float", nullable: true),
                    SoGioOT = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChamCong__B80B19CA7B96F6FC", x => new { x.MaNV, x.NgayChamCong });
                });

            migrationBuilder.CreateTable(
                name: "ChucVu",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false),
                    MaPhongBan = table.Column<int>(type: "int", nullable: true),
                    TenChucVu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChucVu__2725D70AF822882B", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "GiamSat",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false),
                    BiGiamSat = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GiamSat__2725D70AE2F261F1", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "Luong",
                columns: table => new
                {
                    MaLuong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<int>(type: "int", nullable: true),
                    Thang = table.Column<int>(type: "int", nullable: true),
                    SoGioLam = table.Column<double>(type: "float", nullable: true),
                    SoGioOT = table.Column<double>(type: "float", nullable: true),
                    TongOT = table.Column<double>(type: "float", nullable: true),
                    HeSoOT = table.Column<double>(type: "float", nullable: true),
                    LuongCoBan = table.Column<decimal>(type: "money", nullable: true),
                    LuongThucNhan = table.Column<decimal>(type: "money", nullable: true),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Luong__6609A48DB49D09BB", x => x.MaLuong);
                    table.ForeignKey(
                        name: "FK_Luong_TrangThai",
                        column: x => x.TrangThai,
                        principalTable: "TrangThai",
                        principalColumn: "MaTrangThai");
                });

            migrationBuilder.CreateTable(
                name: "NhanLuong",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false),
                    MaLuong = table.Column<int>(type: "int", nullable: false),
                    NgayNhan = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanLuon__E1454D42E270AC80", x => new { x.MaNV, x.MaLuong });
                    table.ForeignKey(
                        name: "FK__NhanLuong__MaLuo__76969D2E",
                        column: x => x.MaLuong,
                        principalTable: "Luong",
                        principalColumn: "MaLuong");
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaPhongBan = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__2725D70AA0CDE1AA", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "PhongBan",
                columns: table => new
                {
                    MaPhongBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoLuongNV = table.Column<int>(type: "int", nullable: true),
                    MaTruongPhong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhongBan__D0910CC899BA9BEF", x => x.MaPhongBan);
                    table.ForeignKey(
                        name: "FK_PhongBan_TruongPhong",
                        column: x => x.MaTruongPhong,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: true),
                    MaVaiTro = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaiKhoan__55F68FC17A841706", x => x.TenDangNhap);
                    table.ForeignKey(
                        name: "FK__TaiKhoan__MaNV__68487DD7",
                        column: x => x.MaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                    table.ForeignKey(
                        name: "FK__TaiKhoan__MaVaiT__693CA210",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "MaVaiTro");
                });

            migrationBuilder.CreateTable(
                name: "ToDoList",
                columns: table => new
                {
                    ToDoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<int>(type: "int", nullable: true),
                    MaNguoiGiao = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateOnly>(type: "date", nullable: true),
                    HanHoanThanh = table.Column<DateOnly>(type: "date", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ToDoList__21D08D200783ED6B", x => x.ToDoID);
                    table.ForeignKey(
                        name: "FK_ToDoList_TrangThai",
                        column: x => x.TrangThai,
                        principalTable: "TrangThai",
                        principalColumn: "MaTrangThai");
                    table.ForeignKey(
                        name: "FK__ToDoList__MaNV__6C190EBB",
                        column: x => x.MaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                    table.ForeignKey(
                        name: "FK__ToDoList__MaNguo__6D0D32F4",
                        column: x => x.MaNguoiGiao,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChucVu_MaPhongBan",
                table: "ChucVu",
                column: "MaPhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_GiamSat_BiGiamSat",
                table: "GiamSat",
                column: "BiGiamSat");

            migrationBuilder.CreateIndex(
                name: "IX_Luong_MaNV",
                table: "Luong",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_Luong_TrangThai",
                table: "Luong",
                column: "TrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_NhanLuong_MaLuong",
                table: "NhanLuong",
                column: "MaLuong");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaPhongBan",
                table: "NhanVien",
                column: "MaPhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_PhongBan_MaTruongPhong",
                table: "PhongBan",
                column: "MaTruongPhong");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_MaNV",
                table: "TaiKhoan",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_MaVaiTro",
                table: "TaiKhoan",
                column: "MaVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_MaNguoiGiao",
                table: "ToDoList",
                column: "MaNguoiGiao");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_MaNV",
                table: "ToDoList",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_TrangThai",
                table: "ToDoList",
                column: "TrangThai");

            migrationBuilder.AddForeignKey(
                name: "FK__ChamCong__MaNV__72C60C4A",
                table: "ChamCong",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK__ChucVu__MaNV__6477ECF3",
                table: "ChucVu",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK__ChucVu__MaPhongB__656C112C",
                table: "ChucVu",
                column: "MaPhongBan",
                principalTable: "PhongBan",
                principalColumn: "MaPhongBan");

            migrationBuilder.AddForeignKey(
                name: "FK__GiamSat__BiGiamS__7A672E12",
                table: "GiamSat",
                column: "BiGiamSat",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK__GiamSat__MaNV__797309D9",
                table: "GiamSat",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK__Luong__MaNV__6FE99F9F",
                table: "Luong",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK__NhanLuong__MaNV__75A278F5",
                table: "NhanLuong",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK__NhanVien__MaPhon__60A75C0F",
                table: "NhanVien",
                column: "MaPhongBan",
                principalTable: "PhongBan",
                principalColumn: "MaPhongBan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhongBan_TruongPhong",
                table: "PhongBan");

            migrationBuilder.DropTable(
                name: "ChamCong");

            migrationBuilder.DropTable(
                name: "ChucVu");

            migrationBuilder.DropTable(
                name: "GiamSat");

            migrationBuilder.DropTable(
                name: "NhanLuong");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "ToDoList");

            migrationBuilder.DropTable(
                name: "Luong");

            migrationBuilder.DropTable(
                name: "VaiTro");

            migrationBuilder.DropTable(
                name: "TrangThai");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "PhongBan");
        }
    }
}
