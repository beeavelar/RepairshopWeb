using System;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class Vehicle : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Client Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Brand { get; set; }

        [Display(Name = "Model")]
        public int VehicleModel { get; set; }

        //public Model Model { get; set; }

        public string Category { get; set; }

        public string Color { get; set; }

        public int? Year { get; set; }

        [Display(Name = "Photo")]
        public Guid ImageId { get; set; }

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
           ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
           : $"https://repairshodebora.blob.core.windows.net/clients/{ImageId}";
    }
}
