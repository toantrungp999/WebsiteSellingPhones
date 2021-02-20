using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace CuaHangDienThoai.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem> ToSelectListHang<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("TenHang"),
                       Value = item.GetPropertyValue("MaHang"),
                       Selected = item.GetPropertyValue("MaHang").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem> ToSelectListModelDienThoai<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("TenModel"),
                       Value = item.GetPropertyValue("MaModel"),
                       Selected = item.GetPropertyValue("MaModel").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem> ToSelectTenKH<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("TenKH"),
                       Value = item.GetPropertyValue("MaKH"),
                       Selected = item.GetPropertyValue("MaKH").Equals(selectedValue.ToString())
                   };
        }

        public static IEnumerable<SelectListItem> ListSapXep()
        {
            SelectListItem sli1 = new SelectListItem
            {
                Text = "Cao --> Thấp",
                Value = "CaoDenThap"
            };
            SelectListItem sli2 = new SelectListItem
            {
                Text = "Thấp --> Cao",
                Value = "ThapDenCao"
            };
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(sli1);
            li.Add(sli2);
            return li;
        }

        public static IEnumerable<SelectListItem> ListTrangThaiDonHang()
        {
            SelectListItem sli1 = new SelectListItem()
            {
                Text = "Chưa duyệt",
                Value = "Chưa duyệt"
            };
            SelectListItem sli2 = new SelectListItem()
            {
                Text = "Đã duyệt",
                Value = "Đã duyệt"
            };
            SelectListItem sli3 = new SelectListItem()
            {
                Text = "Đã hủy",
                Value = "Đã hủy"
            };
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(sli1);
            li.Add(sli2);
            li.Add(sli3);
            return li;
        }

        public static IEnumerable<SelectListItem> ListThongKe()
        {
            SelectListItem sli1 = new SelectListItem
            {
                Text = "Doanh số theo điện thoại",
                Value = "DoanhSoTheoDT"
            };
            SelectListItem sli2 = new SelectListItem
            {
                Text = "Doanh thu theo điện thoại",
                Value = "DoanhThuTheoDT"
            };
            SelectListItem sli3 = new SelectListItem
            {
                Text = "Doanh thu theo ngày",
                Value = "DoanhThuTheoNgay"
            };
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(sli1);
            li.Add(sli2);
            li.Add(sli3);
            return li;
        }
    }
}
