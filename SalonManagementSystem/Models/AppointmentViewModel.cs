namespace SalonManagementSystem.Models
{
    public class AppointmentViewModel
    {
        public Service Service { get; set; }
        public List<TimeSpan> AvailableTimes { get; set; }
    }

}
