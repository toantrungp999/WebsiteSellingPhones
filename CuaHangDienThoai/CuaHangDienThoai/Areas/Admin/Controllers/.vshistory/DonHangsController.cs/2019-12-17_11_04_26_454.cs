using System;
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
    public class DonHangsController : Controller
    {
        private readonly MobileContext _mb;
        [BindProperty]
        public DonHangViewChiTiet DonHangVM { get; set; }
        //public DonHang donHang { get; set; }
        private int PageSize = 5;
        public DonHangsController(MobileContext mb)
        {
            _mb = mb;
            DonHangVM = new DonHangViewChiTiet()
            {
                ChiTietDonHangs = _mb.ChiTietDonHang.ToList(),
                DonHangs = new Models.DonHang()
            };
            
        }
        public IActionResult Index(int donhangsPage = 1, string searchMaDH = null, string searchTenKH = null)
        { 
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            DonHangsPaging donHangsPaging = new DonHangsPaging()
            {
                DonHangs = new List<Models.DonHang>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/DonHangs?donhangsPage=:");
            param.Append("&searchMaDH=");
            if (searchMaDH != null)
            {
                param.Append(searchMaDH);
            }
            param.Append("&searchTenKH=");
            if (searchTenKH != null)
            {
                param.Append(searchTenKH);
            }
            donHangsPaging.DonHangs = _mb.DonHang.Include(h => h.KhachHang).ToList();
            if (searchTenKH != null)
            {
                donHangsPaging.DonHangs = donHangsPaging.DonHangs.Where(dt => dt.KhachHang.TenKH.ToLower().Contains(searchTenKH.ToLower())).ToList();
            }
            if (searchMaDH != null)
            {
                donHangsPaging.DonHangs = donHangsPaging.DonHangs.Where(dt => dt.MaDH.ToString().Contains(searchMaDH.ToLower())).ToList();
            }


            var count = donHangsPaging.DonHangs.Count;

            donHangsPaging.DonHangs = donHangsPaging.DonHangs.OrderByDescending(p => p.NgayLapDH)
                .Skip((donhangsPage - 1) * PageSize)
                .Take(PageSize).ToList();


            donHangsPaging.PagingInfo = new PagingInfo
            {
                CurrentPage = donhangsPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            return View(donHangsPaging);
        }
        /// Chức năng làm thêm
        public async Task<IActionResult> Duyet(int? maDH)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                if (maDH == null)
                    return NotFound();
                DonHangVM.DonHangs = await _mb.DonHang.Include(m => m.KhachHang).SingleOrDefaultAsync(m => m.MaDH == maDH);
                DonHangVM.ChiTietDonHangs = await _mb.ChiTietDonHang.Include(ct => ct.DienThoai).ThenInclude(ct => ct.ModelDienThoai).Where(ct => ct.MaDH == maDH).ToListAsync();
                if (maDH == null)
                    return NotFound();
                if (DonHangVM.DonHangs == null)
                    return NotFound();
                return View(DonHangVM);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duyet(int maDH)
        {
           
            if (ModelState.IsValid)
            {
                var donHangFromMb = _mb.DonHang.Include(dh=>dh.KhachHang).Where(m => m.MaDH == maDH).FirstOrDefault();
                HoaDon hoaDon = new HoaDon()
                {
                    MaDH = donHangFromMb.MaDH,
                    MaKH = donHangFromMb.MaKH,
                    NgayLapHD = DateTime.Now,
                    TenKH= donHangFromMb.KhachHang.TenKH,
                    SoDT= donHangFromMb.KhachHang.SoDienThoai,
                    GioiTinh= donHangFromMb.KhachHang.GioiTinh,
                    DiaChi=donHangFromMb.KhachHang.DiaChi
                };

                _mb.Add(hoaDon);
                _mb.SaveChanges();
                var chiTiet = _mb.ChiTietDonHang.Where(ct => ct.MaDH == maDH).ToList();
                foreach(var chiTietDonHang in chiTiet)
                {
                    int soLuong = Convert.ToInt32(Request.Form[chiTietDonHang.MaDT.ToString()]);
                    if (soLuong != 0)
                    {
                        ChiTietHoaDon ct = new ChiTietHoaDon()
                        {
                            MaHD = hoaDon.MaHD,
                            MaDT = chiTietDonHang.MaDT,
                            SoLuong = soLuong,
                            TongGia = chiTietDonHang.TongGia/chiTietDonHang.SoLuong*soLuong,
                        };
                        _mb.Add(ct);
                        
                    }
                    hoaDon.TongThanhToan = hoaDon.TongThanhToan + chiTietDonHang.TongGia / chiTietDonHang.SoLuong * soLuong;
                    _mb.Update(hoaDon);
                    _mb.SaveChanges();

                };

                return RedirectToAction("Duyet", "HoaDons", new { hoaDon.MaHD });
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDonHang(int maDH)
        {
            if (ModelState.IsValid)
            {
                var donHangFromMb = _mb.DonHang.Where(m => m.MaDH == maDH).FirstOrDefault();
                if (donHangFromMb.TrangThai != "Đã Hủy")
                {
                    donHangFromMb.TrangThai = "Đã Hủy";
                    var ctdh = _mb.ChiTietDonHang.Where(ct => ct.MaDH == maDH).ToList();
                    foreach(var ct in ctdh)
                    {
                        var dt = _mb.DienThoai.Find(ct.MaDT);
                        dt.SoLuong = dt.SoLuong + ct.SoLuong;
                    }
                    await _mb.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
                
            }
            return RedirectToAction("Duyet", "DonHangs");
        }
    }
}