﻿using Microsoft.AspNetCore.Http;
using RepairshopWeb.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class RegisterNewUserViewModel : User
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)] 
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
