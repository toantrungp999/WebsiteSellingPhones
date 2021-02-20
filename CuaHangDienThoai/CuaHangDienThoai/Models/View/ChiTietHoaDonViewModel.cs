using System.Collections.Generic;

namespace CuaHangDienThoai.Models.View
{
    public class ChiTietHoaDonViewModel
    {
        public HoaDon HoaDon { get; set; }
        public IEnumerable<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
