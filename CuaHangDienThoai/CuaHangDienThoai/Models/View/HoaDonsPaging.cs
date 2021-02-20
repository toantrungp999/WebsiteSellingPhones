using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuaHangDienThoai.Models.View
{
    public class HoaDonsPaging
    {
        public List<HoaDon> HoaDons { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
