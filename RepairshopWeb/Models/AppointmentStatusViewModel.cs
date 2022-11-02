using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class AppointmentStatusViewModel
    {
        public int Id { get; set; }

        [Display(Name = "What is the status of the appointment? ")]
        public string AppointmentStatus { get; set; }
    }
}
