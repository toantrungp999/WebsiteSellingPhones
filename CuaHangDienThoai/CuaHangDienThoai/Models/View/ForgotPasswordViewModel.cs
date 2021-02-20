using System.ComponentModel.DataAnnotations;

namespace CuaHangDienThoai.Models.View
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
    }
}
