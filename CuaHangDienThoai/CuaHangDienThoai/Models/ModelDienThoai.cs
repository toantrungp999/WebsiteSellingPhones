using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CuaHangDienThoai.Models
{
    public class ModelDienThoai
    {
        [Key]
        public int MaModel { get; set; }
        [Display(Name = "Tên mẫu điện thoại")]
        public string TenModel { get; set; }
        public int MaHang { get; set; }
        public string Ram { get; set; }
        public string Rom { get; set; }
        [Display(Name = "Màn hình")]
        public string ManHinh { get; set; }
        public string Camera { get; set; }
        public string Pin { get; set; }
        public string Chip { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày ra mắt")]
        public DateTime NgayRaMat { get; set; }
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
        [Display(Name = "Hình")]
        public string Hinh { get; set; }
        public Hang Hang { get; set; }
        public ICollection<DienThoai> DienThoais { get; set; }
    }
}
