using Microsoft.EntityFrameworkCore;
using SalonManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admin { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تعريف العلاقة بين Employee و Service
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Service)
            .WithMany(s => s.Employees)
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // تعريف حدود ساعات العمل
        modelBuilder.Entity<Employee>()
            .Property(e => e.StartWorkingHours)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.EndWorkingHours)
            .IsRequired();

        modelBuilder.Entity<Appointment>()
      .HasOne(a => a.User)
      .WithMany(u => u.Appointments)
      .HasForeignKey(a => a.UserId)
      .OnDelete(DeleteBehavior.Cascade); 
    }
}
