using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using System.Collections.Generic;
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
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }

    }
}
