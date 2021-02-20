using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Models;
using Microsoft.AspNetCore.Http;
using CuaHangDienThoai.Common;
using CuaHangDienThoai.Extensions;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaiKhoanAdminsController : Controller
    {
        private readonly MobileContext _mb;

        public TaiKhoanAdminsController(MobileContext mb)
        {
            _mb = mb;
        }

        // GET: Admin/TaiKhoanAdmins
        public async Task<IActionResult> Index()
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
            return View(await _mb.TaiKhoanAdmin.ToListAsync());
        }

        // GET: Admin/TaiKhoanAdmins/Details/5
        public async Task<IActionResult> Details(string id)
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
            if (id == null)
            {
                return NotFound();
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var taiKhoanAdmin = await _mb.TaiKhoanAdmin
                .FirstOrDefaultAsync(m => m.User == id);
            if (taiKhoanAdmin == null)
            {
                return NotFound();
            }

            return View(taiKhoanAdmin);
        }

        // GET: Admin/TaiKhoanAdmins/Create
        public IActionResult Create()
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoanAdmin taiKhoanAdmin)
        {
            if (ModelState.IsValid)
            {
                if(_mb.TaiKhoanAdmin.Where(tk => tk.User == taiKhoanAdmin.User).SingleOrDefault() != null)
                {
                    ModelState.AddModelError("", "User đã tồn tại vui lòng nhập lại!");
                }
                else
                {
                    var lst = _mb.TaiKhoanAdmin.Where(tk => tk.Email== taiKhoanAdmin.Email).FirstOrDefault();
                    if (!Compare.CompareString(lst.Email, taiKhoanAdmin.Email))
                    {
                        ModelState.AddModelError("", "Email đã tồn tại vui lòng nhập lại!");
                    }
                    else
                    {
                        taiKhoanAdmin.Pass = MD5.GetMD5(taiKhoanAdmin.Pass);
                        _mb.Add(taiKhoanAdmin);
                        await _mb.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
   
               
            }
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            return View(taiKhoanAdmin);
        }

        // GET: Admin/TaiKhoanAdmins/Edit/5
        public async Task<IActionResult> Edit(string id)
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
            if (id == null)
            {
                return NotFound();
            }
             ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var taiKhoanAdmin = await _mb.TaiKhoanAdmin.FindAsync(id);
            if (taiKhoanAdmin == null)
            {
                return NotFound();
            }
            return View(taiKhoanAdmin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,TaiKhoanAdmin taiKhoanAdmin)
        {
            if (id != taiKhoanAdmin.User)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var taiKhoan = _mb.TaiKhoanAdmin.Where(tk => tk.User == taiKhoanAdmin.User).SingleOrDefault();
                if(taiKhoan.Email != taiKhoanAdmin.Email)
                {
                    if (_mb.TaiKhoanAdmin.Where(tk => Compare.CompareString(tk.Email, taiKhoanAdmin.Email)).SingleOrDefault() != null)
                    {
                        ViewBag.khachHangAndDonHangs = News.SendName(_mb);
                        ModelState.AddModelError("", "Email đã tồn tại vui lòng nhập lại!");
                        return View(taiKhoanAdmin);
                    }
                }

                try
                {
                    var _taiKhoan = _mb.TaiKhoanAdmin.Where(u => u.User == id).SingleOrDefault();
                    _taiKhoan.Name = taiKhoanAdmin.Name;
                    _taiKhoan.PhoneNumber = taiKhoanAdmin.PhoneNumber;
                    _taiKhoan.Role = taiKhoanAdmin.Role;
                    _taiKhoan.Email = taiKhoanAdmin.Email;
                    _taiKhoan.TrangThai = taiKhoanAdmin.TrangThai;
                    await _mb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanAdminExists(taiKhoanAdmin.User))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoanAdmin);
        }
        public async Task<IActionResult> EditMatKhau(string user)
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
            if (user == null || user.Trim().Length == 0)
                return NotFound();
            ViewBag.khachHangAndDonHangs = News.SendName(_mb);
            var taiKhoans = await _mb.TaiKhoanAdmin.FindAsync(user);

            if (taiKhoans == null)
                return null;
            return View(taiKhoans);
        }
        [HttpPost, ActionName("EditMatKhau")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMatKhauPost(string user, TaiKhoanAdmin taiKhoan)
        {
            if (taiKhoan.User != user)
                return NotFound();
            if (ModelState.IsValid)
            {
                var _taiKhoan = _mb.TaiKhoanAdmin.Where(u => u.User == user).SingleOrDefault();
                _taiKhoan.Pass = MD5.GetMD5(taiKhoan.Pass);
                await _mb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }
        private bool TaiKhoanAdminExists(string id)
        {
            return _mb.TaiKhoanAdmin.Any(e => e.User == id);
        }
    }
}
