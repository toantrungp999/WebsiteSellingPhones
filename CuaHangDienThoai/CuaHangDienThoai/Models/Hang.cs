using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CuaHangDienThoai.Models
{
    public class Hang
    {
        [Key]
        public int MaHang { get; set; }
        [Display(Name = "Tên Hãng")]
        public string TenHang { get; set; }
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
        public ICollection<ModelDienThoai> ModelDienThoais { get; set; }
    }
}
