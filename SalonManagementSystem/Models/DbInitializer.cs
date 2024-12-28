using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SalonManagementSystem.Models
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext _context)
        {
            // ضمان إنشاء قاعدة البيانات إذا لم تكن موجودة
            _context.Database.EnsureCreated();

            // التحقق إذا كان هناك مدراء موجودون بالفعل
            if (_context.Admin.Any())
            {
                return; // إذا كان هناك مدراء، لا تضف شيئًا
            }

            // تعريف مديرين فقط
            var admins = new[]
            {
                new Admin
                {
                    Email = "b201210575@gmail.com",
                    Password = "b201210575"
                },
                new Admin
                {
                    Email = "g201210587@gmail.com",
                    Password = "g201210587"
                }
            };

            // إضافة المديرين إلى قاعدة البيانات
            _context.Admin.AddRange(admins);
            _context.SaveChanges();
        }
    }
}
