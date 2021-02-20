using System.Collections.Generic;

namespace CuaHangDienThoai.Models.View
{
    public class DienThoaisViewModel
    {
        public DienThoai DienThoais { get; set; }
        public IEnumerable<ModelDienThoai> ModelDienThoais { get; set; }
    }
}
