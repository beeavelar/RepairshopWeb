using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RepairshopWeb.Data.Entities
{
    [Table("RepairOrders")]
    public class RepairOrder : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Local Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Total Services to Do")]
        public int TotalServicesToDo => Items == null ? 0 : Items.Count();

        [Display(Name = "Total to Pay")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalToPay => Items == null ? 0 : Items.Sum(i => i.RepairPrice);

        [Display(Name = "Payment State")]
        public string PaymentState { get; set; }

        [Display(Name = "Billing Number")]
        public Billing Billing { get; set; }

        [Display(Name = "Repair Status")]
        public string RepairStatus { get; set; }

        [Required]
        public User User { get; set; }

        public Appointment Appointment { get; set; }

        public int AppointmentId { get; set; }

        public IEnumerable<RepairOrderDetail> Items { get; set; } //Aqui que faz a ligação com a tabela de RepairOrder - Ligação de 1 para muitos - 1 RepairOrder tem vários itens

    }
}
