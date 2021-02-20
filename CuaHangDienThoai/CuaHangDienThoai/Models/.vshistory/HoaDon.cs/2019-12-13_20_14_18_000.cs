using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CuaHangDienThoai.Models
{
    public class HoaDon
    {
        [Key]
        [Display(Name = "Mã hóa đơn")]
        public int MaHD { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public int MaDH { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TenKH { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SoDT { get; set; }

        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        public int? MaKH { get; set; }
        [Display(Name = "Tổng tiền")]
        public int TongThanhToan { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày lập hóa đơn")]
        public DateTime NgayLapHD { get; set; }
        public DonHang DonHang { get; set; }
        public KhachHang KhachHang { get; set; }
        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
