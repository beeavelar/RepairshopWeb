using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairshopWeb.Data.Entities
{
    [Table("Vehicles")]
    public class Vehicle 
    {
        //[Key]
        //public int Id { get; set; }

        //[Required]
        //[Display(Name = "License Plate")]
        //public string LicensePlate { get; set; }

        //[Required]
        //[Display(Name = "Client nName")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a client.")]
        //public Client Cliente { get; set; }

        //[MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        //public string Brand { get; set; }

        //[Display(Name = "Model")]
        //public int VehicleModel { get; set; }

        ////public Model Model { get; set; }

        //public string Category { get; set; }

        //public string Color { get; set; }

        //public int? Year { get; set; }

        //[Display(Name = "Photo")]
        //public Guid ImageId { get; set; }

        //public User User { get; set; }

        //public string ImageFullPath => ImageId == Guid.Empty
        //   ? $"https://repairshopweb.azurewebsites.net/images/noimage.jpg"
        //   : $"https://repairshodebora.blob.core.windows.net/clients/{ImageId}";

    }
}
