using Microsoft.AspNetCore.Http;
using RepairshopWeb.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class VehicleViewModel : Vehicle
    {
        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
