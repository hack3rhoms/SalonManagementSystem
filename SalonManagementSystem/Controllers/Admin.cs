using Microsoft.AspNetCore.Mvc;
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

        // AddEmployee
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(Employee employee)
        {
            // البحث عن تضارب الساعات
            var conflictingSlots = _context.Employees
                .Where(e =>
                    (employee.StartWorkingHours < e.EndWorkingHours && employee.EndWorkingHours > e.StartWorkingHours))
                .ToList();

            if (conflictingSlots.Any())
            {
                // إذا كان هناك تضارب، أعرض رسالة خطأ وأعد الساعات المحجوزة للعرض في الواجهة
                ModelState.AddModelError("", "The selected time slot is not available. Please choose a different time.");
                ViewBag.BookedSlots = conflictingSlots.Select(e => new { e.StartWorkingHours, e.EndWorkingHours });
                return View(employee);
            }
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("ManageEmployees");
            }
            return View(employee);
        }

    }
}
