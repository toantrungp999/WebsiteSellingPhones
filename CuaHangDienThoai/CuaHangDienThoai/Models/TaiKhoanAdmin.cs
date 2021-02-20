using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuaHangDienThoai.Models
{
    public class TaiKhoanAdmin
    {
        [Key]
        public string User { get; set; }
        public string Pass { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Quyền hạn")]
        public string Role { get; set; }
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }

    }
}
