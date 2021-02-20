using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CuaHangDienThoai.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string TenKH { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
        public ICollection<GioHang> GioHangs { get; set; }
        public ICollection<DonHang> DonHangs { get; set; }
        public ICollection<HoaDon>HoaDons { get; set; }
    }
}
