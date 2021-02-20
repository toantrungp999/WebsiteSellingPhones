using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CuaHangDienThoai.Models
{
    public class DienThoai
    {
        [Key]
        [Display(Name = "Mã điện thoại")]
        public int MaDT { get; set; }
        public int MaModel { get; set; }
        [Display(Name = "Màu")]
        public string Mau { get; set; }
        [Display(Name = "Giá")]
        public int Gia { get; set; }
        [Display(Name = "Giảm giá")]
        public int GiamGia { get; set; }
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Display(Name = "Hình")]
        public string Hinh { get; set; }

        public ModelDienThoai ModelDienThoai { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public ICollection<GioHang> GioHangs { get; set; }
        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    }
}
