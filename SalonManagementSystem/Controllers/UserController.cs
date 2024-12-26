using Microsoft.AspNetCore.Mvc;

namespace SalonManagementSystem.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
