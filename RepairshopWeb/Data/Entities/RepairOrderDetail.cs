﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("RepairOrdersDetails")]
    public class RepairOrderDetail : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Vehicle License Plate")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a vehicle plate.")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a service.")]
        public Service Service { get; set; }

        [Display(Name = "Mechanic Name")]
        public int MechanicId { get; set; }

        [ForeignKey("MechanicId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a mechanic.")]
        public Mechanic Mechanic { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal RepairPrice { get; set; }

    }
}