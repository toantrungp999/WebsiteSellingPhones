using System.ComponentModel.DataAnnotations;


namespace CuaHangDienThoai.Models
{
    public class TaiKhoan
    {
        [Key]
        [Display(Name = "Tên tài khoản")]
        public string TenTK { get; set; }
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }
        public int MaKH { get; set; }
        
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
