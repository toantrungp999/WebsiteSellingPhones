using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CuaHangDienThoai.Models;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
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
                var tk = _db.TaiKhoan.Find(taiKhoan.TenTK);
                if(tk == null)
                {
                    TempData["DangNhap"] = "Tên đăng nhập không chính xác";
                    return RedirectToAction("Index");
                }
                else
                {
                    if(tk.TenTK != taiKhoan.TenTK)
                    {
                        TempData["DangNhap"] = "Tên đăng nhập không chính xác";
                        return RedirectToAction("Index");
                    }
                    else if(tk.MatKhau != MD5.GetMD5(taiKhoan.MatKhau))
                    {
                        TempData["DangNhap"] = "Mật khẩu không chính xác";
                        return View("Index", taiKhoan);
                    }
                    else
                    {
                        tk.KhachHang = await _db.KhachHang.FindAsync(tk.MaKH);
                        var obj = new DangNhap()
                        {
                            MaKH = tk.MaKH,
                            TenKH = tk.KhachHang.TenKH
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

            if(_db.TaiKhoan.Find(DangKyVM.TaiKhoan.TenTK)!=null)
            {
                TempData["DangKyTK"] = "Tên tài khoản này đã có người sử dụng, vui lòng thử lại";
                return View(DangKyVM);
            }

            if (SoSanh.TonTai(_db.KhachHang.Where(kh=>kh.Email==DangKyVM.KhachHang.Email).Select(kh=>kh.Email).ToList(),DangKyVM.KhachHang.Email))
            {
                TempData["DangKyEmail"] = "Email này đã có người sử dụng, vui lòng thử lại";
                return View(DangKyVM);
            }
            DangKyVM.TaiKhoan.TrangThai = true;
            DangKyVM.TaiKhoan.MatKhau = MD5.GetMD5(DangKyVM.TaiKhoan.MatKhau);
            _db.KhachHang.Add(DangKyVM.KhachHang);
            DangKyVM.TaiKhoan.KhachHang = DangKyVM.KhachHang;
            _db.TaiKhoan.Add(DangKyVM.TaiKhoan);
            await _db.SaveChangesAsync();
            TempData["DangKy"] = "Đăng ký thành công, hãy đăng nhập để mua sản phẩm";
            return RedirectToAction("Index");
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("DangNhap");
            return RedirectToAction("Index", "Home");
        }

    }
}