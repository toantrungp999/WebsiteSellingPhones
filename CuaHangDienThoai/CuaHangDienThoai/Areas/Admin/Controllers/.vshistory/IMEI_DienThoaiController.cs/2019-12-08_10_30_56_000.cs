using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Models;
using CuaHangDienThoai.Models.View;
using Microsoft.AspNetCore.Http;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Extensions;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IMEI_DienThoaiController : Controller
    {
        private readonly MobileContext _mb;

        [BindProperty]
        public CTHoaDonViewIMEI CTHoaDonViewIMEIVM { get; set; }

        public IMEI_DienThoaiController(MobileContext context)
        {
            _mb = context;
            CTHoaDonViewIMEIVM = new CTHoaDonViewIMEI()
            {
                iMEI_DienThoais = _mb.IMEI_DienThoai.ToList(),
                chiTietHoaDon = new Models.ChiTietHoaDon()
            };
        }
        public async Task<IActionResult> Index(string searchMaHD = null, string searchMaDT = null, string searchIMEI=null)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            //bool cont = true;
            var chiTiet = _mb.ChiTietHoaDon.Include(ct => ct.IMEI_DienThoais).ToList();
           
            if (searchIMEI != null)
            {
                var _chiTiet = _mb.ChiTietHoaDon.Join(_mb.IMEI_DienThoai, ct => ct.MaHD
                                                                 , im => im.MaHD
                                                                 , (ct, im) => new { ChiTiet = ct, IMEI = im }).ToList();
                _chiTiet = _chiTiet.Where(ct=>ct.IMEI.IMEI.ToLower().Equals(searchIMEI.ToLower())).ToList();
                chiTiet = _chiTiet.Select(ct => ct.ChiTiet).ToList();
            }
                if (searchMaDT != null)
            {
                chiTiet = chiTiet.Where(dt => dt.MaDT.ToString().ToLower().Contains(searchMaDT.ToLower())).ToList();
            }
            if (searchMaHD != null)
            {
                chiTiet = chiTiet.Where(dt => dt.MaHD.ToString().ToLower().Contains(searchMaHD.ToLower())).ToList();
            }
            return View(chiTiet);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            if (id == null)
            {
                return NotFound();
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            CTHoaDonViewIMEIVM.chiTietHoaDon = await _mb.ChiTietHoaDon.SingleOrDefaultAsync(ct=>(ct.MaHD.ToString()+ct.MaDT.ToString()).Equals(id));
            CTHoaDonViewIMEIVM.iMEI_DienThoais = await _mb.IMEI_DienThoai.Where(ct => (ct.MaHD.ToString() + ct.MaDT.ToString()).Equals(id)).ToListAsync();
            int i = 0;
            if(TempData["message"]==null)
            {
                foreach (var im in CTHoaDonViewIMEIVM.iMEI_DienThoais)
                {
                    TempData[CTHoaDonViewIMEIVM.chiTietHoaDon.MaDT.ToString() + i.ToString()] = im.IMEI;
                    i++;
                    if (i > CTHoaDonViewIMEIVM.chiTietHoaDon.SoLuong)
                        break;
                }
            }
            
            if (CTHoaDonViewIMEIVM.chiTietHoaDon == null)
            {
                return NotFound();
            }
            return View(CTHoaDonViewIMEIVM);
        }
        /// Chức năng làm thêm
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrCreate(string id)
        {
            bool check = true;
            if (ModelState.IsValid)
            {
                CTHoaDonViewIMEIVM.chiTietHoaDon = await _mb.ChiTietHoaDon.SingleOrDefaultAsync(ct => (ct.MaHD.ToString() + ct.MaDT.ToString()).Equals(id));
                CTHoaDonViewIMEIVM.iMEI_DienThoais = await _mb.IMEI_DienThoai.Where(ct => (ct.MaHD.ToString() + ct.MaDT.ToString()).Equals(id)).ToListAsync();
                foreach(var im in CTHoaDonViewIMEIVM.iMEI_DienThoais)
                {
                    _mb.Attach(im);
                    _mb.Remove(im);
                }
                string location = "";
                for (int i = 0; i < CTHoaDonViewIMEIVM.chiTietHoaDon.SoLuong; i++)
                {
                    location = CTHoaDonViewIMEIVM.chiTietHoaDon.MaDT.ToString() + i.ToString();
                    string IMEI = Request.Form[location];
                   
                    if (_mb.IMEI_DienThoai.Where(im => (im.IMEI == IMEI) && !(im.MaHD.ToString() + im.MaDT.ToString()).Equals(id)).SingleOrDefault() != null)
                    {
                        check = false;
                        TempData[location] = "Trùng IMEI vui lòng thử lại";
                    }
                    else
                    {
                        TempData[location] = IMEI;
                    }
                }
               
                try
                {
                    if (!check)
                    {
                        throw new Exception();
                    }
                    for (int i = 0; i < CTHoaDonViewIMEIVM.chiTietHoaDon.SoLuong; i++)
                    {
                        location = CTHoaDonViewIMEIVM.chiTietHoaDon.MaDT.ToString() + i.ToString();
                        string IMEI = Request.Form[location];
                        
                        
                        
                        IMEI_DienThoai iMEI_DienThoai = new IMEI_DienThoai()
                        {
                            IMEI = IMEI,
                            MaDT = CTHoaDonViewIMEIVM.chiTietHoaDon.MaDT,
                            MaHD = CTHoaDonViewIMEIVM.chiTietHoaDon.MaHD
                        };
                        _mb.Add(iMEI_DienThoai);
                    }
                    await _mb.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["message"] = "Fail";
                    TempData[location] = "Trùng IMEI vui lòng thử lại";
                    return RedirectToAction("Edit", new { id });
                }
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}
