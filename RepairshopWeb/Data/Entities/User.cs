using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more then {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more then {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [Display(Name = "Select a role to the new user: ")]
        public string Role { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
          ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
          : $"https://repairshodebora.blob.core.windows.net/users/{ImageId}";


        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

    }
}
