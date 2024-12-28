using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonManagementSystem.Models;
using System.Linq;

namespace SalonManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Welcome()
        {
            if (HttpContext.Session.GetInt32("Id") is null)
            {
                TempData["error"] = "please login first";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> myAppointment()
        {
            if (HttpContext.Session.GetInt32("Id") is null)
            {
                TempData["error"] = "please login first";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("Id");

                // استرجاع المواعيد الخاصة بالمستخدم وتضمين بيانات الخدمة
                var appointments = await _context.Appointments
                    .Include(a => a.Service) // تضمين بيانات الخدمة
                    .Where(a => a.UserId == userId) // الفلترة بناءً على معرف المستخدم
                    .OrderBy(a => a.AppointmentDateTime) // ترتيب حسب التاريخ والوقت
                    .ToListAsync(); // استخدام await للحصول على النتيجة

                return View(appointments); // تمرير القائمة إلى العرض
            }
        }
        public IActionResult DeleteAppointment(int id)
        {
            // البحث عن الموعد باستخدام المعرف (id)
            var appointment = _context.Appointments.Find(id);
                // إزالة الموعد من قاعدة البيانات
                _context.Appointments.Remove(appointment);
                _context.SaveChanges(); // حفظ التغييرات
            

            // إعادة التوجيه إلى قائمة المواعيد
            return RedirectToAction("myAppointment");
        }






    }
}
