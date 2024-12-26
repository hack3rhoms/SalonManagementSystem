using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonManagementSystem.Models;

namespace SalonManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
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
            ViewBag.Services = _context.Services.ToList(); // جلب قائمة الخدمات
            ViewBag.TimeSlots = GetTimeSlots(); // جلب قائمة الأوقات
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            ViewBag.Services = _context.Services.ToList();
            ViewBag.TimeSlots = GetTimeSlots();

            var isOverlap = _context.Employees
                .Any(e => e.ServiceId == employee.ServiceId &&
                          e.StartWorkingHours < employee.EndWorkingHours &&
                          employee.StartWorkingHours < e.EndWorkingHours);

            if (isOverlap)
            {
                ModelState.AddModelError("", "There is a time overlap with another employee for the same service.");
                return View(employee);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewEmployees");
        }
        [HttpGet]
        private List<SelectListItem> GetTimeSlots(int serviceId = 0)
        {
            var timeSlots = new List<SelectListItem>();
            var startTime = new TimeSpan(8, 0, 0); // الساعة 8:00 صباحًا
            var endTime = new TimeSpan(22, 0, 0); // الساعة 10:00 مساءً

            // جلب الأوقات المحجوزة بالفعل
            var bookedTimes = _context.Employees
                .Where(e => e.ServiceId == serviceId)
                .Select(e => e.StartWorkingHours)
                .ToList();

            for (var time = startTime; time <= endTime; time = time.Add(new TimeSpan(0, 30, 0))) // فواصل نصف ساعة
            {
                var isBooked = bookedTimes.Contains(time); // تحقق إذا كان الوقت محجوز
                timeSlots.Add(new SelectListItem
                {
                    Value = time.ToString(@"hh\:mm"),
                    Text = time.ToString(@"hh\:mm"),
                    Disabled = isBooked // تعطيل الوقت إذا كان محجوز
                });
            }
            return timeSlots;
        }



        // عرض قائمة الموظفين
        public IActionResult ViewEmployees()
        {
            var employees = _context.Employees.Include(e => e.Service).ToList();
            return View(employees);
        }
        // إجراء لحذف الموظف
        public IActionResult DeleteEmployee(int id)
        {
            // البحث عن الموظف باستخدام المعرف (id)
            var employee = _context.Employees.Find(id);

            // التحقق إذا كان الموظف موجودًا
            if (employee != null)
            {
                // إزالة الموظف من قاعدة البيانات
                _context.Employees.Remove(employee);
                _context.SaveChanges(); // حفظ التغييرات
            }

            // إعادة التوجيه إلى قائمة الموظفين
            return RedirectToAction("ViewEmployees");
        }




        // Manage Services
        public IActionResult ManageServices()
        {
                                    
              return View();
        }

         public IActionResult AddService()
         {
              return View();
         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddService(Service service, IFormFile ServiceImage)
        {
            
                using (var memoryStream = new MemoryStream())
                {
                    ServiceImage.CopyTo(memoryStream);
                    service.ImageData = memoryStream.ToArray(); // تخزين البيانات كـ byte[]
                    service.ImageType = ServiceImage.ContentType; // تخزين نوع الصورة (مثل image/jpeg)
                }

            // حفظ البيانات في قاعدة البيانات
            _context.Services.Add(service);
            _context.SaveChanges();
            return RedirectToAction("ViewServices");
        }

        // View Services
        public IActionResult ViewServices()
         {
            var services = _context.Services.ToList();
            return View(services);
         }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewServices");
        }


        //New 
        public IActionResult Appointments()
        {
            // جلب قائمة المواعيد مع معلومات الخدمة
            var appointments = _context.Appointments
                .Include(a => a.Service) // تضمين معلومات الخدمة
                .OrderBy(a => a.AppointmentDateTime) // ترتيب حسب التاريخ
                .ToList();

            return View(appointments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAppointment(int id)
        {
            // البحث عن الموعد بناءً على المعرّف
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                return RedirectToAction("Appointments");
            }

            // حذف الموعد
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Appointment deleted successfully.";
            return RedirectToAction("Appointments");
        }


    }
}
