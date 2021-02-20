using System.Collections.Generic;

namespace CuaHangDienThoai.Models.View
{
    public class ModelDienThoaiViewModel
    {
        public ModelDienThoai ModelDienThoais { get; set; }
        public IEnumerable<Hang> Hangs { get; set; }
    }
}
