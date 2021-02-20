using System.Collections.Generic;

namespace CuaHangDienThoai.Extensions
{
    public static class SoSanh
    {
        public static bool TonTai(List<string> danhSach, string chuoi)
        {
            foreach(string a in danhSach)
            {
                if (a == chuoi)
                    return true;
            }
            return false;
        }
    }
}
