﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("RepairOrdersDetailsTemps")]
    public class RepairOrderDetailTemp : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        [Display(Name = "Mechanic")]
        public int MechanicId { get; set; }

        [ForeignKey("MechanicId")]
        public Mechanic Mechanic { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal RepairPrice { get; set; }

        public User User { get; set; }
    }
}
