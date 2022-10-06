using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class RepairOrderStatusViewModel
    {
        public int Id { get; set; }

        [Display(Name = "What is the status of the repair order? ")]
        public string RepairStatus { get; set; }
    }
}
