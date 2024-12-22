using System.ComponentModel.DataAnnotations;

namespace SalonManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // اسم الموظف
        public int ServiceId { get; set; } // معرّف الخدمة المرتبطة
        public Service Service { get; set; } // الخدمة المرتبطة

        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan StartWorkingHours { get; set; } // بداية ساعات العمل
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan EndWorkingHours { get; set; } // نهاية ساعات العمل
    }
}
