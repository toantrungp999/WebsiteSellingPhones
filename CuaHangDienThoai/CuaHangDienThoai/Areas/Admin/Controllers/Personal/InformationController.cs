using System.Linq;
using System.Threading.Tasks;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using CuaHangDienThoai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InformationController : Controller
    {
        private readonly MobileContext _mb;
        public InformationController(MobileContext mb)
        {
            _mb = mb;
        }

        public async Task<IActionResult> Index()
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            var User= HttpContext.Session.GetString(CommonAdmin.USER_SESSION);
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            return View(await _mb.TaiKhoanAdmin.FindAsync(User));
        }
        public async Task<IActionResult> Edit(string user)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            var User = HttpContext.Session.GetString(CommonAdmin.USER_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if(User!=user)
            {
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var taiKhoan = await _mb.TaiKhoanAdmin.FindAsync(user);
            if (taiKhoan == null)
                return null;
            return View(taiKhoan);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInformation(string user, TaiKhoanAdmin taiKhoanAdmin)
        {
            if(user!=taiKhoanAdmin.User)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var taiKhoan = _mb.TaiKhoanAdmin.Where(u => u.User == user).FirstOrDefault();
                taiKhoan.Name = taiKhoanAdmin.Name;
                taiKhoan.PhoneNumber = taiKhoanAdmin.PhoneNumber;
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoanAdmin);
            //string IMEI = Request.Form[location];
        }
        /// Chức năng làm thêm
        public async Task<IActionResult> EditPass(string user)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            var User = HttpContext.Session.GetString(CommonAdmin.USER_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if (User != user)
            {
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var taiKhoan = await _mb.TaiKhoanAdmin.FindAsync(user);
            if (taiKhoan == null)
                return null;
            return View(taiKhoan);
        }
        [HttpPost, ActionName("EditPass")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassPost(string user, TaiKhoanAdmin taiKhoanAdmin)
        {
            var taiKhoan = _mb.TaiKhoanAdmin.Where(u => u.User == user).SingleOrDefault();
            if (taiKhoan == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                if(taiKhoan.Pass!= MD5.GetMD5(taiKhoanAdmin.Pass))
                {
                    ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                    ModelState.AddModelError("", "Nhập sai mật khẩu.");
                    return View(taiKhoanAdmin);
                }
                taiKhoan.Pass = MD5.GetMD5(Request.Form["new-pass"]);
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }
    }
}