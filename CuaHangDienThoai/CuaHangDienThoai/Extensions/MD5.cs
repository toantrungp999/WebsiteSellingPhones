using System.Security.Cryptography;
using System.Text;

namespace CuaHangDienThoai.Extensions
{
    public static class MD5
    {
        public static string GetMD5(string chuoi)
        {
            StringBuilder str_md5 = new StringBuilder();
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5.Append(b.ToString("X2"));
            }

            return str_md5.ToString();
        }
    }
}
