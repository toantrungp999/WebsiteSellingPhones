
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CuaHangDienThoai.Models
{
    public class ChiTietHoaDon
    {
        [Display(Name = "Mã hóa đơn")]
        public int MaHD { get; set; }//key
        [Display(Name = "Mã điện thoại")]
        public int MaDT { get; set; }//key
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Display(Name = "Tổng giá")]
        public int TongGia { get; set; }
        public HoaDon HoaDon { get; set; }
        public DienThoai DienThoai { get; set; }
        public ICollection<IMEI_DienThoai> IMEI_DienThoais{ get; set; }

    }
}
