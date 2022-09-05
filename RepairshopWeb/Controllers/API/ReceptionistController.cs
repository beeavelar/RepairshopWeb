using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;

namespace RepairshopWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistRepository _receptionistRepository;

        public ReceptionistController(IReceptionistRepository receptionistRepository)
        {
            _receptionistRepository = receptionistRepository;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            return Ok(_receptionistRepository.GetAllWithUsers());
        }
    }
}
