using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RepairshopWeb.Data.Entities;

namespace RepairshopWeb.Models
{
    public class ClientViewModel : Client
    {
        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
