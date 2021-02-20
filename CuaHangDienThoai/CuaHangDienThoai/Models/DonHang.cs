using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CuaHangDienThoai.Models
{
    public class DonHang
    {
        [Key]
        [Display(Name = "Mã đơn hàng")]
        public int MaDH { get; set; }
        [Display(Name = "Tên khách hàng")]
        public int MaKH { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày lập đơn hàng")]
        public DateTime NgayLapDH { get; set; }
        [Display(Name = "Tổng giá")]
        public int TongGia { get; set; }
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }        
        public KhachHang KhachHang { get; set; }
        public HoaDon HoaDon { get; set; }
    }
}
