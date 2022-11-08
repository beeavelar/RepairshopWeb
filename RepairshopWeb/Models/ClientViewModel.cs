using Microsoft.AspNetCore.Http;
using RepairshopWeb.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class ClientViewModel : Client
    {
        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
