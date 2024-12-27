using Microsoft.EntityFrameworkCore;
using System;

namespace SalonManagementSystem.Models
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext  _context) 
        {

            _context.Database.EnsureCreated();


            if (_context.Admin.Any())
            {
                return;
            }

            var admin = new Admin
            {
                Email = "a@gmail.com",
                Password = "1234"
            };

            _context.Admin.Add(admin); 
            _context.SaveChanges();
        }
    }
}
