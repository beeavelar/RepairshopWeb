using Microsoft.AspNetCore.Http;
using RepairshopWeb.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class ChangeUserViewModel : User
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
