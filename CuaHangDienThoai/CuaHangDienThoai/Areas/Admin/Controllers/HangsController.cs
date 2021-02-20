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
    public class HangsController : Controller
    {
        private readonly MobileContext _mb;
        public HangsController(MobileContext mb)
        {
            _mb = mb;
        }
        public IActionResult Index()
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                 ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                return View(_mb.Hang.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
        }
        public IActionResult Create()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hang hang)
        {
            if (ModelState.IsValid)
            {
                 ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                _mb.Add(hang);
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hang);
        }
        public async Task<IActionResult> Edit(int? maHang)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                 ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                if (maHang == null)
                    return NotFound();
                var hang = await _mb.Hang.FindAsync(maHang);
                if (hang == null)
                    return null;
                return View(hang);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maHang, Hang hang)
        {
            if (maHang != hang.MaHang)
                return NotFound();
            if (ModelState.IsValid)
            {
                _mb.Update(hang);
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hang);
        }
        public async Task<IActionResult> Details(int? maHang)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            if (maHang == null)
                return NotFound();
            var hang = await _mb.Hang.FindAsync(maHang);
            if (hang == null)
                return null;
            return View(hang);
        }

    }
}