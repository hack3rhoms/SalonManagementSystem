using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonManagementSystem.Models;
using System.Diagnostics;

namespace SalonManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // تعريف _context

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // تمرير السياق
        }

        public IActionResult Index()
        {
            var services = _context.Services.Take(3).ToList(); // جلب أول ثلاث خدمات
            return View(services);
        }
    

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Appointments(int serviceId)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);
            if (service == null)
            {
                return NotFound();
            }

            // تحديد الأوقات المتاحة بشكل افتراضي (9 صباحًا إلى 5 مساءً)
            var startHour = 9;
            var endHour = 17;
            var availableTimes = new List<TimeSpan>();
            for (int hour = startHour; hour <= endHour; hour++)
            {
                availableTimes.Add(new TimeSpan(hour, 0, 0)); // كل ساعة
                availableTimes.Add(new TimeSpan(hour, 30, 0)); // كل نصف ساعة
            }

            // إنشاء نموذج لعرض الأوقات
            var model = new AppointmentViewModel
            {
                Service = service,
                AvailableTimes = availableTimes
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult ConfirmAppointment(int serviceId, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);
            if (service == null)
            {
                return NotFound();
            }

            // إنشاء الموعد
            var localAppointmentDateTime = appointmentDate.Add(appointmentTime); // دمج التاريخ والوقت
            var utcAppointmentDateTime = DateTime.SpecifyKind(localAppointmentDateTime, DateTimeKind.Utc); // تحديد النوع كـ UTC

            var appointment = new Appointment
            {
                ServiceId = serviceId,
                AppointmentDateTime = utcAppointmentDateTime, // تخزين UTC
                UserId = "currentUserId" // معرف المستخدم إذا كنت تستخدم نظام تسجيل دخول
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Your appointment has been booked successfully!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
  






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
