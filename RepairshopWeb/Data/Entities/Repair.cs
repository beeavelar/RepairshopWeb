﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Repairs")]
    public class Repair : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        [Display(Name = "Repair Description")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Repair Price")]
        public decimal RepairPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Labor Price")]
        public decimal LaborPrice { get; set; }

        public User User { get; set; }
    }

}
