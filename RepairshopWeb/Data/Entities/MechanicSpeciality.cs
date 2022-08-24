﻿using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class MechanicSpeciality : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Speciality")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public string Name { get; set; }

    }
}
