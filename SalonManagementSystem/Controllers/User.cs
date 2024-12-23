using Microsoft.AspNetCore.Mvc;

namespace SalonManagementSystem.Controllers
{
    public class User : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
