using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using CuaHangDienThoai.Models;
using CuaHangDienThoai.Models.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangsController : Controller
    {
        private readonly MobileContext _mb;
        private int PageSize = 9;
        public KhachHangsController(MobileContext mb)
        {
            _mb = mb;
        }
        public IActionResult Index(int khachhangsPage = 1, string searchTenKH = null, string searchSDT = null)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if (!Role.Equals("Super Admin"))
            {
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            else
            {
                KhachHangsPaging khachHangsPaging = new KhachHangsPaging()
                {
                    KhachHangs = new List<Models.KhachHang>()
                };
                StringBuilder param = new StringBuilder();
                param.Append("/Admin/KhachHangs?khachhangsPage=:");
                param.Append("&searchSDT=");
                if (searchSDT != null)
                {
                    param.Append(searchSDT);
                }
                param.Append("&searchTenKH=");
                if (searchTenKH != null)
                {
                    param.Append(searchTenKH);
                }

                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                khachHangsPaging.KhachHangs = _mb.KhachHang.ToList();
                if (searchSDT != null)
                {
                    khachHangsPaging.KhachHangs = khachHangsPaging.KhachHangs.Where(dt => dt.SoDienThoai.Contains(searchSDT)).ToList();
                }
                if (searchTenKH != null)
                {
                    khachHangsPaging.KhachHangs = khachHangsPaging.KhachHangs.Where(dt => dt.TenKH.ToLower().Contains(searchTenKH.ToLower())).ToList();
                }

                var count = khachHangsPaging.KhachHangs.Count;

                khachHangsPaging.KhachHangs = khachHangsPaging.KhachHangs.OrderBy(p => p.TenKH)
                    .Skip((khachhangsPage - 1) * PageSize)
                    .Take(PageSize).ToList();

                khachHangsPaging.PagingInfo = new PagingInfo
                {
                    CurrentPage = khachhangsPage,
                    ItemsPerPage = PageSize,
                    TotalItems = count,
                    urlParam = param.ToString()
                };
                return View(khachHangsPaging);
            }

        }
        public async Task<IActionResult> EditGmail (int? maKH)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if (!Role.Equals("Super Admin"))
            {
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            if (maKH == null)
                return NotFound();
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var khachHang = await _mb.KhachHang.FindAsync(maKH);
            if (khachHang == null)
                return null;
            return View(khachHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGmail(int maKH, KhachHang khachHang)
        {
            var _khachHang = _mb.KhachHang.Find(maKH);
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            if (_khachHang.Email != khachHang.Email)
            {
                if (_mb.KhachHang.Where(kh => kh.Email == khachHang.Email).SingleOrDefault() != null)
                {
                    
                    ModelState.AddModelError("", "Email đã tồn tại vui lòng nhập lại!");
                    return View(khachHang);
                }
                var KHang = _mb.KhachHang.Where(u => u.MaKH == maKH).SingleOrDefault();
                KHang.Email = khachHang.Email;
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        public async Task<IActionResult> DetailsKhachHang(int? maKH)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if (!Role.Equals("Super Admin"))
            { 
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            if (maKH == null)
                return NotFound();
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var khachHang = await _mb.KhachHang.FindAsync(maKH);
            if (khachHang == null)
                return null;
            return View(khachHang);
            
        }
        public async Task<IActionResult> EditTaiKhoan(int? maKH)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if (!Role.Equals("Super Admin"))
            {
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            if (maKH == null)
                return NotFound();
            var taiKhoans = await _mb.TaiKhoan.Include(tk => tk.KhachHang).SingleOrDefaultAsync(tk => tk.MaKH == maKH);

            if (taiKhoans == null)
                return null;
            return View(taiKhoans);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaiKhoan(string tenTK, TaiKhoan taiKhoan)
        {
            if (tenTK == taiKhoan.TenTK)
            {
                if (ModelState.IsValid)
                {
                    var taiKhoanFromMb = _mb.TaiKhoan.Where(tk => tk.TenTK == taiKhoan.TenTK).FirstOrDefault();
                    
                    
                    taiKhoanFromMb.TrangThai = taiKhoan.TrangThai;
                    await _mb.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(taiKhoan);
            }
            return NotFound();

        }

        public async Task<IActionResult> EditMatKhau(string tenTK)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if(Role==null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            else if (!Role.Equals("Super Admin"))
            {
                return RedirectToAction("NoRight", "Home", new { area = "Admin" });
            }
            if (tenTK == null || tenTK.Trim().Length==0)
                return NotFound();
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var taiKhoans = await _mb.TaiKhoan.FindAsync(tenTK);

            if (taiKhoans == null)
                return null;
            return View(taiKhoans);
        }
        [HttpPost,ActionName("EditMatKhau")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMatKhauPost(string tenTK, TaiKhoan taiKhoan)
        {
            if (taiKhoan.TenTK != tenTK)
                return NotFound();
            if (ModelState.IsValid)
            {
                var _taiKhoan = _mb.TaiKhoan.Where(u => u.TenTK == tenTK).SingleOrDefault();

                _taiKhoan.MatKhau = MD5.GetMD5(taiKhoan.MatKhau);    
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }
    }
}