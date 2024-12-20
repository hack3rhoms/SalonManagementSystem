using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonManagementSystem.Models;



namespace SalonManagementSystem.Controllers
{
    public class Admin : Controller
    {
        private readonly ApplicationDbContext _context;
        public Admin(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult ManageEmployees()
        {
            return View();
        }


        // Add new Employee
        public IActionResult AddEmployee()
        {
            // استرجاع الساعات المحجوزة
            var bookedSlots = _context.Employees
                .Select(e => new { e.StartWorkingHours, e.EndWorkingHours })
                .ToList();

            // تحديد جميع الأوقات الممكنة (مثال: من 8 صباحًا إلى 6 مساءً)
            var allSlots = new List<TimeSpan>();
            for (var time = TimeSpan.FromHours(8); time < TimeSpan.FromHours(18); time += TimeSpan.FromMinutes(60))
            {
                allSlots.Add(time);
            }

            // استثناء الساعات المحجوزة
            var availableSlots = allSlots
                .Where(slot => !bookedSlots.Any(b => slot >= b.StartWorkingHours && slot < b.EndWorkingHours))
                .ToList();

            ViewBag.AvailableSlots = availableSlots;
            ViewBag.BookedSlots = bookedSlots;

            // جلب قائمة الخدمات
            var services = _context.Services
                .Select(s => new { s.Id, s.Name }) // استرجاع معرّف الخدمة واسمها فقط
                .ToList();

            // إرسال الخدمات إلى الواجهة
            ViewBag.Services = services;

            return View();
        }



        // view 
        public IActionResult ViewEmployees()
        {
            // استرجاع جميع الموظفين مع الخدمات المرتبطة
            var employees = _context.Employees
                .Include(e => e.Service) // جلب الخدمة المرتبطة مع كل موظف
                .ToList();

            // إرسال البيانات إلى الواجهة
            return View(employees);
        }

        // ManageServices

        public IActionResult ManageServices()
        {
            // عرض صفحة إنشاء خدمة جديدة
            return View();
        }
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return RedirectToAction("ViewServices");
            }
            return View(service);
        }

        // صفحة عرض الخدمات
        public IActionResult ViewServices()
        {
            var services = _context.Services.ToList();
            return View(services);
        }


    }
}
