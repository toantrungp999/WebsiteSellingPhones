using CuaHangDienThoai.Data;
using CuaHangDienThoai.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;


namespace CuaHangDienThoai.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MobileContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MobileContext>>()))
            {
                // Look for any movies.
                if (context.TaiKhoanAdmin.Any())
                {
                    return;   // DB has been seeded
                }

                context.TaiKhoanAdmin.AddRange(
                    new TaiKhoanAdmin
                    {
                        User = "SuperAdmin",
                        Pass = MD5.GetMD5("SuperAdmin"),
                        Name = "Phạm Toàn Trung",
                        PhoneNumber = "0947902015",
                        Role = "Super Admin",
                        TrangThai = true
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
