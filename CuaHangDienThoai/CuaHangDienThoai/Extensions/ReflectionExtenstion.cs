namespace CuaHangDienThoai.Extensions
{
    public static class ReflectionExtenstion
    {
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }
    }
}
