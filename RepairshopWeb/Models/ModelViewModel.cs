using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class ModelViewModel
    {
        public int BrandId { get; set; }


        public int ModelId { get; set; }


        [Required]
        [Display(Name = "Model")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }

    }
}
