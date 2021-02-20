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
    public class HoaDonsController : Controller
    {
        private readonly MobileContext _mb;
        private int PageSize = 5;
        [BindProperty]
        public HoaDonViewChiTietHoaDon HoaDonVM { get; set; }
        public HoaDonsController(MobileContext mb)
        {
            _mb = mb;
            HoaDonVM = new HoaDonViewChiTietHoaDon()
            {
                ChiTietHoaDons = _mb.ChiTietHoaDon.ToList(),
                HoaDons = new Models.HoaDon()
            };
        }
        public async Task<IActionResult> Index(int hoadonsPage = 1, string searchMaHD = null, string searchTenKH = null, string searchSoDT=null)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            HoaDonsPaging hoaDonsPaging = new HoaDonsPaging()
            {
                HoaDons = new List<Models.HoaDon>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/HoaDons?hoadonsPage=:");
            param.Append("&searchMaHD=");
            if (searchMaHD != null)
            {
                param.Append(searchMaHD);
            }
            param.Append("&searchTenKH=");
            if (searchTenKH != null)
            {
                param.Append(searchTenKH);
            }
            param.Append("&searchSoDT=");
            if (searchTenKH != null)
            {
                param.Append(searchSoDT);
            }
            hoaDonsPaging.HoaDons = _mb.HoaDon.ToList();
            if (searchTenKH != null)
            {
                hoaDonsPaging.HoaDons = hoaDonsPaging.HoaDons.Where(dt => dt.TenKH.ToLower().Contains(searchTenKH.ToLower())).ToList();
            }
            if (searchMaHD != null)
            {
                hoaDonsPaging.HoaDons = hoaDonsPaging.HoaDons.Where(dt => dt.MaHD.ToString().Contains(searchMaHD.ToLower())).ToList();
            }
            if (searchSoDT != null)
            {
                hoaDonsPaging.HoaDons = hoaDonsPaging.HoaDons.Where(dt => dt.SoDT.ToString().Contains(searchSoDT)).ToList();
            }


            var count = hoaDonsPaging.HoaDons.Count;

            hoaDonsPaging.HoaDons = hoaDonsPaging.HoaDons.OrderByDescending(p => p.NgayLapHD)
                .Skip((hoadonsPage - 1) * PageSize)
                .Take(PageSize).ToList();


            hoaDonsPaging.PagingInfo = new PagingInfo
            {
                CurrentPage = hoadonsPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            return View(hoaDonsPaging);
        }
        public async Task<IActionResult> Details(int maHD)
        {
            TempData.Clear();
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            HoaDonVM.HoaDons= await _mb.HoaDon.SingleOrDefaultAsync(m => m.MaHD == maHD);
            HoaDonVM.ChiTietHoaDons = await _mb.ChiTietHoaDon.Include(ct=>ct.DienThoai).ThenInclude(ct=>ct.ModelDienThoai).Include(ct=>ct.IMEI_DienThoais).Where(ct => ct.MaHD == maHD).ToListAsync();
            return View(HoaDonVM);
        }
        /// Chức năng làm thêm
        public async Task<IActionResult> Duyet(int? maHD)
        {
            
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            HoaDonVM.HoaDons = await _mb.HoaDon.Include(m=>m.DonHang).SingleOrDefaultAsync(m => m.MaHD == maHD);
            var check = HoaDonVM.HoaDons.DonHang.TrangThai;
            string message = TempData["message"] as string;
            if (message!= "Fail" && check =="Đã duyệt")
            {
                return RedirectToAction(nameof(Index));
            }
            else if(check == "Chưa duyệt")
            {
                var update = _mb.DonHang.Include(dh => dh.HoaDon).Where(dh => dh.HoaDon.MaHD == maHD).SingleOrDefault();
                update.TrangThai = "Đã duyệt";
                await _mb.SaveChangesAsync();
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            
            HoaDonVM.ChiTietHoaDons = await _mb.ChiTietHoaDon.Include(ct => ct.DienThoai).ThenInclude(ct => ct.ModelDienThoai).Where(ct => ct.MaHD == maHD).ToListAsync();
            return View(HoaDonVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duyet(int maHD)
        {
            bool check = true;
            var chiTietHoaDon = _mb.ChiTietHoaDon.Where(ct=>ct.MaHD==maHD).ToList();
            if (ModelState.IsValid)
            {
                string location="";
                foreach (var chiTiet in chiTietHoaDon)
                {
                    for(int i=0;i<chiTiet.SoLuong;i++)
                    {
                        location = chiTiet.MaDT.ToString() + i.ToString();
                        string IMEI = Request.Form[location];
                        if (_mb.IMEI_DienThoai.Where(im => im.IMEI == IMEI).SingleOrDefault() != null)
                        {
                            check = false;
                            TempData[location] = "Trùng IMEI vui lòng thử lại";
                        }
                        else
                        {
                            TempData[location] = IMEI;
                        }
                        
                    }
                }
                try
                {
                    if (!check)
                    {
                        throw new Exception();
                    }
                    string temp="";
                    foreach (var chiTiet in chiTietHoaDon)
                    {
                        for (int i = 0; i < chiTiet.SoLuong; i++)
                        {
                            location = chiTiet.MaDT.ToString() + i.ToString();
                            string IMEI = Request.Form[location];
                            
                            IMEI_DienThoai iMEI_DienThoai = new IMEI_DienThoai()
                            {
                                IMEI = IMEI,
                                MaDT = chiTiet.MaDT,
                                MaHD = chiTiet.MaHD
                            };
                            if(temp == IMEI)
                                TempData[location] = "Trùng IMEI vui lòng thử lại";
                            _mb.Add(iMEI_DienThoai);
                            temp = IMEI;
                        } 
                    }
                    _mb.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["message"] = "Fail";
                    TempData.Keep();
                    
                    return RedirectToAction("Duyet", new { maHD });
                }
            }
            return RedirectToAction("Duyet", new { maHD });
        }
    }
}