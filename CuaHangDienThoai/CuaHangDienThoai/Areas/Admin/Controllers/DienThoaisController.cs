using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using CuaHangDienThoai.Models;
using CuaHangDienThoai.Models.View;
using CuaHangDienThoai.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DienThoaisController : Controller
    {
        private readonly MobileContext _mb;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [BindProperty]
        public DienThoaisViewModel DienThoaiVM { get; set; }
        private int PageSize = 5;
        [Obsolete]
        public DienThoaisController(MobileContext mb, IHostingEnvironment hostingEnvironment)
        {
            _mb = mb;
            _hostingEnvironment = hostingEnvironment;
            DienThoaiVM = new DienThoaisViewModel()
            {
                ModelDienThoais = _mb.ModelDienThoai.ToList(),
                DienThoais = new Models.DienThoai()
            };
        }
        public IActionResult Index(int dienthoaisPage = 1, string searchName = null, string searchColor = null)
        {

            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                DienThoaisPaging dienThoaisPaging = new DienThoaisPaging()
                {
                    DienThoais = new List<Models.DienThoai>()
                };
                StringBuilder param = new StringBuilder();
                param.Append("/Admin/DienThoais?dienthoaisPage=:");
                param.Append("&searchName=");
                if (searchName != null)
                {
                    param.Append(searchName);
                }
                param.Append("&searchColor=");
                if (searchColor != null)
                {
                    param.Append(searchColor);
                }

                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                //var dienThoais = _mb.DienThoai.Include(h => h.ModelDienThoai).ToList();
                dienThoaisPaging.DienThoais = _mb.DienThoai.Include(h => h.ModelDienThoai).ToList();
                if (searchName != null)
                {
                    dienThoaisPaging.DienThoais = dienThoaisPaging.DienThoais.Where(dt => dt.ModelDienThoai.TenModel.ToLower().Contains(searchName.ToLower())).ToList();
                }
                if (searchColor != null)
                {
                    dienThoaisPaging.DienThoais = dienThoaisPaging.DienThoais.Where(dt => dt.Mau.ToLower().Contains(searchColor.ToLower())).ToList();
                }

                var count = dienThoaisPaging.DienThoais.Count;

                dienThoaisPaging.DienThoais = dienThoaisPaging.DienThoais.OrderByDescending(p => p.MaDT)
                    .Skip((dienthoaisPage - 1) * PageSize)
                    .Take(PageSize).ToList();


                dienThoaisPaging.PagingInfo = new PagingInfo
                {
                    CurrentPage = dienthoaisPage,
                    ItemsPerPage = PageSize,
                    TotalItems = count,
                    urlParam = param.ToString()
                };

                return View(dienThoaisPaging);
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
                return View(DienThoaiVM);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
                return View(DienThoaiVM);

            //DienThoaiVM.DienThoais.Hinh = modelDT.Hinh;
            _mb.DienThoai.Add(DienThoaiVM.DienThoais);
            await _mb.SaveChangesAsync();


            //var modelDT = await _mb.ModelDienThoai.FindAsync(DienThoaiVM.DienThoais.MaModel);
            string webRootPart = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var dienThoai = _mb.DienThoai.Find(DienThoaiVM.DienThoais.MaDT);

            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPart, SD.ImageFolderDienThoai);
                var extension = Path.GetExtension(files[0].FileName);
                using (var filestream = new FileStream(Path.Combine(uploads, DienThoaiVM.DienThoais.MaDT + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);

                }
                dienThoai.Hinh = @"\" + SD.ImageFolderDienThoai + @"\" + DienThoaiVM.DienThoais.MaDT + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPart, SD.ImageFolderDienThoai + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPart + @"\" + SD.ImageFolderDienThoai + @"\" + DienThoaiVM.DienThoais.MaDT + ".png");
                dienThoai.Hinh = @"\" + SD.ImageFolderDienThoai + @"\" + DienThoaiVM.DienThoais.MaDT + ".png";
            }
            await _mb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? maDT)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                if (maDT == null)
                    return NotFound();
                DienThoaiVM.DienThoais = await _mb.DienThoai.Include(m => m.ModelDienThoai).SingleOrDefaultAsync(m => m.MaDT == maDT);
                if (DienThoaiVM.ModelDienThoais == null)
                    return NotFound();
                return View(DienThoaiVM);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int maDT)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var dienThoaiFromMb = _mb.DienThoai.Where(m => m.MaDT == DienThoaiVM.DienThoais.MaDT).FirstOrDefault();
                if (files.Count > 0 && files[0] != null)
                {
                    var uploads = Path.Combine(webRootPath, SD.ImageFolderDienThoai);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(dienThoaiFromMb.Hinh);
                    if (System.IO.File.Exists(Path.Combine(uploads, DienThoaiVM.DienThoais.MaDT + extension_old)))
                        System.IO.File.Delete(Path.Combine(uploads, DienThoaiVM.DienThoais.MaDT + extension_old));
                    using (var filestream = new FileStream(Path.Combine(uploads, DienThoaiVM.DienThoais.MaDT + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);

                    }
                    DienThoaiVM.DienThoais.Hinh = @"\" + SD.ImageFolderDienThoai + @"\" + DienThoaiVM.DienThoais.MaDT + extension_new;
                }
                if (DienThoaiVM.DienThoais.Hinh != null)
                    dienThoaiFromMb.Hinh = DienThoaiVM.DienThoais.Hinh;
                dienThoaiFromMb.MaModel = DienThoaiVM.DienThoais.MaModel;
                dienThoaiFromMb.Mau = DienThoaiVM.DienThoais.Mau;
                dienThoaiFromMb.Gia = DienThoaiVM.DienThoais.Gia;
                dienThoaiFromMb.GiamGia = DienThoaiVM.DienThoais.GiamGia;
                dienThoaiFromMb.SoLuong = DienThoaiVM.DienThoais.SoLuong;

                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(DienThoaiVM);
        }
        public async Task<IActionResult> Details(int? maDT)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                if (maDT == null)
                    return NotFound();
                DienThoaiVM.DienThoais = await _mb.DienThoai.Include(m => m.ModelDienThoai).SingleOrDefaultAsync(m => m.MaDT == maDT);
                if (DienThoaiVM.ModelDienThoais == null)
                    return NotFound();
                return View(DienThoaiVM);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
        }
        public async Task<IActionResult> Delete(int? maDT)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role != null)
            {
                ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                if (maDT == null)
                    return NotFound();
                DienThoaiVM.DienThoais = await _mb.DienThoai.Include(m => m.ModelDienThoai).SingleOrDefaultAsync(m => m.MaDT == maDT);
                if (DienThoaiVM.ModelDienThoais == null)
                    return NotFound();
                return View(DienThoaiVM);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> DeleteConfirmed(int maDT)
        {
            string webPathRood = _hostingEnvironment.WebRootPath;
            DienThoai dienThoai = await _mb.DienThoai.FindAsync(maDT);
            if (ModelState.IsValid)
            {
                var uploads = Path.Combine(webPathRood, SD.ImageFolderDienThoai);
                var extention = Path.GetExtension(dienThoai.Hinh);
                if (System.IO.File.Exists(Path.Combine(uploads, dienThoai.MaDT + extention)))
                    System.IO.File.Delete(Path.Combine(uploads, dienThoai.MaDT + extention));
                _mb.Remove(dienThoai);
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}