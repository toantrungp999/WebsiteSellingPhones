
namespace CuaHangDienThoai.Common
{
    public static class Compare
    {
        public static bool CompareString(string str1, string str2)
        {
            if(str1.Length==0 || str2.Length==0)
            {
                return false;
            }
            else if(str1.Length!=str2.Length)
            {
                return false;
            }
            int k = 0;
            for (int i=0;i<str1.Length;i++)
            {
                
                if (str1[i] == str2[i])
                    k++;
            }
            if (k != str1.Length)
                return false;
            return true;
        }

    }
}
