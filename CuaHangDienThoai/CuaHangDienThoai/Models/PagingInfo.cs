﻿using System;

namespace CuaHangDienThoai.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int totalPage => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

        //This will be used to build URL
        public string urlParam { get; set; }
    }
}
