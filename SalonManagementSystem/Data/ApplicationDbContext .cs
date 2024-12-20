using Microsoft.EntityFrameworkCore;
using SalonManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Service> Services { get; set; }

}
