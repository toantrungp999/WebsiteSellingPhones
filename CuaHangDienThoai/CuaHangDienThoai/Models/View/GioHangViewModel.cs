using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuaHangDienThoai.Models;

namespace CuaHangDienThoai.Models.View
{
    public class GioHangViewModel
    {
        public List<GioHang> DanhSachGH { get; set; }

        public List<DanhSachPost> DanhSachPost { get; set; }

        public KhachHang KhachHang { get; set; }
    }

    public class DanhSachPost
    {
        public int MaDT { get; set; }
        public int SoLuong { get; set; }
        public bool TrangThai { get; set; }
    }
}
