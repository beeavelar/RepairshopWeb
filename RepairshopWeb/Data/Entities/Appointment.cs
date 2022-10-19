using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Appointments")]
    public class Appointment : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? AppointmentDate { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Display(Name = "Alert Appoin. Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? AlertDate { get; set; }

        [Display(Name = "Repair Status")]
        public string RepairStatus { get; set; }

    }
}
