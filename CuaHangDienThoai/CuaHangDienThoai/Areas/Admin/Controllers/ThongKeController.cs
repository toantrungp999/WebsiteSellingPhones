using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CuaHangDienThoai.Extensions;
using CuaHangDienThoai.Data;
using CuaHangDienThoai.Models;
using CuaHangDienThoai.Models.View;
using Microsoft.EntityFrameworkCore;

namespace CuaHangDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeController : Controller
    {
        public readonly MobileContext _db;
        [BindProperty]
        public ThongKeViewModel ThongKeVM { get; set; }
        
        public ThongKeController(MobileContext db)
        {
            _db = db;
            ThongKeVM = new ThongKeViewModel();
        }
        public IActionResult Index(string loaiThongKe = null, int? hang=null, string batDau = null, string ketThuc = null, bool? tatCa =null)
        {
            ViewBag.khachHangAndDonHangs = News.SendName(_db);
            ThongKeVM.DanhSachHang = _db.Hang.Where(h => h.TrangThai == true).ToList();
            if (tatCa == false)
            {
                if (batDau == null || ketThuc == null)
                {
                    TempData["ThongKe"] = "Vui lòng chọn khoảng thời gian";
                    return View(ThongKeVM);
                }
            }
            //else
            //{
            //    if (loaiThongKe == "DoanhSoTheoDT" || loaiThongKe == "DoanhThuTheoDT")
            //        return RedirectToAction("DoanhThuVaDoanhSoTheoDT", new { loaiThongKe = loaiThongKe, hang = hang, batDau = Convert.ToDateTime("1/1/2000").Date.ToString(), ketThuc = DateTime.Now.Date.ToString(), tatCa = tatCa });
            //    else if (loaiThongKe == "DoanhThuTheoNgay")
            //        return RedirectToAction("DoanhThuTheoNgay", new { loaiThongKe = loaiThongKe, hang = hang, batDau = Convert.ToDateTime("1/1/2000").Date.ToString(), ketThuc = DateTime.Now.Date.ToString(), tatCa = tatCa });
            //}
            if (loaiThongKe == "DoanhSoTheoDT" || loaiThongKe == "DoanhThuTheoDT")
                return RedirectToAction("DoanhThuVaDoanhSoTheoDT", new { loaiThongKe = loaiThongKe, hang = hang, batDau = batDau, ketThuc = ketThuc, tatCa = tatCa });
            else if(loaiThongKe == "DoanhThuTheoNgay")
                return RedirectToAction("DoanhThuTheoNgay", new { loaiThongKe = loaiThongKe, hang = hang, batDau = batDau, ketThuc = ketThuc, tatCa = tatCa });

            TempData["ThongKe"] = "Vui lòng chọn loại thống kê";
            return View(ThongKeVM);
        }
        public IActionResult DoanhThuVaDoanhSoTheoDT(string loaiThongKe = null, int? hang = null, string batDau = null, string ketThuc = null, bool? tatCa = null)
        {
            ViewBag.khachHangAndDonHangs = News.SendName(_db);
            ThongKeVM.DanhSachHang = _db.Hang.Where(h => h.TrangThai == true).ToList();

            //var listThongKe1 = _db.ChiTietHoaDon.Include(ct=>ct.HoaDon).Where(ct=>ct.HoaDon.NgayLapHD >= batDau && ct.HoaDon.NgayLapHD<=ketThuc)
            //    .GroupBy(ct => ct.MaDT).Select(g => new
            //{
            //    MaDT = g.Key,
            //    SoLuong = g.Sum(jn => jn.SoLuong),
            //    DoanhThu = g.Sum(jn => jn.TongGia)
            //}).OrderByDescending(g=>g.SoLuong).ToList();

            ThongKeVM.DanhSachDoanhSoVaDanhThuDT = new List<DoanhSoVaDanhThuDT>();
            if(loaiThongKe == "DoanhThuTheoDT")
            {
                if (tatCa == true)
                {
                    ThongKeVM.TenThongKe = "Doanh thu theo điện thoại";
                }
                else
                {
                    ThongKeVM.TenThongKe = "Doanh thu theo điện thoại từ ngày " + Convert.ToDateTime(batDau).ToString("dd/MM/yyyy") + " đến " + Convert.ToDateTime(ketThuc).ToString("dd/MM/yyyy");
                }
            }
            else if(loaiThongKe == "DoanhSoTheoDT")
            {
                if (tatCa == true)
                {
                    ThongKeVM.TenThongKe = "Doanh số theo điện thoại";
                }
                else
                {
                    ThongKeVM.TenThongKe = "Doanh số theo điện thoại từ ngày " + Convert.ToDateTime(batDau).ToString("dd/MM/yyyy") + " đến " + Convert.ToDateTime(ketThuc).ToString("dd/MM/yyyy");
                }
            }
            if (hang != null)
                ThongKeVM.TenThongKe = ThongKeVM.TenThongKe + " - hãng " + _db.Hang.Find(hang).TenHang;

            DateTime ngayBatDau;
            DateTime ngayKetThuc;
            if(tatCa==true)
            {
                ngayBatDau = Convert.ToDateTime("1/1/2000").Date;
                ngayKetThuc = DateTime.Now.AddDays(1).Date;
            }
            else
            {
                ngayBatDau = Convert.ToDateTime(batDau).Date;
                ngayKetThuc = Convert.ToDateTime(ketThuc).AddDays(1).Date;
            }
            if (hang == null)
            {
                var listThongKe = _db.ChiTietHoaDon.Where(ct => ct.HoaDon.NgayLapHD >= ngayBatDau && ct.HoaDon.NgayLapHD <= ngayKetThuc)
                        .Join(_db.DienThoai, ct => ct.MaDT, dt => dt.MaDT, (ct, dt) => new { dt.MaModel, ct.SoLuong, ct.TongGia })
                        .GroupBy(jn => jn.MaModel).Select(g => new
                        {
                            MaModel = g.Key,
                            DoanhSo = g.Sum(jn => jn.SoLuong),
                            DoanhThu = g.Sum(jn => jn.TongGia)
                        }).OrderByDescending(jn => jn.DoanhSo).ToList();


                foreach (var item in listThongKe)
                {
                    ThongKeVM.DanhSachDoanhSoVaDanhThuDT.Add(new DoanhSoVaDanhThuDT()
                    {
                        ModelDT = _db.ModelDienThoai.Find(item.MaModel),
                        SoLuong = item.DoanhSo,
                        DoanhThu = item.DoanhThu
                    });
                }
            }
            else
            {
                var listThongKe = _db.ChiTietHoaDon.Where(ct => ct.HoaDon.NgayLapHD >= ngayBatDau && ct.HoaDon.NgayLapHD <= ngayKetThuc)
                       .Join(_db.DienThoai.Include(dt => dt.ModelDienThoai).Where(dt => dt.ModelDienThoai.MaHang == hang), ct => ct.MaDT, dt => dt.MaDT, (ct, dt) => new { dt.MaModel, ct.SoLuong, ct.TongGia })
                       .GroupBy(jn => jn.MaModel).Select(g => new
                        {
                            MaModel = g.Key,
                            DoanhSo = g.Sum(jn => jn.SoLuong),
                            DoanhThu = g.Sum(jn => jn.TongGia)
                        }).OrderByDescending(jn => jn.DoanhSo).ToList();


                foreach (var item in listThongKe)
                {
                    ThongKeVM.DanhSachDoanhSoVaDanhThuDT.Add(new DoanhSoVaDanhThuDT()
                    {
                        ModelDT = _db.ModelDienThoai.Find(item.MaModel),
                        SoLuong = item.DoanhSo,
                        DoanhThu = item.DoanhThu
                    });
                }
            }
            if(loaiThongKe == "DoanhThuTheoDT")
                return View("DoanhThuTheoDT", ThongKeVM);
            if(loaiThongKe == "DoanhSoTheoDT")
                return View("DoanhSoTheoDT", ThongKeVM);

            return RedirectToAction("Index");
        }


        public IActionResult DoanhThuTheoNgay(string loaiThongKe = null, int? hang = null, string batDau = null, string ketThuc = null, bool? tatCa = null)
        {
            ViewBag.khachHangAndDonHangs = News.SendName(_db);
            ThongKeVM.DanhSachHang = _db.Hang.Where(h => h.TrangThai == true).ToList();

            if (tatCa == true)
            {
                ThongKeVM.TenThongKe = "Doanh thu theo ngày";
            }
            else
            {
                ThongKeVM.TenThongKe = "Doanh thu theo ngày từ " + Convert.ToDateTime(batDau).ToString("dd/MM/yyyy") + " đến " + Convert.ToDateTime(ketThuc).ToString("dd/MM/yyyy");
            }
            if (hang != null)
                ThongKeVM.TenThongKe = ThongKeVM.TenThongKe + " - hãng " + _db.Hang.Find(hang).TenHang;

            ThongKeVM.DanhSachDoanhThuNgay = new List<DoanhThuNgay>();

            DateTime ngayBatDau;
            DateTime ngayKetThuc;
            if (tatCa == true)
            {
                ngayBatDau = Convert.ToDateTime("1/1/2000").Date;
                ngayKetThuc = DateTime.Now.AddDays(1).Date;
            }
            else
            {
                ngayBatDau = Convert.ToDateTime(batDau).Date;
                ngayKetThuc = Convert.ToDateTime(ketThuc).AddDays(1).Date;
            }

            if (hang == null)
            {
                var listThongKe = _db.ChiTietHoaDon.Where(ct => ct.HoaDon.NgayLapHD >= ngayBatDau && ct.HoaDon.NgayLapHD <= ngayKetThuc)
                    .Select(ct => new { TongGia = ct.TongGia, NgayLapHD = ct.HoaDon.NgayLapHD }).ToList()
                    .GroupBy(sl => sl.NgayLapHD.Date)
                    .Select(g => new DoanhThuNgay
                    {
                        Ngay = g.Key,
                        DoanhThu = g.Sum(sl => sl.TongGia)
                    }).OrderByDescending(dt => dt.Ngay).ToList();

                ThongKeVM.DanhSachDoanhThuNgay = listThongKe;
            }
            else 
            {
                var listThongKe = _db.ChiTietHoaDon.Where(ct => ct.HoaDon.NgayLapHD >= ngayBatDau && ct.HoaDon.NgayLapHD <= ngayKetThuc && ct.DienThoai.ModelDienThoai.MaHang == hang)
                    .Select(ct => new { TongGia = ct.TongGia, NgayLapHD = ct.HoaDon.NgayLapHD }).ToList()
                    .GroupBy(sl => sl.NgayLapHD.Date)
                    .Select(g => new DoanhThuNgay
                    {
                        DoanhThu = g.Sum(sl => sl.TongGia)
                    }).OrderByDescending(dt => dt.Ngay).ToList();

                ThongKeVM.DanhSachDoanhThuNgay = listThongKe;
            }
            return View(ThongKeVM);
        }
    }
}
                 