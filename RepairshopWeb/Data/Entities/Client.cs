using System;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }

        [MaxLength(8, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public string PostalCode { get; set; }

        [MaxLength(13, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public string Phone { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [MaxLength(9, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public int? Nif { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
           ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
           : $"https://repairshodebora.blob.core.windows.net/clients/{ImageId}"; 
    }
}

