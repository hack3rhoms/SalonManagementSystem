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

        // Add new Employee - GET
        public IActionResult AddEmployee()
        {
            // جلب قائمة الخدمات من قاعدة البيانات
            var services = _context.Services
                .Select(s => new { s.Id, s.Name }) // استرجاع المعرف والاسم فقط
                .ToList();

            // التحقق من وجود خدمات
            if (!services.Any())
            {
                // عرض رسالة خطأ إذا لم تكن هناك خدمات
                ModelState.AddModelError("", "No services available. Please add services before adding an employee.");
                return View(); // يمكن عرض صفحة خطأ مخصصة هنا
            }

            // تعيين قائمة الخدمات في ViewBag
            ViewBag.Services = services;

            // تمرير نموذج جديد للعرض
            return View(new Employee());
        }



        // Add new Employee - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(Employee employee)
        {
            // التحقق من صحة ساعات العمل
            if (employee.StartWorkingHours >= employee.EndWorkingHours)
            {
                ModelState.AddModelError("", "End time must be after start time.");
                ReloadViewBag(); // إعادة تحميل ViewBag في حالة وجود خطأ
                return View(employee);
            }

            if (ModelState.IsValid)
            {
                // إضافة الموظف إلى قاعدة البيانات
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("ManageEmployees");
            }

            ReloadViewBag(); // إعادة تحميل ViewBag في حالة وجود خطأ
            return View(employee);
        }

        // إعادة تحميل ViewBag
        private void ReloadViewBag()
        {
            ViewBag.Services = _context.Services
                .Select(s => new { s.Id, s.Name })
                .ToList();
        }







        // View Employees
        public IActionResult ViewEmployees()
        {
            // استرجاع جميع الموظفين مع الخدمات المرتبطة
            var employees = _context.Employees
                .Include(e => e.Service) // جلب الخدمة المرتبطة مع كل موظف
                .ToList();

            // إرسال البيانات إلى الواجهة
            return View(employees);
        }

        // Manage Services
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

        // View Services
        public IActionResult ViewServices()
        {
            var services = _context.Services.ToList();
            return View(services);
        }   
    }
}
