using CuaHangDienThoai.Common;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly MobileContext _mb;
        public HomeController(MobileContext mb)
        {
            _mb = mb;
        }
        public IActionResult Index()
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            } 
        }
        public IActionResult NoRight()
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null && Role.Equals("Super Admin"))
            {
                return RedirectToAction("Index", "DonHangs", new { area = "Admin" });
            }
            else if(Role==null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            return View();
        }
    }
}