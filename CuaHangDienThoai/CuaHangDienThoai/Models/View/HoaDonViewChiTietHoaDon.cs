using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuaHangDienThoai.Models.View
{
    public class HoaDonViewChiTietHoaDon
    {
        public HoaDon HoaDons { get; set; }
        public IEnumerable<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
