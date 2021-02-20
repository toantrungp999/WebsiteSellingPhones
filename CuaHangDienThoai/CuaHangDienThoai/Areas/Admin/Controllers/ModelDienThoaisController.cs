using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Models.View;
using CuaHangDienThoai.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Extensions;
using System.Text;
using CuaHangDienThoai.Models;
using System.Collections.Generic;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModelDienThoaisController : Controller
    {
        private readonly MobileContext _mb;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [BindProperty]
        public ModelDienThoaiViewModel ModelDienThoaiVM { get; set; }
        private int PageSize = 5;
        [Obsolete]
        public ModelDienThoaisController(MobileContext mb, IHostingEnvironment hostingEnvironment)
        {
            _mb = mb;
            _hostingEnvironment = hostingEnvironment;
            ModelDienThoaiVM = new ModelDienThoaiViewModel()
            {
                Hangs = _mb.Hang.ToList(),
                ModelDienThoais = new Models.ModelDienThoai()
            };
        }
        public async Task<IActionResult> Index(int modeldienthoaisPage = 1, string searchName = null, string searchHang = null, string searchRam = null, string searchRom = null)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role==null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            ModelDienThoaisPaging ModelDienThoaisPaging = new ModelDienThoaisPaging()
            {
                ModelDienThoais = new List<Models.ModelDienThoai>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/ModelDienThoais?modeldienthoaisPage=:");
            param.Append("&searchName=");
            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchHang=");
            if (searchRom != null)
            {
                param.Append(searchHang);
            }
            param.Append("&searchRam=");
            if (searchRom != null)
            {
                param.Append(searchRam);
            }
            param.Append("&searchRom=");
            if (searchRom != null)
            {
                param.Append(searchRom);
            }

            
            ModelDienThoaisPaging.ModelDienThoais = _mb.ModelDienThoai.Include(m=>m.Hang).ToList();
            if (searchName != null)
            {
                ModelDienThoaisPaging.ModelDienThoais = ModelDienThoaisPaging.ModelDienThoais.Where(dt => dt.TenModel.ToLower().Contains(searchName.ToLower())).ToList();
            }
            if (searchHang != null)
            {
                ModelDienThoaisPaging.ModelDienThoais = ModelDienThoaisPaging.ModelDienThoais.Where(dt => dt.Hang.TenHang.ToLower().Contains(searchHang.ToLower())).ToList();
            }
            if (searchRam != null)
            {
                ModelDienThoaisPaging.ModelDienThoais = ModelDienThoaisPaging.ModelDienThoais.Where(dt => dt.Ram.ToLower().Contains(searchRam.ToLower())).ToList();
            }
            if (searchRom != null)
            {
                ModelDienThoaisPaging.ModelDienThoais = ModelDienThoaisPaging.ModelDienThoais.Where(dt => dt.Rom.ToLower().Contains(searchRom.ToLower())).ToList();
            }
            var count = ModelDienThoaisPaging.ModelDienThoais.Count;

            ModelDienThoaisPaging.ModelDienThoais = ModelDienThoaisPaging.ModelDienThoais.OrderByDescending(p => p.MaModel)
                .Skip((modeldienthoaisPage - 1) * PageSize)
                .Take(PageSize).ToList();


            ModelDienThoaisPaging.PagingInfo = new PagingInfo
            {
                CurrentPage = modeldienthoaisPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
           
            return View(ModelDienThoaisPaging);
        }

        public IActionResult Create()
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            return View(ModelDienThoaiVM);
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
                return View(ModelDienThoaiVM);
            _mb.ModelDienThoai.Add(ModelDienThoaiVM.ModelDienThoais);
            await _mb.SaveChangesAsync();

            string webRootPart = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var productFromDb = _mb.ModelDienThoai.Find(ModelDienThoaiVM.ModelDienThoais.MaModel);

            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPart, SD.ImageFolderModelDienThoai);
                var extension = Path.GetExtension(files[0].FileName);
                using (var filestream = new FileStream(Path.Combine(uploads, ModelDienThoaiVM.ModelDienThoais.MaModel + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);

                }
                productFromDb.Hinh = @"\" + SD.ImageFolderModelDienThoai + @"\" + ModelDienThoaiVM.ModelDienThoais.MaModel + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPart, SD.ImageFolderModelDienThoai + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPart + @"\" + SD.ImageFolderModelDienThoai + @"\" + ModelDienThoaiVM.ModelDienThoais.MaModel + ".png");
                productFromDb.Hinh = @"\" + SD.ImageFolderModelDienThoai + @"\" + ModelDienThoaiVM.ModelDienThoais.MaModel + ".png";
            }
            await _mb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? maModel)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            if (maModel == null)
                return NotFound();
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            ModelDienThoaiVM.ModelDienThoais = await _mb.ModelDienThoai.Include(m => m.Hang).SingleOrDefaultAsync(m => m.MaModel == maModel);
            if (ModelDienThoaiVM.ModelDienThoais == null)
                return NotFound();
            return View(ModelDienThoaiVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int maModel)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                
                var files = HttpContext.Request.Form.Files;
                var modelDienThoaiFromMb = _mb.ModelDienThoai.Where(m => m.MaModel == ModelDienThoaiVM.ModelDienThoais.MaModel).FirstOrDefault();
                if (files.Count > 0 && files[0] != null)
                {
                    var uploads = Path.Combine(webRootPath, SD.ImageFolderModelDienThoai);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(modelDienThoaiFromMb.Hinh);
                    if (System.IO.File.Exists(Path.Combine(uploads, ModelDienThoaiVM.ModelDienThoais.MaModel + extension_old)))
                        System.IO.File.Delete(Path.Combine(uploads, ModelDienThoaiVM.ModelDienThoais.MaModel + extension_old));
                    using (var filestream = new FileStream(Path.Combine(uploads, ModelDienThoaiVM.ModelDienThoais.MaModel + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);

                    }
                    ModelDienThoaiVM.ModelDienThoais.Hinh = @"\" + SD.ImageFolderModelDienThoai + @"\" + ModelDienThoaiVM.ModelDienThoais.MaModel + extension_new;
                }
                if (ModelDienThoaiVM.ModelDienThoais.Hinh != null)
                    modelDienThoaiFromMb.Hinh = ModelDienThoaiVM.ModelDienThoais.Hinh;
                modelDienThoaiFromMb.TenModel = ModelDienThoaiVM.ModelDienThoais.TenModel;
                modelDienThoaiFromMb.MaHang = ModelDienThoaiVM.ModelDienThoais.MaHang;
                modelDienThoaiFromMb.Ram = ModelDienThoaiVM.ModelDienThoais.Ram;
                modelDienThoaiFromMb.Rom = ModelDienThoaiVM.ModelDienThoais.Rom;
                modelDienThoaiFromMb.ManHinh = ModelDienThoaiVM.ModelDienThoais.ManHinh;
                modelDienThoaiFromMb.Camera = ModelDienThoaiVM.ModelDienThoais.Camera;
                modelDienThoaiFromMb.Chip = ModelDienThoaiVM.ModelDienThoais.Chip;
                modelDienThoaiFromMb.NgayRaMat = ModelDienThoaiVM.ModelDienThoais.NgayRaMat;
                modelDienThoaiFromMb.TrangThai = ModelDienThoaiVM.ModelDienThoais.TrangThai;
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ModelDienThoaiVM);
        }
        public async Task<IActionResult> Details(int? maModel)
        {
            var Role = HttpContext.Session.GetString(CommonAdmin.ROLE_SESSION);
            if (Role == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Identity" });
            }
            if (maModel == null)
                return NotFound();
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            ModelDienThoaiVM.ModelDienThoais = await _mb.ModelDienThoai.Include(m => m.Hang).SingleOrDefaultAsync(m => m.MaModel == maModel); 
            if (ModelDienThoaiVM.ModelDienThoais == null)
                return NotFound();
            return View(ModelDienThoaiVM);
        }
    }
}