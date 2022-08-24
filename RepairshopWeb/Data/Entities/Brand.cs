using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Brand")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        public string Name { get; set; }

        public ICollection<Model> Models { get; set; }
    }
}
