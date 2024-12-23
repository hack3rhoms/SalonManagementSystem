namespace SalonManagementSystem.Models
{
    public class Service
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // اسم الخدمة
        public decimal Price { get; set; } // سعر الخدمة
        public int Duration { get; set; } // مدة الخدمة
        public byte[] ImageData { get; set; } // بيانات الصورة
        public string ImageType { get; set; } // نوع الصورة (MIME type)

        // العلاقة مع العمال
        public ICollection<Employee> Employees { get; set; } // قائمة العمال الذين يقدمون هذه الخدمة
    }
}
