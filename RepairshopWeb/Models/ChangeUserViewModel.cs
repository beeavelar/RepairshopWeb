﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
