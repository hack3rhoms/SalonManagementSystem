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
            return View();
        }
    }
}
