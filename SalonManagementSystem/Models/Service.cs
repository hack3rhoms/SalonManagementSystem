namespace SalonManagementSystem.Models
{
    public class Service
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // اسم الخدمة
        public decimal Price { get; set; } // سعر الخدمة
        public TimeSpan Duration { get; set; } // مدة الخدمة
    }
}
