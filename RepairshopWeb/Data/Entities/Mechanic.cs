using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Mechanics")]
    public class Mechanic : IEntity
    {
        [Key]
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

        [Display(Name = "Zip Code")]
        [MaxLength(8, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone")]
        public int? Phone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "NIF")]
        public int Nif { get; set; }

        [Display(Name = "NISS")]
        public int? Niss { get; set; }

        [Display(Name = "ID Nº")]
        public string IdentityDocument { get; set; }

        public string Speciality { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        public User User { get; set; }

        public ICollection<RepairOrder> RepairOrders { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
          ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
          : $"https://repairshopapp.blob.core.windows.net/photos/{ImageId}";

        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
