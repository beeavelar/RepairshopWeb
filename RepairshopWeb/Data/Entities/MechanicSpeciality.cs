using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Data.Entities
{
    public class MechanicSpeciality : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characteres length.")]
        [Display(Name = "Speciality Name")]
        public string SpecialityName { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }
}
