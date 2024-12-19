namespace SalonManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // اسم الموظف
        public string Specialization { get; set; } // التخصص
        public TimeSpan StartWorkingHours { get; set; } // بداية ساعات العمل
        public TimeSpan EndWorkingHours { get; set; } // نهاية ساعات العمل
    }
}
