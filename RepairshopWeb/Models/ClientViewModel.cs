using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;

namespace RepairshopWeb.Models
{
    public class ClientViewModel : Client
    {
        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
