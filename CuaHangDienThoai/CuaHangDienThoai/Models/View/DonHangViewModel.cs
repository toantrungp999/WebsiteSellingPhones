using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuaHangDienThoai.Models.View
{
    public class DonHangViewModel
    {
        public List<DonHang> ListDonHang { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
