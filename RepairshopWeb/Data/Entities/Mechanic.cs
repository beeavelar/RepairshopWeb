using System;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class Mechanic : IEntity
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

        [Display(Name = "Postal Code")]
        [MaxLength(8, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public string PostalCode { get; set; }

        [Display(Name = "Cellphone")]
        public int? Phone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "NIF")]
        public int Nif { get; set; }

        [Display(Name = "NISS")]
        public int? Niss { get; set; }

        [Display(Name = "Identity Number")]
        public string IdentityDocument { get; set; }

        //public MechanicSpeciality Speciality { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
           ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
           : $"https://repairshodebora.blob.core.windows.net/clients/{ImageId}";
    }
}
