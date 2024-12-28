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
            if (HttpContext.Session.GetInt32("Id") is null)
            {
                TempData["error"] = "please login first";
                return RedirectToAction("Login", "Home");
            }
            else
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

                // الحصول على الأوقات المحجوزة (مقارنة فقط بالوقت)
                var reservedTimes = _context.Appointments
                    .Where(a => a.ServiceId == serviceId)
                    .Select(a => a.AppointmentDateTime.TimeOfDay) // استخدام TimeOfDay للحصول على الوقت فقط
                    .ToList();

                // إزالة الأوقات المحجوزة من الأوقات المتاحة
                availableTimes = availableTimes
                    .Where(time => !reservedTimes.Contains(time))
                    .ToList();

                // إنشاء نموذج لعرض الأوقات
                var model = new AppointmentViewModel
                {
                    Service = service,
                    AvailableTimes = availableTimes
                };

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult ConfirmAppointment(int serviceId, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);
            if (service == null)
            {
                return NotFound();
            }
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // التأكد من أن المستخدم قد قام بتسجيل الدخول
            }
            // إنشاء الموعد
            var localAppointmentDateTime = appointmentDate.Add(appointmentTime); // دمج التاريخ والوقت
            var utcAppointmentDateTime = DateTime.SpecifyKind(localAppointmentDateTime, DateTimeKind.Utc); // تحديد النوع كـ UTC

            var appointment = new Appointment
            {
                ServiceId = serviceId,
                AppointmentDateTime = utcAppointmentDateTime, 
                UserId = userId.Value
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Your appointment has been booked successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {

                TempData["LoginMessage"] = "You are already logged in as an Admin.";
                return RedirectToAction("Welcome", "Admin");

            }

            // التحقق مما إذا كان المستخدم مسجل الدخول بالفعل كمدرس
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                TempData["LoginMessage"] = "You are already logged in as an Teacher.";
                return RedirectToAction("Welcome", "User");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.Login login)
        {
            if (ModelState.IsValid)
            {
                // البحث عن المدرس بناءً على البريد الإلكتروني وكلمة المرور
                var admin = _context.Admin
                 .FirstOrDefault(e => e.Email == login.Email && e.Password == login.Password);

                if (admin != null)
                {
                    HttpContext.Session.SetInt32("AdminId", admin.AdminId);

                    // تخزين كلمة المرور (ليس آمناً عادةً، يفضل استخدام التوكنات)
                    HttpContext.Session.SetString("SessionUser", login.Password);

                    return RedirectToAction("Welcome", "Admin");
                }
                var user = _context.Users
                 .FirstOrDefault(e => e.Email == login.Email && e.Password == login.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("Id", user.Id);

                    // تخزين كلمة المرور (ليس آمناً عادةً، يفضل استخدام التوكنات)
                    HttpContext.Session.SetString("SessionUser", login.Password);

                    return RedirectToAction("Welcome", "User");
                }
            }
            return View(login);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // إزالة كل بيانات الجلسة
            HttpContext.Session.Clear();

            // إعادة توجيه المستخدم إلى صفحة تسجيل الدخول
            return RedirectToAction("Index", "Home");
        }




        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register  model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email || u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password // تخزين كلمة المرور كما هي
                };

                // إضافة المستخدم إلى قاعدة البيانات
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // إعادة توجيه المستخدم إلى صفحة تسجيل الدخول
                return RedirectToAction("Login", "Home", new { message = "RegistrationSuccessful" });
            }

            return View(model);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
