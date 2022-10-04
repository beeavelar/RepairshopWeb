using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RepairshopWeb.Models
{
    public class AddItemViewModel
    {
        [Display(Name="Service")]
        [Range(1,int.MaxValue, ErrorMessage = "Select a service")]
        public int ServiceId { get; set; }

        [Display(Name = "Vehicle")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a vehicle...")]
        public int VehicleId { get; set; }

        [Display(Name = "Mechanic")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a mechanic...")]
        public int MechanicId { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }

        public IEnumerable<SelectListItem> Mechanics { get; set; }
    }
}
