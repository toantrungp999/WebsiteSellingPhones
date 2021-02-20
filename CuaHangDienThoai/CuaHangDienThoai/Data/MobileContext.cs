using CuaHangDienThoai.Models;
using Microsoft.EntityFrameworkCore;


namespace CuaHangDienThoai.Data
{
    public class MobileContext : DbContext
    {
        public MobileContext(DbContextOptions<MobileContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>().HasKey(ctdh => new { ctdh.MaDH, ctdh.MaDT });
            modelBuilder.Entity<GioHang>().HasKey(ctgh => new { ctgh.MaKH, ctgh.MaDT });
            modelBuilder.Entity<ChiTietHoaDon>().HasKey(ctgh => new { ctgh.MaHD, ctgh.MaDT });


            modelBuilder.Entity<KhachHang>()
                .HasOne<TaiKhoan>(kh => kh.TaiKhoan)
                .WithOne(tk => tk.KhachHang)
                .HasForeignKey<TaiKhoan>(tk => tk.MaKH);

            modelBuilder.Entity<DienThoai>()
                .HasOne<ModelDienThoai>(dt => dt.ModelDienThoai)
                .WithMany(m => m.DienThoais)
                .HasForeignKey(dt => dt.MaModel);

            modelBuilder.Entity<DonHang>()
            .HasOne<KhachHang>(dh => dh.KhachHang)
            .WithMany(kh => kh.DonHangs)
            .HasForeignKey(dh => dh.MaKH);

            modelBuilder.Entity<GioHang>()
            .HasOne<KhachHang>(gh => gh.KhachHang)
            .WithMany(kh => kh.GioHangs)
            .HasForeignKey(gh => gh.MaKH);

            modelBuilder.Entity<ChiTietDonHang>()
            .HasOne<DienThoai>(ct => ct.DienThoai)
            .WithMany(dt => dt.ChiTietDonHangs)
            .HasForeignKey(ct => ct.MaDT);

            modelBuilder.Entity<ChiTietDonHang>()
           .HasOne<DonHang>(ct => ct.DonHang)
           .WithMany(dh => dh.ChiTietDonHangs)
           .HasForeignKey(ct => ct.MaDH);

            modelBuilder.Entity<GioHang>()
            .HasOne<DienThoai>(gh => gh.DienThoai)
            .WithMany(dt => dt.GioHangs)
            .HasForeignKey(gh => gh.MaDT);

            modelBuilder.Entity<DonHang>()
           .HasOne<KhachHang>(dh => dh.KhachHang)
           .WithMany(kh => kh.DonHangs)
           .HasForeignKey(dh => dh.MaKH);

            //Hoa don
            modelBuilder.Entity<HoaDon>()
          .HasOne<DonHang>(hd => hd.DonHang)
          .WithOne(dh => dh.HoaDon)
          .HasForeignKey<HoaDon>(hd => hd.MaDH);

            modelBuilder.Entity<HoaDon>()
          .HasOne<KhachHang>(hd => hd.KhachHang)
          .WithMany(dh => dh.HoaDons)
          .HasForeignKey(hd => hd.MaKH);

            //ct hoa don

            modelBuilder.Entity<ChiTietHoaDon>()
          .HasOne<HoaDon>(ct => ct.HoaDon)
          .WithMany(hd => hd.ChiTietHoaDons)
          .HasForeignKey(hd => hd.MaHD);

            modelBuilder.Entity<ChiTietHoaDon>()
         .HasOne<DienThoai>(ct => ct.DienThoai)
         .WithMany(dt => dt.ChiTietHoaDons)
         .HasForeignKey(ct => ct.MaDT);

            modelBuilder.Entity<IMEI_DienThoai>()
          .HasOne<ChiTietHoaDon>(imei => imei.ChiTietHoaDon)
          .WithMany(hd => hd.IMEI_DienThoais)
          .HasForeignKey(imei => new { imei.MaHD, imei.MaDT });

            modelBuilder.Entity<ModelDienThoai>()
        .HasOne<Hang>(dt => dt.Hang)
        .WithMany(h => h.ModelDienThoais)
        .HasForeignKey(ct => ct.MaHang);
        }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<DienThoai> DienThoai { get; set; }
        public DbSet<ModelDienThoai> ModelDienThoai { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<GioHang> GioHang { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public DbSet<IMEI_DienThoai> IMEI_DienThoai { get; set; }
        public DbSet<Hang> Hang { get; set; }
        public DbSet<TaiKhoanAdmin> TaiKhoanAdmin { get; set; }
    }
}
