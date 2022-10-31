using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Appointments")]
    public class Appointment : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Local Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? AppointmentDate { get; set; }

        [Display(Name = "Alert Appoin. Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? AlertDate { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        public RepairOrder RepairOrder { get; set; }

        [Required]
        public User User { get; set; }

        public IEnumerable<AppointmentDetail> Items { get; set; } //Aqui que faz a ligação com a tabela de Appointment - Ligação de 1 para muitos - 1 Appointment tem vários itens
    }
}
