using System.ComponentModel.DataAnnotations;

namespace RepairshopWeb.Models
{
    public class ClientStatusViewModel
    {
        public int Id { get; set; }

        [Display(Name = "What is the status of the client? ")]
        public string ClientStatus { get; set; }
    }
}
