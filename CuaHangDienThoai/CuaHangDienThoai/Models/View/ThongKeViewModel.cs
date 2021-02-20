using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuaHangDienThoai.Models.View
{
    public class ThongKeViewModel
    {
        public List<Hang> DanhSachHang { get; set; }
        public string TenThongKe { get; set; }
        public List<DoanhSoVaDanhThuDT> DanhSachDoanhSoVaDanhThuDT { get; set; }
        public List<DoanhThuNgay> DanhSachDoanhThuNgay { get; set; }
    }

    public class DoanhSoVaDanhThuDT
    {
        public ModelDienThoai ModelDT { get; set; }
        public int SoLuong { get; set; }
        public int DoanhThu { get; set; }
    }

    public class DoanhThuNgay
    {
        public DateTime Ngay { get; set; }
        public int DoanhThu { get; set; }
    }
}
