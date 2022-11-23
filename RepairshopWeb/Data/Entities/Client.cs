using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Clients")]
    public class Client : IEntity
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
        public string PostalCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public int Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "NIF")]
        public int Nif { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        [Display(Name = "Client Status")]
        public string ClientStatus { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        public ICollection<Billing> Billings { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public string UserClientId { get; set; }
        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
           ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
           : $"https://repairshopapp.blob.core.windows.net/photos/{ImageId}";


        public string FullName => $"{FirstName} {LastName}";
    }
}

