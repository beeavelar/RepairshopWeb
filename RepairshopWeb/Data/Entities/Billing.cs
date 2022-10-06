using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Billings")]
    public class Billing : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Repair Order ID")]
        public int RepairOrderId { get; set; }

        [ForeignKey("RepairOrderId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a repair order number.")]
        public RepairOrder RepairOrder { get; set; }

        [Display(Name = "Client Name")]
        public int ClientId { get; set; }

        [Display(Name = "Nif")]
        public int Nif { get; set; }

        [ForeignKey("ClientId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public Client Client { get; set; }

        [Display(Name = "Vehicle License Plate")]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Total To Pay")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalToPay { get; set; }

        public User User { get; set; }

    }
}
