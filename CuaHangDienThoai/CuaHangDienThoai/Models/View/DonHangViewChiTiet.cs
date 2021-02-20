using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuaHangDienThoai.Models.View
{
    public class DonHangViewChiTiet
    {
        public DonHang DonHangs { get; set; }
        public IEnumerable<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
