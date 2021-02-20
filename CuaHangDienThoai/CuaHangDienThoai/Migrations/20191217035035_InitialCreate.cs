using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CuaHangDienThoai.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hang",
                columns: table => new
                {
                    MaHang = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHang = table.Column<string>(nullable: true),
                    TrangThai = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hang", x => x.MaHang);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKH = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKH = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    SoDienThoai = table.Column<string>(nullable: true),
                    GioiTinh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoanAdmin",
                columns: table => new
                {
                    User = table.Column<string>(nullable: false),
                    Pass = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    TrangThai = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoanAdmin", x => x.User);
                });

            migrationBuilder.CreateTable(
                name: "ModelDienThoai",
                columns: table => new
                {
                    MaModel = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenModel = table.Column<string>(nullable: true),
                    MaHang = table.Column<int>(nullable: false),
                    Ram = table.Column<string>(nullable: true),
                    Rom = table.Column<string>(nullable: true),
                    ManHinh = table.Column<string>(nullable: true),
                    Camera = table.Column<string>(nullable: true),
                    Pin = table.Column<string>(nullable: true),
                    Chip = table.Column<string>(nullable: true),
                    NgayRaMat = table.Column<DateTime>(nullable: false),
                    TrangThai = table.Column<bool>(nullable: false),
                    Hinh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelDienThoai", x => x.MaModel);
                    table.ForeignKey(
                        name: "FK_ModelDienThoai_Hang_MaHang",
                        column: x => x.MaHang,
                        principalTable: "Hang",
                        principalColumn: "MaHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDH = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<int>(nullable: false),
                    NgayLapDH = table.Column<DateTime>(nullable: false),
                    TongGia = table.Column<int>(nullable: false),
                    TrangThai = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.MaDH);
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    TenTK = table.Column<string>(nullable: false),
                    MatKhau = table.Column<string>(nullable: true),
                    MaKH = table.Column<int>(nullable: false),
                    TrangThai = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.TenTK);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DienThoai",
                columns: table => new
                {
                    MaDT = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaModel = table.Column<int>(nullable: false),
                    Mau = table.Column<string>(nullable: true),
                    Gia = table.Column<int>(nullable: false),
                    GiamGia = table.Column<int>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false),
                    Hinh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DienThoai", x => x.MaDT);
                    table.ForeignKey(
                        name: "FK_DienThoai_ModelDienThoai_MaModel",
                        column: x => x.MaModel,
                        principalTable: "ModelDienThoai",
                        principalColumn: "MaModel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    MaHD = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDH = table.Column<int>(nullable: false),
                    TenKH = table.Column<string>(nullable: true),
                    SoDT = table.Column<string>(nullable: true),
                    GioiTinh = table.Column<string>(nullable: true),
                    MaKH = table.Column<int>(nullable: true),
                    TongThanhToan = table.Column<int>(nullable: false),
                    DiaChi = table.Column<string>(nullable: true),
                    NgayLapHD = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.MaHD);
                    table.ForeignKey(
                        name: "FK_HoaDon_DonHang_MaDH",
                        column: x => x.MaDH,
                        principalTable: "DonHang",
                        principalColumn: "MaDH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaDH = table.Column<int>(nullable: false),
                    MaDT = table.Column<int>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false),
                    DonGia = table.Column<int>(nullable: false),
                    GiamGia = table.Column<int>(nullable: false),
                    TongGia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => new { x.MaDH, x.MaDT });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_MaDH",
                        column: x => x.MaDH,
                        principalTable: "DonHang",
                        principalColumn: "MaDH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DienThoai_MaDT",
                        column: x => x.MaDT,
                        principalTable: "DienThoai",
                        principalColumn: "MaDT",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaKH = table.Column<int>(nullable: false),
                    MaDT = table.Column<int>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => new { x.MaKH, x.MaDT });
                    table.ForeignKey(
                        name: "FK_GioHang_DienThoai_MaDT",
                        column: x => x.MaDT,
                        principalTable: "DienThoai",
                        principalColumn: "MaDT",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHang_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    MaHD = table.Column<int>(nullable: false),
                    MaDT = table.Column<int>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false),
                    TongGia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => new { x.MaHD, x.MaDT });
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_DienThoai_MaDT",
                        column: x => x.MaDT,
                        principalTable: "DienThoai",
                        principalColumn: "MaDT",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_HoaDon_MaHD",
                        column: x => x.MaHD,
                        principalTable: "HoaDon",
                        principalColumn: "MaHD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMEI_DienThoai",
                columns: table => new
                {
                    IMEI = table.Column<string>(nullable: false),
                    MaDT = table.Column<int>(nullable: false),
                    MaHD = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMEI_DienThoai", x => x.IMEI);
                    table.ForeignKey(
                        name: "FK_IMEI_DienThoai_ChiTietHoaDon_MaHD_MaDT",
                        columns: x => new { x.MaHD, x.MaDT },
                        principalTable: "ChiTietHoaDon",
                        principalColumns: new[] { "MaHD", "MaDT" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDT",
                table: "ChiTietDonHang",
                column: "MaDT");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_MaDT",
                table: "ChiTietHoaDon",
                column: "MaDT");

            migrationBuilder.CreateIndex(
                name: "IX_DienThoai_MaModel",
                table: "DienThoai",
                column: "MaModel");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaKH",
                table: "DonHang",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaDT",
                table: "GioHang",
                column: "MaDT");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaDH",
                table: "HoaDon",
                column: "MaDH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaKH",
                table: "HoaDon",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_IMEI_DienThoai_MaHD_MaDT",
                table: "IMEI_DienThoai",
                columns: new[] { "MaHD", "MaDT" });

            migrationBuilder.CreateIndex(
                name: "IX_ModelDienThoai_MaHang",
                table: "ModelDienThoai",
                column: "MaHang");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_MaKH",
                table: "TaiKhoan",
                column: "MaKH",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "IMEI_DienThoai");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "TaiKhoanAdmin");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "DienThoai");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "ModelDienThoai");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "Hang");

            migrationBuilder.DropTable(
                name: "KhachHang");
        }
    }
}
