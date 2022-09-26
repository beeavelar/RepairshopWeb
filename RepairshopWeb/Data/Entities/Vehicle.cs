using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Vehicles")]
    public class Vehicle : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }

        [Column("ClientId")]
        [Display(Name = "Client Name")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public Client Client { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Brand { get; set; }

        [Display(Name = "Model")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string VehicleModel { get; set; }

        public string Color { get; set; }

        [Range(1950, 2022)]
        public int Year { get; set; }

        public User User { get; set; }

        public ICollection<RepairOrder> RepairOrders { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
