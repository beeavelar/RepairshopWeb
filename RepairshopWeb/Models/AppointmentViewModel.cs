using RepairshopWeb.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime RepairDate { get; set; }

        [Display(Name = "Alert Repair Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime AlertDate { get; set; }
    }
}
