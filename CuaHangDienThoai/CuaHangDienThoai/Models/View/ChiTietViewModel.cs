using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CuaHangDienThoai.Models.View
{
    public class ChiTietViewModel
    {
        public ModelDienThoai ModelDT { get; set; }
        public IEnumerable<DienThoai> DanhSachDT { get; set; }
        [Display(Name = "Số Lượng")]
        public int SoLuong { get; set; }
        public int MaDT { get; set; }
    }
}
