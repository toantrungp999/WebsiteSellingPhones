using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CuaHangDienThoai.Models;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using Microsoft.EntityFrameworkCore;
using CuaHangDienThoai.Areas.Customer.Identity;
using CuaHangDienThoai.Models.View;

namespace CuaHangDienThoai.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DangNhapController : Controller
    {
        private readonly MobileContext _db;
        [BindProperty]
        public DangKyViewModel DangKyVM { get; set; }
        public DangNhapController(MobileContext db)
        {
            _db = db;
            DangKyVM = new DangKyViewModel();
        }
        public IActionResult Index()
        {
            var taiKhoan = new TaiKhoan();
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                if (_db.TaiKhoan.Where(tk => (tk.TenTK == taiKhoan.TenTK && tk.MatKhau == MD5.GetMD5(taiKhoan.MatKhau) && tk.TrangThai == true)).FirstOrDefault() != null)
                {
                    taiKhoan = await _db.TaiKhoan.FindAsync(taiKhoan.TenTK);
                    taiKhoan.KhachHang = await _db.KhachHang.FindAsync(taiKhoan.MaKH);
                    var obj = new DangNhap()
                    {
                        MaKH = taiKhoan.MaKH,
                        TenKH = taiKhoan.KhachHang.TenKH
                    };
                    HttpContext.Session.SetObject("DangNhap", obj);
                    TempData["DangNhap"] = "Đăng nhập thành công, bạn có thể mua hàng rồi!!!";
                    if (TempData["MaModel"] == null)
                        return RedirectToAction("Index", "Home");
                    else
                    {
                        string maModel = TempData["MaModel"].ToString();
                        TempData["MaModel"] = null;

                        return Redirect("/Customer/Home/ChiTiet?MaModel=" + maModel);
                    }
                    

                }
                else
                {
                    TempData["DangNhap"] = "Sai tài khoản hoặc mật khẩu";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult DangKy()
        {
            return View(DangKyVM);
        }

        [HttpPost, ActionName("DangKy")]    
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyPost()
        {
            if(!ModelState.IsValid)
            {
                return View(DangKyVM);
            }
            if(await _db.TaiKhoan.FindAsync(DangKyVM.TaiKhoan.TenTK) != null)
            {
                TempData["DangKy"] = "Tên tài khoản này đã có người sử dụng, vui lòng thử lại";
                return View(DangKyVM);
            }
            DangKyVM.TaiKhoan.TrangThai = true;
            DangKyVM.TaiKhoan.KhachHang = DangKyVM.KhachHang;
            _db.KhachHang.Add(DangKyVM.KhachHang);
            _db.TaiKhoan.Add(DangKyVM.TaiKhoan);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("DangNhap");
            return RedirectToAction("Index", "Home");
        }

    }
}