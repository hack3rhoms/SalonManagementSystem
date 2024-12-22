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
            ViewBag.Services = _context.Services.ToList(); // جلب قائمة الخدمات
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            // تحقق من صحة النموذج
            
                ModelState.AddModelError("", "Please fill all required fields correctly.");
                ViewBag.Services = _context.Services.ToList();
            

            // تحقق من الحقول الزمنية
            if (employee.StartWorkingHours >= employee.EndWorkingHours)
            {
                ModelState.AddModelError("", "Start working hours must be less than end working hours.");
                ViewBag.Services = _context.Services.ToList();
                return View(employee);
            }

            // تحقق من تداخل الوقت
            var isOverlap = _context.Employees
                .Any(e => e.ServiceId == employee.ServiceId &&
                          e.StartWorkingHours < employee.EndWorkingHours &&
                          employee.StartWorkingHours < e.EndWorkingHours);

            if (isOverlap)
            {
                ModelState.AddModelError("", "There is a time overlap with another employee for the same service.");
                ViewBag.Services = _context.Services.ToList();
                return View(employee);
            }

            // حفظ الموظف في قاعدة البيانات
            
                _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewEmployees");
            
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
         public IActionResult AddService(Service service)
         {
         
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
    }
}
