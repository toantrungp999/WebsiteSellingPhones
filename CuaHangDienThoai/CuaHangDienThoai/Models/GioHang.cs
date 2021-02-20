
namespace CuaHangDienThoai.Models
{
    public class GioHang
    {
        public int MaKH { get; set; }
        public int MaDT { get; set; }
        public int SoLuong { get; set; }
        public DienThoai DienThoai { get; set; }
        public KhachHang KhachHang { get; set; }

    }
}
