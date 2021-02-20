using System.Collections.Generic;

namespace CuaHangDienThoai.Models.View
{
    public class TrangChuViewModel
    {
        public List<ModelDienThoai> DanhSachModel { get; set; }
        public List<Hang> DanhSachHang { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
