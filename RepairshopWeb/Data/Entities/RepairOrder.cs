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
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime RepairOrderDate { get; set; }

        [Display(Name = "Appointment Date")]
        public Appointment Appointment { get; set; }

        [Display(Name = "License Plate")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a vehicle license plate.")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Total Services To Do")]
        public int TotalServicesToDo => Items == null ? 0 : Items.Count();

        [Display(Name = "Total Mechanics Working")]
        public decimal TotalMechanicsWorking => Items == null ? 0 : Items.Sum(i => i.MechanicId);

        [Display(Name = "Total To Pay")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalToPay => Items == null ? 0 : Items.Sum(i => i.RepairPrice);

        [Display(Name = "Payment State")]
        public string PaymentState { get; set; }

        [Required]
        public User User { get; set; }

        public IEnumerable<RepairOrderDetail> Items { get; set; } //Aqui que faz a ligação com a tabela de RepairOrder - Ligação de 1 para muitos - 1 RepairOrder tem vários itens

    }
}
