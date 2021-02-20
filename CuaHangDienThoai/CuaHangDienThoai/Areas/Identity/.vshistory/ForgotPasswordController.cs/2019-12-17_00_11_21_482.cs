
using System;
using System.Linq;
using System.Threading.Tasks;
using CuaHangDienThoai.Areas.Identity.Services;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Models.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangDienThoai.Areas.Identity
{
    [Area("Identity")]
    public class ForgotPasswordController : Controller
    {
        private readonly MobileContext _mb;
        private readonly EmailSender _emailSender;
        [BindProperty]
        public ForgotPasswordViewModel Input { get;set; }
        public ForgotPasswordController(MobileContext mb, EmailSender emailSender)
        {
            _mb = mb;
            _emailSender = emailSender;
            Input = new ForgotPasswordViewModel();
           
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost,ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan()
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = _mb.TaiKhoanAdmin.Where(tk => tk.Email==Input.Email).SingleOrDefault();
                if (taiKhoan != null)
                {
                    if(Compare.CompareString(taiKhoan.Email,Input.Email))
                    {
                        ModelState.AddModelError("", "Email không tồn tại.");
                        return View("Index");
                    }
                    if (!taiKhoan.TrangThai)
                    {
                        ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                        return View("Index");
                    }
                    else
                    {                        
                        try
                        {
                            string code = CreateMaXacNhan();
                            HttpContext.Session.SetString(CommonAdmin.Code, code);
                            await _emailSender.SendEmailAsync(
                            Input.Email,
                            "Đặt lại mật khẩu",
                            $"Xin chào {taiKhoan.Name}!<br>Mã xác nhận của tài khoản '{taiKhoan.User}' là: <h2>{code}</h2>Hãy dùng mã này để đặt lại mật khẩu. Mã có thời hạn trong 1 tiếng");
                            
                            return RedirectToAction("ResetPass", "ResetPassword", new { Input.Email });
                        }
                        catch
                        {
                            return View("Index");
                        }

                    }

                }
                else
                {
                    ModelState.AddModelError("", "Email không tồn tại.");
                }
            }
            return View("Index");
        }
        
        private string CreateMaXacNhan()
        {
            Random rd = new Random();
            return rd.Next(100001, 999999).ToString();    
        }    
    }
}