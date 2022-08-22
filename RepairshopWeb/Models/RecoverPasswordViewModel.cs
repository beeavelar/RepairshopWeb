using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
