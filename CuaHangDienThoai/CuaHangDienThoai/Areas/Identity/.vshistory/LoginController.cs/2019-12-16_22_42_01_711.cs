using System.Linq;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using CuaHangDienThoai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangDienThoai.Areas.Identity
{
    [Area("Identity")]
    public class LoginController : Controller
    {
        private readonly MobileContext _mb;
        public LoginController(MobileContext mb)
        {
            _mb = mb;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(TaiKhoanAdmin TKAdmin)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = _mb.TaiKhoanAdmin.Where(tk => (tk.User == TKAdmin.User &&
                tk.Pass.Equals(MD5.GetMD5(TKAdmin.Pass)))).SingleOrDefault();
                if (taiKhoan != null)
                {
                    if(!taiKhoan.TrangThai)
                    {
                        ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                        return View("Index");
                    }
                    else
                    {
                        HttpContext.Session.SetString(CommonAdmin.USER_SESSION, taiKhoan.User);
                        HttpContext.Session.SetString(CommonAdmin.NAME_SESSION, taiKhoan.Name);
                        HttpContext.Session.SetString(CommonAdmin.ROLE_SESSION, taiKhoan.Role);
                        return RedirectToAction("Index", "DonHangs", new { area = "Admin" });
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
            }
            return View("Index");
        }
    }
}