using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class Model : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Model")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
