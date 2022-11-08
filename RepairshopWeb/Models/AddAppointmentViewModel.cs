using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class AddAppointmentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Alert Appoin. Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime AlertDate { get; set; }

        [Display(Name = "Client")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a service.")]
        public int ClientId { get; set; }

        public Guid UserId { get; set; }

        [Display(Name = "Vehicle")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a vehicle.")]
        public int VehicleId { get; set; }

        public IEnumerable<SelectListItem> Clients { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }
    }
}
