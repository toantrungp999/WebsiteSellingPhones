
using System.ComponentModel.DataAnnotations;

namespace CuaHangDienThoai.Models
{
    public class ChiTietDonHang
    {
        [Display(Name = "Mã đơn hàng")]
        public int MaDH { get; set; }
        [Display(Name = "Mã điện thoại")]
        public int MaDT { get; set; }
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int GiamGia { get; set; }
        public int TongGia { get; set; }
        public DonHang DonHang { get; set; }
        public DienThoai DienThoai { get; set; }
    }
}
