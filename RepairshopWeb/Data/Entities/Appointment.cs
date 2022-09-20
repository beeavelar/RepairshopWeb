﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Appointments")]
    public class Appointment : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Repair Order Number")]
        public int RepairOrderId { get; set; }

        [ForeignKey("RepairOrderId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a repair order number.")]
        public RepairOrder RepairOrder { get; set; }

        [Display(Name = "Client Name")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public Client Client { get; set; }

        [Display(Name = "License Plate")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime RepairDate { get; set; }

        [Display(Name = "Alert Repair Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime AlertRepairDate { get; set; }

        [Display(Name = "Repair Status")]
        public string RepairStatus { get; set; }

        public User User { get; set; }

    }
}
